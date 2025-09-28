using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;
using System;

namespace Game.Logic.PetEffects.Element.Passives
{
    public class PE1355 : BasePetEffect
    {
        private int m_type = 0;
        private int m_count = 0;
        private int m_probability = 0;
        private int m_delay = 0;
        private int m_coldDown = 0;
        private int m_currentId;
        private int m_added = 0;
        private double m_damage = 0;
        public PE1355(int count, int probability, int type, int skillId, int delay, string elementID)
            : base(ePetEffectType.PE1355, elementID)
        {
            m_count = count;
            m_coldDown = 0;
            m_probability = probability == -1 ? 10000 : probability;
            m_type = type;
            m_delay = delay;
            m_currentId = skillId;
        }

        public override bool Start(Living living)
        {
            PE1355 effect = living.PetEffectList.GetOfType(ePetEffectType.PE1355) as PE1355;
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
            player.BeforeTakeDamage += Player_BeforeTakeDamage;
            player.AfterKilledByLiving += Player_AfterKilledByLiving;
            //player.BeginSelfTurn += Player_BeginSelfTurn;
            player.EndAttacking += Player_EndAttacking;
        }

        private void Player_EndAttacking(Living living)
        {
            if (m_coldDown >= 1)
            {
                if (m_coldDown > 1)
                {
                    m_coldDown -= 2;
                    living.BaseDamage -= 40 * 2;
                }
                else
                {
                    m_coldDown--;
                    living.BaseDamage -= 40;
                }
            }
        }

        private void Player_BeforeTakeDamage(Living living, Living source, ref int damageAmount, ref int criticalAmount)
        {
            if (m_delay == 0)
            {
                m_damage = living.BaseDamage + (40 * 6);
                m_delay = 1;
            }
            if (m_coldDown < 4)
            {
                living.Game.SendPetBuff(living, ElementInfo, true);
                if (m_added == 0)
                    m_added = 40 * (living as Player).BallCount;
                living.BaseDamage += m_added;
                living.BaseDamage = Math.Min(living.BaseDamage, m_damage);
                m_count += m_added;
                IsTrigger = true;
                m_coldDown += (living as Player).BallCount;
                m_coldDown = m_coldDown > 4 ? 4 : m_coldDown;
            }
            
        }

        private void Player_BeginSelfTurn(Living living)
        {
            if (m_coldDown >= 2)
            {
                m_coldDown -= 2;
                living.BaseDamage -= 40 * 2;
            }
            //m_added = 20;           
        }
        private void Player_AfterKilledByLiving(Living living, Living target, int damageAmount, int criticalAmount)
        {
            //if (IsTrigger)
            {
                target.Game.SendPetBuff(target, ElementInfo, true);
                IsTrigger = false;
            }
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.AfterKilledByLiving -= Player_AfterKilledByLiving;
        }        
    }
}
