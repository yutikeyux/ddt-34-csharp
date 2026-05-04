using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
    public class CE : BasePetEffect
    {
        private readonly int m_type = 0;
        private readonly int m_count = 0;
        private int m_probability = 0;
        private readonly int m_delay = 0;
        private readonly int m_coldDown = 0;
        private readonly int m_currentId;
        private readonly int m_added = 0;

        public CE(int count, int probability, int type, int skillId, int delay, string elementID)
            : base(ePetEffectType.CE, elementID)
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
            if (living.PetEffectList.GetOfType(ePetEffectType.CE) is CE effect)
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
            player.PlayerClearBuffSkillPet += Player_PlayerClearBuffSkillPet;
        }

        private void Player_PlayerClearBuffSkillPet(Player player)
        {
            _ = Stop();
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
        }
    }
}
