namespace Game.Server.Achievement
{
    /// <summary>
    /// Pet sahiplenme takibi. Oyuncu her pet sahiplendiginde sayaci artar.
    /// Achievement RecordType: 105
    /// </summary>
    public class AdoptPetCondition : BaseUserRecord
    {
        public AdoptPetCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.AdoptPetEvent += Player_AdoptPet;
        }

        private void Player_AdoptPet()
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.AdoptPetEvent -= Player_AdoptPet;
        }
    }
}
