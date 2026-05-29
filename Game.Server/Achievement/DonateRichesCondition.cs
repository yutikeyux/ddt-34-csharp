namespace Game.Server.Achievement
{
    /// <summary>
    /// Zenginlik bagisi takibi. Oyuncu lonca/sisteme bagis yaptiginda toplam miktari gunceller.
    /// Achievement RecordType: 110
    /// </summary>
    public class DonateRichesCondition : BaseUserRecord
    {
        public DonateRichesCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.Riches += Player_Riches;
        }

        private void Player_Riches(int value, int type)
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, value);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.Riches -= Player_Riches;
        }
    }
}
