using Game.Logic;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    /// <summary>
    /// Quest CondictionType = 56
    /// Boss oldurme gorevi. Para1 = hedef NPC ID (-1 ise herhangi boss), Para2 = kac boss oldurulmeli
    /// </summary>
    public class BossKillQuestCondition : BaseCondition
    {
        public BossKillQuestCondition(BaseQuest quest, QuestConditionInfo info, int value)
            : base(quest, info, value)
        {
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.AfterKillingBoss += Player_AfterKillingBoss;
        }

        public override bool IsCompleted(GamePlayer player)
        {
            return base.Value <= 0;
        }

        private void Player_AfterKillingBoss(AbstractGame game, NpcInfo npc, int damage)
        {
            if (base.Value <= 0) return;

            // Para1 = hedef boss NPC ID, -1 = herhangi bir boss
            if (m_info.Para1 == -1 || m_info.Para1 == npc.ID)
            {
                base.Value--;
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.AfterKillingBoss -= Player_AfterKillingBoss;
        }
    }
}
