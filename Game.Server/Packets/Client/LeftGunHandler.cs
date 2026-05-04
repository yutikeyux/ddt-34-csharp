using Bussiness;
using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
    [PacketHandler((byte)ePackageType.LEFT_GUN_ROULETTE_SOCKET, "物品炼化")]
    public class LeftGunHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            //int cmd = packet.ReadInt();
            if (client.Player.Extra.Info.LeftRoutteCount > 0 && client.Player.Extra.Info.LeftRoutteRate <= 0f)
            {
                RandomSafe randomSafe = new();
                float result = 0f;
                string[] rates = GameProperties.LeftRouterRateData.Split('|');
                int randNum = randomSafe.Next(55);
                if (randNum < 15)
                {
                    result = float.Parse(rates[4]);
                }
                else if (randNum < 25)
                {
                    result = float.Parse(rates[3]);
                }
                else if (randNum < 35)
                {
                    result = float.Parse(rates[2]);
                }
                else if (randNum < 45)
                {
                    result = float.Parse(rates[1]);
                }
                else if (randNum < 55)
                {
                    result = float.Parse(rates[0]);
                }
                if (result > 0f)
                {
                    client.Player.Extra.Info.LeftRoutteCount--;
                    client.Player.Extra.Info.LeftRoutteRate = result;
                }
                client.Out.SendLeftRouleteResult(client.Player.Extra.Info);
                return 0;
            }
            return 0;
        }
    }
}
