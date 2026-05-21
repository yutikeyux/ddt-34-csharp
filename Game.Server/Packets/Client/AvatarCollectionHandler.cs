using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;

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

        // Hata mesajları
        private const string ERROR_ITEM_NOT_FOUND = "Böyle bir çizim bulunamadı.";
        private const string ERROR_SET_NOT_AVAILABLE = "Bu çizim seti mevcut değil.";
        private const string ERROR_INSUFFICIENT_GOLD = "Yetersiz altın.";
        private const string ERROR_INSUFFICIENT_HONOR = "Yetersiz onur puanı.";
        private const string ERROR_ACTIVATION_FAILED = "Çizim etkinleştirme hatası.";
        private const string ERROR_NOT_ACTIVATED = "Bu kıyafeti henüz etkinleştirmediniz.";
        private const string ERROR_RENEW_NOT_ENOUGH_ITEMS = "Yenileme için çizim setinin yarısından fazlasını etkinleştirmeniz gerekmektedir.";
        private const string ERROR_CANNOT_RENEW = "Bu çizim seti yenilenemez!";
        private const string ERROR_INVALID_DAYS = "Geçersiz gün değeri.";

        // Başarı mesajları
        private const string SUCCESS_ACTIVATION = "Çizim başarıyla etkinleştirildi!";
        private const string SUCCESS_RENEWAL = "Çizim seti başarıyla {0} gün yenilendi.";

        #endregion

        #region Packet Handler

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client == null || client.Player == null)
            {
                Console.WriteLine("[AvatarCollectionHandler] Null client or player.");
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
        /// Eşya etkinleştirme (subCommand = 3)
        /// </summary>
        private int HandleActivateItem(GameClient client, GSPacketIn packet)
        {
            int groupId = packet.ReadInt();
            int templateId = packet.ReadInt();
            int sex = packet.ReadInt();

            if (groupId <= 0 || templateId <= 0)
            {
                client.Player.SendMessage(ERROR_ITEM_NOT_FOUND);
                return 1;
            }

            // DÜZELTME: Orijinal kodda eşya bulunamayınca ERROR_BAG_FULL dönüyordu.
            // Doğru mesaj ERROR_ITEM_NOT_FOUND olmalı.
            ItemInfo playerItem = client.Player.EquipBag.GetItemByTemplateID(0, templateId);
            if (playerItem == null)
            {
                client.Player.SendMessage(ERROR_ITEM_NOT_FOUND);
                return 1;
            }

            ClothGroupTemplateInfo clothGroup =
                ClothGroupTemplateInfoMgr.GetClothGroup(groupId, templateId, sex);
            if (clothGroup == null)
            {
                client.Player.SendMessage(ERROR_ITEM_NOT_FOUND);
                return 1;
            }

            ClothPropertyTemplateInfo clothProperty =
                ClothPropertyTemplateInfoMgr.GetClothPropertyWithID(clothGroup.ID);
            if (clothProperty == null)
            {
                client.Player.SendMessage(ERROR_SET_NOT_AVAILABLE);
                return 1;
            }

            if (client.Player.PlayerCharacter.Gold < clothGroup.Cost)
            {
                client.Player.SendMessage(ERROR_INSUFFICIENT_GOLD);
                return 1;
            }

            return ExecuteActivation(client, clothGroup, clothProperty);
        }

        /// <summary>
        /// Koleksiyon yenileme (subCommand = 4).
        /// AS tarafından gelen paket: avatarId (int) + days (int)
        /// AS: sendAvatarCollectionDelayTime(data.id, selectedCount)
        /// </summary>
        private int HandleRenewCollection(GameClient client, GSPacketIn packet)
        {
            int avatarId = packet.ReadInt();
            int days = packet.ReadInt();

            if (days <= 0)
            {
                client.Player.SendMessage(ERROR_INVALID_DAYS);
                return 1;
            }

            // avatarId == -1 → toplu yenileme
            return avatarId == -1
                ? HandleMassRenewal(client, days)
                : HandleSingleRenewal(client, avatarId, days);
        }

        #endregion

        #region Activation Logic

        private int ExecuteActivation(
            GameClient client,
            ClothGroupTemplateInfo clothGroup,
            ClothPropertyTemplateInfo clothProperty)
        {
            bool isNewlyActivated = false;

            // Altını önce düş
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

                UserAvatarCollectionDataInfo newItem =
                    new(clothGroup.TemplateID, clothGroup.Sex);

                if (!collection.AddItem(newItem))
                {
                    // Ekleme başarısız → altını iade et
                    client.Player.AddGold(clothGroup.Cost);
                    client.Player.SendMessage(ERROR_ACTIVATION_FAILED);
                    return 1;
                }

                // Toplam ve mevcut eleman sayıları
                int totalItems = ClothGroupTemplateInfoMgr.CountClothGroupWithID(collection.AvatarID);
                int halfItems = totalItems / 2;
                int currentCount = collection.Items != null ? collection.Items.Count : 0;

                // Koleksiyonun yarısı tamamlandıysa ve daha önce aktif değilse etkinleştir
                if (currentCount >= halfItems && !collection.IsActive)
                {
                    collection.ActiveAvatar(DEFAULT_ACTIVATE_DAYS);
                    isNewlyActivated = true;
                }

                // Paketleri gönder
                SendActivationPackets(client, collection, clothGroup, isNewlyActivated);

                // -------------------------------------------------------
                // DÜZELTME: Her eleman eklenişinde UpdatePlayerProperties
                // çağrılıyor.
                //
                // Akış:
                //   EquipBag.UpdatePlayerProperties()
                //     → GetBaseAttack()  → DamageAvatar (avatar koleksiyon bonusu) hesaplanır
                //     → GetBaseDefence() → GuardAvatar  (avatar koleksiyon bonusu) hesaplanır
                //     → UpdateBaseProperties() çağrılır
                //     → UpdateFightPower() çağrılır   ← savaş gücü güncellenir
                //     → OnPropertiesChanged()
                //       → UpdateProperties()
                //         → Out.SendUpdatePrivateInfo(...)  ← istemciye güncelleme paketi
                //         → Out.SendUpdatePublicPlayer(...) ← oda içi güncelleme paketi
                //
                // Orijinal kodda bu sadece halfItems veya totalItems eşiğinde
                // tetikleniyordu; ara elemanlarda güç güncellemesi gerçekleşmiyordu.
                // -------------------------------------------------------
                client.Player.EquipBag.UpdatePlayerProperties();

                client.Player.SendMessage(SUCCESS_ACTIVATION);
                return 1;
            }
            catch (Exception ex)
            {
                // Hata → altını iade et
                client.Player.AddGold(clothGroup.Cost);
                Console.WriteLine("[AvatarCollectionHandler] Activation error: " + ex);
                client.Player.SendMessage(ERROR_ACTIVATION_FAILED);
                return 0;
            }
        }

        /// <summary>
        /// Koleksiyonu getir; yoksa oluştur ve listeye ekle.
        /// </summary>
        private UserAvatarCollectionInfo GetOrCreateCollection(
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
                    DateTime.Now
                );
                client.Player.AvatarCollect.AddAvatarCollection(collection);
            }

            return collection;
        }

        /// <summary>
        /// Etkinleştirme sonrası istemciye gönderilecek paketler.
        /// </summary>
        private void SendActivationPackets(
            GameClient client,
            UserAvatarCollectionInfo collection,
            ClothGroupTemplateInfo clothGroup,
            bool isNewlyActivated)
        {
            // Koleksiyon ilk kez aktif olduysa zaman bilgisi paketi gönder
            if (isNewlyActivated)
            {
                GSPacketIn activationPacket = new(PACKET_ID);
                activationPacket.WriteByte(CMD_RENEW_COLLECTION); // 4
                activationPacket.WriteInt(collection.AvatarID);
                activationPacket.WriteInt(collection.Sex);
                activationPacket.WriteDateTime(collection.TimeEnd);
                client.Player.SendTCP(activationPacket);
            }

            // Eşya etkinleştirme onay paketi
            GSPacketIn confirmPacket = new(PACKET_ID);
            confirmPacket.WriteByte(CMD_ACTIVATE_ITEM); // 3
            confirmPacket.WriteInt(clothGroup.ID);
            confirmPacket.WriteInt(clothGroup.TemplateID);
            confirmPacket.WriteInt(clothGroup.Sex);
            client.Player.SendTCP(confirmPacket);
        }

        #endregion

        #region Renewal Logic

        /// <summary>
        /// Toplu yenileme (avatarId = -1).
        /// Süresi dolmuş ve yenilenebilir tüm koleksiyonları uzatır.
        /// </summary>
        private int HandleMassRenewal(GameClient client, int days)
        {
            int renewedCount = 0;

            foreach (UserAvatarCollectionInfo collection in
                     client.Player.AvatarCollect.AvatarCollect)
            {
                // Süresi henüz dolmamış koleksiyonları atla
                if (collection.IsAvailable())
                {
                    continue;
                }

                // Yeterli eleman kontrolü (activeCount >= totalCount / 2)
                if (!CanRenewCollection(collection))
                {
                    continue;
                }

                ClothPropertyTemplateInfo property =
                    ClothPropertyTemplateInfoMgr.GetClothPropertyWithID(collection.AvatarID);

                if (property == null)
                {
                    continue;
                }

                // AS tarafındaki needHonor hesabıyla birebir eşleşen maliyet:
                //   Tam koleksiyon → Cost * 2
                //   Yarısı dolu   → Cost
                int totalItems = ClothGroupTemplateInfoMgr.CountClothGroupWithID(collection.AvatarID);
                int currentItems = collection.Items != null ? collection.Items.Count : 0;
                int baseHonor = currentItems >= totalItems
                    ? property.Cost * 2
                    : property.Cost;

                int totalCost = baseHonor * days;

                // DÜZELTME: AS tarafıyla örtüşen onur kontrolü:
                // totalCost <= 0 → geçersiz değer, atla
                // myHonor < totalCost → yetersiz onur, atla
                if (totalCost <= 0 ||
                    client.Player.PlayerCharacter.myHonor < totalCost)
                {
                    continue;
                }

                client.Player.RemovemyHonor(totalCost);
                collection.ActiveAvatar(days);
                SendRenewalPacket(client, collection);
                renewedCount++;
            }

            if (renewedCount > 0)
            {
                // Koleksiyon listesini istemciye gönder
                client.Player.Out.SendAvatarCollect(client.Player.AvatarCollect);

                // Tüm yenilenen koleksiyonların bonusları güce yansısın
                client.Player.EquipBag.UpdatePlayerProperties();

                client.Player.SendMessage(string.Format(SUCCESS_RENEWAL, days));
            }
            else
            {
                client.Player.SendMessage(ERROR_RENEW_NOT_ENOUGH_ITEMS);
            }

            return 1;
        }

        /// <summary>
        /// Tekil yenileme.
        /// AS: sendAvatarCollectionDelayTime(data.id, selectedCount)
        /// selectedCount = kullanıcının onayladığı gün sayısı
        /// </summary>
        private int HandleSingleRenewal(GameClient client, int avatarId, int days)
        {
            UserAvatarCollectionInfo collection =
                client.Player.AvatarCollect.GetAvatarCollectWithAvatarID(avatarId);

            if (collection == null)
            {
                client.Player.SendMessage(ERROR_NOT_ACTIVATED);
                return 1;
            }

            // Eleman listesi henüz yüklü değilse yükle
            if (collection.Items == null)
            {
                collection.UpdateItems();
            }

            // Yenilenebilirlik kontrolü (activeCount >= totalCount / 2)
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

            // AS tarafıyla birebir eşleşen maliyet hesabı:
            //   Tam koleksiyon → Cost * 2, değilse → Cost
            int totalItems = ClothGroupTemplateInfoMgr.CountClothGroupWithID(collection.AvatarID);
            int currentItems = collection.Items != null ? collection.Items.Count : 0;
            int baseHonor = currentItems >= totalItems
                ? property.Cost * 2
                : property.Cost;

            int totalCost = baseHonor * days;

            // DÜZELTME: AS tarafındaki honor kontrolüyle örtüşen kontrol.
            // totalCost <= 0 → geçersiz değer (negatif days * honor durumu)
            // myHonor < totalCost → yetersiz onur
            if (totalCost <= 0 ||
                client.Player.PlayerCharacter.myHonor < totalCost)
            {
                client.Player.SendMessage(ERROR_INSUFFICIENT_HONOR);
                return 1;
            }

            client.Player.RemovemyHonor(totalCost);
            collection.ActiveAvatar(days);
            SendRenewalPacket(client, collection);

            // Yenileme sonrası koleksiyon bonusu güce yansısın
            client.Player.EquipBag.UpdatePlayerProperties();

            client.Player.SendMessage(string.Format(SUCCESS_RENEWAL, days));
            return 1;
        }

        /// <summary>
        /// Yenileme onay paketini istemciye gönderir.
        /// </summary>
        private void SendRenewalPacket(
            GameClient client,
            UserAvatarCollectionInfo collection)
        {
            GSPacketIn packet = new(PACKET_ID);
            packet.WriteByte(CMD_RENEW_COLLECTION); // 4
            packet.WriteInt(collection.AvatarID);
            packet.WriteInt(collection.Sex);
            packet.WriteDateTime(collection.TimeEnd);
            client.Player.SendTCP(packet);
        }

        /// <summary>
        /// Koleksiyonun yenilenip yenilenemeyeceğini kontrol eder.
        /// AS tarafındaki kural: activeCount >= totalCount / 2
        /// </summary>
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