using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    /// <summary>
    /// Quest CondictionType = 60
    /// VIP seviyesine ulasma gorevi. Para1 = hedef VIP seviyesi
    /// </summary>
    public class VIPReachQuestCondition : BaseCondition
    {
        public VIPReachQuestCondition(BaseQuest quest, QuestConditionInfo info, int value)
            : base(quest, info, value)
        {
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.Event_0 += Player_VIPUpgrade;
        }

        public override bool IsCompleted(GamePlayer player)
        {
            return base.Value <= 0;
        }

        private void Player_VIPUpgrade(int vipLevel, int vipExp)
        {
            // Para1 = hedef VIP seviyesi
            if (vipLevel >= m_info.Para1 && base.Value > 0)
            {
                base.Value = 0;
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.Event_0 -= Player_VIPUpgrade;
        }
    }
}
