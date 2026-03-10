using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Server;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.Rooms;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq; // Savaş filtrelemeleri için eklendi
using System.Net;
using System.Reflection;
using System.Text;

namespace Game.Server.API
{
    internal static class GameApiServer
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static HttpListener _listener;
        private static bool _running;

        private static string ADMIN_KEY =>
            System.Configuration.ConfigurationManager.AppSettings["AdminKey"] ?? "DEGISTIR";

        private static int API_PORT =>
            int.TryParse(System.Configuration.ConfigurationManager.AppSettings["ApiPort"], out var p) ? p : 9500;

        public static void Start(int port = -1)
        {
            if (_running) return;

            var p = port > 0 ? port : API_PORT;

            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://*:{p}/");
            _listener.Start();
            _running = true;

            Console.WriteLine($"[API] {p} portunda dinleniyor...");
            System.Threading.Tasks.Task.Run(Loop);
        }

        public static void Stop()
        {
            try { _running = false; _listener?.Stop(); } catch { }
        }

        private static async System.Threading.Tasks.Task Loop()
        {
            while (_running)
            {
                HttpListenerContext ctx;
                try { ctx = await _listener.GetContextAsync(); }
                catch { if (!_running) break; else continue; }
                _ = System.Threading.Tasks.Task.Run(() => Handle(ctx));
            }
        }

        // Oyuncu enerjisi hesabı (PlayerInfo için gerekli)
        private static int CalcEnergy(int agility)
        {
            return 240 + (int)(agility / 30.0);
        }

        private static void Handle(HttpListenerContext ctx)
        {
            try
            {
                ctx.Response.AddHeader("Access-Control-Allow-Origin", "*");
                ctx.Response.AddHeader("Content-Type", "application/json; charset=utf-8");

                var req = ctx.Request;
                var path = req.Url.AbsolutePath.TrimEnd('/').ToLowerInvariant();

                Console.WriteLine($"[API] Incoming {req.HttpMethod} {path}, X-API-Key={req.Headers["X-API-Key"] ?? "<yok>"}");

                if (req.HttpMethod == "OPTIONS")
                {
                    ctx.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type,X-API-Key");
                    ctx.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST,OPTIONS");
                    ctx.Response.StatusCode = 200; ctx.Response.Close(); return;
                }

                // ==========================================
                // HERKESE AÇIK (PUBLIC) ENDPOINTLER
                // ==========================================
                if (path == "" || path == "/")
                {
                    WriteJson(ctx, new { ok = true, name = "Game API", version = 1 });
                    return;
                }

                if (path == "/ping" && req.HttpMethod == "GET")
                {
                    WriteJson(ctx, new { message = "Oyun sunucusu API canlı 🚀" });
                    return;
                }

                if (path == "/api/game/status" && req.HttpMethod == "GET")
                {
                    var allClients = GameServer.Instance.GetAllClients();
                    int onlineCount = allClients != null ? allClients.Length : 0;

                    List<BaseRoom> allUsingRoom = RoomMgr.GetAllUsingRoom();
                    int activeRoomCount = 0;
                    int playingPlayerCount = 0;
                    foreach (var room in allUsingRoom)
                    {
                        if (!room.IsEmpty)
                        {
                            activeRoomCount++;
                            if (room.IsPlaying)
                            {
                                playingPlayerCount += room.PlayerCount;
                            }
                        }
                    }
                    double ramUsageMB = (double)GC.GetTotalMemory(false) / 1024.0 / 1024.0;

                    WriteJson(ctx, new
                    {
                        message = "OK",
                        onlineCount,
                        activeRoomCount,
                        playingPlayerCount,
                        ramUsageMB = Math.Round(ramUsageMB, 2),
                        serverUptime = DateTime.Now.Subtract(System.Diagnostics.Process.GetCurrentProcess().StartTime).TotalHours
                    });
                    return;
                }

                if (path == "/api/game/onlineplayers" && req.HttpMethod == "GET")
                {
                    var players = WorldMgr.GetAllPlayers();
                    var list = new List<object>();
                    foreach (var p in players)
                    {
                        var c = p.PlayerCharacter;
                        list.Add(new { Nickname = c.NickName, Level = c.Grade, FightPower = c.FightPower });
                    }
                    WriteJson(ctx, new { Count = list.Count, Players = list });
                    return;
                }

                // !stat komutu için gerekli update endpointi
                if (path.StartsWith("/api/game/update/") && req.HttpMethod == "GET")
                {
                    var nick = path.Substring("/api/game/update/".Length);

                    if (string.IsNullOrWhiteSpace(nick))
                    {
                        ctx.Response.StatusCode = 400;
                        WriteJson(ctx, new { error = "Nick gerekli." });
                        return;
                    }

                    var pl = WorldMgr.GetClientByPlayerNickName(nick);
                    if (pl == null)
                    {
                        ctx.Response.StatusCode = 404;
                        WriteJson(ctx, new { error = "Oyuncu çevrimdışı veya bulunamadı." });
                        return;
                    }

                    try
                    {
                        pl.SavePlayerInfo();
                        var c = pl.PlayerCharacter;

                        WriteJson(ctx, new
                        {
                            success = true,
                            message = "Oyuncu bilgileri güncellendi.",
                            nickName = c.NickName,
                            level = c.Grade,
                            fightPower = c.FightPower
                        });
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.Error("[API] /api/game/update hata:", ex);
                        ctx.Response.StatusCode = 500;
                        WriteJson(ctx, new { error = "Sunucu hatası: " + ex.Message });
                        return;
                    }
                }

                // !stat ve VIP Senkronizasyonu için playerinfo endpointi
                if (path.StartsWith("/api/game/playerinfo/") && req.HttpMethod == "GET")
                {
                    var nick = path.Substring("/api/game/playerinfo/".Length);

                    // 1) Önce online oyuncu var mı bak
                    var p = WorldMgr.GetClientByPlayerNickName(nick);
                    if (p != null)
                    {
                        var c = p.PlayerCharacter;
                        double baseAttack = p.GetBaseAttack();
                        double baseDefence = p.GetBaseDefence();
                        int hp = c.hp;
                        int energy = CalcEnergy(c.Agility);

                        WriteJson(ctx, new
                        {
                            Username = c.UserName,
                            Nickname = c.NickName,
                            Level = c.Grade,
                            Money = c.Money,
                            Attack = c.Attack,
                            Defence = c.Defence,
                            Agility = c.Agility,
                            Luck = c.Luck,
                            HP = hp,
                            Damage = (int)baseAttack,
                            Guard = (int)baseDefence,
                            Energy = energy,
                            FightPower = c.FightPower,
                            VIPLevel = c.VIPLevel,
                            IsOnline = true
                        });
                        return;
                    }

                    // 2) Offline ise DB'den PlayerInfo çek
                    using (var pb = new PlayerBussiness())
                    {
                        var info = pb.GetUserSingleByNickName(nick);
                        if (info == null)
                        {
                            ctx.Response.StatusCode = 404;
                            WriteJson(ctx, new { error = "Oyuncu bulunamadı" });
                            return;
                        }

                        int hp = info.hp;
                        int energy = CalcEnergy(info.Agility);

                        WriteJson(ctx, new
                        {
                            Username = info.UserName,
                            Nickname = info.NickName,
                            Level = info.Grade,
                            Money = info.Money,
                            Attack = info.Attack,
                            Defence = info.Defence,
                            Agility = info.Agility,
                            Luck = info.Luck,
                            HP = hp,
                            Damage = info.Attack,
                            Guard = info.Defence,
                            Energy = energy,
                            FightPower = info.FightPower,
                            VIPLevel = info.VIPLevel,
                            IsOnline = false
                        });
                        return;
                    }
                }

                // ==========================================
                // ŞANSLI ITEM VE DUYURU METNİ GÜNCELLEME
                // ==========================================
                if (path == "/api/game/setluckyitems" && req.HttpMethod == "POST")
                {
                    try
                    {
                        var d = ReadJsonBodyJ(req);
                        var ids = d["ItemIDs"]?.ToObject<List<int>>();
                        var template = d["Template"]?.ToString();

                        if (ids != null)
                        {
                            PVEGame.LuckyNoticeItems = ids;

                            if (!string.IsNullOrEmpty(template))
                            {

                                string internalTemplate = template
                                    .Replace("{nick}", "{0}")
                                    .Replace("{item}", "{1}");

                                PVEGame.LuckyNoticeTemplate = internalTemplate;
                            }
                            WriteJson(ctx, new { success = true });
                            return;
                        }
                    }
                    catch (Exception ex) { WriteJson(ctx, new { error = ex.Message }); }
                }

                if (path == "/api/game/testlucky" && req.HttpMethod == "POST")
                {
                    try
                    {
                        var players = WorldMgr.GetAllPlayers();
                        Console.WriteLine($"[API-TEST] Aktif oyuncu sayisi: {players.Length}");

                        // PVEGame'deki statik şablonu al
                        string template = global::Game.Logic.PVEGame.LuckyNoticeTemplate;
                        string testNick = players.Length > 0 ? players[0].PlayerCharacter.NickName : "TestOyuncu";
                        string testItem = "Saka taşı 1. seviye";

                        // Mesajı hazırla
                        string formattedMsg = string.Format(template, testNick, testItem);

                        // Paket 10 oluştur
                        GSPacketIn pkg = new GSPacketIn((short)10);
                        pkg.WriteInt(3);
                        pkg.WriteString(formattedMsg);

                        // 1. LoginServer'a gönder (Kesin çözüm)
                        GameServer.Instance.LoginServer.SendPacket(pkg);

                        // 2. Eğer online oyuncu varsa hepsine tek tek bas
                        foreach (var p in players)
                        {
                            p.SendTCP(pkg);
                        }

                        WriteJson(ctx, new { success = true, message = "Test duyurusu firlatildi." });
                        return;
                    }
                    catch (Exception ex)
                    {
                        WriteJson(ctx, new { success = false, error = ex.Message });
                    }
                }

                // ==========================================
                // YETKİ KONTROLÜ (API KEY GEREKTİRENLER)
                // ==========================================
                if (!IsAuthorized(req))
                {
                    ctx.Response.StatusCode = 401;
                    WriteJson(ctx, new { error = "Unauthorized", tip = "X-API-Key header gerekli" });
                    return;
                }

                if (path == "/api/game/announce" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);
                    string type = (d["Type"]?.ToString() ?? "sari").ToLowerInvariant();
                    string text = d["Message"]?.ToString() ?? "";

                    switch (type)
                    {
                        case "kucuk":
                            {
                                var pkt = new GSPacketIn(71);
                                pkt.WriteInt(0); pkt.WriteString("Sistem"); pkt.WriteString(text);
                                GameServer.Instance.LoginServer.SendPacket(pkt); SendAllTCP(pkt); break;
                            }
                        case "buyuk":
                            {
                                var pkt = new GSPacketIn(72);
                                pkt.WriteInt(0); pkt.WriteInt(0); pkt.WriteString("Sistem"); pkt.WriteString(text);
                                GameServer.Instance.LoginServer.SendPacket(pkt); SendAllTCP(pkt); break;
                            }
                        case "kirmizi":
                            {
                                var pkt = new GSPacketIn(73, 0);
                                pkt.WriteInt(1); pkt.WriteInt(0); pkt.WriteString("Sistem"); pkt.WriteString(text); pkt.WriteString("Yönetim");
                                GameServer.Instance.LoginServer.SendPacket(pkt); SendAllTCP(pkt); break;
                            }
                        case "mor":
                            new ManageBussiness().SystemNotice(text); break;
                        case "admin":
                            SendAllText("[ADMIN] " + text); break;
                        case "sari":
                            SendAllText(text); break;
                        default:
                            ctx.Response.StatusCode = 400;
                            WriteJson(ctx, new { error = "Geçersiz duyuru tipi", allow = new[] { "kucuk", "buyuk", "kirmizi", "mor", "admin", "sari" } });
                            return;
                    }
                    WriteJson(ctx, new { message = "Duyuru gönderildi", type, text });
                    return;
                }

                if (path == "/api/game/specialmessage" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);
                    string text = d["Message"]?.ToString() ?? "";

                    if (string.IsNullOrEmpty(text)) { ctx.Response.StatusCode = 400; WriteJson(ctx, new { error = "Mesaj gerekli" }); return; }

                    foreach (var pl in WorldMgr.GetAllPlayers())
                    {
                        pl.Out.SendMessage((eMessageType)5, "[ YÖNETİM ]: " + text);
                    }
                    WriteJson(ctx, new { message = "Özel mesaj gönderildi", text });
                    return;
                }

                if (path == "/api/game/kick" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);
                    string nickname = d["Nickname"]?.ToString();
                    var p = WorldMgr.GetClientByPlayerNickName(nickname);
                    if (p == null) { ctx.Response.StatusCode = 404; WriteJson(ctx, new { error = "Oyuncu bulunamadı" }); return; }
                    using (var mb = new ManageBussiness()) mb.KitoffUserByNickName(nickname, "API kick");
                    p.Disconnect();
                    WriteJson(ctx, new { message = $"{nickname} kicklendi" });
                    return;
                }

                if (path == "/api/game/ban" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);
                    string nickname = d["Nickname"]?.ToString();
                    string reason = d["Reason"]?.ToString() ?? "Belirtilmedi";
                    DateTime until = DateTime.UtcNow.AddYears(25);
                    using (var mb = new ManageBussiness()) mb.ForbidPlayerByNickName(nickname, until, false, reason);
                    foreach (var pl in WorldMgr.GetAllPlayers()) pl.SendMessage($"Oyuncu <{nickname}> banlandı. Sebep: {reason}");
                    WriteJson(ctx, new { message = $"{nickname} banlandı", until });
                    return;
                }

                if (path == "/api/game/banbyusername" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);
                    string username = d["Username"]?.ToString();
                    string reason = d["Reason"]?.ToString() ?? "Belirtilmedi";
                    DateTime until = DateTime.UtcNow.AddYears(25);

                    if (string.IsNullOrEmpty(username)) { ctx.Response.StatusCode = 400; WriteJson(ctx, new { error = "Username gerekli" }); return; }

                    using (var mb = new ManageBussiness()) mb.ForbidPlayerByUserName(username, until, false, reason);
                    foreach (var pl in WorldMgr.GetAllPlayers()) pl.SendMessage($"Oyuncu <{username}> (Kullanıcı Adı) banlandı. Sebep: {reason}");
                    WriteJson(ctx, new { message = $"{username} (Kullanıcı Adı) banlandı", until });
                    return;
                }

                if (path == "/api/game/unban" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);
                    string nickname = d["Nickname"]?.ToString();
                    using (var mb = new ManageBussiness()) mb.ForbidPlayerByNickName(nickname, DateTime.UtcNow, true);
                    foreach (var pl in WorldMgr.GetAllPlayers()) pl.SendMessage($"Oyuncu <{nickname}> banı kaldırıldı.");
                    WriteJson(ctx, new { message = $"{nickname} unban" });
                    return;
                }

                if (path == "/api/game/giveitem" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);
                    string nickname = d["Nickname"]?.ToString();
                    int itemId = d["ItemID"]?.ToObject<int?>() ?? 0;
                    int count = d["Count"]?.ToObject<int?>() ?? 1;
                    string title = d["Title"]?.ToString() ?? "Yönetim Hediyesi";
                    string content = d["Content"]?.ToString() ?? "API üzerinden";
                    bool isBinds = d["IsBinds"]?.ToObject<bool?>() ?? true;

                    var p = WorldMgr.GetClientByPlayerNickName(nickname);
                    if (p == null) { ctx.Response.StatusCode = 404; WriteJson(ctx, new { error = "Oyuncu bulunamadı" }); return; }

                    var pc = p.PlayerCharacter;
                    new PlayerBussiness().SendMailAndItem(title, content, pc.ID,
                        itemId, count, 0, 0, 0, 0, 0, 0, 0, 0, isBinds);

                    p.SendMessage($"[Discord Ödülün] oyun içi mail kutuna gönderildi.");
                    WriteJson(ctx, new { message = "Hediye gönderildi", nickname, itemId, count });
                    return;
                }

                if (path == "/api/game/mailall" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);
                    int itemId = d["ItemID"]?.ToObject<int?>() ?? 0;
                    int count = d["Count"]?.ToObject<int?>() ?? 1;
                    string title = d["Title"]?.ToString() ?? "Toplu Ödül";
                    string content = d["Content"]?.ToString() ?? "Online ödül";

                    int sent = 0;
                    foreach (var pl in WorldMgr.GetAllPlayers())
                    {
                        var pc = pl.PlayerCharacter;
                        new PlayerBussiness().SendMailAndItem(title, content, pc.ID,
                            itemId, count, 0, 0, 0, 0, 0, 0, 0, 0, true);
                        pl.SendMessage("[Online Ödül] Mail kutunu kontrol et.");
                        sent++;
                    }
                    WriteJson(ctx, new { message = "Toplu hediye gönderildi", sent });
                    return;
                }

                if (path == "/api/game/givecurrency" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);
                    string nickname = d["Nickname"]?.ToString();
                    string currency = (d["CurrencyType"]?.ToString() ?? "kupon").ToLowerInvariant();
                    int amount = d["Amount"]?.ToObject<int?>() ?? 0;

                    var pl = WorldMgr.GetClientByPlayerNickName(nickname);
                    if (pl == null) { ctx.Response.StatusCode = 404; WriteJson(ctx, new { error = "Oyuncu bulunamadı" }); return; }

                    switch (currency)
                    {
                        case "kupon": pl.AddMoney(amount); break;
                        case "exp": pl.AddGP(amount); break;
                        case "onur": pl.AddHonor(amount); break;
                        case "hediyealtin": pl.AddGiftToken(amount); break;
                        case "mukafat": pl.AddOffer(amount); break;
                        default:
                            ctx.Response.StatusCode = 400;
                            WriteJson(ctx, new { error = "Geçersiz currency", allow = new[] { "kupon", "exp", "onur", "hediyealtin", "mukafat" } });
                            return;
                    }
                    pl.SendMessage($"[Ödül] {amount} {currency} eklendi.");
                    WriteJson(ctx, new { message = "OK", nickname, currency, amount });
                    return;
                }

                if (path == "/api/game/reloadall" && req.HttpMethod == "POST")
                {
                    System.Threading.Tasks.Task.Run(() =>
                    {
                        TryStaticBool("Bussiness.Managers.BallMgr", "ReLoad");
                        TryStaticBool("Bussiness.Managers.MapMgr", "ReLoadMap");
                        TryStaticBool("Bussiness.Managers.MapMgr", "ReLoadMapServer");
                        TryStaticBool("Bussiness.Managers.PropItemMgr", "Reload");
                        TryStaticBool("Bussiness.Managers.ItemMgr", "ReLoad");
                        TryStaticBool("Bussiness.Managers.ShopMgr", "ReLoad");
                        TryStaticBool("Bussiness.Managers.QuestMgr", "ReLoad");
                        TryStaticBool("Bussiness.Managers.FusionMgr", "ReLoad");
                        TryStaticBool("Bussiness.Managers.ConsortiaMgr", "ReLoad");
                        TryStaticBool("Bussiness.Managers.RateMgr", "ReLoad");
                        TryStaticBool("Bussiness.Managers.NPCInfoMgr", "ReLoad");
                        TryStaticBool("Bussiness.Managers.FightRateMgr", "ReLoad");
                        TryStaticBool("Bussiness.Managers.AwardMgr", "ReLoad");
                        TryStaticBool("Bussiness.Managers.LanguageMgr", "Reload", new object[] { "" });

                        foreach (var pl in WorldMgr.GetAllPlayers())
                            pl.SendMessage("Veritabanı tabloları yenilendi.");
                    });

                    WriteJson(ctx, new { message = "Reload başlatıldı" });
                    return;
                }

                // ==========================================
                // DISCORD HESAP EŞLEŞTİRME SİSTEMİ (LİNKLER)
                // ==========================================
                if (path == "/api/game/link/info" && req.HttpMethod == "GET")
                {
                    try
                    {
                        string discordIdStr = ctx.Request.QueryString["discordId"];
                        if (string.IsNullOrEmpty(discordIdStr) || !long.TryParse(discordIdStr, out long discordId))
                        {
                            ctx.Response.StatusCode = 400;
                            WriteJson(ctx, new { error = "discordId gerekli veya hatalı." });
                            return;
                        }

                        int userId = DiscordLinkMgr.GetUserIdByDiscordId(discordId);
                        if (userId <= 0)
                        {
                            ctx.Response.StatusCode = 404;
                            WriteJson(ctx, new { error = "Bu Discord hesabına bağlı oyun hesabı bulunamadı." });
                            return;
                        }

                        using (var pb = new PlayerBussiness())
                        {
                            var info = pb.GetUserSingleByUserID(userId);
                            if (info == null)
                            {
                                ctx.Response.StatusCode = 404;
                                WriteJson(ctx, new { error = "Oyun hesabı veritabanında bulunamadı." });
                                return;
                            }

                            WriteJson(ctx, new
                            {
                                userId = info.ID,
                                nickName = info.NickName,
                                level = info.Grade,
                                sex = info.Sex,
                                style = info.Style,
                                colors = info.Colors
                            });
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[API] /api/game/link/info HATA: " + ex);
                        ctx.Response.StatusCode = 500;
                        WriteJson(ctx, new { error = "Sunucu hatası: " + ex.Message });
                        return;
                    }
                }

                if (path == "/api/game/link/list" && req.HttpMethod == "GET")
                {
                    int top = 100;
                    try
                    {
                        string qsTop = ctx.Request.QueryString["top"];
                        if (!string.IsNullOrEmpty(qsTop) && int.TryParse(qsTop, out int parsed))
                        {
                            if (parsed > 0 && parsed <= 500)
                                top = parsed;
                        }
                    }
                    catch { }

                    var links = DiscordLinkMgr.GetAllLinks(top);
                    var result = new List<object>();

                    using (var pb = new PlayerBussiness())
                    {
                        foreach (var link in links)
                        {
                            var info = pb.GetUserSingleByUserID(link.UserID);

                            result.Add(new
                            {
                                userId = link.UserID,
                                discordId = link.DiscordID,
                                createdAt = link.CreatedAt,
                                updatedAt = link.UpdatedAt,
                                nickName = info?.NickName,
                                userName = info?.UserName,
                                level = info?.Grade ?? 0
                            });
                        }
                    }

                    WriteJson(ctx, new
                    {
                        count = result.Count,
                        links = result
                    });
                    return;
                }

                if (path == "/api/game/link/unlink" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);

                    int userId = d["userId"]?.ToObject<int?>() ?? 0;
                    long discordId = 0;
                    long.TryParse(d["discordId"]?.ToString() ?? "0", out discordId);

                    if (userId <= 0 && discordId <= 0)
                    {
                        ctx.Response.StatusCode = 400;
                        WriteJson(ctx, new { error = "userId veya discordId gerekli" });
                        return;
                    }

                    int affected = 0;

                    if (userId > 0)
                        affected += DiscordLinkMgr.UnlinkByUserId(userId);

                    if (discordId > 0)
                        affected += DiscordLinkMgr.UnlinkByDiscordId(discordId);

                    WriteJson(ctx, new { removed = affected });
                    return;
                }

                if (path == "/api/game/link/clear" && req.HttpMethod == "POST")
                {
                    int removed = DiscordLinkMgr.ClearAllLinks();
                    WriteJson(ctx, new { removed });
                    return;
                }

                if (path == "/api/game/link/confirm" && req.HttpMethod == "POST")
                {
                    try
                    {
                        var d = ReadJsonBodyJ(req);
                        string code = d["code"]?.ToString();
                        string discordIdStr = d["discordId"]?.ToString();

                        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(discordIdStr))
                        {
                            ctx.Response.StatusCode = 400;
                            WriteJson(ctx, new { error = "code ve discordId gerekli" });
                            return;
                        }

                        if (!long.TryParse(discordIdStr, out long discordId))
                        {
                            ctx.Response.StatusCode = 400;
                            WriteJson(ctx, new { error = "discordId format hatalı" });
                            return;
                        }

                        int userId = DiscordLinkMgr.ConsumeCodeAndBindUser(code, discordId);

                        if (userId <= 0)
                        {
                            ctx.Response.StatusCode = 400;
                            WriteJson(ctx, new { error = "Kod geçersiz, süresi dolmuş veya hesap zaten bağlı." });
                            return;
                        }

                        string nick = "";
                        int level = 0;
                        string style = "";
                        string colors = "";
                        bool sex = false;

                        using (var pb = new PlayerBussiness())
                        {
                            var info = pb.GetUserSingleByUserID(userId);
                            if (info != null)
                            {
                                nick = info.NickName;
                                level = info.Grade;
                                style = info.Style;
                                colors = info.Colors;
                                sex = info.Sex;

                                // ==========================================
                                // GÖREV EVENT TETİKLEYİCİ (EKLENEN KISIM)
                                // ==========================================
                                try
                                {
                                    // Bağlanan oyuncunun Client'ını bul
                                    var onlineClient = WorldMgr.GetClientByPlayerNickName(nick);

                                    // Client ve Player null kontrolü
                                    if (onlineClient != null && onlineClient.Client.Player != null)
                                    {
                                        // GamePlayer sınıfına eklediğimiz metod ile eventi tetikle
                                        onlineClient.Client.Player.OnDiscordLinkSuccess();
                                        log.Info($"[API] Discord bağlantı görevi tetiklendi: {nick}");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Error("[API] Discord görev event hatası: ", ex);
                                }
                                // ==========================================
                            }
                        }

                        WriteJson(ctx, new
                        {
                            message = "Hesap başarıyla bağlandı.",
                            userId,
                            discordId,
                            nickName = nick,
                            level,
                            sex,
                            style,
                            colors
                        });
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[API] /api/game/link/confirm HATA: " + ex);
                        ctx.Response.StatusCode = 500;
                        WriteJson(ctx, new { error = "Sunucu hatası: " + ex.Message });
                        return;
                    }
                }

                // ==========================================
                // YENİ: SAVAŞ (BATTLE) İZLEME VE MÜDAHALE
                // ==========================================

                // 1. Savaşları Listeleme
                if (path == "/api/game/battles" && req.HttpMethod == "GET")
                {
                    var activeRooms = RoomMgr.GetAllUsingRoom().Where(r => r.IsPlaying).ToList();
                    var list = new List<object>();

                    foreach (var room in activeRooms)
                    {
                        var players = room.GetPlayers().Select(x => x.PlayerCharacter.NickName).ToList();
                        list.Add(new
                        {
                            RoomId = room.RoomId,
                            RoomType = room.RoomType.ToString(),
                            MapId = room.MapId,
                            PlayerCount = players.Count,
                            Players = players
                        });
                    }
                    WriteJson(ctx, new { Count = activeRooms.Count, Battles = list });
                    return;
                }

                // 2. Odaya Mesaj Gönderme veya Oyuncu Atma
                if (path == "/api/game/battle/action" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);
                    string action = d["Action"]?.ToString().ToLowerInvariant();
                    string target = d["Target"]?.ToString();
                    string message = d["Message"]?.ToString() ?? "";

                    if (action == "msgroom" && int.TryParse(target, out int roomId))
                    {
                        var room = RoomMgr.GetAllUsingRoom().FirstOrDefault(r => r.RoomId == roomId);
                        if (room != null)
                        {
                            GSPacketIn pkg = new GSPacketIn(3);
                            pkg.WriteInt(3);
                            pkg.WriteString("[YÜCE ADMIN]: " + message); //:ASD:AS:DAS:D:ASDA:D:AS:D:ASD: amkkkkk
                            room.SendToAll(pkg);

                            WriteJson(ctx, new { success = true, message = "Odaya mesaj iletildi." });
                            return;
                        }
                    }
                    else if (action == "kickplayer")
                    {
                        var p = WorldMgr.GetClientByPlayerNickName(target);
                        if (p != null && p.CurrentRoom != null)
                        {
                            RoomMgr.ExitRoom(p.CurrentRoom, p);
                            p.SendMessage("Yönetici tarafından savaştan atıldınız!");
                            WriteJson(ctx, new { success = true, message = $"{target} savaştan atıldı." });
                            return;
                        }
                        WriteJson(ctx, new { error = "Oyuncu savaşta değil veya bulunamadı." });
                        return;
                    }
                    ctx.Response.StatusCode = 400;
                    WriteJson(ctx, new { error = "Geçersiz işlem veya hedef." });
                    return;
                }

                // 3. TANRI GÜÇLERİ (God Powers) - LAMBDA HATASIZ KESİN ÇÖZÜM
                if (path == "/api/game/battle/godpower" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);
                    string action = d["Action"]?.ToString().ToLowerInvariant();
                    string target = d["Target"]?.ToString();
                    int amount = d["Amount"]?.ToObject<int?>() ?? 10000;

                    var gp = WorldMgr.GetClientByPlayerNickName(target);

                    if (gp == null)
                    {
                        ctx.Response.StatusCode = 404;
                        WriteJson(ctx, new { error = $"'{target}' oyunda bulunamadı. Büyük/küçük harf kontrol et." });
                        return;
                    }

                    if (gp.CurrentRoom == null || !gp.CurrentRoom.IsPlaying)
                    {
                        ctx.Response.StatusCode = 400;
                        WriteJson(ctx, new { error = $"'{target}' şu an bir savaşta değil, lobide bekliyor." });
                        return;
                    }

                    try
                    {
                        object room = gp.CurrentRoom;
                        PropertyInfo gameProp = room.GetType().GetProperty("Game");
                        object game = gameProp?.GetValue(room, null);

                        if (game == null)
                        {
                            ctx.Response.StatusCode = 400;
                            WriteJson(ctx, new { error = "Savaş odası bulundu fakat harita henüz yüklenmemiş." });
                            return;
                        }

                        // ========================================================
                        // LAMBDA (=>) KULLANMADAN FİZİKSEL KARAKTER BULMA
                        // ========================================================
                        object physicalPlayer = null;

                        MethodInfo getAllMethod = game.GetType().GetMethod("GetAllPlayers", Type.EmptyTypes)
                                               ?? game.GetType().GetMethod("GetAllFightPlayers", Type.EmptyTypes);

                        if (getAllMethod != null)
                        {
                            var playersList = getAllMethod.Invoke(game, null) as System.Collections.IEnumerable;
                            if (playersList != null)
                            {
                                foreach (object pObj in playersList)
                                {
                                    if (pObj == null) continue;

                                    PropertyInfo detailProp = pObj.GetType().GetProperty("PlayerDetail");
                                    if (detailProp != null)
                                    {
                                        object detailObj = detailProp.GetValue(pObj, null);
                                        if (detailObj == gp)
                                        {
                                            physicalPlayer = pObj;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (physicalPlayer == null)
                        {
                            MethodInfo findMethod = game.GetType().GetMethod("FindPlayer", new Type[] { typeof(int) });
                            if (findMethod != null) physicalPlayer = findMethod.Invoke(game, new object[] { gp.PlayerId });

                            if (physicalPlayer == null)
                            {
                                Type igpType = gp.GetType().GetInterface("IGamePlayer") ?? gp.GetType();
                                MethodInfo findMethod2 = game.GetType().GetMethod("GetPlayer", new Type[] { igpType });
                                if (findMethod2 != null) physicalPlayer = findMethod2.Invoke(game, new object[] { gp });
                            }
                        }

                        if (physicalPlayer == null) physicalPlayer = gp.Players;

                        if (physicalPlayer == null)
                        {
                            ctx.Response.StatusCode = 400;
                            WriteJson(ctx, new { error = $"'{target}' haritaya henüz ayak basmadı veya izleyici modunda." });
                            return;
                        }

                        PropertyInfo isLivingProp = physicalPlayer.GetType().GetProperty("IsLiving");
                        bool isLiving = isLivingProp != null && (bool)isLivingProp.GetValue(physicalPlayer, null);

                        if (!isLiving)
                        {
                            ctx.Response.StatusCode = 400;
                            WriteJson(ctx, new { error = $"'{target}' isimli oyuncu zaten ölmüş/hayalet modunda!" });
                            return;
                        }


                        // ========================================================
                        // BÜYÜ ZAMANI (GOD MODE V2 - TEMİZLENMİŞ)
                        // ========================================================
                        switch (action)
                        {
                            case "smite":
                                MethodInfo dieMethod = physicalPlayer.GetType().GetMethod("Die", Type.EmptyTypes);
                                dieMethod?.Invoke(physicalPlayer, null);
                                WriteJson(ctx, new { success = true, message = $"'{target}' isimli oyuncuya yıldırım çarptı (Tek yedi)!" });
                                break;

                            case "heal":
                                MethodInfo addBloodMethod = physicalPlayer.GetType().GetMethod("AddBlood", new Type[] { typeof(int) });
                                addBloodMethod?.Invoke(physicalPlayer, new object[] { amount });
                                WriteJson(ctx, new { success = true, message = $"'{target}' canı {amount} yenilendi." });
                                break;

                            case "dander":
                                MethodInfo setDanderMethod = physicalPlayer.GetType().GetMethod("SetDander", new Type[] { typeof(int) });
                                setDanderMethod?.Invoke(physicalPlayer, new object[] { 200 });
                                WriteJson(ctx, new { success = true, message = $"'{target}' öfkesi (POW) fullendi." });
                                break;

                            case "dondur":
                                MethodInfo addDelayMethod = physicalPlayer.GetType().GetMethod("AddDelay", new Type[] { typeof(int) });
                                if (addDelayMethod != null)
                                {
                                    addDelayMethod.Invoke(physicalPlayer, new object[] { 4000 });
                                }
                                else
                                {
                                    PropertyInfo delayProp = physicalPlayer.GetType().GetProperty("Delay");
                                    if (delayProp != null)
                                    {
                                        int currentDelay = (int)delayProp.GetValue(physicalPlayer, null);
                                        delayProp.SetValue(physicalPlayer, currentDelay + 4000, null);
                                    }
                                }
                                WriteJson(ctx, new { success = true, message = $"'{target}' donduruldu! (Sırası çalındı, oynayamaz)." });
                                break;

                            case "bomba":
                                PropertyInfo bloodProp = physicalPlayer.GetType().GetProperty("Blood");
                                int currentBlood = (int)(bloodProp?.GetValue(physicalPlayer, null) ?? 1000);

                                MethodInfo addBombaMethod = physicalPlayer.GetType().GetMethod("AddBlood", new Type[] { typeof(int) });
                                addBombaMethod?.Invoke(physicalPlayer, new object[] { -(currentBlood - 1) });

                                WriteJson(ctx, new { success = true, message = $"'{target}' kafasına roket yedi! Sadece 1 HP'si kaldı." });
                                break;

                            default:
                                ctx.Response.StatusCode = 400;
                                WriteJson(ctx, new { error = "Bilinmeyen eylem (action) tipi." });
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("GodPower API Hatasi: ", ex);
                        ctx.Response.StatusCode = 500;
                        WriteJson(ctx, new { error = "Sistem hatasi (Oyun Motoru): " + (ex.InnerException?.Message ?? ex.Message) });
                    }
                    return;
                }

                // Eşleşmeyen rotalar için 404
                ctx.Response.StatusCode = 404;
                WriteJson(ctx, new { error = "Endpoint bulunamadı" });
            }
            catch (Exception ex)
            {
                ctx.Response.StatusCode = 500;
                WriteJson(ctx, new { error = ex.Message });
            }
        }

        private static bool IsAuthorized(HttpListenerRequest req)
        {
            var headerKey = req.Headers["X-API-Key"];
            return headerKey == ADMIN_KEY;
        }

        private static void WriteJson(HttpListenerContext ctx, object obj)
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.None);
            byte[] buf = Encoding.UTF8.GetBytes(json);
            ctx.Response.ContentEncoding = Encoding.UTF8;
            ctx.Response.ContentType = "application/json; charset=utf-8";
            ctx.Response.OutputStream.Write(buf, 0, buf.Length);
            ctx.Response.OutputStream.Flush();
            ctx.Response.Close();
        }

        private static JObject ReadJsonBodyJ(HttpListenerRequest req)
        {
            using (var reader = new System.IO.StreamReader(req.InputStream, req.ContentEncoding))
            {
                string body = reader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(body)) return new JObject();
                return JObject.Parse(body);
            }
        }

        private static void SendAllTCP(GSPacketIn pkt)
        {
            foreach (var pl in WorldMgr.GetAllPlayers())
                pl.SendTCP(pkt);
        }

        private static void SendAllText(string msg)
        {
            foreach (var pl in WorldMgr.GetAllPlayers())
                pl.SendMessage(msg);
        }


        private static void TryStaticBool(string typeFullName, string methodName, object[] args = null)
        {
            try
            {
                var t = Type.GetType(typeFullName);
                var m = t?.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
                m?.Invoke(null, args ?? Array.Empty<object>());
            }
            catch { }
        }
    }
}