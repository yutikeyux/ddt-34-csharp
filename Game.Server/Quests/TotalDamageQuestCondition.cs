using Game.Logic;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    /// <summary>
    /// Quest CondictionType = 62
    /// Toplam hasar verme gorevi. Para1 = oda tipi (0=match, 1=free, -1=hepsi), Para2 = hedef hasar
    /// </summary>
    public class TotalDamageQuestCondition : BaseCondition
    {
        public TotalDamageQuestCondition(BaseQuest quest, QuestConditionInfo info, int value)
            : base(quest, info, value)
        {
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.AfterKillingLiving += Player_AfterKillingLiving;
        }

        public override bool IsCompleted(GamePlayer player)
        {
            return base.Value <= 0;
        }

        private void Player_AfterKillingLiving(AbstractGame game, int type, int id, bool isLiving, int damage, bool isSpanArea)
        {
            if (base.Value <= 0) return;
            if (damage <= 0) return;

            bool matchRoom = (m_info.Para1 == 0 || m_info.Para1 == -1) && game.RoomType == eRoomType.Match;
            bool freeRoom = (m_info.Para1 == 1 || m_info.Para1 == -1) && game.RoomType == eRoomType.Freedom;

            if (matchRoom || freeRoom)
            {
                base.Value -= damage;
                if (base.Value < 0) base.Value = 0;
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.AfterKillingLiving -= Player_AfterKillingLiving;
        }
    }
}
