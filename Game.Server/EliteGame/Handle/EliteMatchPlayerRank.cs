using Game.Base.Packets;
using Game.Server.Packets;

namespace Game.Server.EliteGame.Handle
{
    [EliteGameHandleAttbute((byte)EliteGamePackageType.ELITE_MATCH_PLAYER_RANK)]
    public class EliteMatchPlayerRank : IEliteGameCommandHadler//
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ELITEGAME);
            pkg.WriteByte((byte)EliteGamePackageType.ELITE_MATCH_PLAYER_RANK);
            pkg.WriteInt(Player.PlayerCharacter.EliteRank);
            pkg.WriteInt(Player.PlayerCharacter.EliteScore);
            Player.Out.SendTCP(pkg);
            return true;
        }
    }
}