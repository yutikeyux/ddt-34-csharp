using Game.Base.Packets;
using Game.Logic;
using Game.Server.Rooms;

namespace Game.Server.GameRoom.Handle
{
    [GameRoomHandleAttbute(10)]
    public class UpdatePlaces : IGameRoomCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            if (Player?.CurrentRoom == null)
                return false;

            byte pos = packet.ReadByte();
            int place = packet.ReadInt();
            bool isOpened = packet.ReadBoolean();
            int placeView = packet.ReadInt();

            // 🔸 placeView geçerli bir yerse (0-9 arası), SwitchToView çağrılır
            if (placeView >= 0 && placeView <= 9)
            {
                Player.CurrentRoom.SwitchToView(Player, placeView);
            }
            // Eğer placeView -100 veya başka geçersiz bir değerse, sadece host işlemleri yapılır
            // (örneğin yer kapatma)

            // 🔸 Sadece Host, yer açma/kapama yapabilir
            if (Player == Player.CurrentRoom.Host)
            {
                // Freedom dışındaki odalarda pos >= 8 geçersiz
                if (Player.CurrentRoom.RoomType != eRoomType.Freedom && pos >= 8)
                {
                    return false;
                }

                RoomMgr.UpdateRoomPos(Player.CurrentRoom, pos, isOpened, place, placeView);
            }

            return true;
        }
    }
}