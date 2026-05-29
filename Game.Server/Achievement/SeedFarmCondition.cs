namespace Game.Server.Achievement
{
    /// <summary>
    /// Ciftlik tohum ekme takibi. Oyuncu her tohum ektiginde sayaci artar.
    /// Achievement RecordType: 113
    /// </summary>
    public class SeedFarmCondition : BaseUserRecord
    {
        public SeedFarmCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.SeedFoodPetEvent += Player_SeedFood;
        }

        private void Player_SeedFood()
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.SeedFoodPetEvent -= Player_SeedFood;
        }
    }
}
