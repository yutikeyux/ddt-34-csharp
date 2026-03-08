using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Server.Packets.Client
{
    [PacketHandler((short)ePackageType.AVATAR_COLLECTION, "Avatar Collection Handler")]
    public class AvatarCollectionHandler : IPacketHandler
    {
        #region Constants

        private const byte CMD_ACTIVATE_ITEM = 3;
        private const byte CMD_RENEW_COLLECTION = 4;
        private const int PACKET_ID = 402;
        private const int DEFAULT_ACTIVATE_DAYS = 7;
        private const string ERROR_ITEM_NOT_FOUND = "Böyle bir çizim bulunamadı.";
        private const string ERROR_SET_NOT_AVAILABLE = "Bu çizim seti mevcut değil.";
        private const string ERROR_INSUFFICIENT_GOLD = "Yetersiz altın.";
        private const string ERROR_BAG_FULL = "Envanter dolu! Mail kutunuzu kontrol edin.";
        private const string ERROR_ACTIVATION_FAILED = "Çizim etkinleştirme hatası.";
        private const string ERROR_NOT_ACTIVATED = "Bu kıyafeti henüz etkinleştirmediniz.";
        private const string ERROR_RENEW_NOT_ENOUGH_ITEMS = "Yenileme için çizim setinin yarısından fazlasını aktifleştirmeniz gerekmektedir.";
        private const string ERROR_CANNOT_RENEW = "Bu çizim seti yenilenemez!";
        private const string ERROR_FEATURE_DISABLED = "Bu özellik şu an aktif değil.";
        private const string SUCCESS_ACTIVATION = "Çizim başarıyla etkinleştirildi!";
        private const string SUCCESS_RENEWAL = "{0} çizim seti başarıyla yenilendi.";

        #endregion

        #region Packet Handler

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client == null || client.Player == null)
            {
                Console.WriteLine("[AvatarCollectionHandler] Null client or player");
                return 0;
            }

            try
            {
                byte subCommand = packet.ReadByte();
                Console.WriteLine("[AvatarCollectionHandler] Command received: " + subCommand);

                switch (subCommand)
                {
                    case CMD_ACTIVATE_ITEM:
                        return HandleActivateItem(client, packet);

                    case CMD_RENEW_COLLECTION:
                        return HandleRenewCollection(client, packet);

                    default:
                        Console.WriteLine("[AvatarCollectionHandler] Unknown command: " + subCommand);
                        return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[AvatarCollectionHandler] Exception: " + ex);
                client.Player.SendMessage("İşlem sırasında bir hata oluştu.");
                return 0;
            }
        }

        #endregion

        #region Command Handlers

        /// <summary>
        /// Handles item activation (subCommand = 3)
        /// </summary>
        private int HandleActivateItem(GameClient client, GSPacketIn packet)
        {
            // Read packet data
            int groupId = packet.ReadInt();
            int templateId = packet.ReadInt();
            int sex = packet.ReadInt();

            // Validate input
            if (groupId <= 0 || templateId <= 0)
            {
                client.Player.SendMessage(ERROR_ITEM_NOT_FOUND);
                return 1;
            }

            // Check if player has the item
            var playerItem = client.Player.EquipBag.GetItemByTemplateID(0, templateId);
            if (playerItem == null)
            {
                client.Player.SendMessage(ERROR_BAG_FULL);
                return 1;
            }

            // Get cloth group info
            var clothGroup = ClothGroupTemplateInfoMgr.GetClothGroup(groupId, templateId, sex);
            if (clothGroup == null)
            {
                client.Player.SendMessage(ERROR_ITEM_NOT_FOUND);
                return 1;
            }

            // Get property info
            var clothProperty = ClothPropertyTemplateInfoMgr.GetClothPropertyWithID(clothGroup.ID);
            if (clothProperty == null)
            {
                client.Player.SendMessage(ERROR_SET_NOT_AVAILABLE);
                return 1;
            }

            // Check gold
            if (client.Player.PlayerCharacter.Gold < clothGroup.Cost)
            {
                client.Player.SendMessage(ERROR_INSUFFICIENT_GOLD);
                return 1;
            }

            // Execute activation
            return ExecuteActivation(client, clothGroup, clothProperty);
        }

        /// <summary>
        /// Handles collection renewal (subCommand = 4)
        /// </summary>
        private int HandleRenewCollection(GameClient client, GSPacketIn packet)
        {
            int avatarId = packet.ReadInt();
            int days = packet.ReadInt();

            if (days <= 0)
            {
                return 0;
            }

            // Mass renewal (avatarId = -1)
            if (avatarId == -1)
            {
                return HandleMassRenewal(client, days);
            }

            // Single renewal
            return HandleSingleRenewal(client, avatarId, days);
        }

        #endregion

        #region Activation Logic

        private int ExecuteActivation(GameClient client, ClothGroupTemplateInfo clothGroup, ClothPropertyTemplateInfo clothProperty)
        {
            bool isNewlyActivated = false;
            bool shouldUpdateProperties = false;

            // Deduct gold first
            int removeResult = client.Player.RemoveGold(clothGroup.Cost);
            if (removeResult <= 0)
            {
                client.Player.SendMessage(ERROR_INSUFFICIENT_GOLD);
                return 1;
            }

            try
            {
                // Get or create collection
                var collection = GetOrCreateCollection(client, clothProperty);

                // Add item to collection
                var newItem = new UserAvatarCollectionDataInfo(clothGroup.TemplateID, clothGroup.Sex);

                if (!collection.AddItem(newItem))
                {
                    // Rollback gold
                    client.Player.AddGold(clothGroup.Cost);
                    client.Player.SendMessage(ERROR_ACTIVATION_FAILED);
                    return 1;
                }

                // Check activation conditions
                int totalItems = ClothGroupTemplateInfoMgr.CountClothGroupWithID(collection.AvatarID);
                int requiredForActivation = totalItems / 2;
                int currentCount = collection.Items != null ? collection.Items.Count : 0;

                // Activate if half collection is complete and not already active
                if (currentCount == requiredForActivation && !collection.IsActive)
                {
                    collection.ActiveAvatar(DEFAULT_ACTIVATE_DAYS);
                    isNewlyActivated = true;
                }

                // Check if we should update player properties
                if (currentCount == requiredForActivation || currentCount == totalItems)
                {
                    shouldUpdateProperties = true;
                }

                // Send packets
                SendActivationPackets(client, collection, clothGroup, isNewlyActivated);

                // Update properties if needed
                if (shouldUpdateProperties)
                {
                    client.Player.EquipBag.UpdatePlayerProperties();
                }

                client.Player.SendMessage(SUCCESS_ACTIVATION);
                return 1;
            }
            catch (Exception ex)
            {
                // Rollback gold on exception
                client.Player.AddGold(clothGroup.Cost);
                Console.WriteLine("[AvatarCollectionHandler] Activation error: " + ex);
                client.Player.SendMessage(ERROR_ACTIVATION_FAILED);
                return 0;
            }
        }

        private UserAvatarCollectionInfo GetOrCreateCollection(GameClient client, ClothPropertyTemplateInfo clothProperty)
        {
            var collection = client.Player.AvatarCollect.GetAvatarCollectWithAvatarID(clothProperty.ID);

            if (collection == null)
            {
                collection = new UserAvatarCollectionInfo(
                    client.Player.PlayerCharacter.ID,
                    clothProperty.ID,
                    clothProperty.Sex,
                    false,
                    DateTime.Now
                );
                client.Player.AvatarCollect.AddAvatarCollection(collection);
            }

            return collection;
        }

        private void SendActivationPackets(GameClient client, UserAvatarCollectionInfo collection,
            ClothGroupTemplateInfo clothGroup, bool isNewlyActivated)
        {
            // Send activation notification if newly activated
            if (isNewlyActivated)
            {
                GSPacketIn activationPacket = new GSPacketIn(PACKET_ID);
                activationPacket.WriteByte(4);
                activationPacket.WriteInt(collection.AvatarID);
                activationPacket.WriteInt(collection.Sex);
                activationPacket.WriteDateTime(collection.TimeEnd);
                client.Player.SendTCP(activationPacket);
            }

            // Send item activation confirmation
            GSPacketIn confirmPacket = new GSPacketIn(PACKET_ID);
            confirmPacket.WriteByte(3);
            confirmPacket.WriteInt(clothGroup.ID);
            confirmPacket.WriteInt(clothGroup.TemplateID);
            confirmPacket.WriteInt(clothGroup.Sex);
            client.Player.SendTCP(confirmPacket);
        }

        #endregion

        #region Renewal Logic

        private int HandleMassRenewal(GameClient client, int days)
        {
            // Feature disabled - can be enabled later
            client.Player.SendMessage(ERROR_FEATURE_DISABLED);
            return 1;

            /* 
            // Implementation when feature is enabled:
            int renewedCount = 0;
            
            foreach (var collection in client.Player.AvatarCollect.AvatarCollect.ToList())
            {
                if (!collection.IsAvailable())
                {
                    var property = ClothPropertyTemplateInfoMgr.GetClothPropertyWithID(collection.AvatarID);
                    if (property != null && CanRenewCollection(collection))
                    {
                        // Calculate cost and deduct
                        int cost = property.Cost * days;
                        if (client.Player.PlayerCharacter.myHonor >= cost && cost > 0)
                        {
                            client.Player.RemovemyHonor(cost);
                            collection.ActiveAvatar(days);
                            renewedCount++;
                        }
                    }
                }
            }

            if (renewedCount > 0)
            {
                client.Player.Out.SendAvatarCollect(client.Player.AvatarCollect);
                client.Player.SendMessage(string.Format(SUCCESS_RENEWAL, renewedCount));
            }
            
            return 1;
            */
        }

        private int HandleSingleRenewal(GameClient client, int avatarId, int days)
        {
            // Feature disabled - can be enabled later
            client.Player.SendMessage(ERROR_FEATURE_DISABLED);
            return 1;

            /*
            // Implementation when feature is enabled:
            var collection = client.Player.AvatarCollect.GetAvatarCollectWithAvatarID(avatarId);
            
            if (collection == null)
            {
                client.Player.SendMessage(ERROR_NOT_ACTIVATED);
                return 1;
            }

            // Ensure items are loaded
            if (collection.Items == null)
            {
                collection.UpdateItems();
            }

            // Check if can renew
            if (!CanRenewCollection(collection))
            {
                client.Player.SendMessage(ERROR_RENEW_NOT_ENOUGH_ITEMS);
                return 1;
            }

            var property = ClothPropertyTemplateInfoMgr.GetClothPropertyWithID(avatarId);
            if (property == null)
            {
                client.Player.SendMessage(ERROR_CANNOT_RENEW);
                return 1;
            }

            // Calculate and check cost
            int cost = property.Cost * days;
            if (client.Player.PlayerCharacter.myHonor < cost || cost <= 0)
            {
                client.Player.SendMessage("Yetersiz onur puanı.");
                return 1;
            }

            // Execute renewal
            client.Player.RemovemyHonor(cost);
            collection.ActiveAvatar(days);

            // Send confirmation
            GSPacketIn packet = new GSPacketIn(PACKET_ID);
            packet.WriteByte(4);
            packet.WriteInt(collection.AvatarID);
            packet.WriteInt(collection.Sex);
            packet.WriteDateTime(collection.TimeEnd);
            client.Player.SendTCP(packet);

            client.Player.SendMessage("Başarıyla yenilendi.");
            return 1;
            */
        }

        private bool CanRenewCollection(UserAvatarCollectionInfo collection)
        {
            int totalItems = ClothGroupTemplateInfoMgr.CountClothGroupWithID(collection.AvatarID);
            int requiredItems = totalItems / 2;
            int currentItems = collection.Items != null ? collection.Items.Count : 0;

            return currentItems >= requiredItems;
        }

        #endregion
    }
}