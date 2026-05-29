namespace Game.Server.Achievement
{
    /// <summary>
    /// Buff/iksir kullanim takibi. Oyuncu her buff kullandiginda sayaci artar.
    /// Achievement RecordType: 111
    /// </summary>
    public class UseBufferCondition : BaseUserRecord
    {
        public UseBufferCondition(GamePlayer player, int type)
            : base(player, type)
        {
            AddTrigger(player);
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.UseBuffer += Player_UseBuffer;
        }

        private void Player_UseBuffer(GamePlayer player)
        {
            m_player.AchievementInventory.UpdateUserAchievement(m_type, 1);
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.UseBuffer -= Player_UseBuffer;
        }
    }
}
