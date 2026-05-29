using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    /// <summary>
    /// Quest CondictionType = 59
    /// Buff/iksir kullanma gorevi. Para2 = kac kez buff kullanilmali
    /// </summary>
    public class UseBufferQuestCondition : BaseCondition
    {
        public UseBufferQuestCondition(BaseQuest quest, QuestConditionInfo info, int value)
            : base(quest, info, value)
        {
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.UseBuffer += Player_UseBuffer;
        }

        public override bool IsCompleted(GamePlayer player)
        {
            return base.Value <= 0;
        }

        private void Player_UseBuffer(GamePlayer player)
        {
            if (base.Value > 0)
            {
                base.Value--;
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.UseBuffer -= Player_UseBuffer;
        }
    }
}
