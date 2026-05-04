using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using Game.Server.SceneMarryRooms;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler(242, "进入礼堂")]
    public class MarryRoomLoginHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            MarryRoom marryRoom = null;
            string msg = "";
            int num = packet.ReadInt();
            string text = packet.ReadString();
            int marryMap = packet.ReadInt();
            if (num != 0)
            {
                marryRoom = MarryRoomMgr.GetMarryRoombyID(num, text ?? "", ref msg);
            }
            else
            {
                if (client.Player.PlayerCharacter.IsCreatedMarryRoom)
                {
                    MarryRoom[] allMarryRoom = MarryRoomMgr.GetAllMarryRoom();
                    MarryRoom[] array = allMarryRoom;
                    foreach (MarryRoom marryRoom2 in array)
                    {
                        if (marryRoom2.Info.GroomID == client.Player.PlayerCharacter.ID || marryRoom2.Info.BrideID == client.Player.PlayerCharacter.ID)
                        {
                            marryRoom = marryRoom2;
                            break;
                        }
                    }
                }
                if (marryRoom == null && client.Player.PlayerCharacter.SelfMarryRoomID != 0)
                {
                    _ = client.Player.Out.SendMarryRoomLogin(client.Player, result: false);
                    MarryRoomInfo marryRoomInfo = null;
                    using (PlayerBussiness playerBussiness = new())
                    {
                        marryRoomInfo = playerBussiness.GetMarryRoomInfoSingle(client.Player.PlayerCharacter.SelfMarryRoomID);
                    }
                    if (marryRoomInfo != null)
                    {
                        _ = client.Player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("MarryRoomLoginHandler.RoomExist", marryRoomInfo.ServerID, client.Player.PlayerCharacter.SelfMarryRoomID));
                        return 0;
                    }
                }
            }
            if (marryRoom != null)
            {
                if (marryRoom.CheckUserForbid(client.Player.PlayerCharacter.ID))
                {
                    _ = client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("MarryRoomLoginHandler.Forbid"));
                    _ = client.Player.Out.SendMarryRoomLogin(client.Player, result: false);
                    return 1;
                }
                if (marryRoom.RoomState == eRoomState.FREE)
                {
                    if (marryRoom.AddPlayer(client.Player))
                    {
                        client.Player.MarryMap = marryMap;
                        _ = client.Player.Out.SendMarryRoomLogin(client.Player, result: true);
                        _ = marryRoom.SendMarryRoomInfoUpdateToScenePlayers(marryRoom);
                        return 0;
                    }
                }
                else
                {
                    _ = client.Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("MarryRoomLoginHandler.AlreadyBegin"));
                }
                _ = client.Player.Out.SendMarryRoomLogin(client.Player, result: false);
            }
            else
            {
                _ = client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation(string.IsNullOrEmpty(msg) ? "MarryRoomLoginHandler.Failed" : msg));
                _ = client.Player.Out.SendMarryRoomLogin(client.Player, result: false);
            }
            return 1;
        }
    }
}
