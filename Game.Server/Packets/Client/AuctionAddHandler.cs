using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.AUCTION_ADD, "添加拍卖")]
    public class AuctionAddHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            eBageType bagType = (eBageType)packet.ReadByte();
            int place = packet.ReadInt();
            _ = packet.ReadByte();
            int price = packet.ReadInt();
            int mouthful = packet.ReadInt();
            int validDate = packet.ReadInt();
            int goodsCount = packet.ReadInt();

            string msg = "AuctionAddHandler.Fail";
            int payType = 1;

            if (client.Player.isPlayerWarrior())
            {
                _ = client.Out.SendMessage(eMessageType.GM_NOTICE, "Sizin bu işlevi gerçekleştirme izniniz maalesef yok..");
                return 0;
            }
            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked"));
                return 0;
            }

            if (price < 0 || (mouthful != 0 && mouthful < price))
            {
                return 0;
            }

            int multiple = 1;
            if (payType != 0)
            {
                //multiple = 10;
                multiple = 1;
                payType = 1;
            }
            int needGold = (int)(multiple * price * 0.03 * (validDate == 0 ? 1 : validDate == 1 ? 3 : 6));
            needGold = needGold < 1 ? 1 : needGold;
            ItemInfo goods = client.Player.GetItemAt(bagType, place);
            if (goods == null)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("AuctionAddHandler.Msg13"));
                return 0;
            }

            if (goods.Count < goodsCount || goodsCount < 0)
            {
                _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("AuctionAddHandler.Msg11"));
                return 0;
            }
            if (client.Player.IsLimitCount(goodsCount))
            {
                return 0;
            }

            int TotalItemAt = goods.Count - goodsCount;
            int limit = GameProperties.LimitLevel(1);
            if (client.Player.PlayerCharacter.Grade < limit)
            {
                _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("AuctionAddHandler.Msg12", limit));
                return 0;
            }
            if (price < 0)
            {
                msg = "AuctionAddHandler.Msg1";
            }
            else if (mouthful != 0 && mouthful < price)
            {
                msg = "AuctionAddHandler.Msg2";
            }
            else if (needGold > client.Player.PlayerCharacter.Gold)
            {
                msg = "AuctionAddHandler.Msg3";
            }
            else if (goods == null)
            {
                msg = "AuctionAddHandler.Msg4";
            }
            else if (goods.IsBinds)
            {
                msg = "AuctionAddHandler.Msg5";
            }
            else
            {
                ItemInfo newitem = ItemInfo.CloneFromTemplate(goods.Template, goods);
                ItemInfo itemAddAution = ItemInfo.CloneFromTemplate(goods.Template, goods);
                itemAddAution.Count = goodsCount;
                //Console.WriteLine("goodsCount: " + goodsCount);
                if (itemAddAution.ItemID == 0)
                {
                    using PlayerBussiness playerBussiness = new();
                    _ = playerBussiness.AddGoods(itemAddAution);
                }

                AuctionInfo info = new()
                {
                    AuctioneerID = client.Player.PlayerCharacter.ID,//获取物品ID
                    AuctioneerName = client.Player.PlayerCharacter.NickName,//获取物品妮称
                    BeginDate = DateTime.Now,
                    BuyerID = 0,
                    BuyerName = "",
                    IsExist = true,
                    ItemID = itemAddAution.ItemID,
                    Mouthful = mouthful,
                    PayType = payType,
                    Price = price,
                    Rise = price / 10
                };
                info.Rise = info.Rise < 1 ? 1 : info.Rise;
                info.Name = itemAddAution.Template.Name;
                info.Category = itemAddAution.Template.CategoryID;
                info.ValidDate = validDate == 0 ? 8 : validDate == 1 ? 24 : 48;
                info.TemplateID = itemAddAution.TemplateID;
                info.goodsCount = goodsCount;
                info.Random = ThreadSafeRandom.NextStatic(GameProperties.BeginAuction, GameProperties.EndAuction);
                using PlayerBussiness db = new();//写数据库
                if (db.AddAuction(info))
                {
                    //goods.Count = goodsCount;
                    _ = client.Player.RemoveAt(bagType, place);// TakeOutItem(goods);
                    if (TotalItemAt > 0)
                    {
                        newitem.Count = TotalItemAt;
                        _ = client.Player.AddTemplate(newitem, bagType, TotalItemAt, eGameView.CaddyTypeGet);
                    }
                    _ = client.Player.SaveIntoDatabase();
                    _ = client.Player.RemoveGold(needGold);
                    msg = "AuctionAddHandler.Msg6";
                    _ = client.Out.SendAuctionRefresh(info, info.AuctionID, true, itemAddAution);
                }
            }
            //client.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Receiver);
            _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation(msg));
            return 0;
        }
    }
}