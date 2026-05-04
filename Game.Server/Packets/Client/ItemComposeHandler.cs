using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Text;


namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.ITEM_COMPOSE, "物品合成")]
    public class ItemComposeHandler : IPacketHandler
    {
        public static Random random = new();
        private static readonly double[] composeRate = new double[] { 0.8, 0.5, 0.3, 0.1, 0.05 };
        //public static int countConnect = 0;
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            //GSPacketIn pkg = packet.Clone();
            //pkg.ClearContext();
            GSPacketIn pkg = new((byte)ePackageType.ITEM_COMPOSE, client.Player.PlayerCharacter.ID);

            DateTime now = DateTime.UtcNow;

            if (client.Player.ComposePacketWindowStart == DateTime.MinValue ||
                (now - client.Player.ComposePacketWindowStart).TotalSeconds >= 1)
            {
                client.Player.ComposePacketWindowStart = now;
                client.Player.ComposePacketCount = 0;
            }

            client.Player.ComposePacketCount++;

            if (client.Player.ComposePacketCount > 2)
            {
                _ = client.Out.SendMessage(eMessageType.ERROR, "Çok hızlı işlem yapıyorsunuz, lütfen yavaşlayın.");
                return 0;
            }


            StringBuilder str = new();
            int mustGold = GameProperties.PRICE_COMPOSE_GOLD;
            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked"));
                return 0;
            }
            if (client.Player.PlayerCharacter.Gold < mustGold)
            {
                _ = client.Out.SendMessage(eMessageType.ERROR, LanguageMgr.GetTranslation("ItemComposeHandler.NoMoney"));
                return 0;
            }
            int itemPlace = -1;
            int godPlace = -1;
            bool isBinds = false;
            bool consortia = packet.ReadBoolean();

            ItemInfo item = client.Player.StoreBag.GetItemAt(1);
            ItemInfo stone = client.Player.StoreBag.GetItemAt(2);
            ItemInfo luck = null;
            ItemInfo god = null;
            if (stone == null || item == null || stone.Count <= 0)
            {
                _ = client.Out.SendMessage(eMessageType.ERROR, LanguageMgr.GetTranslation("ItemComposeHandler.Msg")); ;
                return 0;
            }
            string BeginProperty = null;
            string AddItem = null;
            using (ItemRecordBussiness db = new())
            {
                db.PropertyString(item, ref BeginProperty);
            }
            if (item != null && stone != null && item.Template.CanCompose && (item.Template.CategoryID < 10 || (stone.Template.CategoryID == 11 && stone.Template.Property1 == 1)))
            {

                isBinds = isBinds || item.IsBinds;
                isBinds = isBinds || stone.IsBinds;
                _ = str.Append(item.ItemID + ":" + item.TemplateID + "," + stone.ItemID + ":" + stone.TemplateID + ",");
                //Random random = new Random();
                bool result = false;
                byte isSuccess = 1;
                //bool isGod = false;                
                double probability = composeRate[stone.Template.Quality - 1] * 100;//stone.Template.Property2;
                if (client.Player.StoreBag.GetItemAt(0) != null)
                {
                    luck = client.Player.StoreBag.GetItemAt(0);
                    if (luck != null && luck.Template.CategoryID == 11 && luck.Template.Property1 == 3)
                    {
                        isBinds = isBinds || luck.IsBinds;
                        AddItem += "|" + luck.ItemID + ":" + luck.Template.Name + "|" + stone.ItemID + ":" + stone.Template.Name;
                        _ = str.Append(luck.ItemID + ":" + luck.TemplateID + ",");
                        probability += probability * luck.Template.Property2 / 100;
                    }

                }
                else
                {
                    probability += probability * 1 / 100;
                }
                if (godPlace != -1)
                {
                    god = client.Player.PropBag.GetItemAt(godPlace);
                    if (god != null && god.Template.CategoryID == 11 && god.Template.Property1 == 7)
                    {
                        isBinds = isBinds || god.IsBinds;
                        //isGod = true;
                        _ = str.Append(god.ItemID + ":" + god.TemplateID + ",");
                        AddItem += "," + god.ItemID + ":" + god.Template.Name;
                    }
                    else
                    {
                        god = null;
                    }
                }
                //判断是公会铁匠铺还是铁匠铺
                if (consortia)
                {

                    ConsortiaInfo info = ConsortiaMgr.FindConsortiaInfo(client.Player.PlayerCharacter.ConsortiaID);
                    //这里添加公会权限限制的判断
                    ConsortiaBussiness csbs = new();
                    ConsortiaEquipControlInfo cecInfo = csbs.GetConsortiaEquipRiches(client.Player.PlayerCharacter.ConsortiaID, 0, 2);

                    if (info == null)
                    {
                        _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemStrengthenHandler.Fail"));
                    }
                    else
                    {

                        if (client.Player.PlayerCharacter.Riches < cecInfo.Riches)
                        {
                            _ = client.Out.SendMessage(eMessageType.ERROR, LanguageMgr.GetTranslation("ItemStrengthenHandler.FailbyPermission"));
                            return 1;
                        }
                        else
                        {
                            probability *= 1 + 0.1 * info.SmithLevel;

                        }
                    }

                }
                probability = Math.Floor(probability * 10) / 10;
                //client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Probability: " + probability.ToString()));
                int rand = random.Next(100);
                switch (stone.Template.Property3)
                {
                    case 1: // Saldırı (Attack)
                        if (stone.Template.Property4 > item.AttackCompose)
                        {
                            // EKLENEN KONTROL: Sıralı sentez kontrolü ve dinamik hata mesajı
                            if (stone.Template.Property4 != item.AttackCompose + 10)
                            {
                                int gerekliSeviye = item.AttackCompose + 10;
                                _ = client.Out.SendMessage(eMessageType.ERROR, string.Format("Önce +{0} sentezini başarıyla basmanız gerekir.", gerekliSeviye));
                                return 0;
                            }

                            result = true;
                            if (probability > rand)
                            {
                                isSuccess = 0;
                                item.AttackCompose = stone.Template.Property4;
                            }

                        }
                        break;
                    case 2: // Savunma (Defend)
                        if (stone.Template.Property4 > item.DefendCompose)
                        {
                            // EKLENEN KONTROL
                            if (stone.Template.Property4 != item.DefendCompose + 10)
                            {
                                int gerekliSeviye = item.DefendCompose + 10;
                                _ = client.Out.SendMessage(eMessageType.ERROR, string.Format("Önce +{0} sentezini başarıyla basmanız gerekir.", gerekliSeviye));
                                return 0;
                            }

                            result = true;
                            if (probability > rand)
                            {
                                isSuccess = 0;
                                item.DefendCompose = stone.Template.Property4;
                            }

                        }
                        break;
                    case 3: // Çeviklik (Agility)
                        if (stone.Template.Property4 > item.AgilityCompose)
                        {
                            // EKLENEN KONTROL
                            if (stone.Template.Property4 != item.AgilityCompose + 10)
                            {
                                int gerekliSeviye = item.AgilityCompose + 10;
                                _ = client.Out.SendMessage(eMessageType.ERROR, string.Format("Önce +{0} sentezini başarıyla basmanız gerekir.", gerekliSeviye));
                                return 0;
                            }

                            result = true;
                            if (probability > rand)
                            {
                                isSuccess = 0;
                                item.AgilityCompose = stone.Template.Property4;
                            }

                        }
                        break;
                    case 4: // Şans (Luck)
                        if (stone.Template.Property4 > item.LuckCompose)
                        {
                            // EKLENEN KONTROL
                            if (stone.Template.Property4 != item.LuckCompose + 10)
                            {
                                int gerekliSeviye = item.LuckCompose + 10;
                                _ = client.Out.SendMessage(eMessageType.ERROR, string.Format("Önce +{0} sentezini başarıyla basmanız gerekir.", gerekliSeviye));
                                return 0;
                            }

                            result = true;
                            if (probability > rand)
                            {
                                isSuccess = 0;
                                item.LuckCompose = stone.Template.Property4;
                            }

                        }
                        break;
                }

                if (result)
                {
                    item.IsBinds = isBinds;
                    if (isSuccess != 0)
                    {
                        _ = str.Append("false!");
                    }
                    else
                    {
                        _ = str.Append("true!");
                        client.Player.OnItemCompose(stone.TemplateID);
                    }
                    //LogMgr.LogItemAdd(client.Player.PlayerCharacter.ID, LogItemType.Compose, BeginProperty, item, AddItem, Convert.ToInt32(result));
                    //client.Player.RemoveItem(stone);
                    _ = client.Player.StoreBag.RemoveTemplate(stone.TemplateID, 1);
                    //client.Player.SaveIntoDatabase();//保存到数据库
                    if (luck != null)
                    {
                        //client.Player.RemoveItem(luck);
                        _ = client.Player.StoreBag.RemoveTemplate(luck.TemplateID, 1);
                    }
                    if (god != null)
                    {
                        _ = client.Player.RemoveItem(god);
                    }
                    _ = client.Player.RemoveGold(mustGold);
                    //client.Player.StoreBag2.ClearBag();
                    //client.Player.StoreBag2.AddItemTo(item, 1);
                    client.Player.StoreBag.UpdateItem(item);
                    pkg.WriteByte(isSuccess);
                    client.Out.SendTCP(pkg);
                    if (itemPlace < 31)
                    {
                        client.Player.EquipBag.UpdatePlayerProperties();
                    }
                }
                else
                {
                    _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemComposeHandler.NoLevel"));
                }
            }
            else
            {
                _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemComposeHandler.Fail"));
            }

            return 0;
        }
    }
}