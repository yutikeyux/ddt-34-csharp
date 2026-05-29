namespace Game.Server.Achievement
{
    /// <summary>
    /// Evlenme takibi. Oyuncu evlendiginde sayaci artar.
    /// Achievement RecordType: 112
    /// </summary>
    public class MarryCondition : BaseUserRecord
    {
        public MarryCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.PlayerMarry += Player_Marry;
        }

        private void Player_Marry()
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.PlayerMarry -= Player_Marry;
        }
    }
}
