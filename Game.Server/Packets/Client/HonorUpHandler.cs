using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler((byte)ePackageType.HONOR_UP_COUNT, "场景用户离开")]
    public class HonorUpHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int type = packet.ReadByte();
            _ = packet.ReadBoolean();
            //Console.WriteLine("?????type: " + type + " isBland:" + isBland);
            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {

                _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked"));
                return 0;
            }
            if (client.Player.PlayerCharacter.Grade < 20) //25 ti 20 yaptım moruk
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("Seviyeniz Yetersiz!"));
                return 0;
            }

            switch (type)
            {
                case 1://
                    break;
                case 2://get honor by monney
                    {
                        int maxBuyHonor = client.Player.PlayerCharacter.MaxBuyHonor + 1;
                        TotemHonorTemplateInfo temp = TotemHonorMgr.FindTotemHonorTemplateInfo(maxBuyHonor);
                        if (temp == null)
                        {
                            return 0;
                        }
                        int needMoney = temp.NeedMoney;
                        int addHonnor = temp.AddHonor;
                        if (client.Player.MoneyDirect(needMoney, IsAntiMult: true, false, true))
                        {

                            _ = client.Player.AddHonor(addHonnor);
                            _ = client.Player.AddMaxHonor(1);
                            //client.Player.AddExpVip(needMoney);
                            //client.Player.RemoveMoney(needMoney);
                            //Console.WriteLine("????needMoney: " + needMoney);
                        }
                    }
                    break;
            }
            _ = client.Player.Out.SendUpdateUpCount(client.Player.PlayerCharacter);
            return 0;
        }
    }
}
