using System;
using System.Collections.Generic;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
    public class CE1553 : BasePetEffect
    {
        private int m_type = 0;
        private int m_count = 0;
        private int m_probability = 0;
        private int m_delay = 0;
        private int m_coldDown = 0;
        private int m_currentId;
        private int m_added = 0;

        public CE1553(int count, int probability, int type, int skillId, int delay, string elementID)
            : base(ePetEffectType.CE1553, elementID)
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
            CE1553 effect = living.PetEffectList.GetOfType(ePetEffectType.CE1553) as CE1553;
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
            player.BeforeTakeDamage += new LivingTakedDamageEventHandle(this.Player_BeforeTakeDamage);
            player.BeginSelfTurn += new LivingEventHandle(this.Player_BeginSelfTurn);
        }

        private void Player_BeforeTakeDamage(Living living, Living source, ref int damageAmount, ref int criticalAmount)
        {
            if (m_added > 0)
                return;
            m_added = 50;
            criticalAmount -= criticalAmount * m_added / 100;
            living.Game.method_10(living, ElementInfo, true);
        }

        private void Player_BeginSelfTurn(Living living)
        {
            --m_count;
            if (m_count >= 0)
                return;
            Stop();
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            m_added = 0;
            player.Game.method_10(player, ElementInfo, false);
            player.BeforeTakeDamage -= new LivingTakedDamageEventHandle(Player_BeforeTakeDamage);
            player.BeginSelfTurn -= new LivingEventHandle(Player_BeginSelfTurn);
        }
    }
}
