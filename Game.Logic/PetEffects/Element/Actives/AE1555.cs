using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.PetEffects.Element.Passives;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;
using static Living;

namespace Game.Logic.PetEffects.Element.Actives
{
    public class AE1555 : BasePetEffect
    {
        private int m_type = 0;
        private int m_count = 0;
        private int m_probability = 0;
        private int m_delay = 0;
        private int m_coldDown = 0;
        private int m_currentId;
        private int m_added = 0;

        public AE1555(int count, int probability, int type, int skillId, int delay, string elementID)
            : base(ePetEffectType.AE1555, elementID)
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
            AE1555 effect = living.PetEffectList.GetOfType(ePetEffectType.AE1555) as AE1555;
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
            player.PlayerSkip += new PlayerEventHandle(this.Player_PlayerSkip);
        }

        private void Player_PlayerSkip(Player player)
        {
            new CE1555(2, m_probability, m_type, m_currentId, m_delay, ElementInfo.ID.ToString()).Start(player);
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerSkip -= new PlayerEventHandle(this.Player_PlayerSkip);
        }
    }
}
