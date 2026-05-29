namespace Game.Server.Achievement
{
    /// <summary>
    /// Tas/gem ekleme takibi. Oyuncu ekipmana her tas eklemesinde sayaci artar.
    /// Achievement RecordType: 104
    /// </summary>
    public class ItemInsertCondition : BaseUserRecord
    {
        public ItemInsertCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.ItemInsert += Player_ItemInsert;
        }

        private void Player_ItemInsert()
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.ItemInsert -= Player_ItemInsert;
        }
    }
}
