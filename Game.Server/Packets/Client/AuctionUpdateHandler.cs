using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.AUCTION_UPDATE, "更新拍卖")]
    public class AuctionUpdateHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int auctionID = packet.ReadInt();
            int money = packet.ReadInt();
            bool val = false;
            int num3 = GameProperties.LimitLevel(0);

            // Chức năng của BAOLT - Lâm đừng copaste nha
            if (client.Player.isPlayerWarrior())
            {
                _ = client.Out.SendMessage(eMessageType.GM_NOTICE, "Sizin bu işlemi gerçekleştirme yetkiniz yok.");
                return 0;
            }
            if (client.Player.PlayerCharacter.Grade < num3)
            {
                _ = client.Out.SendMessage(eMessageType.GM_NOTICE, string.Format("Bu işlemi gerçekleştirmek için {0} seviye gereklidir!.", num3));
                return 0;
            }
            GSPacketIn gSPacketIn = new(193, client.Player.PlayerCharacter.ID);
            string text = "AuctionUpdateHandler.Fail";
            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                _ = client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
                return 0;
            }
            using (PlayerBussiness playerBussiness = new())
            {
                AuctionInfo auctionSingle = playerBussiness.GetAuctionSingle(auctionID);
                if (auctionSingle == null)
                {
                    text = "AuctionUpdateHandler.Msg1";
                }
                else if (auctionSingle.PayType == 0 && money > client.Player.PlayerCharacter.Gold)
                {
                    text = "AuctionUpdateHandler.Msg2";
                }
                else if (auctionSingle.PayType == 1 && !client.Player.MoneyDirect(money, IsAntiMult: true, true, false))
                {
                    text = "";
                }
                else if (auctionSingle.BuyerID == 0 && auctionSingle.Price > money)
                {
                    text = "AuctionUpdateHandler.Msg4";
                }
                else if (auctionSingle.BuyerID != 0 && auctionSingle.Price + auctionSingle.Rise > money && (auctionSingle.Mouthful == 0 || auctionSingle.Mouthful > money))
                {
                    text = "AuctionUpdateHandler.Msg5";
                }
                else
                {
                    int buyerID = auctionSingle.BuyerID;
                    auctionSingle.BuyerID = client.Player.PlayerCharacter.ID;
                    auctionSingle.BuyerName = client.Player.PlayerCharacter.NickName;
                    auctionSingle.Price = money;
                    if (auctionSingle.Mouthful != 0 && money >= auctionSingle.Mouthful)
                    {
                        auctionSingle.Price = auctionSingle.Mouthful;
                        auctionSingle.IsExist = false;
                    }
                    if (playerBussiness.UpdateAuction(auctionSingle, GameProperties.Cess))
                    {
                        if (auctionSingle.PayType == 0)
                        {
                            _ = client.Player.RemoveGold(auctionSingle.Price);
                        }
                        if (auctionSingle.IsExist)
                        {
                            text = "AuctionUpdateHandler.Msg6";
                        }
                        else
                        {
                            text = "AuctionUpdateHandler.Msg7";
                            _ = client.Out.SendMailResponse(auctionSingle.AuctioneerID, eMailRespose.Receiver);
                            _ = client.Out.SendMailResponse(auctionSingle.BuyerID, eMailRespose.Receiver);
                        }
                        if (buyerID != 0)
                        {
                            _ = client.Out.SendMailResponse(buyerID, eMailRespose.Receiver);
                        }
                        val = true;
                    }
                }
                _ = client.Out.SendAuctionRefresh(auctionSingle, auctionID, auctionSingle?.IsExist ?? false, null);
                if (text != "")
                {
                    _ = client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(text));
                }
            }
            gSPacketIn.WriteBoolean(val);
            gSPacketIn.WriteInt(auctionID);
            client.Out.SendTCP(gSPacketIn);
            return 0;
        }
    }
}
