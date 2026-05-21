using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using System;
using System.Collections.Concurrent;

namespace Game.Server.Packets.Client
{
    [PacketHandler(300, "Anti-Cheat Handler")]
    public class AntiCheatHandler : IPacketHandler
    {
        // --- Sabitler ---
        private const int SUBCODE_SPEED_CHECK = 0;
        private const long SPEED_CHECK_INTERVAL_SEC = 300;  // 5 dakika
        private const long SPEED_TOLERANCE_SEC = 20;   // tolerans payı
        private const int MAX_VIOLATIONS = 3;    // ban öncesi max ihlal
        private const int BAN_MINUTES = 20;

        // Thread-safe ihlal sayacı (PlayerID → kayıt)
        private static readonly ConcurrentDictionary<int, ViolationRecord> _violations
            = new ConcurrentDictionary<int, ViolationRecord>();

        // ----------------------------------------------------------------
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client?.Player == null) return 0;

            int subCode;
            try { subCode = packet.ReadInt(); }
            catch { return 0; }

            switch (subCode)
            {
                case SUBCODE_SPEED_CHECK:
                    HandleSpeedCheck(client);
                    break;
                default:
                    LogSuspicious(client, $"Bilinmeyen alt kod: {subCode}");
                    break;
            }

            return 0;
        }

        // ----------------------------------------------------------------
        // HIZ / ZAMANLAMA KONTROLÜ
        // ----------------------------------------------------------------
        private void HandleSpeedCheck(GameClient client)
        {
            long now = GetUnixNow();
            long lastCheck = client.Player.TimeCheckHack;
            long elapsed = now - lastCheck;

            // İlk bağlantı — referans zamanı ayarla
            if (lastCheck == 0)
            {
                client.Player.TimeCheckHack = now;
                SendAck(client, SUBCODE_SPEED_CHECK);
                return;
            }

            bool tooFast = elapsed < (SPEED_CHECK_INTERVAL_SEC - SPEED_TOLERANCE_SEC);
            bool tooSlow = elapsed > (SPEED_CHECK_INTERVAL_SEC * 4); // uzun süre paket gelmedi

            if (tooFast)
            {
                // Zaman manipülasyonu / Cheat Engine şüphesi
                string detail = $"Beklenen ~{SPEED_CHECK_INTERVAL_SEC}sn, gerçek: {elapsed}sn";
                RegisterViolation(client, "SpeedHack", detail);
            }
            else if (tooSlow)
            {
                // Paketin kasıtlı dondurulması / proxy şüphesi
                string detail = $"Paket {elapsed}sn gecikti (olası freeze/proxy)";
                RegisterViolation(client, "PacketFreeze", detail);
            }
            else
            {
                // Temiz — zamanı güncelle ve istemciyi onayla
                client.Player.TimeCheckHack = now;
                SendAck(client, SUBCODE_SPEED_CHECK);
            }
        }

        // ----------------------------------------------------------------
        // İHLAL KAYIT & CEZA SİSTEMİ
        // ----------------------------------------------------------------
        private void RegisterViolation(GameClient client, string type, string detail)
        {
            int playerId = client.Player.PlayerCharacter.ID;
            long now = GetUnixNow();

            var record = _violations.AddOrUpdate(
                playerId,
                _ => new ViolationRecord { Count = 1, FirstTime = now, LastType = type },
                (_, r) =>
                {
                    // 1 saatlik pencerede tekrar → artır; pencere dışındaysa sıfırla
                    if (now - r.FirstTime < 3600)
                        r.Count++;
                    else
                    {
                        r.Count = 1;
                        r.FirstTime = now;
                    }
                    r.LastType = type;
                    return r;
                });

            Console.WriteLine(
                $"[AntiCheat] {type} | {client.Player.PlayerCharacter.UserName} " +
                $"| {detail} | İhlal #{record.Count}/{MAX_VIOLATIONS}");

            client.Player.AddLog(type, detail);

            if (record.Count >= MAX_VIOLATIONS)
            {
                ApplyBan(client, type);
                _violations.TryRemove(playerId, out _);
            }
            else
            {
                // Henüz ban değil — sadece uyar
                client.Player.SendMessage(
                    $"⚠ Şüpheli aktivite tespit edildi ({type}). " +
                    $"Devam ederse hesabın yasaklanacak. ({record.Count}/{MAX_VIOLATIONS})");
            }
        }

        private void ApplyBan(GameClient client, string reason)
        {
            string nick = client.Player.PlayerCharacter.NickName;
            string zone = client.Player.ZoneName;
            string user = client.Player.PlayerCharacter.UserName;

            client.Player.SendMessage(
                "Hile kullanımı tespit edildi. Hesabınız 20 dakika askıya alındı.");

            _ = WorldMgr.SendSysNotice(
                $"[Sistem] [{zone}] bölgesindeki [{nick}] adlı oyuncu " +
                $"hile ({reason}) kullanırken yakalandı ve uzaklaştırıldı.");

            Console.WriteLine($"[AntiCheat][BAN] {user} | Sebep: {reason}");

            client.Player.AddLog("BAN", $"Anti-cheat ban: {reason}");
            _ = client.Player.SaveIntoDatabase();
            _ = client.Player.SavePlayerInfo();

            using (ManageBussiness mnBusiness = new())
            {
                _ = mnBusiness.ForbidPlayerByUserID(
                    client.Player.PlayerCharacter.ID,
                    DateTime.Now.AddMinutes(BAN_MINUTES),
                    true,
                    $"AntiCheat: {reason}");
            }

            client.Disconnect();
        }

        // ----------------------------------------------------------------
        // YARDIMCI METOTLAR
        // ----------------------------------------------------------------
        private static void SendAck(GameClient client, int subCode)
        {
            GSPacketIn pkg = new(300);
            pkg.WriteInt(subCode);
            client.Out.SendTCP(pkg);
        }

        private static void LogSuspicious(GameClient client, string msg)
        {
            string user = client.Player?.PlayerCharacter?.UserName ?? "?";
            Console.WriteLine($"[AntiCheat][Şüpheli] {user}: {msg}");
            client.Player?.AddLog("Suspicious", msg);
        }

        private static long GetUnixNow()
            => (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        // ----------------------------------------------------------------
        // İhlal kayıt modeli
        // ----------------------------------------------------------------
        private class ViolationRecord
        {
            public int Count { get; set; }
            public long FirstTime { get; set; }
            public string LastType { get; set; } = "";
        }
    }
}