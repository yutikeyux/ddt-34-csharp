namespace Game.Server.Achievement
{
    /// <summary>
    /// Pet seviye atlama takibi. Oyuncu petini her seviye atlattiginda sayaci artar.
    /// Achievement RecordType: 106
    /// </summary>
    public class PetLevelUpCondition : BaseUserRecord
    {
        public PetLevelUpCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.UpLevelPetEvent += Player_UpLevelPet;
        }

        private void Player_UpLevelPet()
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.UpLevelPetEvent -= Player_UpLevelPet;
        }
    }
}
