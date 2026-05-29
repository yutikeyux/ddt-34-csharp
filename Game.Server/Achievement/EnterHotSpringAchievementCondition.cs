namespace Game.Server.Achievement
{
    /// <summary>
    /// Kaplicaya giris basarimi. Her kaplicaya girisde sayac artar.
    /// Achievement RecordType: 125
    /// </summary>
    public class EnterHotSpringAchievementCondition : BaseUserRecord
    {
        public EnterHotSpringAchievementCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.EnterHotSpringEvent += Player_EnterHotSpring;
        }

        private void Player_EnterHotSpring(GamePlayer player)
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.EnterHotSpringEvent -= Player_EnterHotSpring;
        }
    }
}
