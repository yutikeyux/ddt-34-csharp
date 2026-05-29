namespace Game.Server.Achievement
{
    /// <summary>
    /// Item eritme takibi. Oyuncu her item erittiginde sayaci artar.
    /// Achievement RecordType: 103
    /// </summary>
    public class ItemMeltCondition : BaseUserRecord
    {
        public ItemMeltCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.ItemMelt += Player_ItemMelt;
        }

        private void Player_ItemMelt(int categoryID)
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.ItemMelt -= Player_ItemMelt;
        }
    }
}
