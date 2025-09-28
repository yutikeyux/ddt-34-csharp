using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Base.Packets;
using log4net;
using Game.Server.Managers;
using SqlDataProvider.Data;
using Bussiness;
using Bussiness.Managers;
using Game.Server.Statics;
using System.Drawing;
using System.Web.UI;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.TRANSNATIONAL_BUYGOODS, "客户端日记")]
    public class BuyTransnationalGoodsHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int gold = 0;
            int money = 0;
            int offer = 0;
            int gifttoken = 0;
            int medal = 0;
            int damageScore = 0;
            int petScore = 0;
            int iTemplateID = 0;
            int iCount = 0;
            int hardCurrency = 0;
            int LeagueMoney = 0;
            int honor = 0;
            int GoodsID = packet.ReadInt();
            //if (!ActiveSystemMgr.CanExchange())
            //{
            //    client.Out.SendMessage(eMessageType.Normal, "Đổi thưởng vào chủ nhật hàng tuần.");
            //    return 0;
            //}
            PyramidInfo pyramid = client.Player.Actives.Pyramid;
            //SpecialItemBoxInfo specialValue = new SpecialItemBoxInfo();
            eMessageType eMsg = eMessageType.Normal;
            string msg = "UserBuyItemHandler.Success";
            ShopItemInfo shopItem = ShopMgr.GetShopItemInfoById(GoodsID);                   //获取商品信息
            if (shopItem == null)
                return 0;
            bool isContinuos = false;
            if (ShopMgr.IsOnShop(shopItem.ID) && shopItem.ShopID == (int)eShopType.Pyramid)
            {
                isContinuos = true;
            }

            if (!isContinuos)
            {
                client.Out.SendMessage(eMessageType.ERROR, LanguageMgr.GetTranslation("UserBuyItemHandler.FailByPermission"));
                return 1;
            }
            Dictionary<int, ItemInfo> ListBuyItem = new Dictionary<int, ItemInfo>();

            ItemTemplateInfo goods = ItemMgr.FindItemTemplate(shopItem.TemplateID);
            ItemInfo cloneitem = ItemInfo.CreateFromTemplate(goods, 1, (int)ItemAddType.Buy);
            if (0 == shopItem.BuyType)                              //时间购买类型
            {
                cloneitem.ValidDate = shopItem.AUnit;
            }
            else                                                  //数量购买类型
            {
                cloneitem.Count = shopItem.AUnit;
            }
            cloneitem.IsBinds = true;//Convert.ToBoolean(shopItem.IsBind);
            if (!ListBuyItem.Keys.Contains(cloneitem.TemplateID))
                ListBuyItem.Add(cloneitem.TemplateID, cloneitem);
            else
                ListBuyItem[cloneitem.TemplateID].Count += cloneitem.Count;

            ShopMgr.SetItemType(shopItem, 1, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref medal, ref honor);
            if (ListBuyItem.Values.Count == 0)
                return 1;
            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked"));
                return 1;
            }
            bool result = false;
            if (pyramid.totalPoint < damageScore)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("BuyTransnationalGoodsHandler.Msg1"));
                return 0;
            }
            else
            {
                pyramid.totalPoint -= damageScore;
                result = true;
            }
            if (result)
            {
                string itemIDs = "";
                foreach (ItemInfo info in ListBuyItem.Values)
                {
                    itemIDs += (itemIDs == "" ? info.TemplateID.ToString() : "," + info.TemplateID.ToString());
                    if (info.Template.MaxCount == 1)
                    {
                        for (int i = 0; i < info.Count; i++)
                        {
                            ItemInfo newitem = ItemInfo.CloneFromTemplate(info.Template, info);
                            newitem.Count = 1;
                            client.Player.AddTemplate(newitem);
                        }

                    }
                    else
                    {
                        int temp_count = 0;
                        for (int i = 0; i < info.Count; i++)
                        {
                            if (temp_count == info.Template.MaxCount)
                            {
                                ItemInfo newitem = ItemInfo.CloneFromTemplate(info.Template, info);
                                newitem.Count = temp_count;
                                client.Player.AddTemplate(newitem);
                                temp_count = 0;
                            }
                            temp_count++;
                        }
                        if (temp_count > 0)
                        {
                            ItemInfo newitem = ItemInfo.CloneFromTemplate(info.Template, info);
                            newitem.Count = temp_count;
                            client.Player.AddTemplate(newitem);
                            temp_count = 0;
                        }
                    }
                }
            }
            else
            {
                eMsg = eMessageType.ERROR;
                msg = "UserBuyItemHandler.FailByPermission";
            }
            client.Out.SendMessage(eMsg, LanguageMgr.GetTranslation(msg));
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ACTIVITY_SYSTEM, client.Player.PlayerCharacter.ID);
            pkg.WriteByte((byte)ActiveSystemPackageType.PYRAMID_STARTORSTOP);
            pkg.WriteBoolean(pyramid.isPyramidStart);//this.model.isPyramidStart = param1.readBoolean();                            
            pkg.WriteInt(pyramid.totalPoint);//model.totalPoint = param1.readInt();
            pkg.WriteInt(pyramid.turnPoint);//model.turnPoint = param1.readInt();
            pkg.WriteInt(pyramid.pointRatio);//model.pointRatio = param1.readInt();
            pkg.WriteInt(pyramid.currentLayer);//model.currentLayer = param1.readInt();
            client.Player.SendTCP(pkg);
            return 0;
        }
    }
}
