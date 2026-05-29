using SqlDataProvider.Data;

namespace Game.Server.Achievement
{
    /// <summary>
    /// Yeni ekipman kazanma basarimi. Her yeni gear alindiginda sayac artar.
    /// Achievement RecordType: 122
    /// </summary>
    public class NewGearAchievementCondition : BaseUserRecord
    {
        public NewGearAchievementCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.NewGearEvent += Player_NewGear;
        }

        private void Player_NewGear(ItemInfo item)
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.NewGearEvent -= Player_NewGear;
        }
    }
}
