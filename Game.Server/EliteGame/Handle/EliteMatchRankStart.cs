using Game.Base.Packets;
using Game.Logic;

namespace Game.Server.EliteGame.Handle
{
    [EliteGameHandleAttbute((byte)EliteGamePackageType.ELITE_MATCH_RANK_START)]
    public class EliteMatchRankStart : IEliteGameCommandHadler//
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            if (ExerciseMgr.EliteStatus == 5 && Player.PlayerCharacter.Grade >= 30)
            {
                Player.Out.SendEliteGameStartRoom();
                return true;
            }
            return false;
        }
    }
}