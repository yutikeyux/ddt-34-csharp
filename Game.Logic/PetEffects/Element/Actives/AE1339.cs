using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
    public class AE1339 : BasePetEffect
    {
        private readonly int m_type = 0;
        private readonly int m_count = 0;
        private int m_probability = 0;
        private readonly int m_delay = 0;
        private readonly int m_coldDown = 0;
        private readonly int m_currentId;
        private readonly int m_added = 0;

        public AE1339(int count, int probability, int type, int skillId, int delay, string elementID)
            : base(ePetEffectType.AE1339, elementID)
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
            if (living.PetEffectList.GetOfType(ePetEffectType.AE1339) is AE1339 effect)
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
            player.PlayerBeginMoving += Player_PlayerBeginMoving;
            player.PlayerBuffSkillPet += Player_PlayerBuffSkillPet;
        }

        private void Player_PlayerBuffSkillPet(Player player)
        {
            if (player.PetEffects.CurrentUseSkill == m_currentId)
            {
                IsTrigger = true;
            }
        }

        private void Player_PlayerBeginMoving(Player player)
        {
            if (IsTrigger)
            {
                CE1336 effect1 = player.PetEffectList.GetOfType(ePetEffectType.CE1336) as CE1336;
                _ = effect1?.Stop();

                CE1337 effect2 = player.PetEffectList.GetOfType(ePetEffectType.CE1337) as CE1337;
                _ = effect2?.Stop();
                player.PetEffects.AddGuardValue = 0;
                IsTrigger = false;
            }
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBeginMoving -= Player_PlayerBeginMoving;
        }
    }
}
