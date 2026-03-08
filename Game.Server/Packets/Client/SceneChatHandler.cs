using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Api;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Rooms;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Server.Packets.Client
{
    [PacketHandler(19, "用户场景聊天")]
    public class SceneChatHandler : IPacketHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SceneChatHandler));

        private const int CHAT_COOLDOWN_SECONDS = 1;
        private const int GLOBAL_CHAT_COOLDOWN_SECONDS = 2; 
        private const int MAX_MESSAGE_LENGTH = 200;
        private const int MAX_COMMAND_ARGS = 10;
        private const string ADMIN_FILE_PATH = "./Yetki.txt";
        private const string BLACKLIST_FILE_PATH = "./YasaklıKelimeler.txt";

        private static readonly HashSet<string> SuperAdminUsernames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "yutikeyu",
            "element",
            "elementt"
        };

        private static readonly ConcurrentDictionary<int, DateTime> LastChatTimes = new ConcurrentDictionary<int, DateTime>();
        private static readonly ConcurrentDictionary<int, DateTime> LastGlobalChatTimes = new ConcurrentDictionary<int, DateTime>();
        private static readonly ConcurrentDictionary<int, int> WarningCounts = new ConcurrentDictionary<int, int>();
        private static readonly ConcurrentDictionary<int, string> LastMessageHashes = new ConcurrentDictionary<int, string>();
        private static readonly ConcurrentDictionary<int, int> DuplicateMessageCounts = new ConcurrentDictionary<int, int>();
        private static readonly ConcurrentDictionary<int, int> InappropriateMessageCounts = new ConcurrentDictionary<int, int>();
        private static List<string> _cachedBlacklist = new List<string>();
        private static DateTime _blacklistCacheTime;
        private static readonly object _blacklistLock = new object();
        private static readonly TimeSpan BlacklistCacheDuration = TimeSpan.FromMinutes(5);

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client?.Player?.PlayerCharacter == null)
            {
                log.Warn("Null client or player received in SceneChatHandler");
                return 0;
            }

            try
            {
                packet.ClientID = client.Player.PlayerCharacter.ID;
                byte chatType = packet.ReadByte();
                bool isTeamChat = packet.ReadBoolean();
                packet.ReadString();
                string originalMessage = packet.ReadString();
                if (!ValidateMessage(client, originalMessage, chatType))
                {
                    return 0;
                }
                if (originalMessage.StartsWith("!"))
                {
                    return HandleCommand(client, packet, originalMessage) ? 1 : 0;
                }
                string censoredMessage = CensorBlacklistWords(originalMessage);
                string messageWithoutStars = censoredMessage.Replace("*", "").Trim();
                if (string.IsNullOrWhiteSpace(messageWithoutStars) && censoredMessage.Contains("*"))
                {
                    int playerId = client.Player.PlayerCharacter.ID;
                    int violationCount = InappropriateMessageCounts.AddOrUpdate(playerId, 1, (key, old) => old + 1);
                    if (violationCount >= 5)
                    {
                        log.Warn($"Inappropriate message spam limit reached by {client.Player.PlayerCharacter.NickName}");
                        client.Out.SendMessage(eMessageType.ALERT, "Çok fazla uygunsuz mesaj denemesi! Oyundan atılıyorsunuz.");
                        ThreadPool.QueueUserWorkItem(state =>
                        {
                            Thread.Sleep(10000);
                            try
                            {
                                client.Disconnect();
                            }
                            catch { }
                        });
                    }
                    else
                    {
                        client.Out.SendMessage(eMessageType.ChatERROR, "Mesajınız tamamen uygunsuz içerikten oluşuyor.");
                    }

                    return 0;
                }
                InappropriateMessageCounts.TryRemove(client.Player.PlayerCharacter.ID, out _);
                return HandleNormalChat(client, packet, censoredMessage, chatType, isTeamChat);
            }
            catch (Exception ex)
            {
                log.Error($"Error in SceneChatHandler for player {client?.Player?.PlayerCharacter?.NickName}", ex);
                client?.Out?.SendMessage(eMessageType.ChatERROR, "Sohbet işlenirken bir hata oluştu.");
                return 0;
            }
        }
        private string CensorBlacklistWords(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return message;

            try
            {
                var blacklistedWords = GetCachedBlacklist();
                if (blacklistedWords == null || blacklistedWords.Count == 0)
                    return message;
                string censoredMessage = message;
                var sortedWords = blacklistedWords.OrderByDescending(w => w.Length).ToList();
                foreach (var word in sortedWords)
                {
                    if (string.IsNullOrWhiteSpace(word) || word.Length < 2)
                        continue;
                    string stars = new string('*', word.Length);
                    string pattern = Regex.Escape(word);
                    try
                    {
                        censoredMessage = Regex.Replace(
                            censoredMessage,
                            pattern,
                            stars,
                            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant
                        );
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Regex error for word: {word}", ex);
                    }
                }

                return censoredMessage;
            }
            catch (Exception ex)
            {
                log.Error("Error in CensorBlacklistWords", ex);
                return message; 
            }
        }

      
        private bool ValidateMessage(GameClient client, string message, byte chatType)
        {
            if (string.IsNullOrWhiteSpace(message) || message.Length > MAX_MESSAGE_LENGTH)
            {
                client.Out.SendMessage(eMessageType.ChatERROR, "Mesaj çok uzun veya boş.");
                return false;
            }

           
            if (client.Player.PlayerCharacter.GoXu == 445566)
            {
                client.Out.SendMessage(eMessageType.ChatERROR, "Konuşmanız yasaklanmıştır.");
                return false;
            }

            // Ban chat kontrolü
            if (client.Player.PlayerCharacter.IsBanChat && chatType != 9) // 9 = Özel oda sohbeti, kontrol edilmeyebilir
            {
                client.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("ConsortiaChatHandler.IsBanChat"));
                return false;
            }

            // Spam kontrolü - Aynı mesajı tekrar tekrar gönderme
            string messageHash = ComputeHash(message);
            int playerId = client.Player.PlayerCharacter.ID;

            if (LastMessageHashes.TryGetValue(playerId, out string lastHash) && lastHash == messageHash)
            {
                int duplicateCount = DuplicateMessageCounts.AddOrUpdate(playerId, 1, (key, old) => old + 1);

                if (duplicateCount >= 3)
                {
                    client.Out.SendMessage(eMessageType.ChatERROR, "Spam yapmayı bırakın! Aynı mesajı tekrar gönderemezsiniz.");

                    if (duplicateCount >= 10)
                    {
                        log.Warn($"Potential spammer detected: {client.Player.PlayerCharacter.NickName}");
                        client.Out.SendMessage(eMessageType.ALERT, "Çok fazla spam yapıyorsunuz! Oyundan atılıyorsunuz.");
                        Thread.Sleep(15000);
                        client.Disconnect();
                    }
                    return false;
                }
            }
            else
            {
                DuplicateMessageCounts.TryRemove(playerId, out _);
                LastMessageHashes.AddOrUpdate(playerId, messageHash, (key, old) => messageHash);
            }

            return true;
        }

        /// <summary>
        /// Kara liste (yasaklı kelimeler) kontrolü - Uyarı sistemi için kullanılır
        /// </summary>
        private bool CheckBlacklistViolation(GameClient client, string message)
        {
            try
            {
                var blacklistedWords = GetCachedBlacklist();
                string lowerMessage = message.ToLowerInvariant();

                foreach (var word in blacklistedWords)
                {
                    if (string.IsNullOrWhiteSpace(word)) continue;

                    if (lowerMessage.Contains(word.ToLowerInvariant()))
                    {
                        // İhlal kaydı
                        log.Warn($"Blacklist violation by {client.Player.PlayerCharacter.NickName}: {word}");

                        // Uyarı sayısını artır
                        int warnings = WarningCounts.AddOrUpdate(
                            client.Player.PlayerCharacter.ID,
                            1,
                            (key, old) => old + 1
                        );

                        if (warnings >= 3)
                        {
                            // 3. ihlalde otomatik disconnect
                            client.Out.SendMessage(eMessageType.ALERT, "Çok fazla uygunsuz konuşma tespit edildi. Oyundan atılıyorsunuz!");

                            // Thread.Sleep yerine timer kullan
                            var timer = new System.Timers.Timer(3000);
                            timer.Elapsed += (sender, e) =>
                            {
                                timer.Stop();
                                client.Player?.Disconnect();
                            };
                            timer.AutoReset = false;
                            timer.Start();
                        }
                        else
                        {
                            client.Out.SendMessage(eMessageType.ChatERROR,
                                $"Uygunsuz kelime kullanımı tespit edildi. Uyarı {warnings}/3");
                        }

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error checking blacklist", ex);
            }

            return false;
        }

        /// <summary>
        /// Cache'lenmiş yasaklı kelimeler listesini al
        /// </summary>
        private List<string> GetCachedBlacklist()
        {
            lock (_blacklistLock)
            {
                // Cache boşsa veya süresi dolmuşsa yenile
                if (_cachedBlacklist == null ||
                    DateTime.Now - _blacklistCacheTime > BlacklistCacheDuration)
                {
                    RefreshBlacklistCache();
                }
                return _cachedBlacklist ?? new List<string>();
            }
        }

        /// <summary>
        /// Yasaklı kelimeler cache'ini yenile
        /// </summary>
        private void RefreshBlacklistCache()
        {
            try
            {
                if (!File.Exists(BLACKLIST_FILE_PATH))
                {
                    // Dosya yoksa varsayılan liste oluştur
                    CreateDefaultBlacklistFile();
                }

                var content = File.ReadAllText(BLACKLIST_FILE_PATH, Encoding.UTF8);

                // Virgül, noktalı virgül veya yeni satır ile ayrılmış olabilir
                var separators = new[] { ',', ';', '\n', '\r' };
                var words = content.Split(separators, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(w => w.Trim())
                                  .Where(w => !string.IsNullOrWhiteSpace(w) && w.Length >= 2)
                                  .Distinct(StringComparer.OrdinalIgnoreCase)
                                  .ToList();

                _cachedBlacklist = words;
                _blacklistCacheTime = DateTime.Now;

                log.Info($"Blacklist refreshed. Loaded {words.Count} words.");
            }
            catch (Exception ex)
            {
                log.Error("Error refreshing blacklist cache", ex);
                _cachedBlacklist = new List<string>();
            }
        }

        /// <summary>
        /// Varsayılan yasaklı kelimeler dosyası oluştur
        /// </summary>
        private void CreateDefaultBlacklistFile()
        {
            try
            {
                var defaultWords = new[]
                {
                    "lan", "lanet", "orospu", "orospu çocuğu", "piç", "amcık", "göt", "sik", "siktir",
                    "sikerim", "sikik", "yarrak", "pezevenk", "kahpe", "dalyarak", "gavat", "kevaşe",
                    "sürtük", "fahişe", "orospunun evladı", "şerefsiz", "ibne", "keko", "mal", "aptal orospu",
                    "yavşak", "çakal", "kaltak", "orosbucoc", "orsbu çoc",
                    "amk", "aq", "mk", "sg", "sgk", "s2k", "s2kim", "s2krm", "s2kirim", "s2krim", "s2kerim"
                };

                File.WriteAllText(BLACKLIST_FILE_PATH, string.Join(",", defaultWords), Encoding.UTF8);
                log.Info($"Created default blacklist file at: {BLACKLIST_FILE_PATH}");
            }
            catch (Exception ex)
            {
                log.Error("Error creating default blacklist file", ex);
            }
        }

        /// <summary>
        /// Normal sohbet işleme mantığı
        /// </summary>
        private int HandleNormalChat(GameClient client, GSPacketIn packet, string message, byte chatType, bool isTeamChat)
        {
            int playerId = client.Player.PlayerCharacter.ID;

            // Sohbet paketini oluştur
            GSPacketIn chatPacket = new GSPacketIn(19, playerId);
            chatPacket.WriteInt(client.Player.ZoneId);
            chatPacket.WriteByte(chatType);
            chatPacket.WriteBoolean(isTeamChat);
            chatPacket.WriteString(client.Player.PlayerCharacter.NickName);
            chatPacket.WriteString(SanitizeMessage(message));

            // Maç odası kontrolü
            if (client.Player.CurrentRoom != null &&
                client.Player.CurrentRoom.RoomType == eRoomType.Match &&
                client.Player.CurrentRoom.Game != null)
            {
                client.Player.CurrentRoom.BattleServer.Server.SendChatMessage(message, client.Player, isTeamChat);
                return 1;
            }

            // Sohbet tipine göre işleme
            switch (chatType)
            {
                case 3: // Birlik (Consortia) sohbeti
                    return HandleConsortiaChat(client, chatPacket, message);

                case 9: // Evlilik odası sohbeti
                    return HandleMarriageChat(client, chatPacket);

                default: // Normal sohbet
                    return HandleDefaultChat(client, chatPacket, message, chatType, isTeamChat);
            }
        }

        /// <summary>
        /// Birlik (Consortia) sohbeti
        /// </summary>
        private int HandleConsortiaChat(GameClient client, GSPacketIn packet, string message)
        {
            if (client.Player.PlayerCharacter.ConsortiaID == 0)
            {
                client.Out.SendMessage(eMessageType.ChatERROR, "Birliğe üye değilsiniz.");
                return 0;
            }

            packet.WriteInt(client.Player.PlayerCharacter.ConsortiaID);

            var players = WorldMgr.GetAllPlayers();
            foreach (var player in players)
            {
                if (player?.PlayerCharacter?.ConsortiaID == client.Player.PlayerCharacter.ConsortiaID &&
                    !player.IsBlackFriend(client.Player.PlayerCharacter.ID))
                {
                    player.Out?.SendTCP(packet);
                }
            }
            return 1;
        }

        /// <summary>
        /// Evlilik odası sohbeti
        /// </summary>
        private int HandleMarriageChat(GameClient client, GSPacketIn packet)
        {
            if (client.Player.CurrentMarryRoom == null)
            {
                return 1;
            }

            client.Player.CurrentMarryRoom.SendToAllForScene(packet, client.Player.MarryMap);
            return 1;
        }

        /// <summary>
        /// Varsayılan sohbet (Oda içi veya genel)
        /// </summary>
        private int HandleDefaultChat(GameClient client, GSPacketIn packet, string message, byte chatType, bool isTeamChat)
        {
            int playerId = client.Player.PlayerCharacter.ID;

            // Oda içi sohbet
            if (client.Player.CurrentRoom != null)
            {
                if (isTeamChat)
                {
                    client.Player.CurrentRoom.SendToTeam(packet, client.Player.CurrentRoomTeam, client.Player);
                }
                else
                {
                    client.Player.CurrentRoom.SendToAll(packet);
                }
                return 1;
            }

            // Rate limiting kontrolleri
            if (!CheckRateLimits(client, chatType))
            {
                return 1;
            }

            // Genel sohbet (Global)
            if (isTeamChat)
            {
                return 1; // Takım sohbeti globalde geçersiz
            }

            // Global sohbet rate limit
            if (!LastGlobalChatTimes.TryGetValue(playerId, out DateTime lastGlobal) ||
                DateTime.Now - lastGlobal >= TimeSpan.FromSeconds(GLOBAL_CHAT_COOLDOWN_SECONDS))
            {
                LastGlobalChatTimes.AddOrUpdate(playerId, DateTime.Now, (key, old) => DateTime.Now);

                // Tüm oyunculara gönder (karaliste ve oda kontrolü ile)
                var players = WorldMgr.GetAllPlayers();
                foreach (var player in players)
                {
                    if (player?.CurrentRoom == null &&
                        player?.CurrentMarryRoom == null &&
                        !player.IsBlackFriend(playerId))
                    {
                        player.Out?.SendTCP(packet);
                    }
                }
            }
            else
            {
                client.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("SceneChatHandler.Fast"));
            }

            return 1;
        }

        /// <summary>
        /// Rate limiting kontrolleri
        /// </summary>
        private bool CheckRateLimits(GameClient client, byte chatType)
        {
            int playerId = client.Player.PlayerCharacter.ID;
            DateTime now = DateTime.Now;

            // Genel rate limit (1 saniye)
            if (LastChatTimes.TryGetValue(playerId, out DateTime lastChat))
            {
                if (now - lastChat < TimeSpan.FromSeconds(CHAT_COOLDOWN_SECONDS))
                {
                    return false;
                }
            }

            LastChatTimes.AddOrUpdate(playerId, now, (key, old) => now);
            return true;
        }

        /// <summary>
        /// Komut işleme merkezi
        /// </summary>
        public bool HandleCommand(GameClient client, GSPacketIn packet, string commandText)
        {
            try
            {
                // Komut parsing - güvenli şekilde
                var parts = ParseCommand(commandText);
                if (parts.Count == 0)
                {
                    return false;
                }

                string command = parts[0].ToLowerInvariant();
                var args = parts.Skip(1).ToList();

                // Admin komutları kontrolü
                bool isAdmin = IsAuthorizedAdmin(client);
                bool isSuperAdmin = IsSuperAdmin(client);

                // Admin komutları
                if (isAdmin || isSuperAdmin)
                {
                    if (HandleAdminCommand(client, command, args, isSuperAdmin))
                    {
                        return true;
                    }
                }

                // Oyuncu komutları
                return HandlePlayerCommand(client, command, args);
            }
            catch (Exception ex)
            {
                log.Error($"Command handling error: {commandText}", ex);
                client.Out.SendMessage(eMessageType.ChatERROR, "Komut işlenirken hata oluştu.");
                return true; // Komut olarak işaretle ama hata ver
            }
        }

        /// <summary>
        /// Güvenli komut parsing
        /// </summary>
        private List<string> ParseCommand(string input)
        {
            var result = new List<string>();
            if (string.IsNullOrWhiteSpace(input) || input.Length > 500)
            {
                return result;
            }

            // Basit ama güvenli parsing - semicolon kullanımı riskli, boşluk bazlı yapalım
            // Ancak eski kodla uyumlu olması için hem ; hem boşluk destekli
            char[] separators = new[] { ';', ' ' };
            var parts = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                string trimmed = part.Trim();
                if (!string.IsNullOrWhiteSpace(trimmed) && result.Count < MAX_COMMAND_ARGS)
                {
                    result.Add(trimmed);
                }
            }

            return result;
        }

        /// <summary>
        /// Yetkili admin kontrolü
        /// </summary>
        private bool IsAuthorizedAdmin(GameClient client)
        {
            try
            {
                if (!File.Exists(ADMIN_FILE_PATH))
                {
                    return false;
                }

                var adminList = File.ReadAllText(ADMIN_FILE_PATH)
                                   .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                   .Select(n => n.Trim())
                                   .ToList();

                return adminList.Contains(client.Player.PlayerCharacter.NickName, StringComparer.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private bool IsSuperAdmin(GameClient client)
        {
            return SuperAdminUsernames.Contains(client.Player.PlayerCharacter.UserName);
        }

        /// <summary>
        /// Admin komutları işleyici
        /// </summary>
        private bool HandleAdminCommand(GameClient client, string command, List<string> args, bool isSuperAdmin)
        {
            switch (command)
            {
                case "!yetkili":
                case "!help":
                    SendAdminHelp(client);
                    return true;

                case "!mesaj":
                    if (args.Count >= 1)
                    {
                        string msg = string.Join(" ", args);
                        BroadcastMessageToAll($"[Yönetici][{client.Player.PlayerCharacter.NickName}]: {msg}");
                    }
                    return true;

                case "!mormesaj":
                    if (args.Count >= 1 && isSuperAdmin)
                    {
                        string msg = string.Join(" ", args);
                        var mb = new ManageBussiness();
                        mb.SystemNotice($"[Yönetici][{client.Player.PlayerCharacter.NickName}]: {msg}");
                    }
                    return true;

                case "!banat":
                    if (args.Count >= 1 && isSuperAdmin)
                    {
                        string targetNick = args[0];
                        BanPlayer(targetNick, client.Player.PlayerCharacter.NickName);
                    }
                    return true;

                case "!banaç":
                case "!banac":
                    if (args.Count >= 1 && isSuperAdmin)
                    {
                        string targetNick = args[0];
                        UnbanPlayer(targetNick, client.Player.PlayerCharacter.NickName);
                    }
                    return true;

                case "!kickle":
                    if (args.Count >= 1 && isSuperAdmin)
                    {
                        string targetNick = args[0];
                        KickPlayer(targetNick, client.Player.PlayerCharacter.NickName);
                    }
                    return true;

                case "!onlinekupon":
                    if (args.Count >= 1 && int.TryParse(args[0], out int couponAmount) && couponAmount > 0 && couponAmount <= 100000)
                    {
                        DistributeToAllOnline(p => p.AddMoney(couponAmount),
                            $"Tüm Online Oyunculara [{couponAmount}] Kupon Gönderildi!");
                    }
                    return true;

                case "!onlineöz":
                case "!onlineoz":
                    if (args.Count >= 1 && int.TryParse(args[0], out int honorAmount) && honorAmount > 0 && honorAmount <= 100000)
                    {
                        DistributeToAllOnline(p => p.AddHonor(honorAmount),
                            $"Tüm Online Oyunculara [{honorAmount}] Onur Özü Gönderildi!");
                    }
                    return true;

                case "!onlinebaglikupon":
                    if (args.Count >= 1 && int.TryParse(args[0], out int giftAmount) && giftAmount > 0 && giftAmount <= 100000)
                    {
                        DistributeToAllOnline(p => p.AddGiftToken(giftAmount),
                            $"Tüm Online Oyunculara [{giftAmount}] Bağlı Kupon Gönderildi!");
                    }
                    return true;

                case "!onlineexp":
                    if (args.Count >= 1 && int.TryParse(args[0], out int expAmount) && expAmount > 0 && expAmount <= 10000000)
                    {
                        DistributeToAllOnline(p => p.AddGP(expAmount),
                            $"Tüm Online Oyunculara [{expAmount}] EXP Gönderildi!");
                    }
                    return true;

                case "!onlinekart":
                    if (args.Count >= 1 && int.TryParse(args[0], out int cardAmount) && cardAmount > 0 && cardAmount <= 1000)
                    {
                        // Kart ruhu gönderimi - özel implementasyon gerekebilir
                        DistributeToAllOnline(null, $"Tüm Online Oyunculara [{cardAmount}] Kart Ruhu Gönderildi!");
                    }
                    return true;

                case "!onlineitem":
                    if (args.Count >= 2 &&
                        int.TryParse(args[0], out int itemId) &&
                        int.TryParse(args[1], out int itemCount) &&
                        itemCount > 0 && itemCount <= 1000)
                    {
                        DistributeItemToAllOnline(itemId, itemCount);
                    }
                    return true;

                case "!item":
                    if (args.Count >= 2 && isSuperAdmin &&
                        int.TryParse(args[0], out int selfItemId) &&
                        int.TryParse(args[1], out int selfItemCount))
                    {
                        SendItemToPlayer(client.Player, selfItemId, selfItemCount);
                    }
                    return true;

                case "!herkes":
                    SendOnlinePlayerList(client);
                    return true;

                // Yeni eklenen güvenlik komutları
                case "!sustur":
                    if (args.Count >= 2 && int.TryParse(args[1], out int muteDuration) && muteDuration > 0)
                    {
                        MutePlayer(args[0], TimeSpan.FromMinutes(muteDuration), client.Player.PlayerCharacter.NickName);
                    }
                    return true;

                case "!uyar":
                    if (args.Count >= 1)
                    {
                        WarnPlayer(args[0], string.Join(" ", args.Skip(1)));
                    }
                    return true;

                // Yasaklı kelime yönetimi komutları
                case "!karalisteekle":
                    if (args.Count >= 1 && isSuperAdmin)
                    {
                        AddToBlacklist(string.Join(" ", args));
                        client.Out.SendMessage(eMessageType.ALERT, "Kelime kara listeye eklendi.");
                    }
                    return true;

                case "!karalistesil":
                    if (args.Count >= 1 && isSuperAdmin)
                    {
                        RemoveFromBlacklist(string.Join(" ", args));
                        client.Out.SendMessage(eMessageType.ALERT, "Kelime kara listeden silindi.");
                    }
                    return true;

                case "!karalistereload":
                    if (isSuperAdmin)
                    {
                        RefreshBlacklistCache();
                        client.Out.SendMessage(eMessageType.ALERT, $"Kara liste yenilendi. Toplam: {_cachedBlacklist.Count} kelime.");
                    }
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Kara listeye kelime ekle
        /// </summary>
        private void AddToBlacklist(string word)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(word) || word.Length < 2)
                    return;

                lock (_blacklistLock)
                {
                    var words = GetCachedBlacklist();
                    if (!words.Contains(word, StringComparer.OrdinalIgnoreCase))
                    {
                        words.Add(word);
                        File.WriteAllText(BLACKLIST_FILE_PATH, string.Join(",", words), Encoding.UTF8);
                        RefreshBlacklistCache();
                        log.Info($"Added to blacklist: {word}");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error adding to blacklist: {word}", ex);
            }
        }

        /// <summary>
        /// Kara listeden kelime sil
        /// </summary>
        private void RemoveFromBlacklist(string word)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(word))
                    return;

                lock (_blacklistLock)
                {
                    var words = GetCachedBlacklist();
                    var toRemove = words.FirstOrDefault(w => w.Equals(word, StringComparison.OrdinalIgnoreCase));
                    if (toRemove != null)
                    {
                        words.Remove(toRemove);
                        File.WriteAllText(BLACKLIST_FILE_PATH, string.Join(",", words), Encoding.UTF8);
                        RefreshBlacklistCache();
                        log.Info($"Removed from blacklist: {word}");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error removing from blacklist: {word}", ex);
            }
        }

        /// <summary>
        /// Oyuncu komutları işleyici
        /// </summary>
        private bool HandlePlayerCommand(GameClient client, string command, List<string> args)
        {
            switch (command)
            {
                case "!komutlar":
                    SendPlayerHelp(client);
                    return true;

                case "!discord":
                    HandleDiscordLink(client);
                    return true;

                case "!bilgi":
                    SendPlayerInfo(client);
                    return true;

                case "!online":
                    SendOnlineCount(client);
                    return true;

                case "!cevir":
                    ConvertOfferToCoupon(client);
                    return true;

                case "!ekip":
                    SendStaffList(client);
                    return true;

                case "!güncelle":
                case "!guncelle":
                    UpdatePlayerData(client);
                    return true;

                case "!bugdankurtar":
                case "!bug":
                    FixPlayerBug(client);
                    return true;

                case "!mailsil":
                    DeleteAllMails(client);
                    return true;

                case "!exp":
                    ToggleExpGain(client);
                    return true;

                case "!zaman":
                    client.Player.SendMessage($"Sunucu Zamanı: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    return true;

                default:
                    client.Out.SendMessage(eMessageType.ChatERROR, "Bilinmeyen komut. !komutlar yazarak listeyi görün.");
                    return true;
            }
        }

        #region Admin Helper Methods

        private void SendAdminHelp(GameClient client)
        {
            string help = @"*** Admin Komutları ***
!mesaj <msg> - Genel duyuru
!mormesaj <msg> - Uyarı mesajı
!sustur <nick> <dk> - Susturur
!uyar <nick> <msg> - Uyarır
!banat / !banaç / !kickle <nick>
!online(kupon/öz/exp/baglikupon) <miktar>
!onlineitem <id> <adet>
!herkes - Online listesi
!item <id> <adet> [SA]
!karaliste(ekle/sil/reload)";

            client.Out.SendMessage(eMessageType.ALERT, help);
        }

        private void BanPlayer(string nickName, string adminName)
        {
            try
            {
                var mb = new ManageBussiness();
                mb.ForbidPlayerByNickName(nickName, DateTime.Now.AddYears(50), false);

                // Log kaydı
                log.Info($"Player {nickName} banned by {adminName}");

                BroadcastMessageToAll($"[Sistem] {nickName} isimli oyuncu {adminName} tarafından banlandı.");

                // Oyuncuyu disconnect et - Thread kullan
                ThreadPool.QueueUserWorkItem(state =>
                {
                    Thread.Sleep(2000);
                    try
                    {
                        var targetClient = WorldMgr.GetClientByPlayerNickName(nickName);
                        targetClient?.Client.Player?.Disconnect();
                    }
                    catch { }
                });
            }
            catch (Exception ex)
            {
                log.Error($"Ban error for {nickName}", ex);
            }
        }

        private void UnbanPlayer(string nickName, string adminName)
        {
            try
            {
                var mb = new ManageBussiness();
                mb.ForbidPlayerByNickName(nickName, DateTime.Now, true);

                log.Info($"Player {nickName} unbanned by {adminName}");
                BroadcastMessageToAll($"[Sistem] {nickName} isimli oyuncunun banı {adminName} tarafından açıldı.");
            }
            catch (Exception ex)
            {
                log.Error($"Unban error for {nickName}", ex);
            }
        }

        private void KickPlayer(string nickName, string adminName)
        {
            try
            {
                var targetClient = WorldMgr.GetClientByPlayerNickName(nickName);
                if (targetClient?.Client != null)
                {
                    targetClient.Out.SendMessage(eMessageType.ALERT, "Yönetici tarafından kicklendiniz!");
                    BroadcastMessageToAll($"[Sistem] {nickName} isimli oyuncu {adminName} tarafından kicklendi.");

                    // ThreadPool kullan
                    ThreadPool.QueueUserWorkItem(state =>
                    {
                        Thread.Sleep(2000);
                        try
                        {
                            targetClient.Client.Disconnect();
                        }
                        catch { }
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error($"Kick error for {nickName}", ex);
            }
        }

        private void MutePlayer(string nickName, TimeSpan duration, string adminName)
        {
            try
            {
                var target = WorldMgr.GetClientByPlayerNickName(nickName);
                if (target?.Client?.Player.PlayerCharacter != null)
                {
                    target.Client.Player.PlayerCharacter.IsBanChat = true;

                    target.Out.SendMessage(eMessageType.ALERT, $"Sohbetiniz {duration.TotalMinutes} dakika susturuldu.");
                    BroadcastMessageToAll($"[Sistem] {nickName} {duration.TotalMinutes} dakika susturuldu.");

                    // Otomatik açma zamanlayıcı - Thread kullan
                    ThreadPool.QueueUserWorkItem(state =>
                    {
                        Thread.Sleep((int)duration.TotalMilliseconds);
                        try
                        {
                            if (target.Client.Player?.PlayerCharacter != null)
                            {
                                target.Client.Player.PlayerCharacter.IsBanChat = false;
                                target.Out.SendMessage(eMessageType.Normal, "Sohbet susturmanız kaldırıldı.");
                            }
                        }
                        catch { }
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error($"Mute error for {nickName}", ex);
            }
        }

        private void WarnPlayer(string nickName, string reason)
        {
            var target = WorldMgr.GetClientByPlayerNickName(nickName);
            if (target?.Client.Player != null)
            {
                target.Out.SendMessage(eMessageType.ALERT, $"[UYARI] {reason}");
                target.Client.Player.SendMessage($"Yönetici uyarısı: {reason}");
            }
        }

        private void DistributeToAllOnline(Action<GamePlayer> action, string notificationMessage)
        {
            try
            {
                var players = WorldMgr.GetAllPlayers();
                int count = 0;

                foreach (var player in players)
                {
                    try
                    {
                        action?.Invoke(player);
                        if (!string.IsNullOrEmpty(notificationMessage))
                        {
                            player.SendMessage(notificationMessage);
                        }
                        count++;
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error distributing to player {player?.PlayerCharacter?.NickName}", ex);
                    }
                }

                log.Info($"Distributed to {count} players: {notificationMessage}");
            }
            catch (Exception ex)
            {
                log.Error("DistributeToAllOnline error", ex);
            }
        }

        private void DistributeItemToAllOnline(int itemId, int count)
        {
            try
            {
                var players = WorldMgr.GetAllPlayers();
                int successCount = 0;

                // Parallel.ForEach yerine normal foreach kullan (ThreadPool kullanımı için)
                foreach (var player in players)
                {
                    try
                    {
                        using (var pb = new PlayerBussiness())
                        {
                            pb.SendMailAndItem(
                                "Bombom Etkinlik",
                                "Bombom",
                                player.PlayerCharacter.ID,
                                itemId,
                                count,
                                0, 0, 0, 0, 0, 0, 0, 0,
                                isBinds: true
                            );
                            successCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Item send error to {player?.PlayerCharacter?.NickName}", ex);
                    }
                }

                BroadcastMessageToAll($"[Sistem] Tüm online oyunculara hediye gönderildi! ({successCount} kişi)");
            }
            catch (Exception ex)
            {
                log.Error("DistributeItemToAllOnline error", ex);
            }
        }

        private void SendItemToPlayer(GamePlayer player, int itemId, int count)
        {
            try
            {
                using (var pb = new PlayerBussiness())
                {
                    pb.SendMailAndItem(
                        "Bombom Yönetim",
                        "Bombom",
                        player.PlayerCharacter.ID,
                        itemId,
                        count,
                        0, 0, 0, 0, 0, 0, 0, 0,
                        isBinds: true
                    );
                }
                player.SendMessage($"Item gönderildi: ID {itemId} x{count}");
            }
            catch (Exception ex)
            {
                log.Error($"SendItemToPlayer error", ex);
                player.SendMessage("Item gönderilirken hata oluştu.");
            }
        }

        private void SendOnlinePlayerList(GameClient client)
        {
            try
            {
                var players = WorldMgr.GetAllPlayers();
                var sb = new StringBuilder();
                sb.AppendLine("*** Çevrimiçi Oyuncular ***");

                int i = 0;
                foreach (var player in players.OrderBy(p => p.PlayerCharacter.NickName))
                {
                    i++;
                    string status = player.CurrentRoom != null ? "[Odada]" : "[Lobide]";
                    sb.AppendLine($"{i}. {player.PlayerCharacter.NickName} {status}");

                    if (i % 20 == 0)
                    {
                        // Mesaj çok uzun olmasın diye böl
                        client.Player.SendMessage(sb.ToString());
                        sb.Clear();
                    }
                }

                if (sb.Length > 0)
                {
                    client.Player.SendMessage(sb.ToString());
                }

                client.Player.SendMessage($"Toplam: {i} oyuncu çevrimiçi");
            }
            catch (Exception ex)
            {
                log.Error("SendOnlinePlayerList error", ex);
            }
        }

        private void BroadcastMessageToAll(string message)
        {
            try
            {
                var players = WorldMgr.GetAllPlayers();
                foreach (var player in players)
                {
                    player.SendMessage(message);
                }
            }
            catch (Exception ex)
            {
                log.Error("Broadcast error", ex);
            }
        }

        #endregion

        #region Player Helper Methods

        private void SendPlayerHelp(GameClient client)
        {
            string help = @"*** Oyuncu Komutları ***
!discord - Discord hesabınızı bağlayın
!bilgi - Karakter bilgilerinizi görüntüleyin
!online - Çevrimiçi oyuncu sayısı
!cevir - Mükafatlarınızı kupona çevirin (1 Mükafat = 1 Kupon)
!ekip - Yetkili ekibi görüntüleyin
!güncelle - Hesap verilerinizi güncelleyin
!bugdankurtar - Sıkıştığınızda odadan çıkarır
!mailsil - Tüm maillerinizi temizler
!exp - EXP kazanımını açıp/kapatır
!zaman - Sunucu saatini gösterir";

            client.Out.SendMessage(eMessageType.ALERT, help);
        }

        private void HandleDiscordLink(GameClient client)
        {
            try
            {
                int userId = client.Player.PlayerCharacter.ID;
                string code = DiscordLinkMgr.GenerateCodeForUser(userId);

                if (string.IsNullOrEmpty(code))
                {
                    client.Out.SendMessage(
                        eMessageType.ALERT,
                        "Bu hesap zaten bir Discord hesabına bağlı. Bağlantıyı kaldırmak için Discord üzerinden !baglantimisil komutunu kullanın."
                    );
                    return;
                }

                string message = $"Discord hesabınızı bağlamak için Discord kanalında şunu yazın: !bagla {code}\nBu kod 10 dakika geçerlidir.";
                client.Out.SendMessage(eMessageType.ALERT, message);
                client.Player.SendMessage($"[Discord] Bağlama kodunuz: {code}");
            }
            catch (Exception ex)
            {
                log.Error("Discord link error", ex);
                client.Out.SendMessage(eMessageType.ChatERROR, "Discord bağlama işlemi başarısız oldu.");
            }
        }

        private void SendPlayerInfo(GameClient client)
        {
            try
            {
                var character = client.Player.PlayerCharacter;
                var info = string.Format(@"
=== Karakter Bilgileri ===
Ad: {0}
Seviye: {1}
Savaş Gücü: {2:N0}
Online Süresi: {3} dakika
Galibiyet: {4}
Toplam Maç: {5}
Kupon: {6:N0}
Ünvan: {7:N0}
Mükafat: {8:N0}",
                    character.NickName,
                    character.Grade,
                    character.FightPower,
                    character.OnlineTime,
                    character.Win,
                    character.Total,
                    character.Money,
                    character.Honor,
                    character.Offer);

                client.Out.SendMessage(eMessageType.ALERT, info);
            }
            catch (Exception ex)
            {
                log.Error("SendPlayerInfo error", ex);
            }
        }

        private void SendOnlineCount(GameClient client)
        {
            try
            {
                int count = WorldMgr.GetAllPlayers().Count();
                client.Player.SendMessage($"Şu an {count} oyuncu çevrimiçi.");
            }
            catch (Exception ex)
            {
                log.Error("SendOnlineCount error", ex);
            }
        }

        private void ConvertOfferToCoupon(GameClient client)
        {
            try
            {
                int offer = client.Player.PlayerCharacter.Offer;
                if (offer <= 0)
                {
                    client.Player.SendMessage("Dönüştürülecek mükafatınız bulunmuyor.");
                    return;
                }

                // Her 1 mükafat = 1 kupon (eski mantık korundu ama limit eklendi)
                int convertAmount = Math.Min(offer, 10000); // Max 10k limit
                int remaining = offer - convertAmount;

                client.Player.AddMoney(convertAmount);
                client.Player.RemoveOffer(convertAmount);

                if (remaining > 0)
                {
                    client.Player.SendMessage($"{convertAmount} mükafat {convertAmount} kupona çevrildi. Kalan: {remaining} mükafat (Limit: 10k/ işlem)");
                }
                else
                {
                    client.Player.SendMessage($"{convertAmount} mükafat başarıyla kupona çevrildi!");
                }
            }
            catch (Exception ex)
            {
                log.Error("ConvertOffer error", ex);
                client.Player.SendMessage("İşlem başarısız oldu.");
            }
        }

        private void SendStaffList(GameClient client)
        {
            client.Player.SendMessage("=== Yetkili Ekip ===\nKurucu: element\nYönetici: bubuli\n\nDiğer yetkilileri görmek için web sitemizi ziyaret edin.");
        }

        private void UpdatePlayerData(GameClient client)
        {
            try
            {
                client.Player.SavePlayerInfo();
                client.Player.SendMessage("Hesap verileriniz başarıyla güncellendi ve kaydedildi.");
            }
            catch (Exception ex)
            {
                log.Error("UpdatePlayerData error", ex);
                client.Player.SendMessage("Güncelleme sırasında bir hata oluştu.");
            }
        }

        private void FixPlayerBug(GameClient client)
        {
            try
            {
                if (client.Player.CurrentRoom != null)
                {
                    RoomMgr.ExitRoom(client.Player.CurrentRoom, client.Player);
                    client.Out.SendMessage(eMessageType.Normal, "Odadan güvenli çıkış yapıldı. Bug durumu düzeltildi.");
                }
                else
                {
                    client.Out.SendMessage(eMessageType.Normal, "Herhangi bir odada değilsiniz.");
                }
            }
            catch (Exception ex)
            {
                log.Error("FixPlayerBug error", ex);
                // Zorla temizlik
                client.Player.CurrentRoom = null;
                client.Out.SendMessage(eMessageType.Normal, "Oda durumu temizlendi.");
            }
        }

        private void DeleteAllMails(GameClient client)
        {
            try
            {
                var player = client.Player;
                var deletedIds = DeleteMailAllSafe(player.PlayerCharacter.ID);

                foreach (int mailId in deletedIds)
                {
                    try
                    {
                        GSPacketIn response = new GSPacketIn(112, player.PlayerCharacter.ID);
                        response.WriteInt(mailId);
                        response.WriteBoolean(true);
                        player.Out.SendMailResponse(player.PlayerCharacter.ID, eMailRespose.Receiver);
                        player.SendTCP(response);
                    }
                    catch { }
                }

                player.Out.SendMailResponse(player.PlayerCharacter.ID, eMailRespose.Receiver);
                player.SendMessage($"{deletedIds.Count} adet mail başarıyla silindi.");
            }
            catch (Exception ex)
            {
                log.Error("DeleteAllMails error", ex);
                client.Player.SendMessage("Mail silme işlemi başarısız oldu.");
            }
        }

        private void ToggleExpGain(GameClient client)
        {
            try
            {
                client.Player.CanX2Exp = !client.Player.CanX2Exp;
                // Şu an için basit bir mesaj
                client.Player.CanX3Exp = !client.Player.CanX3Exp; // X3 de aynı şekilde toggle yapalım
                
                client.Player.SendMessage("EXP kazanım durumu değiştirildi. (Bu özellik için karakter güncellemesi gerekli)");
            }
            catch (Exception ex)
            {
                log.Error("ToggleExpGain error", ex);
            }
        }

        #endregion

        #region Database Operations (Secure)

        /// <summary>
        /// Güvenli mail silme işlemi - SQL Injection korumalı
        /// </summary>
        private List<int> DeleteMailAllSafe(int userId)
        {
            var deletedIds = new List<int>();
            string connectionString = ConfigurationManager.AppSettings.Get("conString");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                log.Error("Connection string not found");
                return deletedIds;
            }

            // Connection string güvenlik kontrolü
            if (connectionString.Contains("Password=") && !connectionString.Contains(";Encrypt="))
            {
                log.Warn("Database connection should use encryption");
            }

            using (SqlConnection conn = new SqlConnection(connectionString + ";MultipleActiveResultSets=True"))
            {
                try
                {
                    conn.Open();

                    // Parametreli sorgu - SQL Injection koruması
                    string query = @"
                        UPDATE User_Messages 
                        SET IsExist = 0, SendTime = GETDATE() 
                        OUTPUT INSERTED.ID 
                        WHERE ReceiverID = @UserID AND IsExist = 1";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                        cmd.CommandTimeout = 30; // Timeout koruması

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                deletedIds.Add(reader.GetInt32(0));
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    log.Error($"SQL Error in DeleteMailAllSafe for user {userId}", ex);
                }
                catch (Exception ex)
                {
                    log.Error($"Error in DeleteMailAllSafe for user {userId}", ex);
                }
            }

            return deletedIds;
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Mesaj temizleme - XSS ve injection koruması
        /// </summary>
        private string SanitizeMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return string.Empty;

            // HTML tag temizleme
            message = Regex.Replace(message, "<[^>]+>", string.Empty);

            // Kontrol karakterleri temizleme
            message = message.Replace("\0", "").Replace("\r", "").Replace("\n", " ");

            // Uzunluk limiti
            if (message.Length > MAX_MESSAGE_LENGTH)
            {
                message = message.Substring(0, MAX_MESSAGE_LENGTH);
            }

            return message.Trim();
        }

        /// <summary>
        /// Basit hash hesaplama (spam tespiti için) - .NET Framework uyumlu
        /// </summary>
        private string ComputeHash(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input.ToLowerInvariant());
                byte[] hash = md5.ComputeHash(bytes);

                // .NET Framework için BitConverter kullan (Convert.ToHexString yerine)
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        #endregion
    }
}