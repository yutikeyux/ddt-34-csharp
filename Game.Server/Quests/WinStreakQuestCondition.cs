using Game.Logic;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    /// <summary>
    /// Quest CondictionType = 63
    /// Ust uste mac kazanma gorevi. Para1 = oda tipi (0=match, 1=free, -1=hepsi), Para2 = hedef seri
    /// Kaybedince seri sifirlanir.
    /// </summary>
    public class WinStreakQuestCondition : BaseCondition
    {
        private int _currentStreak;

        public WinStreakQuestCondition(BaseQuest quest, QuestConditionInfo info, int value)
            : base(quest, info, value)
        {
            _currentStreak = 0;
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.GameOver += Player_GameOver;
        }

        public override bool IsCompleted(GamePlayer player)
        {
            return base.Value <= 0;
        }

        private void Player_GameOver(AbstractGame game, bool isWin, int gainXp, bool isSpanArea, bool isCouple)
        {
            if (base.Value <= 0) return;

            bool validRoom = false;
            switch (game.RoomType)
            {
                case eRoomType.Match:
                    validRoom = (m_info.Para1 == 0 || m_info.Para1 == -1);
                    break;
                case eRoomType.Freedom:
                    validRoom = (m_info.Para1 == 1 || m_info.Para1 == -1);
                    break;
            }

            if (!validRoom) return;

            if (isWin)
            {
                _currentStreak++;
                if (_currentStreak >= m_info.Para2)
                {
                    base.Value = 0;
                }
            }
            else
            {
                _currentStreak = 0;
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.GameOver -= Player_GameOver;
        }
    }
}
