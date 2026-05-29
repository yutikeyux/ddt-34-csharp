namespace Game.Server.Achievement
{
    /// <summary>
    /// Totem tasina muhrur takma basarimi. Her muhrur takildikca sayac artar.
    /// Achievement RecordType: 121
    /// </summary>
    public class TotemGemstoneAchievementCondition : BaseUserRecord
    {
        public TotemGemstoneAchievementCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.UserToemGemstonetEvent += Player_TotemGemstone;
        }

        private void Player_TotemGemstone()
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.UserToemGemstonetEvent -= Player_TotemGemstone;
        }
    }
}
