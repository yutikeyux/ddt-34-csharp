using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    /// <summary>
    /// Quest CondictionType = 64
    /// Art arda giris yapma gorevi. Para2 = hedef gun sayisi
    /// Her login'de sayac azalir.
    /// </summary>
    public class ConsecutiveLoginQuestCondition : BaseCondition
    {
        public ConsecutiveLoginQuestCondition(BaseQuest quest, QuestConditionInfo info, int value)
            : base(quest, info, value)
        {
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.PlayerLogin += Player_Login;
        }

        public override bool IsCompleted(GamePlayer player)
        {
            return base.Value <= 0;
        }

        private void Player_Login()
        {
            if (base.Value > 0)
            {
                base.Value--;
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.PlayerLogin -= Player_Login;
        }
    }
}
