using Game.Logic;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    public class DiscordBaglaCondition : BaseCondition
    {
        public DiscordBaglaCondition(BaseQuest quest, QuestConditionInfo info, int value)
            : base(quest, info, value)
        {
        }

        // Görev aktif olduğunda event'i bağlar
        public override void AddTrigger(GamePlayer player)
        {
            player.DiscordBaglaEvent += Player_DiscordBaglaEvent;
        }

        // Görevin tamamlanıp tamamlanmadığını kontrol eder
        // Value (Kalan Hedef) 0 veya daha aşağısıysa tamamlanmış sayılır.
        public override bool IsCompleted(GamePlayer player)
        {
            return base.Value <= 0;
        }

        // Event tetiklendiğinde çalışır (Discord bağlandığında)
        private void Player_DiscordBaglaEvent(GamePlayer player)
        {
            // Hedef sayısını 1 azalt
            base.Value--;
        }

        // Görev bittiğinde veya iptal edildiğinde event'i temizler
        public override void RemoveTrigger(GamePlayer player)
        {
            player.DiscordBaglaEvent -= Player_DiscordBaglaEvent;
        }
    }
}