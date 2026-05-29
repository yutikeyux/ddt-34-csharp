using Game.Logic;

namespace Game.Server.Achievement
{
    /// <summary>
    /// Oyun kazanma takibi. Oyuncu bir maci kazandiginda sayaci artar (herhangi mod).
    /// Achievement RecordType: 107
    /// </summary>
    public class GameOverWinCondition : BaseUserRecord
    {
        public GameOverWinCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.GameOver += Player_GameOver;
        }

        private void Player_GameOver(AbstractGame game, bool isWin, int gainXp, bool isSpanArea, bool isCouple)
        {
            if (isWin)
            {
                m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.GameOver -= Player_GameOver;
        }
    }
}
