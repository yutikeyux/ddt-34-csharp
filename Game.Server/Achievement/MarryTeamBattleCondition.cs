using Game.Logic;

namespace Game.Server.Achievement
{
    /// <summary>
    /// Es ile birlikte savas kazanma basarimi.
    /// Achievement RecordType: 120
    /// </summary>
    public class MarryTeamBattleCondition : BaseUserRecord
    {
        public MarryTeamBattleCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.GameMarryTeam += Player_GameMarryTeam;
        }

        private void Player_GameMarryTeam(AbstractGame game, bool isWin, int gainXp, int countPlayersTeam)
        {
            if (isWin)
            {
                m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.GameMarryTeam -= Player_GameMarryTeam;
        }
    }
}
