using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameUtils;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.NEWCHICKENBOX_SYS, "客户端日记")]
    public class ChickenBoxHandler : IPacketHandler
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ChickenBoxHandler));

        private const int ItemAddType = 105;
        private const int LuckyStarCooldownSeconds = 7;
        private const int MaxStrengthenLevel = 15;

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client?.Player?.Actives?.Info == null)
            {
                Logger.Error("ChickenBoxHandler: Client veya Player verisi null");
                return 1;
            }

            int cmd = packet.ReadInt();
            GSPacketIn pkg = new((byte)ePackageType.NEWCHICKENBOX_SYS);
            ActiveSystemInfo chickenBox = client.Player.Actives.Info;

            try
            {
                switch (cmd)
                {
                    case (int)NewChickenBoxPackageType.TAKEOVERCARD:
                        return HandleTakeOverCard(client, packet, pkg, chickenBox);

                    case (int)NewChickenBoxPackageType.USEEAGLEEYE:
                        return HandleUseEagleEye(client, packet, pkg, chickenBox);

                    case (int)NewChickenBoxPackageType.FLUSHCHICKENVIEW:
                        return HandleFlushChickenView(client, chickenBox);

                    case (int)NewChickenBoxPackageType.AllITEMSHOW:
                        return HandleAllItemShow(client);

                    case (int)NewChickenBoxPackageType.CLICKSTARTBNT:
                        return HandleClickStartButton(client, pkg);

                    case (int)NewChickenBoxPackageType.ENTERCHICKENVIEW:
                        return HandleEnterChickenView(client);

                    case (int)NewChickenBoxPackageType.ENTER_GAME:
                        return HandleEnterGame(client);

                    case (int)NewChickenBoxPackageType.CLOSE_GAME:
                        return HandleCloseGame();

                    case (int)NewChickenBoxPackageType.START_TURN:
                        return HandleStartTurn(client);

                    case (int)NewChickenBoxPackageType.TURN_COMPLETE:
                        return HandleTurnComplete(client);

                    default:
                        Logger.Warn($"Bilinmeyen NewChickenBoxPackageType: {(NewChickenBoxPackageType)cmd}");
                        return 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ChickenBoxHandler hata (cmd: {cmd})", ex);
                client.Player.SendMessage(LanguageMgr.GetTranslation("İşlem sırasında bir hata oluştu."));
                return 1;
            }
        }

        private int HandleTakeOverCard(GameClient client, GSPacketIn packet, GSPacketIn pkg, ActiveSystemInfo chickenBox)
        {
            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked"));
                return 1;
            }

            int position = packet.ReadInt();
            int openCounts = chickenBox.canOpenCounts;

            if (openCounts <= 0)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("Kart açma hakkınız kalmadı!"));
                return 1;
            }

            // Ödülü getir - Veritabanından taze çekerek compose değerlerini garantile
            NewChickenBoxItemInfo item = GetAwardFromDatabase(client, position);

            if (item == null)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("Bu ödül bulunamadı!"));
                return 1;
            }

            Logger.Info($"TakeOverCard - Position: {position}, TemplateID: {item.TemplateID}, " +
                       $"Attack: {item.AttackCompose}, Defend: {item.DefendCompose}, " +
                       $"Agility: {item.AgilityCompose}, Luck: {item.LuckCompose}, " +
                       $"Strengthen: {item.StrengthenLevel}");

            int priceIndex = Math.Min(openCounts - 1, client.Player.Actives.openCardPrice.Length - 1);
            int needMoney = client.Player.Actives.openCardPrice[priceIndex];

            if (!client.Player.MoneyDirect(needMoney, IsAntiMult: false, false, true))
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("Yetersiz bakiye!"));
                return 1;
            }

            try
            {
                item.IsBinds = true;
                item.IsSelected = true;

                ItemTemplateInfo template = ItemMgr.FindItemTemplate(item.TemplateID);
                if (template == null)
                {
                    Logger.Error($"Item template bulunamadı: {item.TemplateID}");
                    client.Player.SendMessage(LanguageMgr.GetTranslation("Ödül şablonu bulunamadı!"));
                    return 1;
                }

                // Item oluştur
                ItemInfo itemAward = ItemInfo.CreateFromTemplate(template, item.Count, ItemAddType);

                // TÜM özellikleri aktar
                ApplyItemProperties(itemAward, item);

                // Envantere ekle
                if (!client.Player.AddTemplate(itemAward, LanguageMgr.GetTranslation("Hazine Tarlası Ödülü!")))
                {
                    Logger.Error($"Item envantere eklenemedi: {item.TemplateID}, Count: {item.Count}");
                    client.Player.SendMessage(LanguageMgr.GetTranslation("Envanter dolu veya ödül eklenemedi!"));
                    return 1;
                }

                // Başarılı yanıt gönder
                SendItemPacketWithCompose(pkg, item, NewChickenBoxPackageType.TACKOVERCARD);
                pkg.WriteInt(client.Player.Actives.freeOpenCardCount);
                client.Out.SendTCP(pkg);

                // Veritabanını güncelle
                _ = client.Player.Actives.UpdateChickenBoxAward(item);

                // Kullanıcıya bilgi ver
                string composeInfo = GetComposeInfoString(item);
                string message = string.IsNullOrEmpty(composeInfo)
                    ? $"Tebrikler! {item.Count} adet {template.Name} kazandınız."
                    : $"Tebrikler! {item.Count} adet {template.Name} ({composeInfo}) kazandınız.";

                client.Player.SendMessage(LanguageMgr.GetTranslation(message));

                chickenBox.canOpenCounts--;

                if (chickenBox.canOpenCounts == 0)
                {
                    GSPacketIn endPkg = new((byte)ePackageType.NEWCHICKENBOX_SYS);
                    endPkg.WriteInt((byte)NewChickenBoxPackageType.OVERSHOWITEMS);
                    client.Player.SendTCP(endPkg);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Logger.Error("TakeOverCard işlemi sırasında hata", ex);
                client.Player.SendMessage(LanguageMgr.GetTranslation("Ödül alınırken hata oluştu!"));
                return 1;
            }
        }

        private int HandleUseEagleEye(GameClient client, GSPacketIn packet, GSPacketIn pkg, ActiveSystemInfo chickenBox)
        {
            int position = packet.ReadInt();
            int eagleEyeCounts = chickenBox.canEagleEyeCounts;

            if (eagleEyeCounts <= 0)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("Kartal Gözü Hakkınız Sona Erdi!"));
                return 1;
            }

            // Veritabanından taze çek
            NewChickenBoxItemInfo item = GetAwardFromDatabase(client, position);

            if (item == null)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("Ödül Bulunamadı!"));
                return 1;
            }

            Logger.Info($"UseEagleEye - Position: {position}, TemplateID: {item.TemplateID}, " +
                       $"Attack: {item.AttackCompose}, Defend: {item.DefendCompose}, " +
                       $"Agility: {item.AgilityCompose}, Luck: {item.LuckCompose}");

            int priceIndex = Math.Min(eagleEyeCounts - 1, client.Player.Actives.eagleEyePrice.Length - 1);
            int needMoney = client.Player.Actives.eagleEyePrice[priceIndex];

            if (!client.Player.MoneyDirect(needMoney, IsAntiMult: false, false, true))
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("Yetersiz bakiye!"));
                return 1;
            }

            try
            {
                item.IsSeeded = true;

                SendItemPacketWithCompose(pkg, item, NewChickenBoxPackageType.EAGLEEYE);
                pkg.WriteInt(client.Player.Actives.freeEyeCount);
                client.Player.SendTCP(pkg);

                _ = client.Player.Actives.UpdateChickenBoxAward(item);
                chickenBox.canEagleEyeCounts--;

                return 0;
            }
            catch (Exception ex)
            {
                Logger.Error("UseEagleEye işlemi sırasında hata", ex);
                return 1;
            }
        }

        /// <summary>
        /// Veritabanından direkt çekerek compose değerlerini garantiler
        /// </summary>
        private NewChickenBoxItemInfo GetAwardFromDatabase(GameClient client, int position)
        {
            try
            {
                // Önce cache'den kontrol et
                NewChickenBoxItemInfo cachedItem = client.Player.Actives.GetAward(position);

                if (cachedItem != null &&
                    cachedItem.AttackCompose == 0 &&
                    cachedItem.DefendCompose == 0 &&
                    cachedItem.AgilityCompose == 0 &&
                    cachedItem.LuckCompose == 0)
                {
                    // Cache'de compose değerleri yoksa veritabanından çek
                    using SqlConnection conn = new();
                    conn.Open();
                    string query = @"
                            SELECT ID, ActivityType, TemplateID, Count, ValidDate, IsBinds, 
                                   StrengthenLevel, AttackCompose, DefendCompose, AgilityCompose, LuckCompose, 
                                   Random, IsSelect 
                            FROM Event_Award_Item 
                            WHERE ActivityType = 3 AND ID = @ID";

                    using SqlCommand cmd = new(query, conn);
                    // TemplateID üzerinden bul veya position ile ilişkilendir
                    // Burada position ile TemplateID eşleştirmesi gerekebilir
                    _ = cmd.Parameters.AddWithValue("@ID", cachedItem.TemplateID); // Veya uygun ID

                    using SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        cachedItem.AttackCompose = reader.GetInt32(reader.GetOrdinal("AttackCompose"));
                        cachedItem.DefendCompose = reader.GetInt32(reader.GetOrdinal("DefendCompose"));
                        cachedItem.AgilityCompose = reader.GetInt32(reader.GetOrdinal("AgilityCompose"));
                        cachedItem.LuckCompose = reader.GetInt32(reader.GetOrdinal("LuckCompose"));
                        cachedItem.StrengthenLevel = reader.GetInt32(reader.GetOrdinal("StrengthenLevel"));

                        Logger.Info($"Database'den compose değerleri yüklendi - " +
                                   $"Attack: {cachedItem.AttackCompose}, " +
                                   $"Defend: {cachedItem.DefendCompose}, " +
                                   $"Agility: {cachedItem.AgilityCompose}, " +
                                   $"Luck: {cachedItem.LuckCompose}");
                    }
                }

                return cachedItem;
            }
            catch (Exception ex)
            {
                Logger.Error($"GetAwardFromDatabase hata: {ex.Message}");
                // Hata durumunda cache'den dön
                return client.Player.Actives.GetAward(position);
            }
        }

        private int HandleFlushChickenView(GameClient client, ActiveSystemInfo chickenBox)
        {
            try
            {
                bool isFreeFlush = !client.Player.Actives.IsFreeFlushTime();
                int needMoney = isFreeFlush ? 0 : client.Player.Actives.flushPrice;

                if (!isFreeFlush)
                {
                    if (!client.Player.MoneyDirect(needMoney, IsAntiMult: false, false, true))
                    {
                        client.Player.SendMessage(LanguageMgr.GetTranslation("Yetersiz bakiye!"));
                        return 1;
                    }
                    client.Player.SendMessage(LanguageMgr.GetTranslation("Başarıyla Yenilendi!"));
                }
                else
                {
                    client.Player.SendMessage(LanguageMgr.GetTranslation("Bugün Ücretsiz Yenileme Hakkınızı Kullandınız!"));
                }

                client.Player.Actives.PayFlushView();
                client.Player.Actives.SendChickenBoxItemList();

                return 0;
            }
            catch (Exception ex)
            {
                Logger.Error("FlushChickenView işlemi sırasında hata", ex);
                return 1;
            }
        }

        private int HandleAllItemShow(GameClient client)
        {
            try
            {
                client.Player.Actives.SendChickenBoxItemList();
                client.Player.Actives.PayFlushView();
                return 0;
            }
            catch (Exception ex)
            {
                Logger.Error("AllItemShow işlemi sırasında hata", ex);
                return 1;
            }
        }

        private int HandleClickStartButton(GameClient client, GSPacketIn pkg)
        {
            try
            {
                client.Player.Actives.Info.isShowAll = false;
                client.Player.Actives.RandomPosition();

                pkg.WriteInt((int)NewChickenBoxPackageType.CANCLICKCARD);
                pkg.WriteBoolean(true);
                client.Player.SendTCP(pkg);

                return 0;
            }
            catch (Exception ex)
            {
                Logger.Error("ClickStartButton işlemi sırasında hata", ex);
                return 1;
            }
        }

        private int HandleEnterChickenView(GameClient client)
        {
            try
            {
                client.Player.Actives.EnterChickenBox();
                client.Player.Actives.SendChickenBoxItemList();
                return 0;
            }
            catch (Exception ex)
            {
                Logger.Error("EnterChickenView işlemi sırasında hata", ex);
                return 1;
            }
        }

        private int HandleEnterGame(GameClient client)
        {
            try
            {
                client.Player.Actives.CreateLuckyStartAward();
                client.Player.Actives.SendLuckStarAllGoodsInfo();
                client.Player.Actives.SendLuckStarRewardRank();
                client.Player.Actives.SendLuckStarRewardRecord();
                return 0;
            }
            catch (Exception ex)
            {
                Logger.Error("EnterGame işlemi sırasında hata", ex);
                return 1;
            }
        }

        private int HandleCloseGame()
        {
            return 0;
        }

        private int HandleStartTurn(GameClient client)
        {
            try
            {
                DateTime nextAvailableTime = client.Player.Actives.LuckyStartStartTurn.AddSeconds(LuckyStarCooldownSeconds);

                if (DateTime.Now < nextAvailableTime)
                {
                    int remainingSeconds = (int)(nextAvailableTime - DateTime.Now).TotalSeconds;
                    client.Player.SendMessage(LanguageMgr.GetTranslation($"Lütfen {remainingSeconds} saniye bekleyin!"));
                    return 1;
                }

                int luckystarID = (int)EquipType.LUCKYSTAR_ID;
                ItemTemplateInfo templateInfo = ItemMgr.FindItemTemplate(luckystarID);

                if (templateInfo == null)
                {
                    Logger.Error($"LuckyStar template bulunamadı: {luckystarID}");
                    client.Player.SendMessage(LanguageMgr.GetTranslation("Şanslı yıldız şablonu bulunamadı!"));
                    return 1;
                }

                PlayerInventory bag = client.Player.GetInventory(templateInfo.BagType);
                ItemInfo luckystarItem = bag?.GetItemByTemplateID(0, luckystarID);

                if (luckystarItem == null || luckystarItem.Count <= 0)
                {
                    client.Player.SendMessage(
                        LanguageMgr.GetTranslation("Bu işlemi yapmak için {0} eşyasına ihtiyacınız var.", templateInfo.Name)
                    );
                    return 1;
                }

                _ = bag.RemoveTemplate(luckystarID, 1);
                client.Player.Actives.ChangeLuckyStartAwardPlace();
                client.Player.Actives.SendLuckStarTurnGoodsInfo();
                client.Player.Actives.LuckyStartStartTurn = DateTime.Now;

                return 0;
            }
            catch (Exception ex)
            {
                Logger.Error("StartTurn işlemi sırasında hata", ex);
                return 1;
            }
        }

        private int HandleTurnComplete(GameClient client)
        {
            try
            {
                client.Player.Actives.SendUpdateReward();
                NewChickenBoxItemInfo award = client.Player.Actives.Award;

                if (award == null)
                {
                    Logger.Error("TurnComplete: Award null");
                    return 1;
                }

                ItemTemplateInfo template = ItemMgr.FindItemTemplate(award.TemplateID);

                if (template == null)
                {
                    Logger.Error($"TurnComplete: Template bulunamadı {award.TemplateID}");
                    return 1;
                }

                Logger.Info($"TurnComplete - TemplateID: {award.TemplateID}, " +
                           $"Attack: {award.AttackCompose}, Defend: {award.DefendCompose}, " +
                           $"Agility: {award.AgilityCompose}, Luck: {award.LuckCompose}");

                if (template.CategoryID != client.Player.Actives.coinTemplateID)
                {
                    ItemInfo item = ItemInfo.CreateFromTemplate(template, award.Count, ItemAddType);

                    if (item != null)
                    {
                        ApplyItemProperties(item, award);
                        _ = client.Player.AddTemplate(item, LanguageMgr.GetTranslation("Hazine Tarlası"));
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                Logger.Error("TurnComplete işlemi sırasında hata", ex);
                return 1;
            }
        }

        private void ApplyItemProperties(ItemInfo target, NewChickenBoxItemInfo source)
        {
            target.IsBinds = source.IsBinds;
            target.ValidDate = source.ValidDate;
            target.StrengthenLevel = Math.Min(source.StrengthenLevel, MaxStrengthenLevel);
            target.AttackCompose = source.AttackCompose;
            target.DefendCompose = source.DefendCompose;
            target.AgilityCompose = source.AgilityCompose;
            target.LuckCompose = source.LuckCompose;
        }

        private string GetComposeInfoString(NewChickenBoxItemInfo item)
        {
            List<string> composes = new();

            if (item.AttackCompose > 0)
            {
                composes.Add($"Atk+{item.AttackCompose}");
            }

            if (item.DefendCompose > 0)
            {
                composes.Add($"Def+{item.DefendCompose}");
            }

            if (item.AgilityCompose > 0)
            {
                composes.Add($"Agi+{item.AgilityCompose}");
            }

            if (item.LuckCompose > 0)
            {
                composes.Add($"Luck+{item.LuckCompose}");
            }

            if (item.StrengthenLevel > 0)
            {
                composes.Insert(0, $"+{item.StrengthenLevel}");
            }

            return string.Join(", ", composes);
        }

        private void SendItemPacketWithCompose(GSPacketIn pkg, NewChickenBoxItemInfo item, NewChickenBoxPackageType type)
        {
            pkg.WriteInt((int)type);
            pkg.WriteInt(item.TemplateID);
            pkg.WriteInt(item.StrengthenLevel);
            pkg.WriteInt(item.Count);
            pkg.WriteInt(item.ValidDate);
            pkg.WriteInt(item.AttackCompose);
            pkg.WriteInt(item.DefendCompose);
            pkg.WriteInt(item.AgilityCompose);
            pkg.WriteInt(item.LuckCompose);
            pkg.WriteInt(item.Position);
            pkg.WriteBoolean(item.IsSelected);
            pkg.WriteBoolean(item.IsSeeded);
            pkg.WriteBoolean(item.IsBinds);
        }
    }
}