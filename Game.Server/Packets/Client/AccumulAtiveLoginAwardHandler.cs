using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.ACCUMULATIVELOGIN_AWARD, "撤消征婚信息")]
    public class AccumulAtiveLoginAwardHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int num = packet.ReadInt();
            //Console.WriteLine("???>>>{0}", num);
            GSPacketIn gSPacketIn = new((int)ePackageType.ACCUMULATIVELOGIN_AWARD, client.Player.PlayerCharacter.ID);
            string text = $"İlk giriş hediyesi alma başarısız!";
            if (client.Player.PlayerCharacter.accumulativeAwardDays < client.Player.PlayerCharacter.accumulativeLoginDays)
            {
                for (int i = client.Player.PlayerCharacter.accumulativeAwardDays; i < client.Player.PlayerCharacter.accumulativeLoginDays; i++)
                {
                    int num2 = i + 1;
                   
                    List<ItemInfo> list = (num2 < 7) ? AccumulActiveLoginMgr.GetAllAccumulAtiveLoginAward(num2) : AccumulActiveLoginMgr.GetSelecedAccumulAtiveLoginAward(num);
                    if (list.Count > 0)
                    {
                        text = $"{num2} Gün Giriş Hediyesi:";
                        _ = WorldEventMgr.SendItemsToMails(list, client.Player.PlayerCharacter.ID, client.Player.PlayerCharacter.NickName, client.Player.ZoneId, null, text);
                        client.Player.PlayerCharacter.accumulativeAwardDays++;
                    }
                    else
                    {
                        client.Player.SendMessage(text);
                    }
                }
            }
            gSPacketIn.WriteInt(client.Player.PlayerCharacter.accumulativeLoginDays);
            gSPacketIn.WriteInt(client.Player.PlayerCharacter.accumulativeAwardDays);
            client.Player.SendTCP(gSPacketIn);
            return 0;
        }
    }
}
