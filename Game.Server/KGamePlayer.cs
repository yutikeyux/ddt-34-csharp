// IGamePlayer.cs
using Game.Server.Rooms;

namespace Game.Server.GameObjects
{
    public interface KGamePlayer
    {
        int ID { get; }
        string NickName { get; }
        int Grade { get; }
        int FightPower { get; }
        bool IsViewer { get; set; }
        BaseRoom CurrentRoom { get; set; }
        int CurrentRoomIndex { get; set; }
        int CurrentRoomTeam { get; set; }

        // m_placesState için PlayerId'ye ihtiyacımız var.
        // GamePlayer'da PlayerId, VirtualGamePlayer'da ID var.
        // Arayüzde bu ortak ismi kullanalım.
        int PlayerId { get; }
    }
}