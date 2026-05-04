using Bussiness;
using Game.Base.Packets;
using Game.Server.HotSpringRooms;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.HOTSPRING_ROOM_ENTER, "礼堂数据")]
    public class HotSpringRoomEnterHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            //int id = packet.ReadInt();
            //packet.ReadString();
            int needGold = 10000;
            int needMoney = 10000;
            int roomId = packet.ReadInt();
            _ = packet.ReadString();
            if (client.Player.CurrentHotSpringRoom == null)
            {
                HotSpringRoom room = HotSpringMgr.GetHotSpringRoombyID(roomId);
                if (room != null)
                {
                    if (room.Info.roomID <= 8)
                    {
                        if (client.Player.PlayerCharacter.Gold < needGold)
                        {
                            client.Player.SendMessage(LanguageMgr.GetTranslation("HotSpringRoomEnterDataHandler.NotEmoughtGold"));
                            return 0;
                        }
                        else
                        {
                            if (room.AddPlayer(client.Player) && client.Player.RemoveGold(needGold) > 0)
                            {
                                _ = client.Out.SendEnterHotSpringRoom(client.Player);
                            }
                        }
                    }
                    else if (room.Info.roomID >= 9)
                    {
                        if (client.Player.PlayerCharacter.Money < needMoney && client.Player.PlayerCharacter.MoneyLock < needMoney)
                        {
                            client.Player.SendMessage(LanguageMgr.GetTranslation("HotSpringRoomEnterDataHandler.NotEmoughtMoney"));
                            return 0;
                        }
                        else
                        {
                            if (room.AddPlayer(client.Player) && client.Player.MoneyDirect(needMoney, true, false, true))
                            {
                                _ = client.Out.SendEnterHotSpringRoom(client.Player);
                            }
                        }
                    }
                }
                else
                {
                    client.Player.SendMessage(LanguageMgr.GetTranslation("SpaRoomLoginHandler.Failed4"));
                }
            }
            return 0;
        }
    }
}
