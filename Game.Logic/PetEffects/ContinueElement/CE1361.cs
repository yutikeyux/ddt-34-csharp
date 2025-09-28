using System;
using System.Collections.Generic;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
    public class CE1361 : BasePetEffect
    {
        private int m_type = 0;
        private int m_count = 0;
        private int m_probability = 0;
        private int m_delay = 0;
        private int m_coldDown = 0;
        private int m_currentId;
        private int m_added = 0;

        public CE1361(int count, int probability, int type, int skillId, int delay, string elementID)
            : base(ePetEffectType.CE1361, elementID)
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
            CE1361 effect = living.PetEffectList.GetOfType(ePetEffectType.CE1361) as CE1361;
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
            player.BeginSelfTurn += Player_BeginSelfTurn;
            player.PlayerClearBuffSkillPet += Player_PlayerClearBuffSkillPet;
        }

        private void Player_PlayerClearBuffSkillPet(Player player)
        {
            player.PetEffects.ActiveBuffBlood = false;
            List<Player> allies = player.Game.GetAllTeamPlayers(player);
            foreach (Player ally in allies)
            {
                ally.PetEffects.ActiveBuffBlood = false;
            }
            Stop();
        }

        private void Player_BeginSelfTurn(Living living)
        {
            m_count--;
            if (m_count < 0)
            {
                
                Stop();
            }
            else
            {
                if (!living.PetEffects.ActiveBuffBlood)
                    return;
                m_added = living.MaxBlood * 2 / 100;
                m_added += 800;
                living.SyncAtTime = true;
                living.AddBlood(m_added);
                living.SyncAtTime = false;
                //List<Player> allies = living.Game.GetAllTeamPlayers(living);
                //foreach (Player ally in allies)
                //{
                //    if (!ally.PetEffects.ActiveBuffBlood)
                //        return;
                //    ally.SyncAtTime = true;
                //    ally.AddBlood(m_added);
                //    ally.SyncAtTime = false;
                //}
            }
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.Game.SendPetBuff(player, ElementInfo, false);
            player.BeginSelfTurn -= Player_BeginSelfTurn;            
        }
    }
}
