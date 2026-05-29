namespace Game.Server.Achievement
{
    /// <summary>
    /// Loncaya katilma basarimi. Lonca degistiginde tetiklenir.
    /// Achievement RecordType: 124
    /// </summary>
    public class GuildJoinAchievementCondition : BaseUserRecord
    {
        public GuildJoinAchievementCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.GuildChanged += Player_GuildChanged;
        }

        private void Player_GuildChanged()
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.GuildChanged -= Player_GuildChanged;
        }
    }
}
