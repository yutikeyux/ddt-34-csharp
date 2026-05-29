namespace Game.Server.Achievement
{
    /// <summary>
    /// Kupon yukleme takibi. Oyuncu her kupon yuklediginde toplam yukleme miktarini gunceller.
    /// Achievement RecordType: 100
    /// </summary>
    public class MoneyChargeCondition : BaseUserRecord
    {
        public MoneyChargeCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.MoneyCharge += Player_MoneyCharge;
        }

        private void Player_MoneyCharge(int money)
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, money);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.MoneyCharge -= Player_MoneyCharge;
        }
    }
}
