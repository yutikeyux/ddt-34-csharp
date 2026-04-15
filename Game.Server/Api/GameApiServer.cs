using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Server;
using Game.Server.GameObjects;
using Game.Server.Games;
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
        public static int DailyMoneyLimit = 16000;
        // Kişiye özel limitler: nick → limit (0 = global limite dön)
        public static Dictionary<string, int> PlayerCustomLimits =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
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

            // ← BURAYA EKLE
            int savedLimit;
            if (int.TryParse(System.Configuration.ConfigurationManager.AppSettings["DailyMoneyLimit"], out savedLimit) && savedLimit > 0)
                DailyMoneyLimit = savedLimit;

            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://*:{p}/");
            _listener.Start();
            _running = true;
            Console.WriteLine($"[API] {p} portunda dinleniyor...");
            System.Threading.Tasks.Task.Run(Loop);
            // Özel oyuncu limitlerini yükle
            try
            {
                string plFile = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(
                        System.Reflection.Assembly.GetExecutingAssembly().Location),
                    "player_limits.json");
                if (System.IO.File.Exists(plFile))
                {
                    var obj = Newtonsoft.Json.Linq.JObject.Parse(
                                  System.IO.File.ReadAllText(plFile));
                    foreach (var kv in obj)
                        if (int.TryParse(kv.Value?.ToString(), out int lv) && lv > 0)
                            PlayerCustomLimits[kv.Key] = lv;
                }
            }
            catch { }
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

                Console.WriteLine($"[API] Incoming {req.HttpMethod} {path}, X-API-Key={req.Headers["X-API-Key"] ?? "<discord botu main.py erişilemedi>"}");

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
                    // Oda → oyuncu haritası: room.RoomId / room.MapId zaten çalışıyor
                    var nickToRoom = new Dictionary<string, (int roomId, int mapId)>(StringComparer.OrdinalIgnoreCase);
                    try
                    {
                        foreach (var room in RoomMgr.GetAllUsingRoom())
                        {
                            try
                            {
                                foreach (var rp in room.GetPlayers())
                                    if (rp?.PlayerCharacter?.NickName != null)
                                        nickToRoom[rp.PlayerCharacter.NickName] = (room.RoomId, room.MapId);
                            }
                            catch { }
                        }
                    }
                    catch { }

                    var players = WorldMgr.GetAllPlayers();
                    var list = new List<object>();
                    foreach (var p in players)
                    {
                        var c = p.PlayerCharacter;
                        int roomId = 0, mapId = 0;
                        if (nickToRoom.TryGetValue(c.NickName, out var loc)) { roomId = loc.roomId; mapId = loc.mapId; }
                        list.Add(new { Nickname = c.NickName, Level = c.Grade, FightPower = c.FightPower, MapId = mapId, RoomId = roomId });
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
                        int pLimit = PlayerCustomLimits.TryGetValue(nick, out int pcl) ? pcl : DailyMoneyLimit;
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
                            DailyMoneyLimit = pLimit,
                            DailyMoneyUsed = c.DailyMoneyUsed,
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
                        int pLimitOff = PlayerCustomLimits.TryGetValue(nick, out int pclOff) ? pclOff : DailyMoneyLimit;
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
                            DailyMoneyLimit = pLimitOff,
                            DailyMoneyUsed = info.DailyMoneyUsed,
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
                        // FİZİKSEL KARAKTER BULMA — GameMgr.GetAllGame() ile direkt erişim
                        // ========================================================
                        // ProxyGame'i tamamen atlıyoruz. GameMgr tüm aktif oyunları bilir.
                        // game.GetAllPlayers() → IGamePlayer[] → PlayerDetail.PlayerCharacter.NickName
                        // ========================================================
                        object physicalPlayer = null;

                        foreach (var baseGame in GameMgr.GetAllGame())
                        {
                            if (physicalPlayer != null) break;
                            try
                            {
                                foreach (var fp in baseGame.GetAllPlayers())
                                {
                                    if (fp == null) continue;
                                    try
                                    {
                                        string nick = fp.PlayerDetail?.PlayerCharacter?.NickName;
                                        if (string.Equals(nick, target, StringComparison.OrdinalIgnoreCase))
                                        { physicalPlayer = fp; break; }
                                    }
                                    catch { }
                                }
                            }
                            catch { }
                        }

                        if (physicalPlayer == null)
                        {
                            ctx.Response.StatusCode = 400;
                            WriteJson(ctx, new
                            {
                                error = $"'{target}' haritaya henüz ayak basmadı veya izleyici modunda.",
                                hint = "GameMgr.GetAllGame() tarandı — oyuncu aktif savaşta değil."
                            });
                            return;
                        }

                        // GameMgr.GetAllGame() direkt gerçek IGamePlayer (Game.Logic.Phy.Object.Player) veriyor.
                        // ProxyPlayer extraction'a artık gerek yok.

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
                                {
                                    MethodInfo addDelayMethod = physicalPlayer.GetType().GetMethod("AddDelay", new Type[] { typeof(int) });
                                    if (addDelayMethod != null)
                                        addDelayMethod.Invoke(physicalPlayer, new object[] { 4000 });
                                    else
                                    {
                                        PropertyInfo delayProp = physicalPlayer.GetType().GetProperty("Delay");
                                        if (delayProp != null)
                                        {
                                            int currentDelay = (int)delayProp.GetValue(physicalPlayer, null);
                                            delayProp.SetValue(physicalPlayer, currentDelay + 4000, null);
                                        }
                                    }
                                    // IsFrost = true → görsel donma efekti gönderir (Living.SendGameUpdateFrozenState)
                                    PropertyInfo isFrostDF = null;
                                    Type tDF = physicalPlayer.GetType();
                                    while (tDF != null && isFrostDF == null) { isFrostDF = tDF.GetProperty("IsFrost"); tDF = tDF.BaseType; }
                                    if (isFrostDF != null && isFrostDF.CanWrite)
                                        isFrostDF.SetValue(physicalPlayer, true, null);
                                    WriteJson(ctx, new { success = true, message = $"'{target}' donduruldu! (4sn delay + IsFrost görsel efekt)" });
                                    break;
                                }

                            case "bomba":
                                PropertyInfo bloodProp = physicalPlayer.GetType().GetProperty("Blood");
                                int currentBlood = (int)(bloodProp?.GetValue(physicalPlayer, null) ?? 1000);

                                MethodInfo addBombaMethod = physicalPlayer.GetType().GetMethod("AddBlood", new Type[] { typeof(int) });
                                addBombaMethod?.Invoke(physicalPlayer, new object[] { -(currentBlood - 1) });

                                WriteJson(ctx, new { success = true, message = $"'{target}' kafasına roket yedi! Sadece 1 HP'si kaldı." });
                                break;

                            case "hiz":
                                {
                                    // Player.SpeedMultX(int) mevcut. Normal değer 3, 2x = 6.
                                    MethodInfo speedMultX = physicalPlayer.GetType().GetMethod("SpeedMultX", new Type[] { typeof(int) });
                                    if (speedMultX != null)
                                    {
                                        speedMultX.Invoke(physicalPlayer, new object[] { 6 });
                                        WriteJson(ctx, new { success = true, message = $"'{target}' hızı 2x oldu! (SpeedMultX 6)" });
                                    }
                                    else
                                    {
                                        // Fallback: MOVE_SPEED public field (normal = 2)
                                        FieldInfo moveSpeedField = physicalPlayer.GetType().GetField("MOVE_SPEED");
                                        if (moveSpeedField != null)
                                        {
                                            moveSpeedField.SetValue(physicalPlayer, 4);
                                            WriteJson(ctx, new { success = true, message = $"'{target}' hızlandı! (MOVE_SPEED 4)" });
                                        }
                                        else WriteJson(ctx, new { success = false, message = "SpeedMultX/MOVE_SPEED bulunamadı." });
                                    }
                                    break;
                                }
                            case "tanri":
                                {
                                    // Player.AddMaxBlood(int) + AddBlood(int) mevcut.
                                    // MaxBlood'u dev yap → pratik olarak ölmez.
                                    MethodInfo addMaxBloodM = physicalPlayer.GetType().GetMethod("AddMaxBlood", new Type[] { typeof(int) });
                                    MethodInfo addBloodTanri = physicalPlayer.GetType().GetMethod("AddBlood", new Type[] { typeof(int) });
                                    if (addMaxBloodM != null && addBloodTanri != null)
                                    {
                                        addMaxBloodM.Invoke(physicalPlayer, new object[] { 9999999 });
                                        addBloodTanri.Invoke(physicalPlayer, new object[] { 9999999 });
                                        WriteJson(ctx, new { success = true, message = $"'{target}' tanrı modunda! MaxHP +9.999.999 eklendi." });
                                    }
                                    else WriteJson(ctx, new { success = false, message = "AddMaxBlood/AddBlood metodu bulunamadı." });
                                    break;
                                }
                            case "kritik":
                                {
                                    // Player.PetEffects.CritRate → Reset() içinde base.PetEffects.CritRate = 0 görülüyor.
                                    PropertyInfo petEffectsProp = null; Type tpKR = physicalPlayer.GetType();
                                    while (tpKR != null && petEffectsProp == null) { petEffectsProp = tpKR.GetProperty("PetEffects", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance); tpKR = tpKR.BaseType; }
                                    object petEffects = petEffectsProp?.GetValue(physicalPlayer, null);
                                    PropertyInfo critRateProp = petEffects?.GetType().GetProperty("CritRate");
                                    if (critRateProp != null && critRateProp.CanWrite)
                                    {
                                        critRateProp.SetValue(petEffects, 100, null);
                                        WriteJson(ctx, new { success = true, message = $"'{target}' kritik şansı %100!" });
                                    }
                                    else WriteJson(ctx, new { success = false, message = "PetEffects.CritRate bulunamadı.", petEffectsFound = petEffects != null });
                                    break;
                                }
                            case "sarhos":
                                {
                                    var BF_A = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                                    MethodInfo addDelayM2 = physicalPlayer.GetType().GetMethod("AddDelay", new Type[] { typeof(int) });
                                    // LockDirection: tüm hiyerarşide public+nonpublic field ara
                                    FieldInfo lockDirF = null;
                                    Type tLD = physicalPlayer.GetType();
                                    while (tLD != null && lockDirF == null) { lockDirF = tLD.GetField("LockDirection", BF_A); tLD = tLD.BaseType; }
                                    if (addDelayM2 != null)
                                        addDelayM2.Invoke(physicalPlayer, new object[] { 9000 });
                                    if (lockDirF != null)
                                        lockDirF.SetValue(physicalPlayer, true);
                                    WriteJson(ctx, new { success = true, message = $"'{target}' sarhoş! (9sn delay + yön kilidi)" });
                                    break;
                                }
                            case "tersine":
                                {
                                    var BF_A = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                                    FieldInfo lockDirF2 = null;
                                    Type tLD2 = physicalPlayer.GetType();
                                    while (tLD2 != null && lockDirF2 == null) { lockDirF2 = tLD2.GetField("LockDirection", BF_A); tLD2 = tLD2.BaseType; }
                                    if (lockDirF2 != null)
                                    {
                                        bool cur = (bool)lockDirF2.GetValue(physicalPlayer);
                                        lockDirF2.SetValue(physicalPlayer, !cur);
                                        string durum = (!cur) ? "kilitlendi (hareket edemez)" : "serbest bırakıldı";
                                        WriteJson(ctx, new { success = true, message = $"'{target}' yön {durum}!" });
                                    }
                                    else WriteJson(ctx, new { success = false, message = "LockDirection field bulunamadı." });
                                    break;
                                }
                            case "kor":
                                {
                                    // AddRemoveEnergy(-99999) → istemciye "energy" paketi gönderir, setter'dan güvenli.
                                    MethodInfo addRemEnergyM = null;
                                    Type tKor = physicalPlayer.GetType();
                                    while (tKor != null && addRemEnergyM == null)
                                    { addRemEnergyM = tKor.GetMethod("AddRemoveEnergy", new Type[] { typeof(int) }); tKor = tKor.BaseType; }
                                    if (addRemEnergyM != null)
                                        addRemEnergyM.Invoke(physicalPlayer, new object[] { -99999 });
                                    else
                                    {
                                        // Fallback: Energy property setter
                                        PropertyInfo energyProp = physicalPlayer.GetType().GetProperty("Energy");
                                        if (energyProp != null && energyProp.CanWrite)
                                            energyProp.SetValue(physicalPlayer, 0, null);
                                    }
                                    FieldInfo canFlyField = physicalPlayer.GetType().GetField("CanFly");
                                    if (canFlyField != null)
                                        canFlyField.SetValue(physicalPlayer, false);
                                    WriteJson(ctx, new { success = true, message = $"'{target}' enerji sıfırlandı + uçuş engellendi! (AddRemoveEnergy)" });
                                    break;
                                }
                            case "buyut":
                                {
                                    // Scale yok. SpeedMultX(6) + AddMaxBlood(50000) → güçlü/büyük hissi.
                                    MethodInfo speedMX2 = physicalPlayer.GetType().GetMethod("SpeedMultX", new Type[] { typeof(int) });
                                    MethodInfo addMaxBM2 = physicalPlayer.GetType().GetMethod("AddMaxBlood", new Type[] { typeof(int) });
                                    MethodInfo addBloodBM = physicalPlayer.GetType().GetMethod("AddBlood", new Type[] { typeof(int) });
                                    speedMX2?.Invoke(physicalPlayer, new object[] { 6 });
                                    addMaxBM2?.Invoke(physicalPlayer, new object[] { 50000 });
                                    addBloodBM?.Invoke(physicalPlayer, new object[] { 50000 });
                                    WriteJson(ctx, new { success = true, message = $"'{target}' büyüdü! (2x hız + 50k HP buff)" });
                                    break;
                                }
                            case "kucult":
                                {
                                    // MOVE_SPEED static field — per-instance set etmek işe yaramaz.
                                    // SpeedMultX(1) → istemciye "speedX" paketi gönderir (en yavaş = 1).
                                    MethodInfo speedMXK = null;
                                    Type tKU = physicalPlayer.GetType();
                                    while (tKU != null && speedMXK == null)
                                    { speedMXK = tKU.GetMethod("SpeedMultX", new Type[] { typeof(int) }); tKU = tKU.BaseType; }
                                    speedMXK?.Invoke(physicalPlayer, new object[] { 1 });

                                    // Enerji de sıfırla (AddRemoveEnergy tercihli)
                                    MethodInfo addRemEK = null;
                                    Type tKUe = physicalPlayer.GetType();
                                    while (tKUe != null && addRemEK == null)
                                    { addRemEK = tKUe.GetMethod("AddRemoveEnergy", new Type[] { typeof(int) }); tKUe = tKUe.BaseType; }
                                    if (addRemEK != null)
                                        addRemEK.Invoke(physicalPlayer, new object[] { -99999 });

                                    WriteJson(ctx, new { success = true, message = $"'{target}' küçüldü! (SpeedMultX 1 — en yavaş, enerji sıfır)" });
                                    break;
                                }
                            case "para_ver":
                                {
                                    // gp.AddMoney() zaten mevcut — reflection'a gerek yok
                                    gp.AddMoney(amount);
                                    gp.SendMessage($"[Yönetim] Hesabına {amount} kupon eklendi!");
                                    WriteJson(ctx, new { success = true, message = $"'{target}' hesabına {amount} kupon eklendi!" });
                                    break;
                                }
                            case "mail_item":
                                {
                                    int itemId = d["ItemId"]?.ToObject<int?>() ?? 0;
                                    if (itemId == 0) { WriteJson(ctx, new { success = false, message = "ItemId parametresi gerekli." }); break; }
                                    var pc = gp.PlayerCharacter;
                                    new PlayerBussiness().SendMailAndItem(
                                        "Yönetim Hediyesi", "God Panel üzerinden gönderildi.",
                                        pc.ID, itemId, 1, 0, 0, 0, 0, 0, 0, 0, 0, true
                                    );
                                    gp.SendMessage($"[Yönetim] Posta kutuna hediye gönderildi. Kontrol et!");
                                    WriteJson(ctx, new { success = true, message = $"'{target}' hesabına item ({itemId}) posta ile gönderildi!" });
                                    break;
                                }
                            case "isinla":
                                {
                                    // Living.BoltMove(int x, int y, int delay) — mevcut ve doğrulanmış.
                                    // Oyuncuyu aktif harita içinde X/Y koordinatına ışınlar.
                                    int bx = d["X"]?.ToObject<int?>() ?? d["MapId"]?.ToObject<int?>() ?? 500;
                                    int by = d["Y"]?.ToObject<int?>() ?? d["RoomId"]?.ToObject<int?>() ?? 300;
                                    MethodInfo boltMoveM = physicalPlayer.GetType().GetMethod(
                                        "BoltMove", new Type[] { typeof(int), typeof(int), typeof(int) });
                                    if (boltMoveM != null)
                                    {
                                        boltMoveM.Invoke(physicalPlayer, new object[] { bx, by, 0 });
                                        WriteJson(ctx, new { success = true, message = $"'{target}' X:{bx} Y:{by} konumuna ışınlandı! (BoltMove)" });
                                    }
                                    else WriteJson(ctx, new { success = false, message = "BoltMove method bulunamadı." });
                                    break;
                                }

                            case "dokunulmaz":
                                {
                                    // PetEffectInfo.ActiveNoDamage = true → oyuncu sıfır hasar alır
                                    PropertyInfo pepD = null; Type tpD = physicalPlayer.GetType();
                                    while (tpD != null && pepD == null) { pepD = tpD.GetProperty("PetEffects", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance); tpD = tpD.BaseType; }
                                    object peD = pepD?.GetValue(physicalPlayer, null);
                                    PropertyInfo noDmgP = peD?.GetType().GetProperty("ActiveNoDamage");
                                    if (noDmgP != null && noDmgP.CanWrite)
                                    {
                                        noDmgP.SetValue(peD, true, null);
                                        WriteJson(ctx, new { success = true, message = $"'{target}' dokunulmaz! Hiç hasar almıyor. (ActiveNoDamage=true)" });
                                    }
                                    else WriteJson(ctx, new { success = false, message = "PetEffects.ActiveNoDamage bulunamadı.", petEffectsFound = peD != null });
                                    break;
                                }

                            case "hasar_yansit":
                                {
                                    // PetEffectInfo.ReboundDamage = 100 → gelen hasarın %100'ü geri döner (public field)
                                    PropertyInfo pepR = null; Type tpR = physicalPlayer.GetType();
                                    while (tpR != null && pepR == null) { pepR = tpR.GetProperty("PetEffects", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance); tpR = tpR.BaseType; }
                                    object peR = pepR?.GetValue(physicalPlayer, null);
                                    FieldInfo rbField = peR?.GetType().GetField("ReboundDamage");
                                    if (rbField != null)
                                    {
                                        rbField.SetValue(peR, 100);
                                        WriteJson(ctx, new { success = true, message = $"'{target}' hasar kalkanı aktif! Hasar %100 geri yansıtılıyor." });
                                    }
                                    else WriteJson(ctx, new { success = false, message = "PetEffects.ReboundDamage bulunamadı.", petEffectsFound = peR != null });
                                    break;
                                }

                            case "kilitle":
                                {
                                    // PetEffectInfo.StopMoving = true → oyuncu hareket edemez
                                    PropertyInfo pepK = null; Type tpK = physicalPlayer.GetType();
                                    while (tpK != null && pepK == null) { pepK = tpK.GetProperty("PetEffects", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance); tpK = tpK.BaseType; }
                                    object peK = pepK?.GetValue(physicalPlayer, null);
                                    PropertyInfo stopMP = peK?.GetType().GetProperty("StopMoving");
                                    if (stopMP != null && stopMP.CanWrite)
                                    {
                                        stopMP.SetValue(peK, true, null);
                                        WriteJson(ctx, new { success = true, message = $"'{target}' kilitlendi! Hareket edemez. (StopMoving=true)" });
                                    }
                                    else WriteJson(ctx, new { success = false, message = "PetEffects.StopMoving bulunamadı.", petEffectsFound = peK != null });
                                    break;
                                }

                            case "gizle":
                                {
                                    // Living.IsHide toggle → istemciye sync'li (setter içinde SendGameUpdateHideState çağırır)
                                    PropertyInfo isHideProp = null;
                                    Type tG = physicalPlayer.GetType();
                                    while (tG != null && isHideProp == null) { isHideProp = tG.GetProperty("IsHide"); tG = tG.BaseType; }
                                    if (isHideProp != null && isHideProp.CanWrite)
                                    {
                                        bool curHide = (bool)(isHideProp.GetValue(physicalPlayer, null) ?? false);
                                        isHideProp.SetValue(physicalPlayer, !curHide, null);
                                        string durumG = !curHide ? "görünmez oldu" : "tekrar görünür oldu";
                                        WriteJson(ctx, new { success = true, message = $"'{target}' {durumG}!" });
                                    }
                                    else WriteJson(ctx, new { success = false, message = "IsHide property bulunamadı." });
                                    break;
                                }

                            case "guclen":
                                {
                                    // PetEffectInfo: BonusBaseDamage=5000, BonusAttack=500, DamagePercent=200
                                    PropertyInfo pepGC = null; Type tpGC = physicalPlayer.GetType();
                                    while (tpGC != null && pepGC == null) { pepGC = tpGC.GetProperty("PetEffects", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance); tpGC = tpGC.BaseType; }
                                    object peGC = pepGC?.GetValue(physicalPlayer, null);
                                    if (peGC != null)
                                    {
                                        Type peType = peGC.GetType();
                                        PropertyInfo bdP = peType.GetProperty("BonusBaseDamage");
                                        PropertyInfo atkP = peType.GetProperty("BonusAttack");
                                        PropertyInfo dmgP = peType.GetProperty("DamagePercent");
                                        if (bdP != null && bdP.CanWrite) bdP.SetValue(peGC, 5000, null);
                                        if (atkP != null && atkP.CanWrite) atkP.SetValue(peGC, 500, null);
                                        if (dmgP != null && dmgP.CanWrite) dmgP.SetValue(peGC, 200, null);
                                        WriteJson(ctx, new { success = true, message = $"'{target}' güçlendi! (+5000 hasar, +500 atak, %200 hasar çarpanı)" });
                                    }
                                    else WriteJson(ctx, new { success = false, message = "PetEffects bulunamadı.", playerType = physicalPlayer.GetType().FullName });
                                    break;
                                }

                            case "savunmasiz":
                                {
                                    // PetEffectInfo.ReduceDefendValue = 9999 → hedefin tüm savunması erimeye
                                    PropertyInfo pepSV = null; Type tpSV = physicalPlayer.GetType();
                                    while (tpSV != null && pepSV == null) { pepSV = tpSV.GetProperty("PetEffects", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance); tpSV = tpSV.BaseType; }
                                    object peSV = pepSV?.GetValue(physicalPlayer, null);
                                    PropertyInfo rdvP = peSV?.GetType().GetProperty("ReduceDefendValue");
                                    if (rdvP != null && rdvP.CanWrite)
                                    {
                                        rdvP.SetValue(peSV, 9999, null);
                                        WriteJson(ctx, new { success = true, message = $"'{target}' savunmasız! Tüm zırhı eridi. (ReduceDefendValue=9999)" });
                                    }
                                    else WriteJson(ctx, new { success = false, message = "PetEffects.ReduceDefendValue bulunamadı.", petEffectsFound = peSV != null });
                                    break;
                                }

                            case "reset_efekt":
                                {
                                    // SetupPetEffect() → tüm PetEffectInfo sıfırlanır (Living'de tanımlı)
                                    // Ek olarak: IsHide=false, IsFrost=false
                                    MethodInfo setupM = null;
                                    Type tRE = physicalPlayer.GetType();
                                    while (tRE != null && setupM == null) { setupM = tRE.GetMethod("SetupPetEffect"); tRE = tRE.BaseType; }
                                    setupM?.Invoke(physicalPlayer, null);

                                    PropertyInfo isHideRE = null, isFrostRE = null;
                                    Type tRE2 = physicalPlayer.GetType();
                                    while (tRE2 != null && (isHideRE == null || isFrostRE == null))
                                    {
                                        if (isHideRE == null) isHideRE = tRE2.GetProperty("IsHide");
                                        if (isFrostRE == null) isFrostRE = tRE2.GetProperty("IsFrost");
                                        tRE2 = tRE2.BaseType;
                                    }
                                    if (isHideRE != null && isHideRE.CanWrite) isHideRE.SetValue(physicalPlayer, false, null);
                                    if (isFrostRE != null && isFrostRE.CanWrite) isFrostRE.SetValue(physicalPlayer, false, null);

                                    WriteJson(ctx, new { success = true, message = $"'{target}' tüm efektler sıfırlandı!" });
                                    break;
                                }

                            case "tur_uzat":
                                {
                                    // Oyuncunun mevcut turuna ek süre ekle.
                                    // Strateji: AddDelay negatif değer → tur sırasını öne çeker (daha erken oynarsın)
                                    // Gerçek tur süresi için game üzerindeki timer/field'ı ara.
                                    int extraMs = (amount > 0 ? amount : 10) * 1000; // amount saniye, ms'e çevir

                                    // 1. game nesnesinde TurnTime / m_turnTime / TurnLeftTime gibi field/prop ara
                                    bool turExtended = false;
                                    string[] turnTimeNames = { "TurnTime", "m_turnTime", "TurnLeftTime", "RoundTime",
                                                                "m_roundTime", "TurnTimeLeft", "CurrentTurnTime", "TimerInterval" };
                                    var BF_TT = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                                    foreach (string ttn in turnTimeNames)
                                    {
                                        try
                                        {
                                            PropertyInfo ttp = null; Type tttCheck = game.GetType();
                                            while (tttCheck != null && ttp == null) { ttp = tttCheck.GetProperty(ttn, BF_TT); tttCheck = tttCheck.BaseType; }
                                            if (ttp != null && ttp.CanWrite && ttp.CanRead)
                                            {
                                                object cur = ttp.GetValue(game, null);
                                                if (cur is int ci) { ttp.SetValue(game, ci + extraMs, null); turExtended = true; break; }
                                                if (cur is long cl) { ttp.SetValue(game, cl + extraMs, null); turExtended = true; break; }
                                            }
                                            FieldInfo ttf = null; Type tttf = game.GetType();
                                            while (tttf != null && ttf == null) { ttf = tttf.GetField(ttn, BF_TT); tttf = tttf.BaseType; }
                                            if (ttf != null)
                                            {
                                                object cur = ttf.GetValue(game);
                                                if (cur is int ci) { ttf.SetValue(game, ci + extraMs); turExtended = true; break; }
                                                if (cur is long cl) { ttf.SetValue(game, cl + extraMs); turExtended = true; break; }
                                            }
                                        }
                                        catch { }
                                    }

                                    // 2. Bulamazsak AddDelay(-extraMs) ile oyuncunun sırasını öne çek
                                    if (!turExtended)
                                    {
                                        MethodInfo addDelayTur = physicalPlayer.GetType().GetMethod("AddDelay", new Type[] { typeof(int) });
                                        if (addDelayTur != null)
                                        {
                                            addDelayTur.Invoke(physicalPlayer, new object[] { -extraMs });
                                            WriteJson(ctx, new { success = true, message = $"'{target}' turu {amount}sn öne çekildi! (AddDelay fallback — TurnTime field bulunamadı)", note = "Gerçek süre uzatma değil, sıra öne alma." });
                                        }
                                        else WriteJson(ctx, new { success = false, message = "TurnTime ve AddDelay bulunamadı." });
                                    }
                                    else
                                        WriteJson(ctx, new { success = true, message = $"'{target}' turu {amount}sn uzatıldı!" });
                                    break;
                                }

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

                // ── GET /api/game/dailylimit?nickname=XXX ─────────────────────
                if (path == "/api/game/dailylimit" && req.HttpMethod == "GET")
                {
                    string nick = req.QueryString["nickname"] ?? "";
                    if (string.IsNullOrWhiteSpace(nick))
                    {
                        ctx.Response.StatusCode = 400;
                        WriteJson(ctx, new { success = false, error = "nickname parametresi eksik" });
                        return;
                    }

                    // Online → memory'den al
                    var p = WorldMgr.GetClientByPlayerNickName(nick);
                    int playerLimit = PlayerCustomLimits.TryGetValue(nick, out int cl) ? cl : DailyMoneyLimit;
                    if (p != null)
                    {
                        var c = p.PlayerCharacter;
                        WriteJson(ctx, new
                        {
                            success = true,
                            nickname = c.NickName,
                            dailyUsed = c.DailyMoneyUsed,
                            limit = playerLimit,
                            isCustomLimit = PlayerCustomLimits.ContainsKey(nick),
                            online = true
                        });
                        return;
                    }

                    // Offline → DB'den al
                    using (var pb = new PlayerBussiness())
                    {
                        var info = pb.GetUserSingleByNickName(nick);
                        if (info == null)
                        {
                            ctx.Response.StatusCode = 404;
                            WriteJson(ctx, new { success = false, error = "Oyuncu bulunamadı" });
                            return;
                        }
                        WriteJson(ctx, new
                        {
                            success = true,
                            nickname = info.NickName,
                            dailyUsed = info.DailyMoneyUsed,
                            limit = playerLimit,
                            isCustomLimit = PlayerCustomLimits.ContainsKey(nick),
                            online = false
                        });
                        return;
                    }
                }

                if (path == "/api/game/setplayerlimit" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);
                    string nick = d["Nickname"]?.ToString() ?? "";
                    int newLimit = d["Limit"]?.ToObject<int?>() ?? 0;

                    if (newLimit <= 0)
                        PlayerCustomLimits.Remove(nick);   // 0 = özel limiti kaldır
                    else
                        PlayerCustomLimits[nick] = newLimit;

                    // Kalıcı kaydet
                    try
                    {
                        string plFile = System.IO.Path.Combine(
                            System.IO.Path.GetDirectoryName(
                                System.Reflection.Assembly.GetExecutingAssembly().Location),
                            "player_limits.json");
                        var jo = new Newtonsoft.Json.Linq.JObject();
                        foreach (var kv in PlayerCustomLimits)
                            jo[kv.Key] = kv.Value;
                        System.IO.File.WriteAllText(plFile, jo.ToString());
                    }
                    catch { }

                    // Online oyuncuya bildir
                    var p = WorldMgr.GetClientByPlayerNickName(nick);
                    if (p != null)
                    {
                        string msg = newLimit > 0
                            ? $"⚙️ Günlük kupon limitiniz yönetici tarafından {newLimit:N0} olarak ayarlandı."
                            : $"⚙️ Günlük kupon limitiniz sistem varsayılanına ({DailyMoneyLimit:N0}) döndürüldü.";
                        p.Out.SendMessage((eMessageType)5, msg);
                    }

                    WriteJson(ctx, new { success = true, nickname = nick, limit = newLimit });
                    return;
                }

                // ── POST /api/game/resetdailylimit ────────────────────────────
                if (path == "/api/game/resetdailylimit" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);
                    string nick = d["Nickname"]?.ToString() ?? "";
                    if (string.IsNullOrWhiteSpace(nick))
                    {
                        ctx.Response.StatusCode = 400;
                        WriteJson(ctx, new { success = false, error = "nickname eksik" });
                        return;
                    }

                    bool memoryReset = false;

                    // 1) Online ise memory'i sıfırla + oyuncuya bildirim gönder
                    var p = WorldMgr.GetClientByPlayerNickName(nick);
                    if (p != null)
                    {
                        p.PlayerCharacter.DailyMoneyUsed = 0;
                        memoryReset = true;
                        p.Out.SendMessage(
                            (eMessageType)5,
                            $"✨ Günlük kupon limitiniz yönetici tarafından sıfırlandı! Tekrar {DailyMoneyLimit:N0} kupona kadar harcama yapabilirsiniz."
                        );
                    }

                    // 2) DB'yi güncelle (online/offline her ikisinde de çalışır)
                    try
                    {
                        string connStr = System.Configuration.ConfigurationManager
                                         .AppSettings["conString"];

                        using (var conn = new System.Data.SqlClient.SqlConnection(connStr))
                        {
                            conn.Open();
                            var cmd = new System.Data.SqlClient.SqlCommand(
                                "UPDATE Sys_Users_Detail SET DailyMoneyUsed = 0 WHERE NickName = @nick",
                                conn
                            );
                            cmd.Parameters.AddWithValue("@nick", nick);
                            cmd.ExecuteNonQuery();
                        }
                        WriteJson(ctx, new { success = true, nickname = nick, memoryReset });
                    }
                    catch (Exception ex)
                    {
                        log.Error("[API] resetdailylimit DB hatası: ", ex);
                        WriteJson(ctx, new { success = false, error = ex.Message, memoryReset });
                    }
                    return;
                }

                // ── Tüm oyuncuların limitini sıfırla (memory + DB) ───────────────
                if (path == "/api/game/resetalldailylimits" && req.HttpMethod == "POST")
                {
                    // 1) Online oyuncuların memory'ini sıfırla
                    int memoryCount = 0;
                    foreach (var onlinePlayer in WorldMgr.GetAllPlayers())
                    {
                        try
                        {
                            if (onlinePlayer?.PlayerCharacter != null)
                            {
                                onlinePlayer.PlayerCharacter.DailyMoneyUsed = 0;
                                onlinePlayer.Out.SendMessage(
                                    (eMessageType)5,
                                    $"✨ Günlük kupon limitleri sıfırlandı! Tekrar {DailyMoneyLimit:N0} kupona kadar harcayabilirsiniz."
                                );
                                memoryCount++;
                            }
                        }
                        catch { }
                    }

                    // 2) DB'de toplu güncelle
                    int dbCount = 0;
                    try
                    {
                        string connStr = System.Configuration.ConfigurationManager.AppSettings["conString"];
                        using (var conn = new System.Data.SqlClient.SqlConnection(connStr))
                        {
                            conn.Open();
                            var cmd = new System.Data.SqlClient.SqlCommand(
                                "UPDATE Sys_Users_Detail SET DailyMoneyUsed = 0", conn);
                            dbCount = cmd.ExecuteNonQuery();
                        }
                        WriteJson(ctx, new { success = true, memoryReset = memoryCount, dbReset = dbCount });
                    }
                    catch (Exception ex)
                    {
                        log.Error("[API] resetalldailylimits DB hatası: ", ex);
                        WriteJson(ctx, new { success = false, error = ex.Message, memoryReset = memoryCount });
                    }
                    return;
                }

                // ── Lobi / Menü oyuncusu ışınlama ───────────────────────────────
                if (path == "/api/game/player/teleport" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);
                    string tpNick = d?["Nickname"]?.ToString();
                    int tpRoomId = d?["RoomId"]?.ToObject<int?>() ?? 0;

                    if (string.IsNullOrEmpty(tpNick))
                    { ctx.Response.StatusCode = 400; WriteJson(ctx, new { success = false, error = "Nickname gerekli" }); return; }

                    // Typed GamePlayer — reflection yok
                    GamePlayer tpGamePlayer = null;
                    foreach (var pl in WorldMgr.GetAllPlayers())
                    {
                        if (string.Equals(pl?.PlayerCharacter?.NickName, tpNick, StringComparison.OrdinalIgnoreCase))
                        { tpGamePlayer = pl; break; }
                    }

                    if (tpGamePlayer == null)
                    { WriteJson(ctx, new { success = false, error = $"'{tpNick}' online değil veya bulunamadı." }); return; }

                    // Hedef odayı RoomMgr'den bul
                    BaseRoom targetRoom = null;
                    foreach (var r in RoomMgr.GetAllUsingRoom())
                        if (r.RoomId == tpRoomId) { targetRoom = r; break; }

                    if (targetRoom == null)
                    { WriteJson(ctx, new { success = false, error = $"Oda {tpRoomId} aktif değil veya bulunamadı. Sadece açık odalar desteklenir." }); return; }

                    // Eğer zaten bu odadaysa işlem yapma
                    if (tpGamePlayer.CurrentRoom == targetRoom)
                    { WriteJson(ctx, new { success = false, error = $"'{tpNick}' zaten bu odada." }); return; }

                    // Mevcut odadan çıkar (varsa)
                    if (tpGamePlayer.CurrentRoom != null)
                        tpGamePlayer.CurrentRoom.RemovePlayerUnsafe(tpGamePlayer);

                    // ── EnterRoomAction ile birebir aynı akış ──────────────────────
                    // 1) WaitingRoom'dan çıkar
                    try { RoomMgr.WaitingRoom.RemovePlayer(tpGamePlayer); } catch { }

                    // 2) Client'a "odaya giriş başarılı" + "oda ekranını aç" paketleri
                    tpGamePlayer.Out.SendRoomLoginResult(true);
                    tpGamePlayer.Out.SendRoomCreate(targetRoom);

                    // 3) Odaya ekle (slot atar, player listesini gönderir, CurrentRoom set eder)
                    bool added = targetRoom.AddPlayerUnsafe(tpGamePlayer);

                    if (added)
                    {
                        // 4) Oda ayarlarını gönder
                        tpGamePlayer.Out.SendGameRoomSetupChange(targetRoom);

                        // 5) Oyun devam ediyorsa → izleyici olarak oyuna sok
                        string gameStatus = "odada (oyun yok)";
                        if (targetRoom.IsPlaying && targetRoom.Game != null)
                        {
                            try
                            {
                                // İzleyici olarak işaretle — BaseGame.AddPlayer IsViewer=true ise
                                // TurnQueue'ya EKLEMEZ, haritaya EKLEMEZ, ama m_players'a ekler
                                // → SendToAll paketlerini alır, sıra gelmez, oyun donmaz
                                var gameObj = targetRoom.Game;
                                var bgame = gameObj as BaseGame; // ProxyGame ise null döner
                                if (bgame == null)
                                {
                                    // ── PVP VIEWER — FightServer'a gönder ──
                                    if (gameObj != null && gameObj.GetType().Name == "ProxyGame")
                                    {
                                        try
                                        {
                                            tpGamePlayer.IsViewer = true;

                                            // FightServerConnector'ı ProxyGame'den al
                                            var fiConn = gameObj.GetType().GetField("fightServerConnector_0",
                                                BindingFlags.NonPublic | BindingFlags.Instance);
                                            var fightConn = fiConn?.GetValue(gameObj);
                                            if (fightConn == null) throw new Exception("FightServerConnector bulunamadı");

                                            // Game ID — backing field üzerinden al (expression-bodied property reflection'da sorun çıkarıyor)
                                            int gameId = 0;
                                            var fiGameId = typeof(AbstractGame).GetField("int_0",
                                                BindingFlags.NonPublic | BindingFlags.Instance);
                                            if (fiGameId != null)
                                            {
                                                gameId = (int)fiGameId.GetValue(gameObj);
                                            }
                                            else
                                            {
                                                // Yedek: property üzerinden dene
                                                var piId = gameObj.GetType().GetProperty("Id");
                                                if (piId != null) gameId = (int)piId.GetValue(gameObj);
                                            }

                                            // code=90 paketi: viewer bilgilerini FightServer'a gönder
                                            var pkg90 = new GSPacketIn(90, gameId);
                                            var pc = tpGamePlayer.PlayerCharacter;
                                            pkg90.WriteInt(pc.ID);
                                            pkg90.WriteString(pc.NickName ?? "");
                                            pkg90.WriteBoolean(pc.Sex);
                                            pkg90.WriteInt(pc.Hide);
                                            pkg90.WriteString(pc.Style ?? "");
                                            pkg90.WriteString(pc.Colors ?? "");
                                            pkg90.WriteString(pc.Skin ?? "");
                                            pkg90.WriteInt(pc.Grade);
                                            pkg90.WriteInt(pc.Repute);
                                            pkg90.WriteInt(pc.ConsortiaID);
                                            pkg90.WriteString(pc.ConsortiaName ?? "");
                                            pkg90.WriteInt(pc.ConsortiaLevel);
                                            pkg90.WriteInt(pc.ConsortiaRepute);
                                            pkg90.WriteBoolean(pc.IsShowConsortia);
                                            pkg90.WriteInt(pc.badgeID);
                                            pkg90.WriteString(pc.Honor ?? "");
                                            pkg90.WriteInt(pc.AchievementPoint);
                                            pkg90.WriteInt(pc.FightPower);
                                            pkg90.WriteInt(pc.Nimbus);
                                            pkg90.WriteInt(pc.Win);
                                            pkg90.WriteInt(pc.Total);
                                            pkg90.WriteInt(pc.Offer);
                                            pkg90.WriteByte(pc.typeVIP);
                                            pkg90.WriteInt(pc.VIPLevel);
                                            pkg90.WriteInt(pc.apprenticeshipState);
                                            pkg90.WriteInt(pc.masterID);
                                            pkg90.WriteString(pc.masterOrApprentices ?? "");
                                            pkg90.WriteBoolean(pc.IsMarried);
                                            if (pc.IsMarried)
                                            {
                                                pkg90.WriteInt(pc.SpouseID);
                                                pkg90.WriteString(pc.SpouseName ?? "");
                                            }
                                            pkg90.WriteInt(pc.hp);
                                            pkg90.WriteInt(tpGamePlayer.ZoneId);
                                            pkg90.WriteString(tpGamePlayer.ZoneName ?? "");

                                            // FightServer'a gönder
                                            var miSendTCP = fightConn.GetType().GetMethod("SendTCP",
                                                BindingFlags.Public | BindingFlags.Instance,
                                                null, new Type[] { typeof(GSPacketIn) }, null);
                                            miSendTCP.Invoke(fightConn, new object[] { pkg90 });

                                            // Oyun bitince IsViewer sıfırla
                                            // AbstractGame.GameStopped event'i
                                            var eiStopped = gameObj.GetType().GetEvent("GameStopped");
                                            if (eiStopped != null)
                                            {
                                                var viewerGP = tpGamePlayer;
                                                GameEventHandle handler = (g) => { viewerGP.IsViewer = false; };
                                                eiStopped.AddEventHandler(gameObj, handler);
                                            }

                                            gameStatus = $"PVP izleyici olarak FightServer'a gönderildi! (GameId:{gameId})";
                                        }
                                        catch (Exception pvpEx)
                                        {
                                            tpGamePlayer.IsViewer = false;
                                            var inner = pvpEx.InnerException ?? pvpEx;
                                            gameStatus = $"PVP viewer HATA: {inner.Message}";
                                        }
                                    }
                                    else
                                    {
                                        gameStatus = $"oyun tipi desteklenmiyor ({gameObj?.GetType().Name ?? "null"})";
                                    }
                                    goto skipViewer;
                                }
                                var gameType = gameObj.GetType();

                                tpGamePlayer.IsViewer = true;

                                // PhysicalId (public field on BaseGame)
                                var fiPhysId = typeof(BaseGame).GetField("PhysicalId",
                                    BindingFlags.Public | BindingFlags.Instance);
                                int physId = (int)fiPhysId.GetValue(gameObj);
                                fiPhysId.SetValue(gameObj, physId + 1);

                                // Player nesnesi oluştur
                                var fp = new Game.Logic.Phy.Object.Player(
                                    (IGamePlayer)tpGamePlayer, physId, bgame, 1,
                                    tpGamePlayer.PlayerCharacter.hp);

                                // BaseGame.AddPlayer(IGamePlayer, Player) — protected
                                // IsViewer=true → m_players'a ekler, TurnQueue'ya EKLEMEZ
                                var miAdd = typeof(BaseGame).GetMethod("AddPlayer",
                                    BindingFlags.NonPublic | BindingFlags.Instance,
                                    null, new Type[] { typeof(IGamePlayer), typeof(Game.Logic.Phy.Object.Player) }, null);
                                miAdd.Invoke(gameObj, new object[] { (IGamePlayer)tpGamePlayer, fp });

                                // ── GAME_CREATE paketini SADECE viewer'a gönder ──────────
                                // SendCreateGame() → SendToAll → TÜM oyunculara gönderir → mevcut oyunu bozar
                                // Aynı paketi oluşturup sadece viewer'a SendTCP ile gönderiyoruz

                                // Hardcoded enum değerleri (reflection hata yapabilir):
                                // ePackageTypeLogic.GAME_CMD = 91
                                // eTankCmdType.GAME_CREATE = 101
                                const byte GAME_CMD = 91;
                                const byte GAME_CREATE_CMD = 101;

                                // Private alanları oku
                                var bf = BindingFlags.NonPublic | BindingFlags.Instance;
                                int roomTypeVal = Convert.ToInt32(typeof(BaseGame).GetField("m_roomType", bf).GetValue(gameObj));
                                int gameTypeVal = Convert.ToInt32(typeof(BaseGame).GetField("m_gameType", bf).GetValue(gameObj));
                                int timeTypeVal = Convert.ToInt32(typeof(BaseGame).GetField("m_timeType", bf).GetValue(gameObj));

                                // LifeTime — SendToAll bunu Parameter2'ye set eder, biz de yapalım
                                var fiLifeTime = typeof(BaseGame).GetProperty("LifeTime",
                                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                int lifeTimeVal = 0;
                                if (fiLifeTime != null)
                                    lifeTimeVal = Convert.ToInt32(fiLifeTime.GetValue(gameObj));
                                else
                                {
                                    // Field olarak dene
                                    var flLife = typeof(BaseGame).GetField("LifeTime",
                                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                    if (flLife != null) lifeTimeVal = Convert.ToInt32(flLife.GetValue(gameObj));
                                }

                                // m_players'dan DOĞRUDAN oku — viewer DAHİL tüm oyuncular
                                // (GetAllFightPlayers artık viewer'ı hariç tutuyor, ama GAME_CREATE'te viewer da lazım)
                                var fiPlayersDict = typeof(BaseGame).GetField("m_players", BindingFlags.NonPublic | BindingFlags.Instance);
                                var playersDict = fiPlayersDict.GetValue(gameObj) as Dictionary<int, Game.Logic.Phy.Object.Player>;
                                List<Game.Logic.Phy.Object.Player> allPlayers;
                                lock (playersDict)
                                {
                                    allPlayers = new List<Game.Logic.Phy.Object.Player>(playersDict.Values);
                                }

                                // GAME_CREATE paketini oluştur (SendCreateGame ile birebir aynı format)
                                var pkg = new GSPacketIn(GAME_CMD);
                                if (lifeTimeVal > 0) pkg.Parameter2 = lifeTimeVal;
                                pkg.WriteByte(GAME_CREATE_CMD);
                                pkg.WriteInt((byte)roomTypeVal);
                                pkg.WriteInt((byte)gameTypeVal);
                                pkg.WriteInt(timeTypeVal);
                                pkg.WriteInt(allPlayers.Count);
                                foreach (var player in allPlayers)
                                {
                                    IGamePlayer pd = player.PlayerDetail;
                                    pkg.WriteInt(pd.ZoneId);
                                    pkg.WriteString(pd.ZoneName ?? "");
                                    pkg.WriteInt(pd.PlayerCharacter.ID);
                                    pkg.WriteString(pd.PlayerCharacter.NickName ?? "");
                                    pkg.WriteBoolean(pd.IsViewer);
                                    pkg.WriteByte(pd.PlayerCharacter.typeVIP);
                                    pkg.WriteInt(pd.PlayerCharacter.VIPLevel);
                                    pkg.WriteBoolean(pd.PlayerCharacter.Sex);
                                    pkg.WriteInt(pd.PlayerCharacter.Hide);
                                    pkg.WriteString(pd.PlayerCharacter.Style ?? "");
                                    pkg.WriteString(pd.PlayerCharacter.Colors ?? "");
                                    pkg.WriteString(pd.PlayerCharacter.Skin ?? "");
                                    pkg.WriteInt(pd.PlayerCharacter.Grade);
                                    pkg.WriteInt(pd.PlayerCharacter.Repute);
                                    if (pd.MainWeapon == null)
                                    {
                                        pkg.WriteInt(0);
                                    }
                                    else
                                    {
                                        pkg.WriteInt(pd.MainWeapon.TemplateID);
                                        pkg.WriteInt(pd.MainWeapon.RefineryLevel);
                                        pkg.WriteString(pd.MainWeapon.Template.Name ?? "");
                                        pkg.WriteDateTime(DateTime.MinValue);
                                    }
                                    if (pd.SecondWeapon == null)
                                    {
                                        pkg.WriteInt(0);
                                    }
                                    else
                                    {
                                        pkg.WriteInt(pd.SecondWeapon.TemplateID);
                                    }
                                    pkg.WriteInt(pd.PlayerCharacter.Nimbus);
                                    pkg.WriteBoolean(pd.PlayerCharacter.IsShowConsortia);
                                    pkg.WriteInt(pd.PlayerCharacter.ConsortiaID);
                                    pkg.WriteString(pd.PlayerCharacter.ConsortiaName ?? "");
                                    pkg.WriteInt(pd.PlayerCharacter.badgeID);
                                    pkg.WriteInt(pd.PlayerCharacter.ConsortiaLevel);
                                    pkg.WriteInt(pd.PlayerCharacter.ConsortiaRepute);
                                    pkg.WriteInt(pd.PlayerCharacter.Win);
                                    pkg.WriteInt(pd.PlayerCharacter.Total);
                                    pkg.WriteInt(pd.PlayerCharacter.FightPower);
                                    pkg.WriteInt(pd.PlayerCharacter.apprenticeshipState);
                                    pkg.WriteInt(pd.PlayerCharacter.masterID);
                                    pkg.WriteString(pd.PlayerCharacter.masterOrApprentices ?? "");
                                    pkg.WriteInt(pd.PlayerCharacter.AchievementPoint);
                                    pkg.WriteString(pd.PlayerCharacter.Honor ?? "");
                                    pkg.WriteInt(pd.PlayerCharacter.Offer);
                                    pkg.WriteBoolean(player.PlayerDetail.MatchInfo.DailyLeagueFirst);
                                    pkg.WriteInt(player.PlayerDetail.MatchInfo.DailyLeagueLastScore);
                                    pkg.WriteBoolean(pd.PlayerCharacter.IsMarried);
                                    if (pd.PlayerCharacter.IsMarried)
                                    {
                                        pkg.WriteInt(pd.PlayerCharacter.SpouseID);
                                        pkg.WriteString(pd.PlayerCharacter.SpouseName ?? "");
                                    }
                                    pkg.WriteInt(0);
                                    pkg.WriteInt(0);
                                    pkg.WriteInt(0);
                                    pkg.WriteInt(0);
                                    pkg.WriteInt(0);
                                    pkg.WriteInt(0);
                                    pkg.WriteInt(player.Team);
                                    pkg.WriteInt(player.Id);
                                    pkg.WriteInt(player.MaxBlood);
                                    if (player.Pet == null)
                                    {
                                        pkg.WriteInt(0);
                                    }
                                    else
                                    {
                                        pkg.WriteInt(1);
                                        pkg.WriteInt(player.Pet.Place);
                                        pkg.WriteInt(player.Pet.TemplateID);
                                        pkg.WriteInt(player.Pet.ID);
                                        pkg.WriteString(player.Pet.Name ?? "");
                                        pkg.WriteInt(player.Pet.UserID);
                                        pkg.WriteInt(player.Pet.Level);
                                        string[] skillEquips = player.Pet.SkillEquip.Split('|');
                                        pkg.WriteInt(skillEquips.Length);
                                        foreach (string skill in skillEquips)
                                        {
                                            var parts = skill.Split(',');
                                            pkg.WriteInt(int.Parse(parts[1]));
                                            pkg.WriteInt(int.Parse(parts[0]));
                                        }
                                    }
                                }

                                // ─── 1) GAME_CREATE → SADECE viewer'a gönder ───
                                ((IGamePlayer)tpGamePlayer).SendTCP(pkg);

                                // ─── 2) MISSION_INFO (113) → viewer'a gönder ───
                                // StartLoading sırasında SendMissionInfo çağrılır, biz de aynısını yapalım
                                var pveGame = gameObj as PVEGame;
                                if (pveGame != null && pveGame.MissionInfo != null)
                                {
                                    var mInfo = pveGame.MissionInfo;
                                    var pkgMission = new GSPacketIn(GAME_CMD);
                                    if (lifeTimeVal > 0) pkgMission.Parameter2 = lifeTimeVal;
                                    pkgMission.WriteByte(113); // GAME_MISSION_INFO
                                    pkgMission.WriteInt(mInfo.Id);
                                    pkgMission.WriteString(mInfo.Name ?? "");
                                    pkgMission.WriteString(mInfo.Success ?? "");
                                    pkgMission.WriteString(mInfo.Failure ?? "");
                                    pkgMission.WriteString(mInfo.Description ?? "");
                                    pkgMission.WriteString(mInfo.Title ?? "");
                                    pkgMission.WriteInt(pveGame.TotalMissionCount);
                                    pkgMission.WriteInt(pveGame.SessionId);
                                    pkgMission.WriteInt(pveGame.TotalTurn);
                                    pkgMission.WriteInt(pveGame.TotalCount);
                                    pkgMission.WriteInt(pveGame.Param1);
                                    pkgMission.WriteInt(pveGame.Param2);
                                    pkgMission.WriteInt(pveGame.WantTryAgain);
                                    pkgMission.WriteString(pveGame.Pic ?? "");
                                    ((IGamePlayer)tpGamePlayer).SendTCP(pkgMission);
                                }

                                // ─── 3) START_LOADING (103) → harita yükle ───
                                // SendStartLoading harita ID'sini ve yükleme dosyalarını gönderir
                                // Bu olmadan client hangi haritayı çizeceğini bilmez → boş ekran
                                {
                                    var pkgLoad = new GSPacketIn(GAME_CMD);
                                    if (lifeTimeVal > 0) pkgLoad.Parameter2 = lifeTimeVal;
                                    pkgLoad.WriteByte(103); // GAME_LOAD / SendStartLoading
                                    pkgLoad.WriteInt(5); // maxTime (kısa tut, zaten oyun oynuyor)
                                    pkgLoad.WriteInt(bgame.Map.Info.ID); // HAR‹TA ID — kritik!

                                    // Loading files — m_loadingFiles (private)
                                    var fiLoadFiles = typeof(BaseGame).GetField("m_loadingFiles", bf);
                                    var loadFiles = fiLoadFiles?.GetValue(gameObj) as System.Collections.IList;
                                    int loadCount = loadFiles?.Count ?? 0;
                                    pkgLoad.WriteInt(loadCount);
                                    if (loadFiles != null)
                                    {
                                        var tLoadInfo = fiLoadFiles.FieldType.GetGenericArguments()[0]; // LoadingFileInfo
                                        var piType = tLoadInfo.GetProperty("Type") ?? tLoadInfo.GetField("Type")?.DeclaringType?.GetProperty("Type");
                                        var piPath = tLoadInfo.GetProperty("Path");
                                        var piClass = tLoadInfo.GetProperty("ClassName");
                                        // LoadingFileInfo alanları field olabilir
                                        var fType = tLoadInfo.GetField("Type");
                                        var fPath = tLoadInfo.GetField("Path");
                                        var fClass = tLoadInfo.GetField("ClassName");
                                        foreach (var lf in loadFiles)
                                        {
                                            int lt = (piType != null) ? (int)piType.GetValue(lf) : (fType != null ? (int)fType.GetValue(lf) : 0);
                                            string lp = (piPath != null) ? (string)piPath.GetValue(lf) : (fPath != null ? (string)fPath.GetValue(lf) : "");
                                            string lc = (piClass != null) ? (string)piClass.GetValue(lf) : (fClass != null ? (string)fClass.GetValue(lf) : "");
                                            pkgLoad.WriteInt(lt);
                                            pkgLoad.WriteString(lp ?? "");
                                            pkgLoad.WriteString(lc ?? "");
                                        }
                                    }

                                    // Pet skill info
                                    var miSpecial = typeof(BaseGame).GetMethod("IsSpecialPVE",
                                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                    bool isSpecial = (bool)(miSpecial?.Invoke(gameObj, null) ?? true);
                                    if (isSpecial)
                                    {
                                        pkgLoad.WriteInt(0);
                                    }
                                    else
                                    {
                                        // PetMgr.GetGameNeedPetSkill() — try/catch ile
                                        try
                                        {
                                            var petMgrType = AppDomain.CurrentDomain.GetAssemblies()
                                                .SelectMany(a => { try { return a.GetTypes(); } catch { return Type.EmptyTypes; } })
                                                .FirstOrDefault(t => t.Name == "PetMgr");
                                            var miPetSkill = petMgrType?.GetMethod("GetGameNeedPetSkill",
                                                BindingFlags.Public | BindingFlags.Static);
                                            if (miPetSkill != null)
                                            {
                                                var petSkills = miPetSkill.Invoke(null, null) as Array;
                                                pkgLoad.WriteInt(petSkills?.Length ?? 0);
                                                if (petSkills != null)
                                                {
                                                    foreach (var ps in petSkills)
                                                    {
                                                        var psPic = ps.GetType().GetProperty("Pic")?.GetValue(ps);
                                                        var psEff = ps.GetType().GetProperty("EffectPic")?.GetValue(ps);
                                                        pkgLoad.WriteString(psPic?.ToString() ?? "");
                                                        pkgLoad.WriteString(psEff?.ToString() ?? "");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                pkgLoad.WriteInt(0);
                                            }
                                        }
                                        catch
                                        {
                                            pkgLoad.WriteInt(0);
                                        }
                                    }

                                    ((IGamePlayer)tpGamePlayer).SendTCP(pkgLoad);
                                }

                                // ─── 4) START_GAME (99) → viewer'a gönder ───
                                // StartGame() metodu bu paketi gönderir — oyuncuların pozisyonlarını,
                                // canlarını, yönlerini içerir. Client bunu alınca oyun ekranına geçer.
                                // GetAllFightingPlayers kullanıyoruz (viewer HARİÇ)
                                var miGetFighting = typeof(BaseGame).GetMethod("GetAllFightingPlayers",
                                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var fightingPlayers = (List<Game.Logic.Phy.Object.Player>)miGetFighting.Invoke(gameObj, null);

                                var pkgStart = new GSPacketIn(GAME_CMD);
                                if (lifeTimeVal > 0) pkgStart.Parameter2 = lifeTimeVal;
                                pkgStart.WriteByte(99); // START_GAME
                                pkgStart.WriteInt(fightingPlayers.Count);
                                foreach (var fPlayer in fightingPlayers)
                                {
                                    pkgStart.WriteInt(fPlayer.Id);
                                    pkgStart.WriteInt(fPlayer.X);
                                    pkgStart.WriteInt(fPlayer.Y);
                                    pkgStart.WriteInt(fPlayer.Direction);
                                    pkgStart.WriteInt(fPlayer.Blood);
                                    pkgStart.WriteInt(fPlayer.MaxBlood);
                                    pkgStart.WriteInt(fPlayer.Team);
                                    pkgStart.WriteInt(fPlayer.Weapon != null ? fPlayer.Weapon.RefineryLevel : 0);
                                    pkgStart.WriteInt(50);
                                    pkgStart.WriteInt(fPlayer.Dander);
                                    // FightBuffs
                                    var buffs = fPlayer.PlayerDetail.FightBuffs;
                                    pkgStart.WriteInt(buffs != null ? buffs.Count : 0);
                                    if (buffs != null)
                                    {
                                        foreach (var buff in buffs)
                                        {
                                            pkgStart.WriteInt(buff.Type);
                                            pkgStart.WriteInt(buff.Value);
                                        }
                                    }
                                    pkgStart.WriteInt(0);
                                    pkgStart.WriteBoolean(fPlayer.IsFrost);
                                    pkgStart.WriteBoolean(fPlayer.IsHide);
                                    pkgStart.WriteBoolean(fPlayer.IsNoHole);
                                    pkgStart.WriteBoolean(false);
                                    pkgStart.WriteInt(0);
                                }
                                pkgStart.WriteDateTime(DateTime.Now);

                                // START_GAME'i gecikmeyle gönder — client haritayı yüklesin
                                var viewerGP_start = (IGamePlayer)tpGamePlayer;
                                var startPkg_delayed = pkgStart;
                                System.Threading.Tasks.Task.Run(async () =>
                                {
                                    try
                                    {
                                        await System.Threading.Tasks.Task.Delay(5000); // 5 saniye bekle
                                        viewerGP_start.SendTCP(startPkg_delayed);
                                    }
                                    catch { }
                                });

                                // ─── 5) Viewer temizleme — oyun Playing'den çıkınca viewer'ı kaldır ───
                                // GameOverred çok geç tetiklenir (GameOver viewer'ı process ettikten SONRA)
                                // Bu yüzden background task ile GameState'i izleyip, Playing'den çıkınca
                                // viewer'ı m_players'dan HEMEN kaldırıyoruz (GameOver işleminden ÖNCE)
                                int viewerPlayerId = fp.Id;
                                var viewerGamePlayer = tpGamePlayer;
                                var viewerBaseGame = bgame;
                                System.Threading.Tasks.Task.Run(async () =>
                                {
                                    try
                                    {
                                        // GameState Playing iken bekle — 300ms aralıklarla kontrol et
                                        while (true)
                                        {
                                            await System.Threading.Tasks.Task.Delay(300);
                                            try
                                            {
                                                var state = viewerBaseGame.GameState.ToString();
                                                // Playing değilse → oyun bitti veya geçiş yapıyor
                                                if (state != "Playing" && state != "GameStart")
                                                    break;
                                            }
                                            catch { break; } // Game disposed vb.
                                        }

                                        // Viewer'ı m_players'dan kaldır — GameOver process etmeden ÖNCE
                                        try
                                        {
                                            var bfPriv = BindingFlags.NonPublic | BindingFlags.Instance;
                                            var fiPlayers = typeof(BaseGame).GetField("m_players", bfPriv);
                                            if (fiPlayers != null)
                                            {
                                                var players = fiPlayers.GetValue(viewerBaseGame) as System.Collections.IDictionary;
                                                if (players != null)
                                                {
                                                    lock (players)
                                                    {
                                                        players.Remove(viewerPlayerId);
                                                    }
                                                }
                                            }
                                        }
                                        catch { }

                                        // IsViewer sıfırla — sonraki oyunlarda normal oyuncu olabilsin
                                        viewerGamePlayer.IsViewer = false;
                                    }
                                    catch { viewerGamePlayer.IsViewer = false; }
                                });

                                gameStatus = $"izleyici olarak oyuna eklendi! (MapID:{bgame.Map?.Info?.ID}, Players:{fightingPlayers.Count})";
                            }
                            catch (Exception gex)
                            {
                                tpGamePlayer.IsViewer = false; // Hata durumunda da sıfırla
                                var inner = gex.InnerException ?? gex;
                                gameStatus = $"HATA: {inner.Message} | Kaynak: {inner.TargetSite?.Name} | Stack: {inner.StackTrace?.Split('\n')[0]}";
                            }
                        skipViewer:;
                        }

                        // 6) Lobi listesini güncelle
                        try { RoomMgr.WaitingRoom.SendUpdateCurrentRoom(targetRoom); } catch { }

                        WriteJson(ctx, new { success = true, message = $"'{tpNick}' oda {tpRoomId}'e ışınlandı! (Map:{targetRoom.MapId}, İzleyici:{tpGamePlayer.IsViewer}, Oyun:{gameStatus})" });
                    }
                    else
                    {
                        // Eklenemedi — lobiye geri gönder
                        try { RoomMgr.WaitingRoom.AddPlayer(tpGamePlayer); } catch { }
                        WriteJson(ctx, new { success = false, error = $"Odaya eklenemedi — oda tamamen dolu. Oyuncu:{targetRoom.PlayerCount}/{targetRoom.PlacesCount}" });
                    }
                    return;
                }

                // ── Limit değiştir ──────────────────────────────────────────────
                if (path == "/api/game/setdailylimit" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);
                    if (d == null || d["limit"] == null)
                    {
                        ctx.Response.StatusCode = 400;
                        WriteJson(ctx, new { success = false, error = "limit alanı gerekli" });
                        return;
                    }

                    int newLimit;
                    if (!int.TryParse(d["limit"].ToString(), out newLimit) || newLimit <= 0)
                    {
                        ctx.Response.StatusCode = 400;
                        WriteJson(ctx, new { success = false, error = "Geçersiz limit değeri" });
                        return;
                    }

                    // RAM'deki limiti güncelle (tüm yeni işlemler bu değeri kullanır)
                    GameApiServer.DailyMoneyLimit = newLimit;

                    // App.config'e de yaz — sunucu yeniden başlasa bile kalıcı olsun
                    try
                    {
                        var cfg = System.Configuration.ConfigurationManager.OpenExeConfiguration(
                                      System.Configuration.ConfigurationUserLevel.None);
                        cfg.AppSettings.Settings["DailyMoneyLimit"].Value = newLimit.ToString();
                        cfg.Save(System.Configuration.ConfigurationSaveMode.Modified);
                        System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                    }
                    catch { /* App.config yazılamazsa sadece RAM'deki değer geçerli kalır */ }

                    WriteJson(ctx, new { success = true, limit = newLimit });
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