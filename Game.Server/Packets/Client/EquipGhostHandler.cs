using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.EQUIP_GHOST, "user ac action")]
    public class EquipGhostHandler : IPacketHandler
    {
        public static Random random = new();

        // Başarı havuzu: küçük = daha kolay
        private const int RATE_STONE_LOW = 20000;  // 11186 taşı (düşük seviye)
        private const int RATE_STONE_LOW_HARD = 120000; // 11186 taşı level>=8 (zorlaştırılmış)
        private const int RATE_STONE_MID = 40000;  // 11187 taşı
        private const int RATE_STONE_HIGH = 120000;  // 11188 taşı (en iyi taş)
        private const int RATE_DEFAULT = 30000;  // diğer taşlar

        // Pity eşiği: kaç başarısızlıkta ödül gelsin
        private const int PITY_THRESHOLD = 20;

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            ItemInfo item = client.Player.StoreBag.GetItemAt(1);
            ItemInfo luckItem = client.Player.StoreBag.GetItemAt(0);
            ItemInfo stone = client.Player.StoreBag.GetItemAt(2);

            // Seviye kontrolü
            if (client.Player.PlayerCharacter.Grade < 45)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.LevelErrorUsing"));
                return 0;
            }

            // Eşya ve taş kontrolü
            if (item == null || stone == null || stone.Template.Property1 != 118)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.EquipGhost.Msg1"));
                return 0;
            }

            // Şans eşyası yanlış tipte ise reddet
            if (luckItem != null && luckItem.Template.Property1 != 117)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.EquipGhost.Msg4"));
                return 0;
            }

            // Spirit listesi al
            List<SpiritInfo> spiList = SpiritInfoMgr.GetSpirit(item.Template.CategoryID);
            if (spiList.Count <= 0)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.EquipGhost.Msg2"));
                return 0;
            }

            // Mevcut ghost verisini al, yoksa oluştur
            UserEquipGhostInfo equip = client.Player.GetGhostEquip(spiList[0].BagType, spiList[0].BagPlace);
            if (equip == null)
            {
                equip = new UserEquipGhostInfo
                {
                    UserID = client.Player.PlayerId,
                    BagType = spiList[0].BagType,
                    Place = spiList[0].BagPlace,
                    Level = 0,
                    TotalGhost = 0
                };
                client.Player.AddEquipGhost(equip);
            }

            // Sonraki seviye bilgisi
            SpiritInfo nextLevelInfo = spiList.SingleOrDefault(a => a.Level == equip.Level + 1);
            if (nextLevelInfo == null)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.EquipGhost.Msg3"));
                return 0;
            }

            // Şans çarpanı (luckItem varsa bonus ekle)
            double luckRatio = (luckItem != null) ? (1.0 + (luckItem.Template.Property2 / 100.0)) : 1.0;

            // Ham oran hesabı: taş seviyesi yüksek + hedef seviye düşük = daha kolay
            double rawRatio = 5.0 * Math.Pow(2.0, Math.Pow(2.0, stone.Template.Level - 1.0) + 2.0 - nextLevelInfo.Level) * luckRatio;
            int rawRatioInt = (int)(rawRatio * 100);

            // Taş tipine göre havuz büyüklüğü belirle
            int rate = stone.Template.TemplateID switch
            {
                11186 when equip.Level >= 8 => RATE_STONE_LOW_HARD,
                11186 => RATE_STONE_LOW,
                11187 => RATE_STONE_MID,
                11188 => RATE_STONE_HIGH,
                _ => RATE_DEFAULT
            };

            // Pity bonusu: her başarısızlıkta havuzu hafifçe küçült (max %40 azaltma)
            int missCount = client.Player.CountMissedEquipGhost;
            double pityMul = Math.Max(0.6, 1.0 - (missCount * 0.002)); // her başarısızlıkta %0.2 kolaylaşır
            int effectiveRate = (int)(rate * pityMul);

            // Eşyaları tüket (taş + şans eşyası)
            _ = client.Player.StoreBag.RemoveCountFromStack(stone, 1);
            _ = client.Player.StoreBag.RemoveCountFromStack(luckItem, 1);

            // Başarı kontrolü
            bool isSuccess = random.Next(effectiveRate) < rawRatioInt;

            if (isSuccess)
            {
                equip.Level++;
                client.Player.CountMissedEquipGhost = 0;
                client.Player.EquipBag.UpdatePlayerProperties();
                _ = client.Out.SendUserSyncEquipGhost(client.Player);
            }
            else
            {
                client.Player.CountMissedEquipGhost++;
            }

            // Pity ödülü: PITY_THRESHOLD başarısızlıkta taş gönder
            if (client.Player.CountMissedEquipGhost >= PITY_THRESHOLD)
            {
                client.Player.CountMissedEquipGhost = 0;
                string title = "Orta Sonbahar Festivali Etkinlik Ödülleri";
                string content = $"Efsununuzu {PITY_THRESHOLD} kez ardarda artırmayı başaramazsanız 10 taş size hediye gelir!";
                _ = client.Player.SendItemToMail(11188, 10, content, title);
            }

            // Seviye → görsel değer tablosu
            double levelGhost = equip.Level switch
            {
                1 => 0.5,
                2 => 1.0,
                3 => 1.5,
                4 => 2.0,
                5 => 2.5,
                6 => 3.0,
                7 => 3.5,
                8 => 4.0,
                9 => 4.5,
                10 => 5.0,
                _ => 0.0,
            };

            // Level 3+ başarıda sunucu geneli duyuru
            if (isSuccess && equip.Level >= 3)
            {
                GameServer.Instance.LoginServer.SendPacket(
                    WorldMgr.SendSysNotice(
                        eMessageType.ChatNormal,
                        LanguageMgr.GetTranslation(
                            "Tebrikler. Efsun Arttı. İtem: ",
                            client.Player.ZoneName,
                            client.Player.PlayerCharacter.NickName,
                            item.TemplateID,
                            levelGhost),
                        item.ItemID,
                        item.TemplateID,
                        null));
            }

            // Sonucu istemciye gönder
            GSPacketIn pkg = new((int)ePackageType.EQUIP_GHOST);
            pkg.WriteBoolean(isSuccess);
            client.SendTCP(pkg);

            return 1;
        }
    }
}