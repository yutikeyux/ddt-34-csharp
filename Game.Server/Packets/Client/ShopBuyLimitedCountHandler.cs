using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
    [PacketHandler((short)ePackageType.SHOP_BUYLIMITEDCOUNT, "场景用户离开")]
    public class ShopBuyLimitedCountHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            GSPacketIn pkg = new((short)ePackageType.SHOP_BUYLIMITEDCOUNT, client.Player.PlayerId);
            //ShopCheapItemsInfo[] shop = ShopMgr.GetAllShopCheapItems();
            pkg.WriteInt(0);
            //foreach (ShopCheapItemsInfo s in shop)
            //{
            //    pkg.WriteInt(s.ID);
            //    pkg.WriteInt(2);
            //}
            client.Player.SendTCP(pkg);
            //GSPacketIn pkg1 = new GSPacketIn((short)ePackageType.UPDATE_SHOP, client.Player.PlayerId);
            //client.Player.SendTCP(pkg1);
            return 0;
        }
    }
}
