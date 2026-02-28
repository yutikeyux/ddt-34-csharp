using Game.Logic;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    public class KesifGecmeCountluHaliCondition : BaseCondition
    {
        public KesifGecmeCountluHaliCondition(BaseQuest quest, QuestConditionInfo info, int value)
            : base(quest, info, value)
        {
            // Not: 'value' parametresi veritabanından gelen mevcut ilerleme durumunu temsil eder.
            // Görev yeni başlıyorsa value 0'dır.
        }

        public override void AddTrigger(GamePlayer player)
        {
            // Oyuncu görevi bittiğinde çalışacak eventi tetikleyiciye ekle
            player.MissionTurnOver += player_MissionOver;
        }

        public override bool IsCompleted(GamePlayer player)
        {
            // Artan mantık: Mevcut değer (Value), hedef değerden (Para2) büyük veya eşitse görev tamamlanmıştır.
            return base.Value <= 0;
        }

        private void player_MissionOver(AbstractGame game, int missionId, int isWin)
        {
            // Eğer görev hedefe zaten ulaştıysa daha fazla saymaya gerek yok
            if (missionId == m_info.Para1)
            {
                base.Value--;
            }
           
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            // Oyuncu görevden ayrıldığında veya iptal ettiğinde eventi kaldır
            player.MissionTurnOver -= player_MissionOver;
        }
    }
}