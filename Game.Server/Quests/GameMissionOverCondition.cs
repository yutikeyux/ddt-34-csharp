using Game.Logic;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    public class GameMissionOverCondition : BaseCondition
    {
        public GameMissionOverCondition(BaseQuest quest, QuestConditionInfo info, int value)
            : base(quest, info, value)
        {
            // value parametresi genellikle Para2'den gelir ve başlangıç sayısını tutar (base.Value).
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.MissionTurnOver += player_MissionOver;
        }

        public override bool IsCompleted(GamePlayer player)
        {
            // Kalan gereksinim sayısı 0 veya daha azsa görev tamamlanmıştır.
            return base.Value <= 0;
        }

        private void player_MissionOver(AbstractGame game, int missionId, int isWin)
        {
            // Eğer görev henüz tamamlanmadıysa (Value > 0) işlem yap
            if (base.Value > 0)
            {
                // Girilen Mission ID (Para1) ile gelen missionId eşleşiyor mu?
                // Veya Para1 -1 olarak ayarlanmışsa (herhangi bir görev) kabul et.
                if (missionId == m_info.Para1 || m_info.Para1 == -1)
                {
                    // İsteğe bağlı: Eğer sadece görevi KAZANINCA sayılmasını istiyorsanız 
                    // aşağıdaki satırın başındaki // işaretini kaldırın:
                    // if (isWin != 1) return; 

                    // Gereken sayıdan 1 eksilt
                    base.Value--;
                }
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.MissionTurnOver -= player_MissionOver;
        }
    }
}