namespace Game.Server.Achievement
{
    /// <summary>
    /// 2v2 mac takibi. Oyuncu 2v2 macinda kazandiginda sayaci artar.
    /// Achievement RecordType: 108
    /// </summary>
    public class GameOver2v2Condition : BaseUserRecord
    {
        public GameOver2v2Condition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.GameOver2v2 += Player_GameOver2v2;
        }

        private void Player_GameOver2v2(bool isWin)
        {
            if (isWin)
            {
                m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.GameOver2v2 -= Player_GameOver2v2;
        }
    }
}
