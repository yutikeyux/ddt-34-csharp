using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Server;
using Game.Server.Managers;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Game.ApiHost
{
    public static class GameApiServer
    {
        private static HttpListener _listener;
        private static bool _running;

        // App.config -> <appSettings><add key="AdminKey" value="SENIN_GUVENLI_KEYIN"/></appSettings>
        private static string ADMIN_KEY =>
            System.Configuration.ConfigurationManager.AppSettings["AdminKey"] ?? "DEGISTIR";

        // App.config -> <appSettings><add key="ApiPort" value="9000"/></appSettings>
        private static int API_PORT =>
            int.TryParse(System.Configuration.ConfigurationManager.AppSettings["ApiPort"], out var p) ? p : 9000;

        public static void Start(int port = -1)
        {
            if (_running) return;

            var p = port > 0 ? port : API_PORT;

            _listener = new HttpListener();
            // Dış IP’yi dinlemek için bir defaya mahsus aşağıdakini Admin PowerShell’de çalıştır:
            // netsh http add urlacl url=http://+:9000/ user=Everyone
            _listener.Prefixes.Add($"http://*:{p}/");
            _listener.Start();
            _running = true;

            Console.WriteLine($"[API] {p} portunda dinleniyor...");
            Task.Run(Loop);
        }

        public static void Stop()
        {
            try { _running = false; _listener?.Stop(); } catch { }
        }

        private static async Task Loop()
        {
            while (_running)
            {
                HttpListenerContext ctx;
                try { ctx = await _listener.GetContextAsync(); }
                catch { if (!_running) break; else continue; }
                _ = Task.Run(() => Handle(ctx));
            }
        }

        private static void Handle(HttpListenerContext ctx)
        {
            try
            {
                // UTF-8 ve CORS
                ctx.Response.AddHeader("Access-Control-Allow-Origin", "*");
                ctx.Response.AddHeader("Content-Type", "application/json; charset=utf-8");

                var req = ctx.Request;
                var path = req.Url.AbsolutePath.TrimEnd('/').ToLowerInvariant();

                // CORS preflight
                if (req.HttpMethod == "OPTIONS")
                {
                    ctx.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type,X-API-Key");
                    ctx.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST,OPTIONS");
                    ctx.Response.StatusCode = 200; ctx.Response.Close(); return;
                }

                // --------- PUBLIC ----------
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
                    var p = WorldMgr.GetClientByPlayerNickName(nick);
                    if (p == null) { ctx.Response.StatusCode = 404; WriteJson(ctx, new { error = "Oyuncu bulunamadı" }); return; }
                    var c = p.PlayerCharacter;
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
                        HP = c.hp,
                        FightPower = c.FightPower,
                        IsOnline = true
                    });
                    return;
                }

                if (path == "/api/game/announce/types" && req.HttpMethod == "GET")
                {
                    WriteJson(ctx, new { types = new[] { "kucuk", "buyuk", "kirmizi", "mor", "admin", "sari" } });
                    return;
                }

                // --------- ADMIN (X-API-Key) ----------
                if (!IsAuthorized(req))
                {
                    ctx.Response.StatusCode = 401;
                    WriteJson(ctx, new { error = "Unauthorized", tip = "X-API-Key header gerekli" });
                    return;
                }

                if (path == "/api/game/announce" && req.HttpMethod == "POST")
                {
                    dynamic d = ReadJsonBody<dynamic>(req);
                    string type = ((string)d?.Type ?? "sari").ToLower();
                    string text = (string)d?.Message ?? "";

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
                            new ManageBussiness().SystemNotice(text);
                            break;
                        case "admin":
                            SendAllText("[ADMIN] " + text);
                            break;
                        case "sari":
                            SendAllText(text);
                            break;
                        default:
                            ctx.Response.StatusCode = 400;
                            WriteJson(ctx, new { error = "Geçersiz duyuru tipi", allow = new[] { "kucuk", "buyuk", "kirmizi", "mor", "admin", "sari" } });
                            return;
                    }
                    WriteJson(ctx, new { message = "Duyuru gönderildi", type, text });
                    return;
                }

                if (path == "/api/game/kick" && req.HttpMethod == "POST")
                {
                    dynamic d = ReadJsonBody<dynamic>(req);
                    string nickname = (string)d?.Nickname;
                    var p = WorldMgr.GetClientByPlayerNickName(nickname);
                    if (p == null) { ctx.Response.StatusCode = 404; WriteJson(ctx, new { error = "Oyuncu bulunamadı" }); return; }
                    using (var mb = new ManageBussiness()) mb.KitoffUserByNickName(nickname, "API kick");
                    p.Disconnect();
                    WriteJson(ctx, new { message = $"{nickname} kicklendi" });
                    return;
                }

                if (path == "/api/game/ban" && req.HttpMethod == "POST")
                {
                    dynamic d = ReadJsonBody<dynamic>(req);
                    string nickname = (string)d?.Nickname;
                    string reason = (string)d?.Reason ?? "Belirtilmedi";
                    DateTime until = DateTime.UtcNow.AddYears(25);
                    using (var mb = new ManageBussiness()) mb.ForbidPlayerByNickName(nickname, until, false);
                    foreach (var pl in WorldMgr.GetAllPlayers()) pl.SendMessage($"Oyuncu <{nickname}> banlandı. Sebep: {reason}");
                    WriteJson(ctx, new { message = $"{nickname} banlandı", until });
                    return;
                }

                if (path == "/api/game/unban" && req.HttpMethod == "POST")
                {
                    dynamic d = ReadJsonBody<dynamic>(req);
                    string nickname = (string)d?.Nickname;
                    using (var mb = new ManageBussiness()) mb.ForbidPlayerByNickName(nickname, DateTime.UtcNow, true);
                    foreach (var pl in WorldMgr.GetAllPlayers()) pl.SendMessage($"Oyuncu <{nickname}> banı kaldırıldı.");
                    WriteJson(ctx, new { message = $"{nickname} unban" });
                    return;
                }

                if (path == "/api/game/giveitem" && req.HttpMethod == "POST")
                {
                    dynamic d = ReadJsonBody<dynamic>(req);
                    string nickname = (string)d?.Nickname;
                    int itemId = (int)(d?.ItemID ?? 0);
                    int count = (int)(d?.Count ?? 1);
                    string title = (string)d?.Title ?? "Yönetim Hediyesi";
                    string content = (string)d?.Content ?? "API üzerinden";
                    bool isBinds = d?.IsBinds == null ? true : (bool)d?.IsBinds;

                    var p = WorldMgr.GetClientByPlayerNickName(nickname);
                    if (p == null) { ctx.Response.StatusCode = 404; WriteJson(ctx, new { error = "Oyuncu bulunamadı" }); return; }

                    var pc = p.PlayerCharacter;
                    new PlayerBussiness().SendMailAndItem(title, content, pc.ID,
                        itemId, count, 0, 0, 0, 0, 0, 0, 0, 0, isBinds);

                    p.SendMessage($"[Hediye] {count}x {itemId} mail kutuna gönderildi.");
                    WriteJson(ctx, new { message = "Hediye gönderildi", nickname, itemId, count });
                    return;
                }

                if (path == "/api/game/mailall" && req.HttpMethod == "POST")
                {
                    dynamic d = ReadJsonBody<dynamic>(req);
                    int itemId = (int)(d?.ItemID ?? 0);
                    int count = (int)(d?.Count ?? 1);
                    string title = (string)d?.Title ?? "Toplu Ödül";
                    string content = (string)d?.Content ?? "Online ödül";
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
                    dynamic d = ReadJsonBody<dynamic>(req);
                    string nickname = (string)d?.Nickname;
                    string currency = ((string)d?.CurrencyType ?? "kupon").ToLower();
                    int amount = (int)(d?.Amount ?? 0);
                    var pl = WorldMgr.GetClientByPlayerNickName(nickname);
                    if (pl == null) { ctx.Response.StatusCode = 404; WriteJson(ctx, new { error = "Oyuncu bulunamadı" }); return; }

                    switch (currency)
                    {
                        case "kupon": pl.AddMoney(amount); break;
                        case "exp": pl.AddGP(amount); break;
                        case "onur": pl.AddHonor(amount); break;
                        case "baglikupon": pl.AddGiftToken(amount); break;
                        case "mukafat": pl.AddOffer(amount); break;
                        default:
                            ctx.Response.StatusCode = 400;
                            WriteJson(ctx, new { error = "Geçersiz currency", allow = new[] { "kupon", "exp", "onur", "baglikupon", "mukafat" } });
                            return;
                    }
                    pl.SendMessage($"[Ödül] {amount} {currency} eklendi.");
                    WriteJson(ctx, new { message = "OK", nickname, currency, amount });
                    return;
                }

                if (path == "/api/game/reloadall" && req.HttpMethod == "POST")
                {
                    Task.Run(() =>
                    {
                        bool _ = BallMgr.ReLoad();
                        _ = MapMgr.ReLoadMap();
                        _ = MapMgr.ReLoadMapServer();
                        _ = PropItemMgr.Reload();
                        _ = ItemMgr.ReLoad();
                        _ = ShopMgr.ReLoad();
                        _ = QuestMgr.ReLoad();
                        _ = FusionMgr.ReLoad();
                        _ = ConsortiaMgr.ReLoad();
                        _ = RateMgr.ReLoad();
                        _ = NPCInfoMgr.ReLoad();
                        _ = FightRateMgr.ReLoad();
                        _ = AwardMgr.ReLoad();
                        _ = LanguageMgr.Reload("");

                        foreach (var pl in WorldMgr.GetAllPlayers())
                            pl.SendMessage("Veritabanı tabloları yenilendi.");
                    });

                    WriteJson(ctx, new { message = "Reload başlatıldı" });
                    return;
                }

                // 404
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
            => req.Headers["X-API-Key"] == ADMIN_KEY;

        private static void WriteJson(HttpListenerContext ctx, object obj)
        {
            string json = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None);
            byte[] buf = Encoding.UTF8.GetBytes(json);
            ctx.Response.ContentEncoding = Encoding.UTF8;
            ctx.Response.ContentType = "application/json; charset=utf-8";
            ctx.Response.OutputStream.Write(buf, 0, buf.Length);
            ctx.Response.OutputStream.Flush();
            ctx.Response.Close();
        }

        private static T ReadJsonBody<T>(HttpListenerRequest req)
        {
            using (var reader = new System.IO.StreamReader(req.InputStream, req.ContentEncoding))
            {
                string body = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(body);
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
    }
}
