using Bussiness;
using Game.Logic.Actions;
using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;
using System;

namespace Game.Logic.PetEffects.Element.Actives
{
    public class AE1556 : BasePetEffect
    {
        private int m_type = 0;
        private int m_count = 0;
        private int m_probability = 0;
        private int m_delay = 0;
        private int m_coldDown = 0;
        private int m_currentId;
        private int m_added = 0;

        public AE1556(int count, int probability, int type, int skillId, int delay, string elementID)
            : base(ePetEffectType.AE1556, elementID)
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
            AE1556 effect = living.PetEffectList.GetOfType(ePetEffectType.AE1556) as AE1556;
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
            player.BeginNextTurn += new LivingEventHandle(Player_BeginNextTurn);
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerShoot -= new PlayerEventHandle(Player_PlayerShoot);
            player.BeginNextTurn -= new LivingEventHandle(Player_BeginNextTurn);
        }

        private void Player_BeginNextTurn(Living living)
        {
            if ((!IsTrigger ? 1 : (!(living is Player) ? 1 : 0)) != 0)
                return;
            (living as Player).Delay += (living as TurnedLiving).Delay * 70 / 100;
            (living as Player).Energy = 120;
            (living as Player).Dander -= 50;
            IsTrigger = false;
        }

        public void Player_PlayerShoot(Player player)
        {
            if ((player.CurrentBall.IsSpecial() ? 1 : (player.PetEffects.CurrentUseSkill != m_currentId ? 1 : 0)) != 0)
                return;
            player.Delay = player.DefaultDelay;
            IsTrigger = true;
            player.EffectTrigger = true;
            player.Game.SendEquipEffect(player, LanguageMgr.GetTranslation("PetAttackEffect.Success"));
            player.Game.AddAction(new LivingSayAction(player, LanguageMgr.GetTranslation("AddTurnEquipEffect.msg"), 9, 0, 1000));
            new CE1556(1, m_probability, m_count, m_currentId, m_delay, ElementInfo.ID.ToString()).Start(player);
        }
    }
}
