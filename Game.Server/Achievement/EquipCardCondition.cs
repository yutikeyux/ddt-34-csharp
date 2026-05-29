namespace Game.Server.Achievement
{
    /// <summary>
    /// Kart takilma takibi. Oyuncu ekipmana kart taktiginda sayaci artar.
    /// Achievement RecordType: 109
    /// </summary>
    public class EquipCardCondition : BaseUserRecord
    {
        public EquipCardCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.EquipCardEvent += Player_EquipCard;
        }

        private void Player_EquipCard()
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.EquipCardEvent -= Player_EquipCard;
        }
    }
}
