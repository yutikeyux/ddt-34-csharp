using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Server;
using Game.Server.RingStation;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.Rooms;
using GameServerScript.AI.NPC;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;




//knka 9000 portu çakışıyordu sistemde iki defa ekli olduğu için
// ben de 9500 yaptım
//game.server üzerinden sağ tıklayıp rebuild atarsan
//game.server klasörüne atar
//orda dll ve pdb yi kopyalayıp
//roadın içine yapıştırdım
//ok şu an
//sen de öyle yapıp başlat roadı
//center fighting ok şu an
//bir de teleportlarda ufak şeyler değiştirdim geri de aldım mı tam bilmiyorum ama senin bıraktığın haldeki gibi olması lazım incelersin
//baseworldbossroom.cs de addplayer ı da değiştirdim gibi bi şe oldu sonra senin bıraktığın hale geri aldım gibi onu da incelersin knka






// 1. DÜZELTME: Namespace'i Game.Server'ın içine taşıdık.
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
            int.TryParse(System.Configuration.ConfigurationManager.AppSettings["ApiPort"], out var p) ? p : 9999; //9500 olarak değiştim çakışıyordu not: yuti



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

        // Bu metot, GamePlayer.cs'ye IsAPITeleporting propertysi eklemeden
        // reflection ile bu bayrağı ayarlamaya çalışır.
        private static void SetIsAPITeleporting(GamePlayer player, bool value)
        {
            try
            {
                Type playerType = player.GetType();
                PropertyInfo prop = playerType.GetProperty("IsAPITeleporting");

                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(player, value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API ERROR] Reflection failed on IsAPITeleporting for {player.PlayerCharacter.NickName}: {ex.Message}");
            }
        }

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

                    List<Game.Server.Rooms.BaseRoom> allUsingRoom = RoomMgr.GetAllUsingRoom();
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
                    var list = new System.Collections.Generic.List<object>();
                    foreach (var p in players)
                    {
                        var c = p.PlayerCharacter;
                        list.Add(new { Nickname = c.NickName, Level = c.Grade, FightPower = c.FightPower });
                    }
                    WriteJson(ctx, new { Count = list.Count, Players = list });
                    return;
                }



                if (path.StartsWith("/api/game/playerinfo/") && req.HttpMethod == "GET")
                {
                    var nick = path.Substring("/api/game/playerinfo/".Length);

                    // 1) Önce online oyuncu var mı bak
                    var p = WorldMgr.GetClientByPlayerNickName(nick);
                    if (p != null)
                    {
                        var c = p.PlayerCharacter;

                        // --- BURASI ÖNEMLİ ---
                        // Hasar/Zırh için Attack/Defence yerine
                        // GamePlayer metodlarını kullanıyoruz:
                        double baseAttack = p.GetBaseAttack();   // Hasar
                        double baseDefence = p.GetBaseDefence();  // Zırh

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

                        // Offline için elimizde GetBaseAttack/GetBaseDefence yok,
                        // en azından Attack/Defence'i Damage/Guard olarak gönderiyoruz.
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
                            IsOnline = false
                        });
                        return;
                    }
                }


                if (path.StartsWith("/api/game/update/") && req.HttpMethod == "GET")
                {
                    var nick = path.Substring("/api/game/update/".Length);

                    if (string.IsNullOrWhiteSpace(nick))
                    {
                        ctx.Response.StatusCode = 400;
                        WriteJson(ctx, new { error = "Nick gerekli." });
                        return;
                    }

                    // Sadece online oyuncuyu güncelleyebiliriz
                    var pl = WorldMgr.GetClientByPlayerNickName(nick);
                    if (pl == null)
                    {
                        ctx.Response.StatusCode = 404;
                        WriteJson(ctx, new { error = "Oyuncu çevrimdışı veya bulunamadı." });
                        return;
                    }

                    try
                    {
                        // client.Player.SavePlayerInfo();’nin GamePlayer versiyonu
                        pl.SavePlayerInfo();
                        pl.SendMessage("Hesabınız Discord üzerinden güncellendi.");

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

                if (path == "/api/game/announce/types" && req.HttpMethod == "GET")
                {
                    WriteJson(ctx, new { types = new[] { "kucuk", "buyuk", "kirmizi", "mor", "admin", "sari" } });
                    return;
                }

                


                if (!IsAuthorized(req))
                {
                    ctx.Response.StatusCode = 401;
                    WriteJson(ctx, new { error = "Unauthorized", tip = "X-API-Key header gerekli" });
                    return;
                }

                if (path == "/api/game/worldboss/start" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);
                    int durationMinutes = d["DurationMinutes"]?.ToObject<int?>() ?? 10;
                    int bossId = 1243; // Varsayılan Boss ID
                    int bossMaxBlood = NPCInfoMgr.GetNpcInfoById(bossId)?.Blood ?? 500000;

                    // --- KİLİT BAŞLANGICI ---
                    lock (RoomMgr.WorldBossRoom)
                    {
                        if (RoomMgr.WorldBossRoom.WorldOpen)
                        {
                            ctx.Response.StatusCode = 400;
                            WriteJson(ctx, new { error = "World Boss zaten açık durumda." });
                            return;
                        }

                        RoomMgr.WorldBossRoom.BeginTime = DateTime.Now;
                        RoomMgr.WorldBossRoom.EndTime = DateTime.Now.AddMinutes(durationMinutes);
                        RoomMgr.WorldBossRoom.MaxBlood = bossMaxBlood;
                        RoomMgr.WorldBossRoom.Blood = bossMaxBlood;
                        RoomMgr.WorldBossRoom.Name = "DISCORD BOSS";
                        RoomMgr.WorldBossRoom.BossResourceId = "1";
                        RoomMgr.WorldBossRoom.CurrentPve = bossId;
                        RoomMgr.WorldBossRoom.FightOver = false;
                        RoomMgr.WorldBossRoom.RoomClose = false;
                        RoomMgr.WorldBossRoom.WorldOpen = true;
                        RoomMgr.WorldBossRoom.FightTime = durationMinutes;
                    }
                    // --- KİLİT BİTİŞİ ---

                    foreach (var pl in WorldMgr.GetAllPlayers())
                    {
                        pl.Out.SendOpenWorldBoss(0, 0);
                        pl.Out.SendMessage((eMessageType)1, $"[Yönetim] Dünya BOSS etkinliği {durationMinutes} dakika süreyle BAŞLATILDI!");
                    }

                    WriteJson(ctx, new { message = $"World Boss başlatıldı. Süre: {durationMinutes} dakika.", duration = durationMinutes });
                    return;
                }

                if (path == "/api/game/worldboss/stop" && req.HttpMethod == "POST")
                {
                    // --- KİLİT BAŞLANGICI ---
                    lock (RoomMgr.WorldBossRoom)
                    {
                        if (!RoomMgr.WorldBossRoom.WorldOpen)
                        {
                            ctx.Response.StatusCode = 400;
                            WriteJson(ctx, new { error = "World Boss zaten kapalı." });
                            return;
                        }

                        RoomMgr.WorldBossRoom.FightOver = true;
                        RoomMgr.WorldBossRoom.SendFightOver();
                        RoomMgr.WorldBossRoom.SendRoomClose();
                        RoomMgr.WorldBossRoom.WorldBossClose();
                        RoomMgr.WorldBossRoom.SendGiftForUserJoined();
                    }
                    // --- KİLİT BİTİŞİ ---

                    foreach (var pl in WorldMgr.GetAllPlayers())
                    {
                        pl.Out.SendMessage((eMessageType)1, "[Yönetim] Dünya BOSS etkinliği MANUEL OLARAK sonlandırıldı ve ödüller dağıtıldı.");
                    }

                    WriteJson(ctx, new { message = "World Boss manuel olarak sonlandırıldı ve ödül dağıtımı başlatıldı." });
                    return;
                }


                if (path == "/api/game/worldboss/teleport" && req.HttpMethod == "POST")
                {
                    var d = ReadJsonBodyJ(req);
                    string nickname = d["Nickname"]?.ToString();

                    var pl = WorldMgr.GetClientByPlayerNickName(nickname);
                    if (pl == null) { WriteJson(ctx, new { error = "Oyuncu bulunamadı" }); return; }

                    if (!RoomMgr.WorldBossRoom.WorldOpen)
                    {
                        WriteJson(ctx, new { error = "World Boss etkinliği şu an aktif değil." }); return;
                    }

                    lock (pl)
                    {
                        // 1. ADIM: Oyuncuyu mevcut konumundan çıkar
                        if (pl.CurrentRoom != null)
                        {
                            // İstemciye (client) "artık bu odada değilsin" paketini gönder
                            pl.Out.SendSceneRemovePlayer(pl);
                            // Sunucudan (server) oyuncuyu kaldır
                            pl.CurrentRoom.RemovePlayerUnsafe(pl);
                        }
                        else
                        {
                            // Lobideyse lobiden çıkar
                            RoomMgr.ExitWaitingRoom(pl);
                        }

                        // 2. ADIM: Oyuncuyu World Boss odasına ekle
                        SetIsAPITeleporting(pl, true);

                        // Sunucu tarafında oyuncunun durumunu ayarla
                        RoomMgr.WorldBossRoom.AssignPlayerToRoom(pl);

                        // Koordinatlarını ayarla (AddPlayer bu bilgiyi pakete yazacak)
                        pl.X = RoomMgr.WorldBossRoom.PlayerDefaultPosX;
                        pl.Y = RoomMgr.WorldBossRoom.PlayerDefaultPosY;

                        // Bu metot oyuncuyu listeye ekler VE istemciye ENTER (kod 3) paketini gönderir
                        bool added = RoomMgr.WorldBossRoom.AddPlayer(pl);

                        SetIsAPITeleporting(pl, false);

                        if (!added)
                        {
                            pl.CurrentRoom = null;
                            WriteJson(ctx, new { error = "Sunucu Hatası: Oyuncu odaya eklenemedi (İç kural reddetti)." });
                            return;
                        }

                        pl.SendMessage("Yönetici tarafından World Boss haritasına ışınlandınız.");
                    }

                    WriteJson(ctx, new { message = $"{nickname} World Boss haritasına ışınlandı." });
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

                    p.SendMessage($"[Discord Ödülün] oyun içi mail kutuna gönderildi."); //AGA OKUYOSAN REBUILD ATMAYI UNUTMA //AGA OKUYOSAN REBUILD ATMAYI UNUTMA //AGA OKUYOSAN REBUILD ATMAYI UNUTMA //AGA OKUYOSAN REBUILD ATMAYI UNUTMA //AGA OKUYOSAN REBUILD ATMAYI UNUTMA 
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

                if (path == "/api/game/chat" && req.HttpMethod == "POST")
                {
                    try
                    {
                        var d = ReadJsonBodyJ(req);

                        string username = d["username"]?.ToString() ?? "Discord";
                        string message = d["message"]?.ToString();
                        string discordIdStr = d["discordId"]?.ToString(); // yeni alan (opsiyonel)

                        if (string.IsNullOrEmpty(message))
                        {
                            WriteJson(ctx, new { error = "Mesaj boş olamaz." });
                            return;
                        }

                        GamePlayer linkedPlayer = null;

                        // Eğer discordId geldiyse -> bağlı oyun hesabını bul
                        if (!string.IsNullOrEmpty(discordIdStr) && long.TryParse(discordIdStr, out long discordId))
                        {
                            int userId = DiscordLinkMgr.GetUserIdByDiscordId(discordId);

                            if (userId > 0)
                            {
                                // Online oyuncular arasından bu UserID'yi bul
                                foreach (var p in WorldMgr.GetAllPlayers())
                                {
                                    if (p.PlayerCharacter != null && p.PlayerCharacter.ID == userId)
                                    {
                                        linkedPlayer = p;
                                        break;
                                    }
                                }
                            }
                        }

                        // Eğer bağlı hesap bulunduysa, oyunda gözükecek isim olarak NickName'i kullan
                        string inGameNick = linkedPlayer != null
                            ? linkedPlayer.PlayerCharacter.NickName
                            : username; // yoksa direkt gönderilen username

                        // --- PAKET ---
                        GSPacketIn pkg = new GSPacketIn((byte)73, 0);

                        pkg.WriteInt(0);              // ZoneID (sunucuya göre istersen doldur)
                        pkg.WriteInt(0);              // PlayerID (0 = sistem/özel)
                        pkg.WriteString(inGameNick);  // Nick
                        pkg.WriteString(message);     // Mesaj
                        pkg.WriteString("");          // ZoneName (boş bırakıyoruz)

                        // Diğer login serverlara gönder
                        foreach (var item in GameServer.Instance.OtherLoginServer)
                        {
                            if (item.IsConnected)
                            {
                                item.SendPacket(pkg);
                            }
                        }

                        // Online tüm oyunculara gönder
                        foreach (var pl in WorldMgr.GetAllPlayers())
                        {
                            if (pl != null && pl.Out != null)
                            {
                                pkg.ClientID = pl.PlayerCharacter.ID;
                                pl.Out.SendTCP(pkg);
                            }
                        }

                        WriteJson(ctx, new
                        {
                            success = true,
                            message_sent = message,
                            from = inGameNick,
                            linked = linkedPlayer != null
                        });
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.Error("API /api/game/chat error:", ex);
                        WriteJson(ctx, new { error = "Sunucu hatası: " + ex.Message });
                        return;
                    }
                }

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
                    var result = new System.Collections.Generic.List<object>();

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

                    WriteJson(ctx, new
                    {
                        removed = affected
                    });
                    return;
                }

                if (path == "/api/game/link/clear" && req.HttpMethod == "POST")
                {
                    int removed = DiscordLinkMgr.ClearAllLinks();

                    WriteJson(ctx, new
                    {
                        removed
                    });
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

                        Console.WriteLine($"[API] /api/game/link/confirm çağrıldı: code={code}, discordId={discordId}");

                        int userId = DiscordLinkMgr.ConsumeCodeAndBindUser(code, discordId);

                        Console.WriteLine($"[API] /api/game/link/confirm sonucu: userId={userId}");

                        if (userId <= 0)
                        {
                            ctx.Response.StatusCode = 400;
                            WriteJson(ctx, new { error = "Kod geçersiz, süresi dolmuş veya hesap zaten bağlı." });
                            return;
                        }

                        // Karakter bilgisini çek
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

                // **BUNDAN SONRA** auth gelsin
                if (!IsAuthorized(req))
                {
                    ctx.Response.StatusCode = 401;
                    WriteJson(ctx, new { error = "Unauthorized", tip = "X-API-Key header gerekli" });
                    return;
                }

                ctx.Response.StatusCode = 404;
                WriteJson(ctx, new { error = "Endpoint yok" });
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
            Console.WriteLine($"[API] Auth check: headerKey={headerKey}, adminKey={ADMIN_KEY}");
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
                var m = t?.GetMethod(methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                m?.Invoke(null, args ?? Array.Empty<object>());
            }
            catch { }
        }
    }
}