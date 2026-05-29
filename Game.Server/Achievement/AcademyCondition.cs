namespace Game.Server.Achievement
{
    /// <summary>
    /// Akademi/ciraklik takibi. Cirak veya usta olma olaylarini sayar.
    /// Achievement RecordType: 115
    /// </summary>
    public class AcademyCondition : BaseUserRecord
    {
        public AcademyCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.AcademyEvent += Player_Academy;
        }

        private void Player_Academy(GamePlayer friendly, int type)
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.AcademyEvent -= Player_Academy;
        }
    }
}
