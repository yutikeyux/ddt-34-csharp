using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
    [PacketHandler((short)ePackageType.AVATAR_COLLECTION, "Avatar Collection Handler")]
    public class AvatarCollectionHandler : IPacketHandler
    {
        #region Sabitler

        private const byte CMD_ACTIVATE_ITEM = 3; // AvatarCollectionPackageType.ACTIVE
        private const byte CMD_RENEW_COLLECTION = 4; // AvatarCollectionPackageType.DELAY_TIME
        private const int PACKET_ID = 402;
        private const int DEFAULT_ACTIVATE_DAYS = 7;

        private const string ERROR_ITEM_NOT_FOUND = "Böyle bir çizim bulunamadı.";
        private const string ERROR_SET_NOT_AVAILABLE = "Bu çizim seti mevcut değil.";
        private const string ERROR_INSUFFICIENT_GOLD = "Yetersiz altın.";
        private const string ERROR_INSUFFICIENT_HONOR = "Yetersiz onur puanı.";
        private const string ERROR_ACTIVATION_FAILED = "Çizim etkinleştirme hatası.";
        private const string ERROR_NOT_ACTIVATED = "Bu kıyafeti henüz etkinleştirmediniz.";
        private const string ERROR_RENEW_NOT_ENOUGH_ITEMS = "Yenileme için çizim setinin yarısından fazlasını etkinleştirmeniz gerekmektedir.";
        private const string ERROR_CANNOT_RENEW = "Bu çizim seti yenilenemez!";
        private const string ERROR_INVALID_DAYS = "Geçersiz gün değeri.";

        private const string SUCCESS_ACTIVATION = "Çizim başarıyla etkinleştirildi!";
        private const string SUCCESS_RENEWAL = "Çizim seti başarıyla {0} gün yenilendi.";

        #endregion

        // -----------------------------------------------------------------------
        // ANA İŞLEYİCİ
        // -----------------------------------------------------------------------

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client == null || client.Player == null)
            {
                Console.WriteLine("[AvatarCollection] Null client veya player.");
                return 0;
            }

            try
            {
                byte subCommand = packet.ReadByte();
                Console.WriteLine($"[AvatarCollection] subCommand={subCommand}");

                switch (subCommand)
                {
                    case CMD_ACTIVATE_ITEM:    // 3 = AvatarCollectionPackageType.ACTIVE
                        return HandleActivateItem(client, packet);

                    case CMD_RENEW_COLLECTION: // 4 = AvatarCollectionPackageType.DELAY_TIME
                        return HandleRenewCollection(client, packet);

                    default:
                        Console.WriteLine($"[AvatarCollection] Bilinmeyen subCommand: {subCommand}");
                        return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[AvatarCollection] HandlePacket hatası: " + ex);
                client.Player.SendMessage("İşlem sırasında bir hata oluştu.");
                return 0;
            }
        }

        // -----------------------------------------------------------------------
        // ETKİNLEŞTİRME — subCommand 3
        //
        // AS sendAvatarCollectionActive(param1, param2, param3):
        //   writeByte(ACTIVE=3)   ← subCommand (zaten okundu)
        //   writeInt(param1)      → groupId
        //   writeInt(param2)      → templateId
        //   writeInt(param3)      → sex
        // -----------------------------------------------------------------------

        private int HandleActivateItem(GameClient client, GSPacketIn packet)
        {
            int groupId = packet.ReadInt(); // param1
            int templateId = packet.ReadInt(); // param2
            int sex = packet.ReadInt(); // param3

            Console.WriteLine($"[AvatarCollection] ACTIVATE → groupId={groupId}, templateId={templateId}, sex={sex}");

            if (groupId <= 0 || templateId <= 0)
            {
                Console.WriteLine("[AvatarCollection] groupId veya templateId geçersiz.");
                client.Player.SendMessage(ERROR_ITEM_NOT_FOUND);
                return 1;
            }

            // --- Envanter kontrolü ---
            // GetItemByTemplateID(minSlot=0, templateId): slot 0'dan tüm EquipBag'i tarar
            ItemInfo playerItem = client.Player.EquipBag.GetItemByTemplateID(0, templateId);
            Console.WriteLine($"[AvatarCollection] EquipBag templateId={templateId} → {(playerItem != null ? "BULUNDU" : "BULUNAMADI")}");

            if (playerItem == null)
            {
                client.Player.SendMessage(ERROR_ITEM_NOT_FOUND);
                return 1;
            }

            // --- Koleksiyon grubu kontrolü ---
            //
            // DİKKAT: ClothGroupTemplateInfoMgr.LoadClothGroup sözlüğe
            //   clothGroup.Add(item.ItemID, item) ile yüklüyor.
            // GetClothGroup ise Values üzerinde item.ID + item.TemplateID + item.Sex
            // üçlüsüyle arıyor.
            //
            // Eğer ItemID != ID ise eşleşme hiç bulunamaz.
            // Aşağıdaki tanı logları bunu gösterecek.
            ClothGroupTemplateInfo clothGroup =
                ClothGroupTemplateInfoMgr.GetClothGroup(groupId, templateId, sex);

            Console.WriteLine($"[AvatarCollection] GetClothGroup({groupId},{templateId},{sex}) → " +
                              $"{(clothGroup != null ? $"BULUNDU cost={clothGroup.Cost}" : "BULUNAMADI")}");

            if (clothGroup == null)
            {
                // Tanı 1: groupId ile herhangi bir kayıt var mı?
                List<ClothGroupTemplateInfo> groupRecords =
                    ClothGroupTemplateInfoMgr.GetClothGroupWithID(groupId);

                Console.WriteLine($"[AvatarCollection] GetClothGroupWithID({groupId}) → {groupRecords?.Count ?? 0} kayıt");
                if (groupRecords != null)
                    foreach (var r in groupRecords)
                        Console.WriteLine($"[AvatarCollection]   ItemID={r.ItemID}, ID={r.ID}, TemplateID={r.TemplateID}, Sex={r.Sex}");

                // Tanı 2: templateId groupId'ye eşit mi? (AS param1=groupId param2=templateId)
                Console.WriteLine($"[AvatarCollection] templateId({templateId}) == groupId({groupId})? {templateId == groupId}");

                client.Player.SendMessage(ERROR_ITEM_NOT_FOUND);
                return 1;
            }

            // --- Koleksiyon özelliği kontrolü ---
            ClothPropertyTemplateInfo clothProperty =
                ClothPropertyTemplateInfoMgr.GetClothPropertyWithID(clothGroup.ID);

            Console.WriteLine($"[AvatarCollection] GetClothPropertyWithID({clothGroup.ID}) → " +
                              $"{(clothProperty != null ? "BULUNDU" : "BULUNAMADI")}");

            if (clothProperty == null)
            {
                client.Player.SendMessage(ERROR_SET_NOT_AVAILABLE);
                return 1;
            }

            // --- Altın kontrolü ---
            Console.WriteLine($"[AvatarCollection] Oyuncu altın={client.Player.PlayerCharacter.Gold}, Gereken={clothGroup.Cost}");

            if (client.Player.PlayerCharacter.Gold < clothGroup.Cost)
            {
                client.Player.SendMessage(ERROR_INSUFFICIENT_GOLD);
                return 1;
            }

            return ExecuteActivation(client, clothGroup, clothProperty);
        }

        // -----------------------------------------------------------------------
        // ETKİNLEŞTİRME UYGULAMA
        // -----------------------------------------------------------------------

        private int ExecuteActivation(
            GameClient client,
            ClothGroupTemplateInfo clothGroup,
            ClothPropertyTemplateInfo clothProperty)
        {
            bool isNewlyActivated = false;

            int removeResult = client.Player.RemoveGold(clothGroup.Cost);
            if (removeResult <= 0)
            {
                client.Player.SendMessage(ERROR_INSUFFICIENT_GOLD);
                return 1;
            }

            try
            {
                UserAvatarCollectionInfo collection =
                    GetOrCreateCollection(client, clothProperty);

                var newItem = new UserAvatarCollectionDataInfo(clothGroup.TemplateID, clothGroup.Sex);

                if (!collection.AddItem(newItem))
                {
                    client.Player.AddGold(clothGroup.Cost);
                    client.Player.SendMessage(ERROR_ACTIVATION_FAILED);
                    return 1;
                }

                int totalItems = ClothGroupTemplateInfoMgr.CountClothGroupWithID(collection.AvatarID);
                int halfItems = totalItems / 2;
                int currentCount = collection.Items != null ? collection.Items.Count : 0;

                if (currentCount >= halfItems && !collection.IsActive)
                {
                    collection.ActiveAvatar(DEFAULT_ACTIVATE_DAYS);
                    isNewlyActivated = true;
                }

                SendActivationPackets(client, collection, clothGroup, isNewlyActivated);

                client.Player.EquipBag.UpdatePlayerProperties();
                client.Player.SendMessage(SUCCESS_ACTIVATION);
                return 1;
            }
            catch (Exception ex)
            {
                client.Player.AddGold(clothGroup.Cost);
                Console.WriteLine("[AvatarCollection] ExecuteActivation hatası: " + ex);
                client.Player.SendMessage(ERROR_ACTIVATION_FAILED);
                return 0;
            }
        }

        private static UserAvatarCollectionInfo GetOrCreateCollection(
            GameClient client,
            ClothPropertyTemplateInfo clothProperty)
        {
            UserAvatarCollectionInfo collection =
                client.Player.AvatarCollect.GetAvatarCollectWithAvatarID(clothProperty.ID);

            if (collection == null)
            {
                collection = new UserAvatarCollectionInfo(
                    client.Player.PlayerCharacter.ID,
                    clothProperty.ID,
                    clothProperty.Sex,
                    false,
                    DateTime.Now);

                client.Player.AvatarCollect.AddAvatarCollection(collection);
            }

            return collection;
        }

        private static void SendActivationPackets(
            GameClient client,
            UserAvatarCollectionInfo collection,
            ClothGroupTemplateInfo clothGroup,
            bool isNewlyActivated)
        {
            if (isNewlyActivated)
            {
                GSPacketIn activationPacket = new GSPacketIn(PACKET_ID);
                activationPacket.WriteByte(CMD_RENEW_COLLECTION);
                activationPacket.WriteInt(collection.AvatarID);
                activationPacket.WriteInt(collection.Sex);
                activationPacket.WriteDateTime(collection.TimeEnd);
                client.Player.SendTCP(activationPacket);
            }

            GSPacketIn confirmPacket = new GSPacketIn(PACKET_ID);
            confirmPacket.WriteByte(CMD_ACTIVATE_ITEM);
            confirmPacket.WriteInt(clothGroup.ID);
            confirmPacket.WriteInt(clothGroup.TemplateID);
            confirmPacket.WriteInt(clothGroup.Sex);
            client.Player.SendTCP(confirmPacket);
        }

        // -----------------------------------------------------------------------
        // YENİLEME — subCommand 4
        //
        // AS sendAvatarCollectionDelayTime(avatarId, days):
        //   writeByte(DELAY_TIME=4)  ← subCommand (zaten okundu)
        //   writeInt(avatarId)
        //   writeInt(days)
        // -----------------------------------------------------------------------

        private int HandleRenewCollection(GameClient client, GSPacketIn packet)
        {
            int avatarId = packet.ReadInt();
            int days = packet.ReadInt();

            Console.WriteLine($"[AvatarCollection] RENEW → avatarId={avatarId}, days={days}");

            if (days <= 0)
            {
                client.Player.SendMessage(ERROR_INVALID_DAYS);
                return 1;
            }

            return avatarId == -1
                ? HandleMassRenewal(client, days)
                : HandleSingleRenewal(client, avatarId, days);
        }

        private int HandleMassRenewal(GameClient client, int days)
        {
            int renewedCount = 0;

            foreach (UserAvatarCollectionInfo collection in
                     client.Player.AvatarCollect.AvatarCollect)
            {
                if (collection.IsAvailable()) continue;
                if (!CanRenewCollection(collection)) continue;

                ClothPropertyTemplateInfo property =
                    ClothPropertyTemplateInfoMgr.GetClothPropertyWithID(collection.AvatarID);
                if (property == null) continue;

                int totalCost = CalculateRenewalCost(collection, property, days);
                if (totalCost <= 0) continue;
                if (client.Player.PlayerCharacter.myHonor < totalCost) continue;

                client.Player.RemovemyHonor(totalCost);
                collection.ActiveAvatar(days);
                SendRenewalPacket(client, collection);
                renewedCount++;
            }

            if (renewedCount > 0)
            {
                client.Player.Out.SendAvatarCollect(client.Player.AvatarCollect);
                client.Player.EquipBag.UpdatePlayerProperties();
                client.Player.SendMessage(string.Format(SUCCESS_RENEWAL, days));
            }
            else
            {
                client.Player.SendMessage(ERROR_RENEW_NOT_ENOUGH_ITEMS);
            }

            return 1;
        }

        private int HandleSingleRenewal(GameClient client, int avatarId, int days)
        {
            UserAvatarCollectionInfo collection =
                client.Player.AvatarCollect.GetAvatarCollectWithAvatarID(avatarId);

            if (collection == null)
            {
                client.Player.SendMessage(ERROR_NOT_ACTIVATED);
                return 1;
            }

            if (collection.Items == null)
                collection.UpdateItems();

            if (!CanRenewCollection(collection))
            {
                client.Player.SendMessage(ERROR_RENEW_NOT_ENOUGH_ITEMS);
                return 1;
            }

            ClothPropertyTemplateInfo property =
                ClothPropertyTemplateInfoMgr.GetClothPropertyWithID(avatarId);
            if (property == null)
            {
                client.Player.SendMessage(ERROR_CANNOT_RENEW);
                return 1;
            }

            int totalCost = CalculateRenewalCost(collection, property, days);

            if (totalCost <= 0 ||
                client.Player.PlayerCharacter.myHonor < totalCost)
            {
                client.Player.SendMessage(ERROR_INSUFFICIENT_HONOR);
                return 1;
            }

            client.Player.RemovemyHonor(totalCost);
            collection.ActiveAvatar(days);
            SendRenewalPacket(client, collection);

            client.Player.EquipBag.UpdatePlayerProperties();
            client.Player.SendMessage(string.Format(SUCCESS_RENEWAL, days));
            return 1;
        }

        private static void SendRenewalPacket(
            GameClient client,
            UserAvatarCollectionInfo collection)
        {
            GSPacketIn packet = new GSPacketIn(PACKET_ID);
            packet.WriteByte(CMD_RENEW_COLLECTION);
            packet.WriteInt(collection.AvatarID);
            packet.WriteInt(collection.Sex);
            packet.WriteDateTime(collection.TimeEnd);
            client.Player.SendTCP(packet);
        }

        // -----------------------------------------------------------------------
        // YARDIMCI METODlar
        // -----------------------------------------------------------------------

        private static int CalculateRenewalCost(
            UserAvatarCollectionInfo collection,
            ClothPropertyTemplateInfo property,
            int days)
        {
            int totalItems = ClothGroupTemplateInfoMgr.CountClothGroupWithID(collection.AvatarID);
            int currentItems = collection.Items != null ? collection.Items.Count : 0;
            int baseHonor = currentItems >= totalItems ? property.Cost * 2 : property.Cost;
            return baseHonor * days;
        }

        private static bool CanRenewCollection(UserAvatarCollectionInfo collection)
        {
            int totalItems = ClothGroupTemplateInfoMgr.CountClothGroupWithID(collection.AvatarID);
            int requiredItems = totalItems / 2;
            int currentItems = collection.Items != null ? collection.Items.Count : 0;
            return currentItems >= requiredItems;
        }
    }
}