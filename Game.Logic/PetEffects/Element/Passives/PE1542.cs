using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace Game.Logic.PetEffects.Element.Passives
{
    public class PE1542 : BasePetEffect
    {
        private int m_type;
        private int m_count;
        private int m_probability;
        private int m_delay;
        private int m_coldDown;
        private int m_currentId;
        private int m_added;

        public PE1542(int count, int probability, int type, int skillId, int delay, string elementID)
          : base(ePetEffectType.PE1542, elementID)
        {
            m_count = count;
            m_coldDown = count;
            m_probability = probability == -1 ? 10000 : probability;
            m_type = type;
            m_delay = delay;
            m_currentId = skillId;
        }

        private void AddBaseDamage(Player player)
        {
            m_added = 30;
            player.PetEffects.DamagePercent += this.m_added;
        }

        private void ResetBaseDamage(Player player)
        {
            player.PetEffects.DamagePercent -= this.m_added;
            this.m_added = 0;
        }

        protected override void OnAttachedToPlayer(Player player)
        {
            player.PlayerShoot += new PlayerEventHandle(this.AddBaseDamage);
            player.AfterPlayerShooted += new PlayerEventHandle(this.ResetBaseDamage);
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerShoot -= new PlayerEventHandle(this.AddBaseDamage);
            player.AfterPlayerShooted -= new PlayerEventHandle(this.ResetBaseDamage);
        }

        public override bool Start(Living living)
        {
            PE1542 effect = living.PetEffectList.GetOfType(ePetEffectType.PE1542) as PE1542;
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
