using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.AUCTION_DELETE, "撤消拍卖")]
    public class AuctionDeleteHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int auctionID = packet.ReadInt();
            string msg = LanguageMgr.GetTranslation("AuctionDeleteHandler.Fail");
            using PlayerBussiness playerBussiness = new();
            if (playerBussiness.DeleteAuction(auctionID, client.Player.PlayerCharacter.ID, ref msg))
            {
                _ = client.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Receiver);
                _ = client.Out.SendAuctionRefresh(null, auctionID, isExist: false, null);
            }
            else
            {
                AuctionInfo auctionSingle = playerBussiness.GetAuctionSingle(auctionID);
                _ = client.Out.SendAuctionRefresh(auctionSingle, auctionID, auctionSingle != null, null);
            }
            _ = client.Out.SendMessage(eMessageType.GM_NOTICE, msg);
            return 0;
        }
    }
}
