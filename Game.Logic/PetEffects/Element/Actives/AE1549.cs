using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace Game.Logic.PetEffects.Element.Actives
{
    public class AE1549 : BasePetEffect
    {
        private int m_type = 0;
        private int m_count = 0;
        private int m_probability = 0;
        private int m_delay = 0;
        private int m_coldDown = 0;
        private int m_currentId;
        private int m_added = 0;

        public AE1549(int count, int probability, int type, int skillId, int delay, string elementID)
            : base(ePetEffectType.AE1549, elementID)
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
            AE1549 effect = living.PetEffectList.GetOfType(ePetEffectType.AE1549) as AE1549;
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
            player.AfterPlayerShooted += new PlayerEventHandle(this.player_AfterPlayerShootedByLiving);
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(player_AfterBuffSkillPetByLiving);
            player.AfterPlayerShooted += new PlayerEventHandle(this.player_AfterPlayerShootedByLiving);
        }

        void player_AfterBuffSkillPetByLiving(Player player)
        {
            if (player.PetEffects.CurrentUseSkill != m_currentId)
                return;
            m_added = 180;
            player.CurrentDamagePlus += (float)(this.m_added / 100);
        }

        void player_AfterPlayerShootedByLiving(Player player)
        {
            if (player.PetEffects.CurrentUseSkill != m_currentId)
                return;
            player.CurrentDamagePlus -= (float)(m_added / 100);
            m_added = 0;
        }
    }
}
