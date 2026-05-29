namespace Game.Server.Achievement
{
    /// <summary>
    /// Item birlestirme (compose) takibi. Oyuncu her item birlestirdiginde sayaci artar.
    /// Achievement RecordType: 101
    /// </summary>
    public class ItemComposeCondition : BaseUserRecord
    {
        public ItemComposeCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.ItemCompose += Player_ItemCompose;
        }

        private void Player_ItemCompose(int composeType)
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.ItemCompose -= Player_ItemCompose;
        }
    }
}
