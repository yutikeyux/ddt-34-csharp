using Game.Logic;

namespace Game.Server.Achievement
{
    /// <summary>
    /// Takim savasi kazanma basarimi. Takim olarak kazanilan maclar sayilir.
    /// Achievement RecordType: 119
    /// </summary>
    public class TeamBattleWinCondition : BaseUserRecord
    {
        public TeamBattleWinCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.GameOverCountTeam += Player_GameOverCountTeam;
        }

        private void Player_GameOverCountTeam(AbstractGame game, bool isWin, int gainXp, int countPlayersTeam)
        {
            if (isWin && countPlayersTeam >= 2)
            {
                m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.GameOverCountTeam -= Player_GameOverCountTeam;
        }
    }
}
