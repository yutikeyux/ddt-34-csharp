using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
    [PacketHandler(200, "验证码")]
    public class CheckCodeHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            bool result = false;
            if (string.IsNullOrEmpty(client.Player.PlayerCharacter.CheckCode))
                return 1;

            //int check  = packet.ReadInt();

            string check = packet.ReadString();
            if (check == "cheat")
            {
                client.Player.PlayerCharacter.CheckCount += 1;
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("CheckCodeHandler.Msg3"));
                client.Disconnect();
            }
            else if (client.Player.PlayerCharacter.CheckCode.ToLower() == check.ToLower())
            {
                client.Player.PlayerCharacter.CheckCount = 0;
                client.Player.PlayerCharacter.CheckCode = "baodeptrai";
                client.Player.resetPassCode();
                client.Out.SendMessage(eMessageType.Normal, "Doğrulama başarılı, oyunun tadını çıkarın.");

                packet.ClearContext();
                packet.WriteByte(1);
               packet.WriteBoolean(false);
                client.Out.SendTCP(packet);
            }
            else if (client.Player.PlayerCharacter.CheckError < 9)
            {
                client.Player.PlayerCharacter.CheckCount += 1;
                client.Out.SendMessage(eMessageType.ChatERROR, "Yanlış kimlik doğrulaması, eğer birçok kez yanlış yapılırsa oyundan atılabilirsiniz.");
                client.Player.PlayerCharacter.CheckError++;
                //client.Player.ShowCheckCode();
            }
            else
            {
                client.Player.PlayerCharacter.CheckCount += 1;
                client.Out.SendMessage(eMessageType.Normal, "Bay bay!.");
                client.Disconnect();
            }
            return 0;
        }
    }
}
