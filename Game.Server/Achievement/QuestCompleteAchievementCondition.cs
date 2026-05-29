using Game.Server.Quests;

namespace Game.Server.Achievement
{
    /// <summary>
    /// Gorev tamamlama basarimi. Tamamlanan her gorev sayaci arttirir.
    /// Achievement RecordType: 118
    /// </summary>
    public class QuestCompleteAchievementCondition : BaseUserRecord
    {
        public QuestCompleteAchievementCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.PlayerQuestFinish += Player_QuestFinish;
        }

        private void Player_QuestFinish(BaseQuest quest)
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.PlayerQuestFinish -= Player_QuestFinish;
        }
    }
}
