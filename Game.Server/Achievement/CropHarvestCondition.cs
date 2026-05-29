namespace Game.Server.Achievement
{
    /// <summary>
    /// Ciftlik hasat toplama takibi. Oyuncu her hasat topladiginda sayaci artar.
    /// Achievement RecordType: 114
    /// </summary>
    public class CropHarvestCondition : BaseUserRecord
    {
        public CropHarvestCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.CropPrimaryEvent += Player_CropPrimary;
        }

        private void Player_CropPrimary()
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.CropPrimaryEvent -= Player_CropPrimary;
        }
    }
}
