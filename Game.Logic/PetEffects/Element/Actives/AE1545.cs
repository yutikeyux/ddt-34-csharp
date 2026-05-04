using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;
using static Living;

namespace Game.Logic.PetEffects.Element.Actives
{
    public class AE1545 : BasePetEffect
    {
        private readonly int m_type = 0;
        private readonly int m_count = 0;
        private int m_probability = 0;
        private readonly int m_delay = 0;
        private readonly int m_coldDown = 0;
        private readonly int m_currentId;
        private readonly int m_added = 0;

        public AE1545(int count, int probability, int type, int skillId, int delay, string elementID)
            : base(ePetEffectType.AE1545, elementID)
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
            if (living.PetEffectList.GetOfType(ePetEffectType.AE1545) is AE1545 effect)
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
            player.PlayerBuffSkillPet += new PlayerEventHandle(player_AfterBuffSkillPetByLiving);
            player.AfterKillingLiving += new KillLivingEventHanlde(player_AfterPlayerShootedByLiving);
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(player_AfterBuffSkillPetByLiving);
            player.AfterKillingLiving += new KillLivingEventHanlde(player_AfterPlayerShootedByLiving);
        }

        private void player_AfterBuffSkillPetByLiving(Player player)
        {
            if (player.PetEffects.CurrentUseSkill == m_currentId)
            {
                IsTrigger = true;
            }
        }

        private void player_AfterPlayerShootedByLiving(Living living, Living target, int damageAmount, int criticalAmount)
        {
            if (IsTrigger)
            {
                target.AddPetEffect(new CE1545(2, m_probability, m_type, m_currentId, m_delay, ElementInfo.ID.ToString()), 0);
                IsTrigger = false;
            }
        }
    }
}
