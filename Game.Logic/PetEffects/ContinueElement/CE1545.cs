using System;
using System.Collections.Generic;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
    public class CE1545 : BasePetEffect
    {
        private int m_type;
        private int m_count;
        private int m_probability;
        private int m_delay;
        private int m_coldDown;
        private int m_currentId;
        private int m_added;

        public CE1545(int count, int probability, int type, int skillId, int delay, string elementID)
          : base(ePetEffectType.CE1545, elementID)
        {
            m_count = count;
            m_coldDown = count;
            m_probability = probability == -1 ? 10000 : probability;
            m_type = type;
            m_delay = delay;
            m_currentId = skillId;
        }

        private void Player_PlayerClearBuffSkillPet(Player player)
        {
            Stop();
        }

        private void Player_BeginNextTurn(Living living)
        {
            if (m_added < 0)
            {
                m_added = (int)(living.BaseDamage * 30.0 / 100.0);
                if (living.BaseDamage < (double)m_added)
                    m_added = (int)living.BaseDamage - 1;
                living.BaseDamage -= (double)m_added;
            }
        }

        private void Player_BeginSelfTurn(Living living)
        {
            m_count--;
            if (m_count < 0)
            {
                Stop();
            }
        }

        protected override void OnAttachedToPlayer(Player player)
        {
            player.BeginNextTurn += new LivingEventHandle(Player_BeginNextTurn);
            player.BeginSelfTurn += new LivingEventHandle(Player_BeginSelfTurn);
            player.PlayerClearBuffSkillPet += new PlayerEventHandle(Player_PlayerClearBuffSkillPet);
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.BaseDamage += (double)m_added;
            m_added = 0;
            player.BeginNextTurn -= new LivingEventHandle(Player_BeginNextTurn);
            player.BeginSelfTurn -= new LivingEventHandle(Player_BeginSelfTurn);
        }

        public override bool Start(Living living)
        {
            CE1545 effect = living.PetEffectList.GetOfType(ePetEffectType.CE1545) as CE1545;
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
    }
}
