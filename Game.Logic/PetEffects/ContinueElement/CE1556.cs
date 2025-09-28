using System;
using System.Collections.Generic;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
    public class CE1556 : BasePetEffect
    {
        private int m_type = 0;
        private int m_count = 0;
        private int m_probability = 0;
        private int m_delay = 0;
        private int m_coldDown = 0;
        private int m_currentId;
        private int m_added = 0;
        private int BaseDamage = 0;

        public CE1556(int count, int probability, int type, int skillId, int delay, string elementID)
            : base(ePetEffectType.CE1556, elementID)
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
            CE1556 effect = living.PetEffectList.GetOfType(ePetEffectType.CE1556) as CE1556;
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

        private void Player_BeginSelfTurn(Living living_0)
        {
            --m_count;
            if (m_count >= 1)
                return;
            Stop();
        }

        protected override void OnAttachedToPlayer(Player player)
        {
            m_added = (1 - (int)((double)player.Blood / (double)player.MaxBlood)) * 100;
            if (m_added <= 80)
            {
                BaseDamage = (int)(player.BaseDamage * (double)m_added / 100.0);
                player.BaseDamage += (double)BaseDamage;
                player.AddPetMP(4);
            }
            else
            {
                m_added = 80;
                BaseDamage = (int)(player.BaseDamage * (double)m_added / 100.0);
                player.BaseDamage += (double)BaseDamage;
                player.AddPetMP(4);
            }
            player.BeginSelfTurn += new LivingEventHandle(Player_BeginSelfTurn);
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BaseDamage -= (double)BaseDamage;
            m_added = 0;
            player.BeginSelfTurn += new LivingEventHandle(Player_BeginSelfTurn);
        }
    }
}
