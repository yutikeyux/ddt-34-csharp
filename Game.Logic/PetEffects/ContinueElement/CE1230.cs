using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
    public class CE1230 : BasePetEffect
    {
        private readonly int m_type = 0;
        private int m_count = 0;
        private int m_probability = 0;
        private readonly int m_delay = 0;
        private readonly int m_coldDown = 0;
        private readonly int m_currentId;
        private readonly int m_added = 0;

        public CE1230(int count, int probability, int type, int skillId, int delay, string elementID)
            : base(ePetEffectType.CE1230, elementID)
        {
            m_count = count;
            m_coldDown = count;
            m_probability = probability == -1 ? 10000 : probability;
            m_type = type;
            m_delay = delay;
            m_currentId = skillId;
        }

        public override bool Start(Living living)
        {
            if (living.PetEffectList.GetOfType(ePetEffectType.CE1230) is CE1230 effect)
            {
                effect.m_probability = m_probability > effect.m_probability ? m_probability : effect.m_probability;
                return true;
            }
            else
            {
                return base.Start(living);
            }
        }

        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginSelfTurn += Player_BeginSelfTurn;
            player.PlayerClearBuffSkillPet += Player_PlayerClearBuffSkillPet;
        }

        private void Player_PlayerClearBuffSkillPet(Player player)
        {
            _ = Stop();
        }

        private void Player_BeginSelfTurn(Living living)
        {
            m_count--;
            if (m_count < 0)
            {
                _ = Stop();
            }
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.Game.SendPetBuff(player, ElementInfo, false);
            player.BeginSelfTurn -= Player_BeginSelfTurn;
        }
    }
}
