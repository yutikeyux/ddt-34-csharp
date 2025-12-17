using System;

namespace Bussiness.Managers
{
    public static class DiscordLinkMgr
    {
        private static readonly Random _rand = new Random();

        public static string GenerateCodeForUser(int userId)
        {
            using (var db = new ManageBussiness())
            {
                // 1) Hesap zaten Discord'a bağlıysa hiç kod üretme
                if (db.IsUserAlreadyLinked(userId))
                    return null;

                // 2) Aktif bir kod varsa, onu geri döndür (yeniden üretme)
                string active = db.GetActiveDiscordLinkCode(userId);
                if (!string.IsNullOrEmpty(active))
                    return active;

                // 3) Yeni kod üret
                string code = CreateCode(6);
                db.AddDiscordLinkCode(userId, code, DateTime.Now.AddMinutes(10));
                return code;
            }
        }

        private static string CreateCode(int len)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            char[] buffer = new char[len];

            lock (_rand)
            {
                for (int i = 0; i < len; i++)
                    buffer[i] = chars[_rand.Next(chars.Length)];
            }

            return new string(buffer);
        }

        public static int GetUserIdByDiscordId(long discordId)
        {
            using (var db = new ManageBussiness())
            {
                return db.GetUserIdByDiscordId(discordId);
            }
        }

        public static int ConsumeCodeAndBindUser(string code, long discordId)
        {
            using (var db = new ManageBussiness())
            {
                return db.ConsumeDiscordLinkCodeAndBindUser(code, discordId);
            }
        }

        public static System.Collections.Generic.List<ManageBussiness.DiscordLinkRecord> GetAllLinks(int top = 100)
        {
            using (var db = new ManageBussiness())
            {
                return db.GetDiscordLinks(top);
            }
        }

        public static int UnlinkByDiscordId(long discordId)
        {
            using (var db = new ManageBussiness())
            {
                return db.DeleteDiscordLinkByDiscordId(discordId);
            }
        }

        public static int UnlinkByUserId(int userId)
        {
            using (var db = new ManageBussiness())
            {
                return db.DeleteDiscordLinkByUserId(userId);
            }
        }

        public static int ClearAllLinks()
        {
            using (var db = new ManageBussiness())
            {
                return db.DeleteAllDiscordLinks();
            }
        }

    }
}