using Game.Logic;

namespace Game.Server.Achievement
{
    /// <summary>
    /// Gorevi tam puan ile gecme basarimi. Full clear yapildiginda sayac artar.
    /// Achievement RecordType: 123
    /// </summary>
    public class MissionFullClearCondition : BaseUserRecord
    {
        public MissionFullClearCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.MissionFullOver += Player_MissionFullOver;
        }

        private void Player_MissionFullOver(AbstractGame game, int missionId, bool isWin, int turnNum)
        {
            if (isWin)
            {
                m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.MissionFullOver -= Player_MissionFullOver;
        }
    }
}
