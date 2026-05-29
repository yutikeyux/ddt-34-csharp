using Game.Logic;
using SqlDataProvider.Data;

namespace Game.Server.Achievement
{
    /// <summary>
    /// Boss oldurme basarimi. Her boss olduruldugunde sayac artar.
    /// Achievement RecordType: 116
    /// </summary>
    public class BossKillAchievementCondition : BaseUserRecord
    {
        public BossKillAchievementCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.AfterKillingBoss += Player_AfterKillingBoss;
        }

        private void Player_AfterKillingBoss(AbstractGame game, NpcInfo npc, int damage)
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.AfterKillingBoss -= Player_AfterKillingBoss;
        }
    }
}
