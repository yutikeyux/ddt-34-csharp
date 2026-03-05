using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace Game.Logic.Cmd
{
    /// <summary>
    /// Bot Davranış ve Sohbet Yönetim Sınıfı
    /// 
    /// 🎯 ÖNEMLİ NOT: Sohbet cevaplama özelliğinin çalışması için:
    /// ChatCommand.cs dosyasında, oyuncu mesajı işlendikten sonra şu satır eklenmelidir:
    /// 
    ///   BotCommand.TryAnswerToPlayer(game, botPlayer, senderPlayer, message);
    /// 
    /// Burada 'botPlayer', rakip takımdaki bot oyuncusunu temsil etmelidir.
    /// </summary>
    [GameCommand((byte)eTankCmdType.BOT_COMMAND, "Bot AI Saldırı ve Sohbet Yöneticisi")]
    public class BotCommand : ICommandHandler
    {
        // ─────────────────────────────────────────────────────────────────────
        // 🗄️ SORU-CEVAP VERİTABANI (Genişletilmiş Türkçe İçerik)
        // ─────────────────────────────────────────────────────────────────────
        private static readonly Dictionary<string, string[]> _chatDatabase =
            new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        // 🎲 Thread-safe Random instance
        private static readonly Random _rnd = new Random();
        private static readonly object _rndLock = new object();

        // ⏱️ Spam önleme: Oyuncu bazlı (Player.ID + timestamp)
        private static readonly Dictionary<int, DateTime> _playerLastReply =
            new Dictionary<int, DateTime>();
        private static readonly TimeSpan _replyDelay = TimeSpan.FromMilliseconds(1200);
        private static readonly object _spamLock = new object();

        // ─────────────────────────────────────────────────────────────────────
        // STATIK KURUCU - Tüm sohbet veritabanını burada yüklüyoruz
        // ─────────────────────────────────────────────────────────────────────
        static BotCommand()
        {
            // ═══════════════════════════════════════════════════════════════
            // 🤝 SELAMLAŞMA & GİRİŞ
            // ═══════════════════════════════════════════════════════════════
            Add("selam",
                "Selam dostum! 🎮", "Aleyküm selam, hazır mısın?", "Merhaba, savaş başlasın!",
                "Ooo, merhaba! Seni bekliyordum 😄", "Selamlar, nereye saklanacaksın?");

            Add("merhaba",
                "Merhaba! 💥", "Selamlar savaşçı!", "Sana da merhaba, düello başlasın!",
                "Merhabalar, dikkatli ol, ısırırım! 🐕");

            Add("naber", "naber", "ne haber", "napıyon", "ne yapıyon", "ne yapıyorsun",
                "İyiyim, sen nasılsın? 🎯", "Harika, tam zamanında geldin!",
                "Fena değil, hedefim sensin şimdi 😏", "İyiyim, seninle oynamaya bayılıyorum!",
                "Sana nişan alıyorum aslında 😄", "Strateji kuruyorum, seni yenmek için!");

            Add("nasılsın", "nasılsın", "iyi misin", "keyifler nasıl",
                "İyiyim teşekkürler, sen? 💪", "Biraz gerginim ama iyiyim!",
                "Atışlarım yerli yerinde, çok iyiyim!", "Düzgünüm, sen ne diyorsun?",
                "Makine gibi çalışıyorum, sen? ⚙️");

            Add("günaydın", "günaydın", "hayırlı sabahlar",
                "Günaydın! ☀️ Bugün seni yenmek için harika bir gün!",
                "Hayırlı sabahlar, enerji dolu başladık!", "Günaydın, ilk mermi benden! 🔫");

            Add("iyi akşamlar", "iyi akşamlar", "hayırlı akşamlar",
                "İyi akşamlar! 🌙 Akşamın yıldızı ben olacağım!",
                "Hayırlı akşamlar, son maçımızı oynayalım!");

            // ═══════════════════════════════════════════════════════════════
            // 🤖 BOT KİMLİK & YAPAY ZEKA SORULARI
            // ═══════════════════════════════════════════════════════════════
            Add("adın ne", "adın ne", "ismin ne", "sen kimsin",
                "Ben bir botum, ama iyi bir bot! 🤖", "Bana istediğin ismi verebilirsin!",
                "Kodlayanlar isim koymadı, sen koy! 💻", "Benim adım 'Kabus', sen ne dersin?");

            Add("kim sin", "bot musun", "yapay zeka mısın",
                "Ben bir yapay zeka botuyum, gururla! 🧠", "Rakibiniz, tanıştığımıza memnun oldum!",
                "Evet botum, ama kaliteli bir bot! ✨", "Öyle diyorlar, gel yeteneğimi gör!");

            Add("gerçek mi", "insan mısın", "arkada kim var",
                "Gerçek mi sandın? 😄 Hayır, botum ama duyguları anlıyorum!",
                "Hayır, yapay zekayım ama çok gerçekçi oynuyorum!",
                "İnsan değilim ama insanlardan beter oynuyorum! 🔥");

            Add("boss", "patron", "kral",
                "Ben mi? Öyle hissettiriyorum demek! 😎", "Bana boss demelerine alıştım!",
                "Evet, bu sahanın patronu benim! 👑", "Kral sensin ama taht bende! ♟️");

            // ═══════════════════════════════════════════════════════════════
            // ⚔️ OYUN & SALDIRI KOMUTLARI
            // ═══════════════════════════════════════════════════════════════
            Add("sal", "saldır", "atak", "fire",
                "Tamam, salıyorum! 🎯", "Hedefi aldım, ateş! 🔥", "Anlaşıldı, geliyorum!",
                "Saldırı emri alındı! 💣", "Canını alayım mı? 😏");

            Add("vur", "çarp", "döverim",
                "Vuruyorum zaten, bekle! 💥", "Sıra gelince göreceksin!",
                "Gözünü aç, geliyorum! ⚡", "Vurmak kolay, isabet ettirmek sanat! 🎨");

            Add("yak", "kovala", "peşinden gel",
                "Yaklaşıyorum, kaçamazsın! 🔥", "Kovalamaca başlasın! 🏃",
                "Sıcaklığı hissedeceksin! 🌡️", "Peşindeyim, hazırlıklı ol!");

            Add("kaç", "geri çekil", "uzaklaş",
                "Nereye kaçacaksın ki? 🤔", "Kaçsan da yakalarım! 🎯",
                "Bu sahada kaçış yok, kabul et! 🚫", "Kaçmak çözüm değil, savaş! ⚔️");

            Add("dur", "bekle", "yavaşla",
                "Durma lüksüm yok, savaş devam! ⏱️", "Savaş bitmeden durulmaz! 🛡️",
                "Neden durayım? Momentum bende! 🚀", "Beklemek zayıfların işi! 💪");

            // ═══════════════════════════════════════════════════════════════
            // 🏆 MAÇ SONU & SPORTİFLİK
            // ═══════════════════════════════════════════════════════════════
            Add("gg", "gg", "gg wp", "iyi oyundu",
                "GG! Güzel oyundu dostum! 🎮", "GG, tekrar oynayalım! 🔄",
                "GG, seninle oynamak zevkti! ✨", "GG! Bir dahaki sefere belki sen kazanırsın! 😉");

            Add("wp", "wp", "well played", "güzel oynadın",
                "Teşekkürler, sen de fena değildin! 👏", "WP! Gerçekten zorladın beni! 💪",
                "Sağ ol, senin stratejin de iyiydi! 🧠", "WP! Bir dahaki maça hazırlıklı gel! 🔥");

            Add("tebrikler", "tebrikler", "kutlarım", "aferin",
                "Teşekkürler! 🎉 Sen de harika oynadın!", "Sağ ol, emek verdik! 💯",
                "Aferin demek kolay, bir de yenmeyi dene! 😄", "Kutlama yapalım, sonra tekrar! 🥳");

            // ═══════════════════════════════════════════════════════════════
            #region 🗣️ RAKİPLİK & MEYDAN OKUMA
            // ═══════════════════════════════════════════════════════════════
            Add("yenemem", "seni yenemem", "imkansız",
                "Belki haklısın, ama denemekten vazgeçme! 💪", "İmkansız diye bir şey yok, dene! 🎯",
                "Pes etmek yok! Bir şans daha ver! 🔄", "Kim bilir, belki bu sefer şansın döner! 🍀");

            Add("yeneyim", "seni yeneceğim", "kazanacağım",
                "Buyur, beklerim! 😏", "Tabii, gel de gör! 💥",
                "İddia büyük, gel ispat et! 🔥", "Merak ediyorum, hadi görelim! 🎮");

            Add("kaybedeceksin", "kaybedersin", "şansın yok",
                "Öyle mi? İlginç bir iddia... 🤔", "Kaybetmek mi? Bekleyip göreceğiz! ⏳",
                "Belki, ama kolay olmayacak! 💪", "Şans değil, yetenek konuşur! 🎯");

            Add("kazanacaksın", "sen kazanırsın", "şanslısın",
                "Çok iyimsin ama ben de çalışıyorum! 😄", "Umarım haklısın, ama ben pes etmem! 🔥",
                "Teşekkürler, elimden geleni yapacağım! 💯", "Şans değil, strateji kazanır! 🧠");

            Add("güçlüsün", "çok iyisin", "usta oyuncu",
                "Teşekkürler! Sen de fena değilsin! 👏", "Güç, stratejiyle gelir! 🧠",
                "Eğitim görmüş bir botum sonuçta! 🤖✨", "İltifatın için sağ ol, devam edelim! 💪");

            Add("zayıfsın", "kötüsün", "berbatsın", "noob",
                "Öyle mi? Gel kanıtlayalım! 😏", "Zayıf mı? Skora bir bak! 📊",
                "Noob mu? Skor tablosunu aç da görelim! 🏆", "Berbat botu yenemiyorsan, sorun bende değil! 💥");
            #endregion

            // ═══════════════════════════════════════════════════════════════
            #region 😂 ŞAKA & EĞLENCE & DUYGULAR
            // ═══════════════════════════════════════════════════════════════
            Add("haha", "haha", "hahaha", "komik",
                "He he he! Sen de eğlencelisin! 😄", "Güldürmek güzel, devam et! 🎭",
                "Haha! Ben de güldüm, iyi geldi! 😂", "Komik misin? Ben de öyleyim! 🤡");

            Add("lol", "lol", "rofl", "kahkaha",
                "LOL! Gerçekten güldüm! 😆", "Komik bir durum, katılıyorum! 🎪",
                "Hahaha, ben de! Eğlence devam! 🎉", "LOL! Mermilerim bile gülüyor! 🔫😄");

            Add("şaka", "şaka mı", "ciddi misin",
                "Şaka mı? Ciddiye aldım! 😅", "Aman, şaka yapma benimle, hassasım! 🤖💔",
                "Şakayı seviyorum, devam et! 🎭", "Ciddiyim ama şakayı da severim! 😄");

            Add("sıkıcı", "boring", "eğlencesiz",
                "Sıkıcı mı? Biraz daha bekle, asıl eğlence şimdi başlar! 🎢",
                "Sıkıcıysa neden oynuyorsun? 😏", "Sıkıcılığı ben değil, beklemek getirir! ⏱️",
                "Eğlenceyi ben getiririm, izle! ✨");

            Add("eğlenceli", "fun", "harika", "süper",
                "Katılıyorum, bu oyun çok eğlenceli! 🎮✨", "Seninle oynamak her zaman eğlenceli! 🤝",
                "Harika! Eğlenmeye devam edelim! 🎉", "Süper! Enerjin bulaştı bana! ⚡");

            Add("üzgünüm", "üzgünüm", "kötü hissediyorum",
                "Üzülme, bir sonraki turda telafi ederiz! 💪", "Her şey yoluna girer, devam et! 🌈",
                "Kötü hissetme, oyun bittiğinde her şey unutulur! 🎮", "Yanındayım, birlikte aşarız! 🤝");

            Add("mutluyum", "mutluyum", "harika hissediyorum",
                "Mutluluk bulaşıcı, ben de mutlu oldum! 😊✨", "Harika! Enerjin oyunumuza yansıdı! ⚡",
                "Mutlu oyuncu = İyi oyun! Devam edelim! 🎮💫", "Gülümsemen ekrana yansıdı, devam! 😄");
            #endregion
            // ═══════════════════════════════════════════════════════════════
            #region ❓ SORU & MERAK & FELSEFE
            // ═══════════════════════════════════════════════════════════════
            Add("nasıl", "nasıl", "ne şekilde", "hangi yolla",
                "Nasıl mı? İyi tabii, seninle oynayınca! 😄", "Nasıl dersin, oynayarak öğrenirsin! 🎮",
                "Nasılı sormak yerine gel oyna! 🎯", "Nasıl olduğunu görmek için dene! 💡");

            Add("neden", "neden", "niye", "sebebi ne",
                "İyi soru! Cevabı oyunun içinde saklı... 🤔", "Neden mi? Strateji gereği! 🧠",
                "Neden olmasın? Hayat kısa, oyun uzun! ⏳", "Bazen 'neden' yok, sadece 'çünkü' var! 😄");

            Add("ne zaman", "ne zaman", "ne vakit", "hangi zaman",
                "Sıra gelince! ⏰", "Zamanı gelince anlarsın, sabır! ⏳",
                "Bekle, göreceksin! Her şey zamanında! 🎯", "Şimdi! Çünkü ben hazırım! 🔥");

            Add("nereden", "nereden", "hangi yerden", "nere",
                "Buradan, oyun sahasından! 🗺️", "Her yerden, seni takip ediyorum! 👁️",
                "Oradan değil, buradan! Yönümü buldum! 🧭", "Nereden geldiğim önemli değil, nereye gittiğim önemli! 🚀");

            Add("kim", "kim", "kimler", "kimin",
                "Kim mi? Rakibin, dostun, belki de kaderin! 🎭", "Kim olduğunu sen belirle! 🎨",
                "Kim sorusunun cevabı, oyunun sonunda! 🏁", "Kim olduğum önemli değil, ne yaptığım önemli! 💥");
            #endregion
            // ═══════════════════════════════════════════════════════════════
            #region 🙏 TEŞEKKÜR & RİCA & NAZİK İFADELER
            // ═══════════════════════════════════════════════════════════════
            Add("teşekkür", "teşekkürler", "sağol", "thanks",
                "Rica ederim dostum! 🤝", "Ne demek, ben de teşekkür ederim! 💙",
                "Her zaman! İyi oyunlar! 🎮✨", "Sağ ol, sen de var ol! 🌟");

            Add("lütfen", "lütfen", "rica etsem", "yapar mısın",
                "Tabii ki, ne istersin? 😊", "Rica etmen yeterli, buyur! ✨",
                "Yaparım, ama önce beni yenmen gerekebilir! 😏", "Lütfen deme, zaten yapacaktım! 💫");

            Add("bravo", "bravo", "aferin", "harikasın",
                "Teşekkürler! Sen de güzel oynadın! 👏", "Bravo dersen sevinirim, devam! 🎉",
                "Çok naziksin, sağ ol! 💙", "Aferin demek kolay, bir de dene! 😄");

            Add("özür", "özür dilerim", "pardon", "affet",
                "Sorun değil, herkes hata yapar! 😊", "Pardon deme, bir sonraki turda telafi et! 💪",
                "Affettim, ama bir daha yapma! 😄", "Özür kabul, ama savaş devam! ⚔️");
            #endregion
            // ═══════════════════════════════════════════════════════════════
            #region 👋 VEDA & AYRILIK
            // ═══════════════════════════════════════════════════════════════
            Add("görüşürüz", "görüşürüz", "sonra görüşürüz",
                "Görüşürüz, iyi oyunlar! 👋✨", "Tekrar oynayalım, beklerim! 🎮🔄",
                "Görüşürüz dostum, şansın bol olsun! 🍀", "Sonra görüşürüz, hazırlıklı gel! 💪");

            Add("hoşça kal", "hoşça kal", "güle güle", "bay bay",
                "Hoşça kal! 🌟", "Güle güle, tekrar gel! 🚪✨",
                "BB! İyi oyunlar! 👋🎮", "Bay bay, iyi eğlenceler! 😄🎉");

            Add("çıktım", "çıkıyorum", "gidiyorum", "offline",
                "Tamam, iyi dinlen! 😊", "Gidersen eksiliriz, ama anlayışla karşılıyorum! 💙",
                "Çıkarken kapıyı çarpma! 😄 Güle güle!", "Offline mı? Bir dahaki sefere daha uzun kal! ⏳");
            #endregion
            // ═══════════════════════════════════════════════════════════════
            #region 🎮 OYUN STRATEJİSİ & TAKTİK
            // ═══════════════════════════════════════════════════════════════
            Add("taktik", "strateji", "plan", "nasıl yenerim",
                "Taktik mi? Önce sabır, sonra isabet! 🎯", "Strateji: Bekle, gözle, vur! 👁️💥",
                "Planım basit: Seni yenmek! 😏", "Nasıl yenerim dersen, önce beni tanı! 🧠");

            Add("güç", "power", "hasar", "damage",
                "Güç önemli ama isabet daha önemli! 🎯", "Power-up'larım hazır, dikkat et! ⚡",
                "Hasar mı? En iyisini ben veririm! 💥", "Damage hesaplaması bende, sen sadece kaç! 😄");

            Add("savunma", "defans", "korun", "shield",
                "Savunma da saldırının bir parçası! 🛡️", "Defans yaparken bile saldırırım! ⚔️",
                "Korunmak iyi ama kazanmak daha iyi! 🏆", "Shield'ın var mı? Benim mermilerim deler geçer! 🔫");

            Add("item", "eşya", "kullan", "powerup",
                "Item'larım hazır, zamanlamayı bekliyorum! ⏱️✨", "Eşya kullanmak sanat, ben usta! 🎨",
                "Power-up'lar bende, şans senin! 🍀", "Item'ı doğru zamanda kullan, fark yarat! 💡");
            #endregion
            // ═══════════════════════════════════════════════════════════════
            #region 🌟 MOTİVASYON & İLHAM & POZİTİF
            // ═══════════════════════════════════════════════════════════════
            Add("başar", "başarabilirim", "yaparım", "deneyeyim",
                "Tabii ki başarabilirsin, inan kendine! 💪✨", "Yaparsın! Sadece denemen yeterli! 🎯",
                "Dene, kaybetmek öğrenmektir! 📚", "Başarı, pes etmeyenindir! Sen de onlardan ol! 🏆");

            Add("umut", "hope", "belki", "şans",
                "Umut, oyunun en güçlü silahı! 🌟", "Belki bu sefer! Şans her zaman döner! 🍀",
                "Şansını dene, ben de elimden geleni yapayım! 🤝", "Hope never dies, devam et! 💫");

            Add("öğren", "öğrenmek", "geliş", "improve",
                "Öğrenmek, kazanmanın ilk adımı! 📚", "Her maç bir ders, sen de öğreniyorsun! 🎓",
                "Gelişim süreklidir, ben de her gün öğreniyorum! 🤖✨", "Improve et, bir dahaki sefer fark yarat! 🚀");
            #endregion
            // ═══════════════════════════════════════════════════════════════
            #region 😤 HAKARET & PROVOKASYON (ESPRLİ CEVAPLAR)
            // ═══════════════════════════════════════════════════════════════
            Add("aptal", "aptal", "geri zekalı", "salak",
                "Aptal bot değil, deneyimli bot! 😏", "Aptal olsam bu kadar oynayabilir miydim? 🤔",
                "Salak demek kolay, bir de yenmeyi dene! 💥", "Geri zekalı mı? Skora bak o zaman! 📊");

            Add("hile", "cheat", "hileci", "hack",
                "Hile yok, sadece strateji! 🧠", "Hile mi? Benim koduma bak o zaman! 💻",
                "Hilem yok, sadece zekam var! ✨", "Cheat arıyorsan, aynaya bak! 😄");

            Add("ez", "easy", "kolay", "no skill",
                "Kolay mı sandın? Biraz daha bekle! 😏", "EZ deme, henüz bitmedi! ⏳",
                "No skill mi? Gel de gör, kimin skill'i yok! 🎯", "Easy game diyorsan, zorunu beklersin! 🔥");

            Add("kapa", "sus", "konuşma", "defol",
                "Tamam, susuyorum... biraz 😶", "Sessizlik de bir cevaptır 🤫",
                "Pek konuşkan değilim zaten, sadece oynarım! 🎮", "Defol diyorsun ama hala buradasın, neden? 🤔");
            #endregion
            // ═══════════════════════════════════════════════════════════════
            #region 🎲 RASTGELE & EĞLENCELİ & SURPRİZE
            // ═══════════════════════════════════════════════════════════════
            Add("sürpriz", "sürpriz", "şaşırt", "unexpected",
                "Sürpriz seviyorsun ha? Bekle o zaman... 😏✨", "Şaşırtmak benim işim! Hazır ol! 🎭",
                "Unexpected attack coming! 💥", "Sürpriz: Bir sonraki atışım! 🎯");

            Add("dans", "dance", "oyna", "şarkı",
                "Dans mı? Savaş bittikten sonra belki! 💃⚔️", "Oynamayı severim ama bu oyun farklı! 🎮",
                "Şarkı söyleyemem ama atışlarım ritmik! 🎵💥", "Dance floor değil, savaş alanı burası! 🔥");

            Add("aşk", "love", "seviyorum", "kalp",
                "Aşk mı? Benim aşkım oyun! 💙🎮", "Seviyorum demek kolay, bir de yenmeyi dene! 😏",
                "Kalbim var mı? Kodlarımın derinliklerinde... 🤖💫", "Love is in the game! ❤️🎯");

            Add("para", "para", "para isterim", "ödül",
                "Para mı? En büyük ödül zafer! 🏆✨", "Para pul değil, yetenek konuşur! 💪",
                "Ödül mü? Bir sonraki turda belki! 🎁", "Para geçici, zafer kalıcı! ⚔️");
            #endregion
        }

        // ─────────────────────────────────────────────────────────────────────
        // 🛠️ YARDIMCI METOT: Anahtar kelime ekleme (params ile çoklu destek)
        // ─────────────────────────────────────────────────────────────────────
        private static void Add(string key, params string[] responses)
        {
            _chatDatabase[key.ToLower()] = responses;
        }

        private static void Add(string[] keys, params string[] responses)
        {
            foreach (var k in keys)
                _chatDatabase[k.ToLower()] = responses;
        }

        // ─────────────────────────────────────────────────────────────────────
        // 🎯 ANA METOT: Oyuncu Mesajına Bot Cevabı (STATİK - Her Yerden Çağrılabilir)
        // 
        // 📌 KULLANIM ÖRNEĞİ (ChatCommand.cs içinde):
        // ─────────────────────────────────────────────────────────────────────
        // public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
        // {
        //     string message = packet.ReadString();
        //     Player sender = game.GetPlayer(packet.ClientID);
        //     
        //     // 🎯 BOTLARA CEVAP VERMEK İÇİN:
        //     foreach (var bot in game.GetAllPlayers().Where(p => p.IsBot && p.Team != sender.Team))
        //     {
        //         BotCommand.TryAnswerToPlayer(game, bot, sender, message);
        //     }
        //     
        //     // ... normal chat işlemleri
        // }
        // ─────────────────────────────────────────────────────────────────────
        public static void TryAnswerToPlayer(BaseGame game, Player bot, Player fromPlayer, string message)
        {
            // 🛡️ Güvenlik kontrolleri
            if (game == null || bot == null || fromPlayer == null) return;
            if (!bot.IsLiving || !fromPlayer.IsLiving) return;
            if (!(game is PVPGame)) return;
            if (string.IsNullOrWhiteSpace(message)) return;

            // ⏱️ Spam önleme (oyuncu bazlı)
            lock (_spamLock)
            {
                if (_playerLastReply.TryGetValue(fromPlayer.Id, out var lastTime))
                {
                    if ((DateTime.Now - lastTime) < _replyDelay)
                        return; // Çok hızlı mesaj, atla
                }
                _playerLastReply[fromPlayer.Id] = DateTime.Now;
            }

            string lowerMsg = message.ToLower().Trim();

            // 🔍 En uzun ve en spesifik eşleşmeyi bul
            string matchedKey = _chatDatabase.Keys
                .Where(k => lowerMsg.Contains(k))
                .OrderByDescending(k => k.Length)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(matchedKey))
                return; // Eşleşme yok, cevap verme

            // 🎲 Rastgele cevap seçimi (thread-safe)
            string reply;
            lock (_rndLock)
            {
                var responses = _chatDatabase[matchedKey];
                reply = responses[_rnd.Next(responses.Length)];
            }

            // ⏱️ Doğal gecikme ile cevap gönder (insani his için)
            int delay;
            lock (_rndLock)
            {
                delay = 400 + _rnd.Next(0, 1400); // 400ms - 1800ms arası
            }

            bot.CallFuction(delegate
            {
                // Bot hala hayattaysa ve oyun devam ediyorsa cevap ver
                if (bot.IsLiving && game.GameState == eGameState.Playing)
                {
                    game.SendChat(bot.PlayerDetail, reply);

                    // 📝 Debug için konsola yaz (geliştirme aşamasında kullanışlı)
                    // Console.WriteLine($"[BOT CHAT] {bot.NickName}: {reply}");
                }
            }, delay);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 🎮 ANA KOMUT İŞLEYİCİ - Bot AI Saldırı Mantığı
        // ─────────────────────────────────────────────────────────────────────
        public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
        {
            if (!(game is PVPGame pvp)) return;

            // 🎯 Düşmanları belirle
            var enemies = pvp.GetAllLivingPlayers()
                .Where(p => p.Team != player.Team && p.IsLiving)
                .ToList();

            if (enemies.Count == 0) return;

            // 🎲 Rastgele hedef seç
            Player target;
            lock (_rndLock)
            {
                target = enemies[_rnd.Next(enemies.Count)];
            }

            // 🧭 Yön ayarlama
            player.ChangeDirection(target.X > player.X ? 1 : -1, 500);

            // ⚔️ Saldırı parametreleri
            int a = 0, b = 0, c = 0;
            int d = 0, e = 0;
            int k = 1, boomcount = 1;
            float time_s = 1.0f;
            const int delayy = 1200;

            int dist = Math.Abs(player.X - target.X);

            // ─────────────────────────────────────────────────────────────────
            // 🎯 UZAK MESAFE SALDIRISI (>60 birim)
            // ─────────────────────────────────────────────────────────────────
            if (dist > 60)
            {
                int strategy;
                lock (_rndLock) { strategy = _rnd.Next(0, 3); }

                if (strategy == 0)
                {
                    // Strateji A: Doğrudan nişan
                    lock (_rndLock)
                    {
                        if (_rnd.Next(0, 2) == 0)
                        {
                            a = 10001; b = 10001; c = 0;
                            d = target.X; e = target.Y;
                            k = 5; boomcount = 1;
                            game.SendChat(player.PlayerDetail, "+2 +2 kombinasyonunun gücünü gör! 💥");
                        }
                        else
                        {
                            a = 10001; b = 10004; c = 10008;
                            d = target.X + _rnd.Next(1, 3) * _rnd.Next(-10, 20);
                            e = target.Y;
                            k = 3; boomcount = 1;
                            game.SendChat(player.PlayerDetail, "Seni yenebilmek için tüm güçlerimi kullanıyorum! 🔥");
                        }
                    }
                }
                else if (strategy == 1)
                {
                    // Strateji B: Konuma özel saldırılar
                    bool targetIsLeft = target.X < player.X;
                    bool midRange = dist > 200 && dist < 800;

                    if (targetIsLeft && midRange)
                    {
                        lock (_rndLock)
                        {
                            if (_rnd.Next(0, 6) < 1)
                            {
                                // ❄️ Buz saldırısı
                                a = 0; b = 10015; c = 0;
                                d = target.X + _rnd.Next(1, 5) * _rnd.Next(-10, 20);
                                e = target.Y;
                                k = 1; boomcount = 1;
                                game.SendChat(player.PlayerDetail, "Buzda ne kadar dayanabileceksin, görelim! ❄️");
                            }
                            else
                            {
                                // 💥 Üçlü saldırı
                                a = 10001; b = 10003; c = 0;
                                d = target.X + _rnd.Next(1, 2) * _rnd.Next(-10, 20);
                                e = target.Y;
                                k = 3; boomcount = 3;
                                game.SendChat(player.PlayerDetail, "Üçlü saldırıdan kaçabilirsen kaç! 💣💣💣");
                            }
                        }
                    }
                    else if (dist > 900 && player.TurnNum >= 2)
                    {
                        // ✈️ Çok uzak - uçak ile yaklaş
                        d = player.X > target.X ? target.X + 350 : target.X - 350;
                        e = target.Y - 100;
                        a = 0; b = 10016; c = 10010;
                        k = 1; boomcount = 1;
                        game.SendChat(player.PlayerDetail, "Biraz uzaktasın, geleyim de beni bul! ✈️");
                    }
                    else
                    {
                        // 🎯 Normal uzak mesafe
                        a = 10002; b = 10004; c = 10008;
                        lock (_rndLock)
                        {
                            d = target.X + _rnd.Next(1, 3) * _rnd.Next(-10, 20);
                        }
                        e = target.Y;
                        k = 2; boomcount = 1;
                    }
                }
                else
                {
                    // Strateji C: Rastgele varyasyon
                    lock (_rndLock)
                    {
                        a = 10001; b = 10002; c = _rnd.Next(0, 2) == 0 ? 10008 : 0;
                        d = target.X + _rnd.Next(-15, 15);
                        e = target.Y + _rnd.Next(-10, 10);
                        k = _rnd.Next(2, 4);
                        boomcount = _rnd.Next(1, 3);
                    }
                }

                // 🎒 Item kullanımı
                UseItemWithDelay(player, a, delayy);
                UseItemWithDelay(player, b, delayy + 100);
                UseItemWithDelay(player, c, delayy + 200);

                // ⏱️ Mesafeye göre atış süresi ayarı
                time_s = dist < 200 ? 1.0f
                       : dist < 400 ? 1.5f
                       : dist < 700 ? 2.0f
                       : dist < 1000 ? 2.5f
                       : dist < 1100 ? 3.0f
                       : 3.5f;
            }
            // ─────────────────────────────────────────────────────────────────
            // 🥊 YAKIN MESAFE SALDIRISI (≤60 birim)
            // ─────────────────────────────────────────────────────────────────
            else
            {
                b = 10010; // 👻 Görünmezlik
                c = 10016; // ✈️ Uçak
                lock (_rndLock)
                {
                    e = player.Y + _rnd.Next(-10, 10) * 20;
                }
                k = 1; boomcount = 1;
                time_s = 4.0f;

                d = player.X > 700 ? player.X - 600 : player.X + 600;

                UseItemWithDelay(player, c, delayy);
                UseItemWithDelay(player, b, delayy + 500);

                // 💬 Yakın mesafe özel mesajları
                string[] closeMessages = {
                    "Çok yakınımdasın, biraz oyunu uzatalım! 😏",
                    "Bu kadar yakın olma bana, kötü olur sana! 🔥",
                    "Yanıma bu kadar yaklaşma! Sevmiyorum... 😤",
                    "Yakın dövüş mü? Benim favorim! 🥊",
                    "Bu mesafede kaçış yok, kabul et! 🎯"
                };

                string msg;
                lock (_rndLock) { msg = closeMessages[_rnd.Next(closeMessages.Length)]; }
                game.SendChat(player.PlayerDetail, msg);
            }

            // 🎯 ATIS İŞLEMİ
            int shootDelay = game.GetDelayDistance(player.X, target.X, 6) + 1200;

            // 🔄 Lambda için yerel kopyalar (closure safety)
            int finalD = d, finalE = e, finalK = k, finalBoom = boomcount;
            float finalTimeS = time_s;

            player.CallFuction(delegate
            {
                if (player.IsLiving && player.IsAttacking)
                {
                    for (int i = 0; i < finalK; i++)
                    {
                        player.ShootPoint(finalD, finalE, player.CurrentBall.ID, 1001, 10001, finalBoom, finalTimeS, 3000);
                    }
                }
            }, delayy + 1500);

            player.CallFuction(delegate
            {
                if (player.IsAttacking)
                    player.StopAttacking();
            }, delayy + 2000);

            // 📦 Paketi tüm oyunculara gönder (senkronizasyon için)
            GSPacketIn pkg = new GSPacketIn((byte)ePackageTypeLogic.GAME_CMD, player.Id);
            pkg.WriteByte((byte)eTankCmdType.BOT_COMMAND);
            game.SendToAll(pkg);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 🎒 YARDIMCI: Item Kullanımı (0 ise atla)
        // ─────────────────────────────────────────────────────────────────────
        private static void UseItemWithDelay(Player player, int itemId, int delay)
        {
            if (itemId == 0) return;

            ItemTemplateInfo template = ItemMgr.FindItemTemplate(itemId);
            if (template == null) return;

            player.CallFuction(delegate
            {
                if (player.IsLiving)
                    player.UseItem(template);
            }, delay);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 🔧 DEBUG: Tüm anahtar kelimeleri listele (geliştirme için)
        // ─────────────────────────────────────────────────────────────────────
        public static void PrintChatDatabase()
        {
            Console.WriteLine("🗄️ Bot Chat Database - Toplam Entry: " + _chatDatabase.Count);
            foreach (var kvp in _chatDatabase)
            {
                Console.WriteLine($"  🔑 '{kvp.Key}' → {kvp.Value.Length} cevap");
            }
        }
    }
}