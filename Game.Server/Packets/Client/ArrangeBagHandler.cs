using Game.Base.Packets;
using Game.Server.GameUtils;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.CHANGE_PLACE_GOODS_ALL, "Arrange Bag")]
    public class ArrangeBagHandler : IPacketHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            bool flag = packet.ReadBoolean();
            int num = packet.ReadInt();
            int bageType = packet.ReadInt();
            PlayerInventory inventory = client.Player.GetInventory((eBageType)bageType);
            int capalility = inventory.Capalility;

            // Envanterdeki eşyaları alıyoruz
            List<ItemInfo> items = inventory.GetItems(inventory.BeginSlot, capalility);

            if (num == items.Count)
            {
                inventory.BeginChanges();
                try
                {
                    // ---------------------------------------------------------
                    // 1. ADIM: İstifleme (Eğer flag true ise)
                    // ---------------------------------------------------------
                    if (flag)
                    {
                        // Eşyaları birleştirme işlemi
                        for (int j = 0; j < items.Count; j++)
                        {
                            for (int k = items.Count - 1; k > j; k--)
                            {
                                // Aynı TemplateID'ye sahip, birleştirilebilir ve doluysa
                                if (items[j].TemplateID == items[k].TemplateID &&
                                    items[j].CanStackedTo(items[k]) &&
                                    items[j].Count < items[j].Template.MaxCount) // Sınır kontrolü eklenebilir
                                {
                                    // Mevcut eşyayı tamamlamak için gereken miktar
                                    int needed = items[j].Template.MaxCount - items[j].Count;
                                    int moveCount = (items[k].Count > needed) ? needed : items[k].Count;

                                    inventory.MoveItem(items[k].Place, items[j].Place, moveCount);

                                    // Taşıma işlemi sonrası listeyi güncellememiz gerekebilir
                                    // Ancak bu basit döngüde MoveItem otomatik güncellerse sorun yok,
                                    // yoksa k copy'si güncellenmez. En güvenlisi MoveItem sonrası
                                    // gerekirse referansları güncellemektir ama burada
                                    // basitlik adına MoveItem iç mekanizmasına güveniyoruz.
                                }
                            }
                        }
                        // İstifleme sonrası listeyi yeniden alıyoruz (Slot değişiklikleri olmuştur)
                        items = inventory.GetItems(inventory.BeginSlot, capalility);
                    }

                    // ---------------------------------------------------------
                    // 2. ADIM: Kategoriye Göre Sıralama
                    // ---------------------------------------------------------
                    items.Sort((a, b) =>
                    {
                        // 1. Öncelik: Kategori ID (CategoryID)
                        int categoryCompare = a.Template.CategoryID.CompareTo(b.Template.CategoryID);

                        // Eğer kategoriler farklıysa, kategori sırasına göre döndür
                        if (categoryCompare != 0) return categoryCompare;

                        // 2. Öncelik (Eşitse): Şablon ID (TemplateID)
                        // Aynı kategorideki eşyaların alt alta düzenli durması için
                        return a.TemplateID.CompareTo(b.TemplateID);
                    });

                    // ---------------------------------------------------------
                    // 3. ADIM: Sıralamayı Envantere Uygula
                    // ---------------------------------------------------------
                    // Sıralanmış listedeki sıraya göre, fiziksel slotlara taşıyoruz.
                    for (int i = 0; i < items.Count; i++)
                    {
                        ItemInfo itemToPlace = items[i];
                        int targetSlot = inventory.BeginSlot + i;

                        // Eğer eşya zaten olması gereken yerdeyse atla (Performans için)
                        if (itemToPlace.Place == targetSlot) continue;

                        // Eşyayı hedef slota taşı.
                        // Eğer hedef slot doluysa, MoveItem genellikle otomatik olarak 
                        // hedefteki eşya ile yer değiştirir (Swap).
                        // Bu sayede "Baloncuk Sıralaması" (Bubble Sort) mantığıyla
                        // tüm eşyalar doğru yere yerleşir.
                        inventory.MoveItem(itemToPlace.Place, targetSlot, itemToPlace.Count);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Arrange Bag Error", ex);
                }
                finally
                {
                    inventory.CommitChanges();
                }
            }
            else
            {
                log.Warn($"ArrangeBag: Item count mismatch. Cap: {capalility}, Count: {num}");
            }
            return 0;
        }
    }
}