using Game.Logic;

namespace Game.Server.Achievement
{
    /// <summary>
    /// Toplam hasar basarimi. Oyuncu verdigi toplam hasari biriktirir.
    /// Achievement RecordType: 117
    /// </summary>
    public class TotalDamageAchievementCondition : BaseUserRecord
    {
        public TotalDamageAchievementCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.AfterKillingLiving += Player_AfterKillingLiving;
        }

        private void Player_AfterKillingLiving(AbstractGame game, int type, int id, bool isLiving, int damage, bool isSpanArea)
        {
            if (damage > 0)
            {
                m_player.AchievementInventory.UpdateUserAchievement(m_type, damage);
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.AfterKillingLiving -= Player_AfterKillingLiving;
        }
    }
}
