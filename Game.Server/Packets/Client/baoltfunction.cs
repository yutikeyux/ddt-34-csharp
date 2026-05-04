using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(300, "baolt function")]
    public class baoltfunction : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int code = packet.ReadInt();
            int code2 = code;
            switch (code2)
            {
                case 0:
                    CheckSpeedHack(client);
                    break;
                default:
                    break;
            }
            return 0;
        }

        public void CheckSpeedHack(GameClient client)
        {
            long minuspercheck = 5; // so phuts check 1 lan tren flash
            long thetimebefore = client.Player.TimeCheckHack;
            long thetimenow = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            if ((thetimenow - thetimebefore) < ((minuspercheck * 60) - 15))
            {
                Console.WriteLine("Hack Speed detect: " + client.Player.PlayerCharacter.UserName);
                client.Player.SendMessage("Cheat Engine kullandığınız için 20 dakika yasaklandınız!");
                _ = WorldMgr.SendSysNotice("[Sistem:] [" + client.Player.ZoneName + "] Oyuncusu [" + client.Player.PlayerCharacter.NickName + "] Cheat Engine kullanırken tespit edildi ve 20 dakika uzaklaştırıldı.");
                client.Player.AddLog("Speed", "Bug speed with cheat");
                _ = client.Player.SaveIntoDatabase();
                _ = client.Player.SavePlayerInfo();
                using (ManageBussiness mnbusiness = new())
                {
                    _ = mnbusiness.ForbidPlayerByUserID(client.Player.PlayerCharacter.ID, DateTime.Now.AddMinutes(20.0), true, "Hack speed");
                }
                client.Disconnect();
            }
            else
            {
                client.Player.TimeCheckHack = thetimenow;
                GSPacketIn pkg = new(300);
                pkg.WriteInt(0);
                client.Out.SendTCP(pkg);
            }
        }

    }
}
