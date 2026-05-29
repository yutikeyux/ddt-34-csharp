namespace Game.Server.Achievement
{
    /// <summary>
    /// Item fuzyon/kaynak takibi. Oyuncu her fuzyon yaptiginda sayaci artar.
    /// Achievement RecordType: 102
    /// </summary>
    public class ItemFusionCondition : BaseUserRecord
    {
        public ItemFusionCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.ItemFusion += Player_ItemFusion;
        }

        private void Player_ItemFusion(int fusionType)
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.ItemFusion -= Player_ItemFusion;
        }
    }
}
