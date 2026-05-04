using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
    [PacketHandler(70, "邀请")]
    public class GameInviteHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client.Player.CurrentRoom != null)
            {
                GamePlayer playerById = WorldMgr.GetPlayerById(packet.ReadInt());
                if (playerById == client.Player)
                {
                    return 0;
                }
                GSPacketIn gSPacketIn = new(70, client.Player.PlayerCharacter.ID);
                foreach (GamePlayer player in client.Player.CurrentRoom.GetPlayers())
                {
                    if (player == playerById)
                    {
                        _ = client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("friendnotinthesameserver.Sameroom"));
                        return 0;
                    }
                }
                if (playerById != null && playerById.CurrentRoom == null)
                {
                    gSPacketIn.WriteInt(client.Player.PlayerCharacter.ID);
                    gSPacketIn.WriteInt(client.Player.CurrentRoom.RoomId);
                    gSPacketIn.WriteInt(client.Player.CurrentRoom.MapId);
                    gSPacketIn.WriteByte(client.Player.CurrentRoom.TimeMode);
                    gSPacketIn.WriteByte((byte)client.Player.CurrentRoom.RoomType);
                    gSPacketIn.WriteByte((byte)client.Player.CurrentRoom.HardLevel);
                    gSPacketIn.WriteByte((byte)client.Player.CurrentRoom.LevelLimits);
                    gSPacketIn.WriteString(client.Player.PlayerCharacter.NickName);
                    gSPacketIn.WriteBoolean((client.Player.PlayerCharacter.typeVIP > 0));
                    gSPacketIn.WriteInt(client.Player.PlayerCharacter.VIPLevel);
                    gSPacketIn.WriteString(client.Player.CurrentRoom.Name);
                    gSPacketIn.WriteString(client.Player.CurrentRoom.Password);
                    if (client.Player.CurrentRoom.RoomType == eRoomType.Dungeon)
                    {
                        if (client.Player.CurrentRoom.Game != null)
                        {
                            gSPacketIn.WriteInt((client.Player.CurrentRoom.Game as PVEGame).SessionId);
                        }
                        else
                        {
                            gSPacketIn.WriteInt(0);
                        }
                    }
                    else
                    {
                        gSPacketIn.WriteInt(-1);
                    }
                    gSPacketIn.WriteBoolean(client.Player.CurrentRoom.isOpenBoss);
                    playerById.Out.SendTCP(gSPacketIn);
                }
                else if (playerById != null && playerById.CurrentRoom != null && playerById.CurrentRoom != client.Player.CurrentRoom)
                {
                    _ = client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("friendnotinthesameserver.Room"));
                }
                else
                {
                    _ = client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("friendnotinthesameserver.Fail"));
                }
            }
            return 0;
        }
    }
}
