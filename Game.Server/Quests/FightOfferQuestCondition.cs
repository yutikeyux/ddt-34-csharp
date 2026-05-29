using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    /// <summary>
    /// Quest CondictionType = 65
    /// Savas puani (offer) toplama gorevi. Para2 = hedef puan miktari
    /// </summary>
    public class FightOfferQuestCondition : BaseCondition
    {
        public FightOfferQuestCondition(BaseQuest quest, QuestConditionInfo info, int value)
            : base(quest, info, value)
        {
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.FightAddOfferEvent += Player_FightAddOffer;
        }

        public override bool IsCompleted(GamePlayer player)
        {
            return base.Value <= 0;
        }

        private void Player_FightAddOffer(int offer)
        {
            if (base.Value > 0 && offer > 0)
            {
                base.Value -= offer;
                if (base.Value < 0) base.Value = 0;
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.FightAddOfferEvent -= Player_FightAddOffer;
        }
    }
}
