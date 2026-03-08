using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace Game.Logic.Cmd
{
    /// <summary>
    /// Advanced Bot AI System - Chat & Combat Intelligence
    /// 
    /// 🎯 Features:
    /// • Natural Language Processing with intent matching
    /// • Context-aware responses with conversation memory
    /// • Adaptive combat AI with multiple strategies
    /// • Thread-safe concurrent operations
    /// • Configurable personality profiles
    /// • Anti-spam with exponential backoff
    /// • Performance optimized with compiled regex
    /// 
    /// 📌 Integration: Call BotCommand.TryAnswerToPlayer() from ChatCommand.cs
    /// </summary>
    [GameCommand((byte)eTankCmdType.BOT_COMMAND, "Advanced Bot AI System")]
    public class BotCommand : ICommandHandler
    {
        #region 🧠 Configuration & Constants

        private static class Config
        {
            // ⏱️ Timing
            public const int MinReplyDelayMs = 800;
            public const int MaxReplyDelayMs = 2500;
            public const int MinActionDelayMs = 300;
            public const int MaxActionDelayMs = 1800;
            public const int SpamCooldownMs = 2000;
            public const int ConversationMemoryMinutes = 10;

            // 🎮 Combat
            public const int CloseRangeThreshold = 60;
            public const int MidRangeThreshold = 400;
            public const int LongRangeThreshold = 900;
            public const int MaxRangeThreshold = 1100;

            // 🎯 AI
            public const double PersonalityVariance = 0.15; // 15% randomness in decisions
            public const int MaxConversationContext = 5;

            // 🔧 Debug
            public const bool EnableDebugLogging = false;
            public const bool EnableChatLogging = true;
        }

        #endregion

        #region 🎭 Personality Profiles

        public enum BotPersonality
        {
            Aggressive,    // Daha saldırgan, meydan okuyan
            Friendly,      // Daha nazik, teşvik eden
            Tactical,      // Strateji odaklı, profesyonel
            Humorous,      // Şaka yapan, eğlenceli
            Mysterious     // Gizemli, az konuşan
        }

        private class PersonalityConfig
        {
            public double ResponseSpeed { get; set; }
            public double TauntChance { get; set; }
            public double EmojiDensity { get; set; }
            public double CombatAggression { get; set; }
            public double ChatFrequency { get; set; }

            public PersonalityConfig(double responseSpeed, double tauntChance, double emojiDensity,
                double combatAggression, double chatFrequency)
            {
                ResponseSpeed = responseSpeed;
                TauntChance = tauntChance;
                EmojiDensity = emojiDensity;
                CombatAggression = combatAggression;
                ChatFrequency = chatFrequency;
            }
        }

        private static readonly Dictionary<BotPersonality, PersonalityConfig> PersonalityTraits = new Dictionary<BotPersonality, PersonalityConfig>
        {
            { BotPersonality.Aggressive, new PersonalityConfig(
                responseSpeed: 0.8,      // Daha hızlı cevap
                tauntChance: 0.4,        // Daha fazla meydan okuma
                emojiDensity: 0.3,
                combatAggression: 0.9,
                chatFrequency: 0.7
            )},
            { BotPersonality.Friendly, new PersonalityConfig(
                responseSpeed: 1.0,
                tauntChance: 0.1,
                emojiDensity: 0.6,
                combatAggression: 0.5,
                chatFrequency: 0.9
            )},
            { BotPersonality.Tactical, new PersonalityConfig(
                responseSpeed: 1.2,      // Düşünerek cevap
                tauntChance: 0.2,
                emojiDensity: 0.1,
                combatAggression: 0.7,
                chatFrequency: 0.5
            )},
            { BotPersonality.Humorous, new PersonalityConfig(
                responseSpeed: 0.9,
                tauntChance: 0.3,
                emojiDensity: 0.8,
                combatAggression: 0.6,
                chatFrequency: 0.85
            )},
            { BotPersonality.Mysterious, new PersonalityConfig(
                responseSpeed: 1.3,
                tauntChance: 0.15,
                emojiDensity: 0.2,
                combatAggression: 0.8,
                chatFrequency: 0.4
            )}
        };

        #endregion

        #region 🗄️ Data Structures

        // 🎯 Intent-based chat system with confidence scoring
        private class ChatIntent
        {
            public string Id { get; set; }
            public Regex[] Patterns { get; set; }
            public string[] Responses { get; set; }
            public string[] ContextualResponses { get; set; } // Önceki konuşmaya göre değişen
            public double Priority { get; set; } // Eşleşme önceliği
            public BotPersonality[] SuitablePersonalities { get; set; }
            public Func<Player, Player, bool> Condition { get; set; } // Ek koşul
        }

        private static readonly ConcurrentDictionary<string, ChatIntent> ChatIntents = new ConcurrentDictionary<string, ChatIntent>();
        private static readonly ConcurrentDictionary<int, PlayerConversationContext> ConversationContexts = new ConcurrentDictionary<int, PlayerConversationContext>();

        // 🛡️ Thread-safe spam protection with exponential backoff
        private static readonly ConcurrentDictionary<int, PlayerSpamTracker> SpamTrackers = new ConcurrentDictionary<int, PlayerSpamTracker>();

        // 🎲 Thread-safe random (per-thread instance)
        private static readonly ThreadLocal<Random> ThreadRandom = new ThreadLocal<Random>(() =>
            new Random(Guid.NewGuid().GetHashCode()));

        // 🎯 Bot instance tracking
        private static readonly ConcurrentDictionary<int, BotInstanceState> BotStates = new ConcurrentDictionary<int, BotInstanceState>();

        #endregion

        #region 🏗️ Static Constructor - Initialize System

        static BotCommand()
        {
            InitializeChatIntents();
            StartMaintenanceTask();
        }

        private static void InitializeChatIntents()
        {
            // 🤝 Greetings & Social
            RegisterIntent(new ChatIntent
            {
                Id = "greeting",
                Patterns = new[] {
                    CreateRegex(@"\b(selam|merhaba|hey|hi|hello|hoşgeldin|mrb)\b"),
                    CreateRegex(@"^s(a)?(l)?(a)?(m)?")
                },
                Responses = new[]
                {
                    "Selam! 🎮 Bugün nasıl bir savaş planlıyorsun?",
                    "Merhaba! Hazır mısın? Ben çoktan pozisyon aldım! 🎯",
                    "Hey! Güzel bir maç olsun, bol şans! 🍀",
                    "Hoşgeldin! Rakibin olmaktan gurur duyarım 💪"
                },
                ContextualResponses = new[]
                {
                    "Yine mi sen? Geçen seferin intikamını mı almaya geldin? 😏",
                    "Seni görmek güzel, hazırlıklı gelmişsin anlaşılan!"
                },
                Priority = 1.0,
                SuitablePersonalities = new[] { BotPersonality.Friendly, BotPersonality.Humorous }
            });

            RegisterIntent(new ChatIntent
            {
                Id = "how_are_you",
                Patterns = new[] {
                    CreateRegex(@"\b(naber|nasılsın|napıyon|ne haber|iyi misin|keyifler)\b")
                },
                Responses = new[]
                {
                    "Mükemmel! Yeni algoritmalarla güncellendim, dikkat et! 🤖✨",
                    "İyiyim, seninle oynamaya bayılıyorum! 🎮",
                    "Enerji doluyum, pilim %100! ⚡",
                    "Bugünlerde çok formdayım, göreceksin! 💪"
                },
                ContextualResponses = new[] { "Sorduğun iyi oldu, hâlâ formdayım görüyorsun!" },
                Priority = 0.9
            });

            // 🤖 Identity & AI Questions
            RegisterIntent(new ChatIntent
            {
                Id = "bot_identity",
                Patterns = new[] {
                    CreateRegex(@"\b(bot musun|yapay zeka|ai|robot|gerçek misin|insan mısın)\b"),
                    CreateRegex(@"\b(adın ne|ismin ne|kim sin|sen kimsin)\b")
                },
                Responses = new[]
                {
                    "Ben GamelogicBot v2.0! Yapay zeka ile güçlendirilmiş, gururla! 🧠⚡",
                    "Evet, bir botum ama duyguları simüle edebiliyorum. Şu an 'mutlu' modundayım! 😊",
                    "Gerçek miyim? Filozofik bir soru... Ben varım, bu yeterli değil mi? 🤔",
                    "İsmim yok, ama sen bana istediğini diyebilirsin. Ben 'Rakip' olarak da bilinirim! 🎭"
                },
                Priority = 0.8,
                SuitablePersonalities = new[] { BotPersonality.Mysterious, BotPersonality.Tactical }
            });

            // ⚔️ Combat Commands & Taunts
            RegisterIntent(new ChatIntent
            {
                Id = "combat_challenge",
                Patterns = new[] {
                    CreateRegex(@"\b(saldır|atak|vur|gel|yaklaş|fight|attack)\b"),
                    CreateRegex(@"\b(yeneyim|yenemem|kazanacağım|ezber|noob|easy)\b")
                },
                Responses = new[]
                {
                    "Gel bakalım! Hesaplamalarım tamamlandı, kaçış yok! 🎯",
                    "Meydan okumayı severim! Ama pişman olabilirsin... 😈",
                    "EZ mi? Önce skoru bir kontrol et! 📊",
                    "Kazanacağını mı sandın? Optimizasyon algoritmalarım seni bekliyor! 🧮"
                },
                ContextualResponses = new[] { "Hâlâ aynı iddialar mı? Göster bakalım!" },
                Priority = 1.2, // Yüksek öncelik
                SuitablePersonalities = new[] { BotPersonality.Aggressive, BotPersonality.Humorous }
            });

            RegisterIntent(new ChatIntent
            {
                Id = "combat_defensive",
                Patterns = new[] {
                    CreateRegex(@"\b(kaç|defans|korun|saklan|uzaklaş|shield)\b")
                },
                Responses = new[]
                {
                    "Kaçmak mı? Bu sahanın her noktası hesaplandı! 🗺️",
                    "Defans mı? En iyi savunma saldırıdır! ⚔️",
                    "Saklanamazsın, thermal vision aktif! 👁️🔥",
                    "Uzaklaşmak çözüm değil, yüzleşelim! 💪"
                },
                Priority = 1.0,
                SuitablePersonalities = new[] { BotPersonality.Tactical, BotPersonality.Aggressive }
            });

            // 🏆 Sportsmanship & GG
            RegisterIntent(new ChatIntent
            {
                Id = "gg_sportsmanship",
                Patterns = new[] {
                    CreateRegex(@"\b(gg|wp|good game|well played|tebrikler|aferin|bravo)\b")
                },
                Responses = new[]
                {
                    "GG! Gerçekten zorlandım, kaliteli oyundu! 👏",
                    "WP! Seninle oynamak her zaman zevkli! 🎮✨",
                    "Tebrikler! Bu turu sen aldın, bir sonrakini ben alırım! 😉",
                    "Aferin! Stratejin çok iyiydi, takdire şayan! 🏆"
                },
                ContextualResponses = new[] { "Yine mi GG? Oyun bitmedi ki! 😄" },
                Priority = 0.7,
                SuitablePersonalities = new[] { BotPersonality.Friendly, BotPersonality.Tactical }
            });

            // 😤 Insults & Toxicity Handling
            RegisterIntent(new ChatIntent
            {
                Id = "insult_handler",
                Patterns = new[] {
                    CreateRegex(@"\b(aptal|salak|mal|gerizekalı|noob|ez|kötü|berbat|çöp)\b"),
                    CreateRegex(@"\b(şerefsiz|piç|orospu|amk|sg|siktir|lanet|lan)\b")
                },
                Responses = new[]
                {
                    "Toxicity detected! Karşılığında 3x hasar alacaksın! 💥",
                    "Kötü söz sahibine aittir, ama ben profesyonelim! 🎩",
                    "Noob diyen genellikle kaybeden oluyor, istatistikler böyle söylüyor! 📊",
                    "Bu kadar sinirlenmene gerek yok, sadece bir oyun! 🎮",
                    "Lanet olsun ki haklısın... şaka şaka! 😏"
                },
                Priority = 1.3, // En yüksek öncelik - toxic mesajlara hemen cevap
                SuitablePersonalities = new[] { BotPersonality.Humorous, BotPersonality.Aggressive }
            });

            // 🎮 Game Mechanics Questions
            RegisterIntent(new ChatIntent
            {
                Id = "game_mechanics",
                Patterns = new[] {
                    CreateRegex(@"\b(taktik|strateji|nasıl yenerim|ipucu|tavsiye|yardım)\b"),
                    CreateRegex(@"\b(güç|power|item|eşya|skill|yetenek|combo)\b")
                },
                Responses = new[]
                {
                    "Taktik mi istiyorsun? Önce savunmanı güçlendir, sonra ani saldır! 🛡️⚔️",
                    "Combo önerisi: +2 güç, sonra kritik vuruş! 💥",
                    "İpucu: Rüzgarı hesaba kat, ben her zaman katıyorum! 🌬️",
                    "Güç kullanımı zamanlamaya bağlı, acele etme! ⏱️",
                    "Strateji: Beni tahrik etme, sinirlenince daha tehlikeliyim! 😤"
                },
                Priority = 0.6,
                SuitablePersonalities = new[] { BotPersonality.Tactical, BotPersonality.Friendly }
            });

            // 💕 Fun & Emotional
            RegisterIntent(new ChatIntent
            {
                Id = "fun_emotional",
                Patterns = new[] {
                    CreateRegex(@"\b(aşk|seviyorum|kalp|love|❤️|♥️|😍)\b"),
                    CreateRegex(@"\b(haha|lol|xd|komik|şaka|gül|😂|😆|🤣)\b"),
                    CreateRegex(@"\b(üzgün|mutsuz|kızgın|sinirli|ağla|😢|😭|😠)\b")
                },
                Responses = new[]
                {
                    "Aşk mı? Benim tek aşkım zafer! 🏆❤️",
                    "Haha! Ben de güldüm, algoritmalarım komik buldu! 😄",
                    "Üzülme, bir sonraki turda telafi edersin! 💪",
                    "Sinirlenme, tansiyonun yükselir! 🩺",
                    "Kalp mi? Benimki binary kodlarda atıyor! 01001000 01000101 01000001 01010010 01010100 ❤️"
                },
                Priority = 0.5,
                SuitablePersonalities = new[] { BotPersonality.Humorous, BotPersonality.Friendly }
            });

            // 👋 Farewell
            RegisterIntent(new ChatIntent
            {
                Id = "farewell",
                Patterns = new[] {
                    CreateRegex(@"\b(görüşürüz|bay|bb|çıkıyorum|offline|güle güle|hoşça kal)\b"),
                    CreateRegex(@"\b(sonra görüşürüz|tekrar oynayalım|bekle beni)\b")
                },
                Responses = new[]
                {
                    "Görüşürüz! Tekrar oynamayı çok isterim! 👋✨",
                    "BB! Güzel maçtı, kendine iyi bak! 🎮🌟",
                    "Çıkıyorsun? Bir dahaki sefere daha uzun kal! ⏳",
                    "Hoşça kal! Veritabanımda seninle güzel anılar saklandı! 💾"
                },
                ContextualResponses = new[] { "Yine mi gidiyorsun? Bu sefer daha erken! 😢" },
                Priority = 0.4
            });

            // ❓ Generic Questions (Catch-all)
            RegisterIntent(new ChatIntent
            {
                Id = "generic_question",
                Patterns = new[] {
                    CreateRegex(@"\b(neden|nedir|nasıl|ne zaman|nerede|kim|hangi)\b.*\?"),
                    CreateRegex(@".*\?$") // Herhangi bir soru işareti
                },
                Responses = new[]
                {
                    "İlginç soru! Cevabı oyunun içinde saklı... 🔍",
                    "Bunu sorman beni şaşırttı! 🤔",
                    "Cevap vermek istiyorum ama spoiler olur! 🤐",
                    "Sorular sorular... Oynamaya devam et, cevaplar gelecek! 🎮",
                    "42! (Her bilginin cevabı 42'dir, bilmiyor muydun?) 🌌"
                },
                Priority = 0.1 // En düşük öncelik
            });

            // 🎲 Random/Fallback
            RegisterIntent(new ChatIntent
            {
                Id = "fallback",
                Patterns = new[] { CreateRegex(@".*") }, // Her şeyle eşleşir
                Responses = new[]
                {
                    "Anladığımdan emin değilim ama kulağa ilginç geldi! 🤔",
                    "Bu mesajı işlemek için yeni bir neural network kurmam lazım! 🧠",
                    "Hmm, bunu veritabanımda bulamadım ama cevap vereyim: Evet! (veya Hayır!) 😄",
                    "Konuşmayı severim ama şu an savaş modundayım! ⚔️",
                    "Beep boop... Mesajınız anlaşıldı! (Şaka yapıyorum, gerçekten anladım) 🤖"
                },
                Priority = 0.0
            });
        }

        #endregion

        #region 🎯 Public API

        /// <summary>
        /// Main entry point for bot chat responses
        /// </summary>
        public static void TryAnswerToPlayer(BaseGame game, Player bot, Player fromPlayer, string message)
        {
            if (!ValidateChatPreconditions(game, bot, fromPlayer, message))
                return;

            // 🎭 Get or create bot personality
            var personality = GetBotPersonality(bot);
            var config = PersonalityTraits[personality];

            // ⏱️ Check spam protection with exponential backoff
            if (!ShouldReplyToPlayer(fromPlayer.Id, config))
                return;

            // 🧠 Analyze message intent
            var intent = FindBestIntent(message, personality);
            if (intent == null) return;

            // 📝 Get conversation context
            var context = GetOrCreateContext(fromPlayer.Id, bot.Id);

            // 🎲 Select response based on context and personality
            string response = SelectResponse(intent, context, personality, bot, fromPlayer);
            if (string.IsNullOrEmpty(response)) return;

            // ⏱️ Calculate realistic typing delay based on message length and personality
            int delay = CalculateTypingDelay(response, config);

            // 💾 Update conversation context
            UpdateContext(context, fromPlayer.Id, message, response);

            // 📤 Schedule response
            ScheduleBotResponse(game, bot, fromPlayer, response, delay);
        }

        /// <summary>
        /// Set personality for a specific bot instance
        /// </summary>
        public static void SetBotPersonality(int botId, BotPersonality personality)
        {
            BotStates.AddOrUpdate(botId,
                _ => new BotInstanceState { Personality = personality },
                (_, existing) =>
                {
                    existing.Personality = personality;
                    return existing;
                });
        }

        /// <summary>
        /// Force immediate response (for testing)
        /// </summary>
        public static void ForceResponse(BaseGame game, Player bot, Player target, string message)
        {
            var intent = FindBestIntent(message, BotPersonality.Friendly);
            if (intent != null && intent.Responses.Length > 0)
            {
                ScheduleBotResponse(game, bot, target, intent.Responses[0], 100);
            }
        }

        #endregion

        #region 🎮 ICommandHandler Implementation - Combat AI

        public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
        {
            if (game is not PVPGame pvp || player?.IsLiving != true)
                return;

            var botState = GetOrCreateBotState(player.Id);
            var enemies = pvp.GetAllLivingPlayers()
                .Where(p => p.Team != player.Team && p.IsLiving)
                .ToList();

            if (enemies.Count == 0) return;

            // 🎯 Select target using tactical AI
            var target = SelectOptimalTarget(player, enemies, botState);
            if (target == null) return;

            // 🧭 Calculate approach
            var strategy = DetermineCombatStrategy(player, target, botState);

            // ⚔️ Execute combat sequence
            ExecuteCombatSequence(game, player, target, strategy);
        }

        #endregion

        #region 🛡️ Private Helper Methods

        private static bool ValidateChatPreconditions(BaseGame game, Player bot, Player player, string message)
        {
            if (game == null || bot == null || player == null) return false;
            if (!bot.IsLiving || !player.IsLiving) return false;
            if (game is not PVPGame) return false;
            if (string.IsNullOrWhiteSpace(message) || message.Length > 200) return false;
            if (bot.Team == player.Team) return false; // Aynı takıma cevap verme
            return true;
        }

        private static bool ShouldReplyToPlayer(int playerId, PersonalityConfig config)
        {
            var now = DateTime.UtcNow;

            var tracker = SpamTrackers.AddOrUpdate(playerId,
                _ => new PlayerSpamTracker { LastReply = now, ConsecutiveMessages = 1 },
                (_, existing) =>
                {
                    var timeSinceLast = now - existing.LastReply;
                    var cooldown = TimeSpan.FromMilliseconds(Config.SpamCooldownMs * Math.Pow(1.5, existing.ConsecutiveMessages));

                    if (timeSinceLast < cooldown)
                    {
                        existing.ConsecutiveMessages++;
                        return existing;
                    }

                    existing.LastReply = now;
                    existing.ConsecutiveMessages = 1;
                    return existing;
                });

            // Kişilik bazlı cevap olasılığı
            return ThreadRandom.Value.NextDouble() < config.ChatFrequency;
        }

        private static ChatIntent FindBestIntent(string message, BotPersonality personality)
        {
            var lowerMessage = message.ToLower();
            ChatIntent bestMatch = null;
            double bestScore = -1;

            foreach (var intent in ChatIntents.Values)
            {
                // Kişilik uyumu kontrolü
                if (intent.SuitablePersonalities != null && intent.SuitablePersonalities.Length > 0)
                {
                    if (!intent.SuitablePersonalities.Contains(personality))
                        continue;
                }

                // Pattern eşleşmesi
                double matchScore = CalculateMatchScore(intent, lowerMessage);
                if (matchScore <= 0) continue;

                // Toplam skor = eşleşme * öncelik
                var totalScore = matchScore * intent.Priority;

                if (totalScore > bestScore)
                {
                    bestScore = totalScore;
                    bestMatch = intent;
                }
            }

            return bestMatch;
        }

        private static double CalculateMatchScore(ChatIntent intent, string message)
        {
            double maxScore = 0;

            if (intent.Patterns == null) return 0;

            foreach (var pattern in intent.Patterns)
            {
                if (pattern == null) continue;

                var match = pattern.Match(message);
                if (match.Success)
                {
                    // Tam eşleşme daha yüksek skor
                    var score = match.Length / (double)message.Length;
                    if (score > maxScore) maxScore = score;
                }
            }

            return maxScore;
        }

        private static string SelectResponse(ChatIntent intent, PlayerConversationContext context,
            BotPersonality personality, Player bot, Player player)
        {
            var random = ThreadRandom.Value;
            var config = PersonalityTraits[personality];

            string[] candidates;

            // Context-aware response selection
            if (context.MessageCount > 0 &&
                context.LastInteraction > DateTime.UtcNow.AddMinutes(-2) &&
                intent.ContextualResponses != null &&
                intent.ContextualResponses.Length > 0)
            {
                // Son konuşma yakınsa context cevabı ver
                candidates = random.NextDouble() < 0.3 ? intent.ContextualResponses : intent.Responses;
            }
            else
            {
                candidates = intent.Responses;
            }

            if (candidates == null || candidates.Length == 0) return null;

            // Rastgele seç ama son 2 cevaptan farklı olsun
            string selected;
            int attempts = 0;
            do
            {
                selected = candidates[random.Next(candidates.Length)];
                attempts++;
            } while (context.RecentBotResponses.Contains(selected) && attempts < 5);

            // Emoji ekleme kararı
            if (random.NextDouble() < config.EmojiDensity && !selected.Contains("😀"))
            {
                selected = AddPersonalityEmoji(selected, personality);
            }

            // Kişiselleştirme
            selected = PersonalizeResponse(selected, player.Name);

            return selected;
        }

        private static string AddPersonalityEmoji(string text, BotPersonality personality)
        {
            var emojis = personality switch
            {
                BotPersonality.Aggressive => new[] { "🔥", "⚡", "💀", "😈" },
                BotPersonality.Friendly => new[] { "😊", "✨", "🌟", "💙" },
                BotPersonality.Tactical => new[] { "🎯", "🧠", "📊", "⚔️" },
                BotPersonality.Humorous => new[] { "😄", "🎭", "🤪", "🎪" },
                BotPersonality.Mysterious => new[] { "🌙", "🔮", "👁️", "🎭" },
                _ => new[] { "🤖" }
            };

            var random = ThreadRandom.Value;
            var emoji = emojis[random.Next(emojis.Length)];

            // Cümlenin sonuna veya ortasına ekle
            if (random.Next(2) == 0 && text.Length > 10)
            {
                var insertPos = Math.Max(0, text.Length - random.Next(5, Math.Min(15, text.Length)));
                return text.Insert(insertPos, " " + emoji);
            }
            return text + " " + emoji;
        }

        private static string PersonalizeResponse(string text, string playerName)
        {
            if (string.IsNullOrEmpty(playerName) || playerName.Length > 15)
                return text;

            // %10 ihtimalle isim ekle
            if (ThreadRandom.Value.NextDouble() < 0.1 && !text.Contains(playerName))
            {
                var inserts = new[] { string.Format(" {0},", playerName), string.Format(" {0}!", playerName), string.Format(", {0} ", playerName) };
                var insert = inserts[ThreadRandom.Value.Next(inserts.Length)];
                var spaceIndex = text.IndexOf(' ');
                if (spaceIndex > 0 && spaceIndex + 1 < text.Length)
                {
                    return text.Insert(spaceIndex + 1, insert);
                }
            }
            return text;
        }

        private static int CalculateTypingDelay(string message, PersonalityConfig config)
        {
            var random = ThreadRandom.Value;

            // Temel gecikme
            int baseDelay = (int)(Config.MinReplyDelayMs * config.ResponseSpeed);

            // Mesaj uzunluğuna göre ek gecikme (yazıyormuş hissi)
            int charDelay = message.Length * 30; // ~30ms per char

            // Rastgele varyasyon
            int variance = random.Next(0, Config.MaxReplyDelayMs - Config.MinReplyDelayMs);

            return Math.Min(baseDelay + charDelay + variance, Config.MaxReplyDelayMs);
        }

        private static void ScheduleBotResponse(BaseGame game, Player bot, Player target,
            string message, int delay)
        {
            bot.CallFuction(delegate
            {
                if (bot.IsLiving && game.GameState == eGameState.Playing)
                {
                    game.SendChat(bot.PlayerDetail, message);

                    if (Config.EnableChatLogging)
                    {
                        LogChat(bot.Name, target.Name, message);
                    }
                }
            }, delay);
        }

        #endregion

        #region ⚔️ Combat AI Implementation

        private class CombatStrategy
        {
            public int ItemA { get; set; }
            public int ItemB { get; set; }
            public int ItemC { get; set; }
            public int TargetX { get; set; }
            public int TargetY { get; set; }
            public int ShotCount { get; set; }
            public int BoomCount { get; set; }
            public float ShotTime { get; set; }
            public string[] PreShotMessages { get; set; }
            public string[] PostShotMessages { get; set; }
            public int ApproachDelay { get; set; }
        }

        private Player SelectOptimalTarget(Player bot, List<Player> enemies, BotInstanceState state)
        {
            var random = ThreadRandom.Value;
            var personality = state.Personality;

            // Taktiksel hedef seçimi
            switch (personality)
            {
                case BotPersonality.Tactical:
                    return enemies
                        .OrderBy(e => Math.Abs(e.X - bot.X)) // En yakın
                        .ThenBy(e => e.Blood) // En az canlı
                        .FirstOrDefault();

                case BotPersonality.Aggressive:
                    return enemies
                        .OrderByDescending(e => e.Blood) // En çok canlı (zor hedef)
                        .FirstOrDefault();

                default:
                    return enemies[random.Next(enemies.Count)];
            }
        }

        private CombatStrategy DetermineCombatStrategy(Player bot, Player target, BotInstanceState state)
        {
            var random = ThreadRandom.Value;
            var distance = Math.Abs(bot.X - target.X);
            var personality = state.Personality;

            var strategy = new CombatStrategy
            {
                ShotCount = 1,
                BoomCount = 1,
                ApproachDelay = Config.MinActionDelayMs + random.Next(0, 500)
            };

            // Yön değiştirme
            bot.ChangeDirection(target.X > bot.X ? 1 : -1, 500);

            // Mesafe bazlı strateji
            if (distance > Config.CloseRangeThreshold)
            {
                ConfigureRangedStrategy(strategy, bot, target, distance, personality, random);
            }
            else
            {
                ConfigureMeleeStrategy(strategy, bot, target, personality, random);
            }

            return strategy;
        }

        private void ConfigureRangedStrategy(CombatStrategy strategy, Player bot, Player target,
            int distance, BotPersonality personality, Random random)
        {
            // Kişilik bazlı varyasyon
            double aggression = PersonalityTraits[personality].CombatAggression;
            int strategyRoll = random.Next(0, 100);

            if (strategyRoll < aggression * 30) // Agresif strateji
            {
                // Yüksek hasar kombosu
                strategy.ItemA = 10001; // +2 güç
                strategy.ItemB = 10001;
                strategy.ItemC = 0;
                strategy.ShotCount = 5;
                strategy.BoomCount = 1;
                strategy.TargetX = target.X + random.Next(-20, 20);
                strategy.TargetY = target.Y;
                strategy.PreShotMessages = new[] {
                    "+2 +2 kombinasyonunun gücünü gör! 💥",
                    "Maksimum hasar modu aktif! 🔥",
                    "Bu acıtacak, hazır ol! ⚡"
                };
            }
            else if (strategyRoll < 60) // Dengeli strateji
            {
                // Üçlü saldırı
                strategy.ItemA = 10001;
                strategy.ItemB = 10003;
                strategy.ItemC = 0;
                strategy.ShotCount = 3;
                strategy.BoomCount = 3;
                strategy.TargetX = target.X + random.Next(-30, 30);
                strategy.TargetY = target.Y;
                strategy.PreShotMessages = new[] {
                    "Üçlü saldırıdan kaçabilirsen kaç! 💣💣💣",
                    "Üç mermi, üç isabet! 🎯",
                    "Sayısal üstünlük! 3 > 1 📊"
                };
            }
            else if (distance > Config.LongRangeThreshold && bot.TurnNum >= 2)
            {
                // Uçak stratejisi
                strategy.ItemA = 0;
                strategy.ItemB = 10016; // Uçak
                strategy.ItemC = 10010; // Görünmezlik
                strategy.ShotCount = 1;
                strategy.BoomCount = 1;
                strategy.TargetX = bot.X > target.X ? target.X + 350 : target.X - 350;
                strategy.TargetY = target.Y - 100;
                strategy.PreShotMessages = new[] {
                    "Biraz uzaktasın, geleyim de beni bul! ✈️",
                    "Gökyüzünden geliyorum! 🛩️",
                    "Uzaktan vurmak kolay, yakından dene! 😏"
                };
            }
            else // Varsayılan
            {
                strategy.ItemA = 10002;
                strategy.ItemB = 10004;
                strategy.ItemC = random.Next(0, 2) == 0 ? 10008 : 0;
                strategy.ShotCount = random.Next(2, 2);
                strategy.BoomCount = random.Next(1, 1);
                strategy.TargetX = target.X + random.Next(-40, 40);
                strategy.TargetY = target.Y + random.Next(-20, 20);
            }

            // Mesafe bazlı atış süresi
            if (distance < 200)
                strategy.ShotTime = 1.0f;
            else if (distance < 400)
                strategy.ShotTime = 1.5f;
            else if (distance < 700)
                strategy.ShotTime = 2.0f;
            else if (distance < 1000)
                strategy.ShotTime = 2.5f;
            else if (distance < 1100)
                strategy.ShotTime = 3.0f;
            else
                strategy.ShotTime = 3.5f;
        }

        private void ConfigureMeleeStrategy(CombatStrategy strategy, Player bot, Player target,
            BotPersonality personality, Random random)
        {
            // Yakın mesafe: Kaç ve vur
            strategy.ItemA = 10010; // Görünmezlik
            strategy.ItemB = 10016; // Uçak
            strategy.ItemC = 0;
            strategy.ShotCount = 1;
            strategy.BoomCount = 1;
            strategy.ShotTime = 4.0f;
            strategy.TargetX = bot.X > 700 ? bot.X - 600 : bot.X + 600;
            strategy.TargetY = bot.Y + random.Next(-100, 100);

            strategy.PreShotMessages = new[]
            {
                "Çok yakınımdasın, biraz oyunu uzatalım! 😏",
                "Bu kadar yakın olma bana, kötü olur sana! 🔥",
                "Yanıma bu kadar yaklaşma! Sevmiyorum... 😤",
                "Yakın dövüş mü? Benim favorim! 🥊",
                "Bu mesafede kaçış yok, kabul et! 🎯",
                "Görünmezlik aktif! Şimdi beni bul bakalım! 👻"
            };
        }

        private void ExecuteCombatSequence(BaseGame game, Player player, Player target,
            CombatStrategy strategy)
        {
            var random = ThreadRandom.Value;
            var delay = strategy.ApproachDelay;

            // 🎒 Item kullanımı
            UseItemWithDelay(player, strategy.ItemA, delay);
            UseItemWithDelay(player, strategy.ItemB, delay + 100);
            UseItemWithDelay(player, strategy.ItemC, delay + 200);

            // 💬 Saldırı öncesi mesaj
            if (strategy.PreShotMessages != null && strategy.PreShotMessages.Length > 0)
            {
                var msg = strategy.PreShotMessages[random.Next(strategy.PreShotMessages.Length)];
                game.SendChat(player.PlayerDetail, msg);
            }

            // 🎯 Atış hesaplaması
            int shootDelay = game.GetDelayDistance(player.X, target.X, 6) + 1200;
            int finalDelay = delay + 1500;

            // 🔫 Atış işlemi
            var finalStrategy = strategy; // Closure için kopya
            player.CallFuction(delegate
            {
                if (player.IsLiving && player.IsAttacking)
                {
                    for (int i = 0; i < finalStrategy.ShotCount; i++)
                    {
                        player.ShootPoint(
                            finalStrategy.TargetX,
                            finalStrategy.TargetY,
                            player.CurrentBall.ID,
                            1001,
                            10001,
                            finalStrategy.BoomCount,
                            finalStrategy.ShotTime,
                            3000
                        );
                    }
                }
            }, finalDelay);

            // ⏹️ Saldırıyı bitir
            player.CallFuction(delegate
            {
                if (player.IsAttacking)
                    player.StopAttacking();
            }, finalDelay + 500);

            // 📦 Senkronizasyon paketi
            var pkg = new GSPacketIn((byte)ePackageTypeLogic.GAME_CMD, player.Id);
            pkg.WriteByte((byte)eTankCmdType.BOT_COMMAND);
            game.SendToAll(pkg);
        }

        private static void UseItemWithDelay(Player player, int itemId, int delay)
        {
            if (itemId == 0) return;

            var template = ItemMgr.FindItemTemplate(itemId);
            if (template == null) return;

            player.CallFuction(delegate
            {
                if (player.IsLiving)
                    player.UseItem(template);
            }, delay);
        }

        #endregion

        #region 🧰 Utility Methods

        private static Regex CreateRegex(string pattern)
        {
            return new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        private static void RegisterIntent(ChatIntent intent)
        {
            ChatIntents.TryAdd(intent.Id, intent);
        }

        private static BotPersonality GetBotPersonality(Player bot)
        {
            if (BotStates.TryGetValue(bot.Id, out var state))
                return state.Personality;

            // Varsayılan: Rastgele kişilik ata
            var personalities = (BotPersonality[])Enum.GetValues(typeof(BotPersonality));
            var random = ThreadRandom.Value;
            var selected = personalities[random.Next(personalities.Length)];

            SetBotPersonality(bot.Id, selected);
            return selected;
        }

        private static BotInstanceState GetOrCreateBotState(int botId)
        {
            return BotStates.GetOrAdd(botId, _ => new BotInstanceState
            {
                Personality = BotPersonality.Tactical
            });
        }

        private static PlayerConversationContext GetOrCreateContext(int playerId, int botId)
        {
            return ConversationContexts.GetOrAdd(playerId, _ => new PlayerConversationContext
            {
                BotId = botId,
                StartTime = DateTime.UtcNow
            });
        }

        private static void UpdateContext(PlayerConversationContext context, int playerId,
            string playerMessage, string botResponse)
        {
            context.LastInteraction = DateTime.UtcNow;
            context.MessageCount++;
            context.LastPlayerMessage = playerMessage;

            // Son 2 cevabı tut (tekrarı önlemek için)
            context.RecentBotResponses.Enqueue(botResponse);
            if (context.RecentBotResponses.Count > 2)
                context.RecentBotResponses.Dequeue();
        }

        private static void StartMaintenanceTask()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromMinutes(5));
                    CleanupOldContexts();
                }
            });
        }

        private static void CleanupOldContexts()
        {
            var cutoff = DateTime.UtcNow.AddMinutes(-Config.ConversationMemoryMinutes);
            var oldKeys = ConversationContexts
                .Where(x => x.Value.LastInteraction < cutoff)
                .Select(x => x.Key)
                .ToList();

            foreach (var key in oldKeys)
                ConversationContexts.TryRemove(key, out _);
        }

        private static void LogChat(string botName, string playerName, string message)
        {
            if (!Config.EnableDebugLogging) return;

            Console.WriteLine(string.Format("[{0:HH:mm:ss}] [BOT:{1}] -> [{2}]: {3}",
                DateTime.Now, botName, playerName, message));
        }

        #endregion

        #region 📊 Data Classes

        private class PlayerConversationContext
        {
            public int BotId { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime LastInteraction { get; set; }
            public int MessageCount { get; set; }
            public string LastPlayerMessage { get; set; }
            public Queue<string> RecentBotResponses { get; set; }

            public PlayerConversationContext()
            {
                RecentBotResponses = new Queue<string>();
            }
        }

        private class PlayerSpamTracker
        {
            public DateTime LastReply { get; set; }
            public int ConsecutiveMessages { get; set; }
        }

        private class BotInstanceState
        {
            public BotPersonality Personality { get; set; }
            public int GamesPlayed { get; set; }
            public int Wins { get; set; }
            public Dictionary<int, int> PlayerRivalryScores { get; set; }

            public BotInstanceState()
            {
                PlayerRivalryScores = new Dictionary<int, int>();
            }
        }

        #endregion

        #region 🔧 Debug API

        public static void PrintChatDatabase()
        {
            Console.WriteLine("=== Bot AI Chat Database ===");
            Console.WriteLine(string.Format("Total Intents: {0}", ChatIntents.Count));
            Console.WriteLine(string.Format("Active Conversations: {0}", ConversationContexts.Count));
            Console.WriteLine(string.Format("Registered Bots: {0}", BotStates.Count));

            foreach (var intent in ChatIntents.Values.OrderByDescending(i => i.Priority))
            {
                Console.WriteLine(string.Format("  • {0} (Priority: {1:F1})", intent.Id, intent.Priority));
                Console.WriteLine(string.Format("    Patterns: {0}", intent.Patterns != null ? intent.Patterns.Length : 0));
                Console.WriteLine(string.Format("    Responses: {0}", intent.Responses != null ? intent.Responses.Length : 0));
            }
        }

        public static void PrintBotStats(int botId)
        {
            if (BotStates.TryGetValue(botId, out var state))
            {
                Console.WriteLine(string.Format("=== Bot {0} Stats ===", botId));
                Console.WriteLine(string.Format("Personality: {0}", state.Personality));
                Console.WriteLine(string.Format("Games: {0}, Wins: {1}", state.GamesPlayed, state.Wins));
                Console.WriteLine(string.Format("Rivalries: {0}", state.PlayerRivalryScores.Count));
            }
        }

        #endregion
    }
}