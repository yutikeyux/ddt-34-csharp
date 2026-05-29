using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    /// <summary>
    /// Quest CondictionType = 57
    /// Lonca bagis gorevi. Para1 = bagis tipi (0=para, 1=altin, -1=herhangi), Para2 = hedef miktar
    /// </summary>
    public class GuildDonateQuestCondition : BaseCondition
    {
        public GuildDonateQuestCondition(BaseQuest quest, QuestConditionInfo info, int value)
            : base(quest, info, value)
        {
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.Riches += Player_Riches;
        }

        public override bool IsCompleted(GamePlayer player)
        {
            return base.Value <= 0;
        }

        private void Player_Riches(int donateValue, int donateType)
        {
            if (base.Value <= 0) return;

            // Para1 = bagis tipi filtresi (-1 = herhangi tur)
            if (m_info.Para1 == -1 || m_info.Para1 == donateType)
            {
                base.Value -= donateValue;
                if (base.Value < 0) base.Value = 0;
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.Riches -= Player_Riches;
        }
    }
}
