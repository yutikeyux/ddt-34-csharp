using Bussiness;
using Game.Logic.Actions;
using Game.Logic.Effects;
using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;
using System.Drawing;
using static Living;

namespace Game.Logic.PetEffects.Element.Actives
{
    public class AE1562 : BasePetEffect
    {
        private int m_type = 0;
        private int m_count = 0;
        private int m_probability = 0;
        private int m_delay = 0;
        private int m_coldDown = 0;
        private int m_currentId;
        private int m_added = 0;
        private int BaseDamage = 0;

        public AE1562(int count, int probability, int type, int skillId, int delay, string elementID)
            : base(ePetEffectType.AE1562, elementID)
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
            AE1562 effect = living.PetEffectList.GetOfType(ePetEffectType.AE1562) as AE1562;
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
            player.PlayerShoot += new PlayerEventHandle(Player_PlayerShoot);
            player.AfterKillingLiving += new KillLivingEventHanlde(Player_AfterKillingLiving);
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerShoot -= new PlayerEventHandle(Player_PlayerShoot);
            player.AfterKillingLiving -= new KillLivingEventHanlde(Player_AfterKillingLiving);
        }

        private void Player_AfterKillingLiving(Living living, Living target, int damageAmount, int criticalAmount)
        {
            if (IsTrigger)
            {
                int reduce = (target as Player).Energy * 80 / 100;
                target.AddEffect(new ReduceStrengthEffect(2, reduce), 0);
                target.Game.SendPetBuff(target, ElementInfo, true);
            }
        }

        private void Player_PlayerShoot(Player player)
        {
            IsTrigger = false;
            if ((player.CurrentBall.IsSpecial() ? 1 : (player.PetEffects.CurrentUseSkill != m_added ? 1 : 0)) == 0)
            {
                IsTrigger = true;
                player.EffectTrigger = true;
                new CE1562(2, m_probability, m_type, m_added, m_delay, ElementInfo.ID.ToString()).Start(player);
                player.Game.SendEquipEffect(player, LanguageMgr.GetTranslation("PetAttackEffect.Success"));
                player.Game.AddAction(new LivingSayAction(player, LanguageMgr.GetTranslation("ReduceStrengthEquipEffect.msg"), 9, 0, 1000));
            }
        }
    }
}
