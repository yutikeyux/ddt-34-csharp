using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;
using static Living;

namespace Game.Logic.PetEffects.Element.Actives
{
    public class AE1545 : BasePetEffect
    {
        private int m_type = 0;
        private int m_count = 0;
        private int m_probability = 0;
        private int m_delay = 0;
        private int m_coldDown = 0;
        private int m_currentId;
        private int m_added = 0;

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
            AE1545 effect = living.PetEffectList.GetOfType(ePetEffectType.AE1545) as AE1545;
            if (effect != null)
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
            player.AfterKillingLiving += new KillLivingEventHanlde(this.player_AfterPlayerShootedByLiving);
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(player_AfterBuffSkillPetByLiving);
            player.AfterKillingLiving += new KillLivingEventHanlde(this.player_AfterPlayerShootedByLiving);
        }

        void player_AfterBuffSkillPetByLiving(Player player)
        {
            if (player.PetEffects.CurrentUseSkill == m_currentId)
            IsTrigger = true;
        }

        void player_AfterPlayerShootedByLiving(Living living, Living target, int damageAmount, int criticalAmount)
        {
            if (IsTrigger)
            {
                target.AddPetEffect((AbstractPetEffect)new CE1545(2, m_probability, m_type, m_currentId, m_delay, ElementInfo.ID.ToString()), 0);
                IsTrigger = false;
            }
        }
    }
}
