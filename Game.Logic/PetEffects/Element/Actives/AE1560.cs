using Bussiness;
using Game.Logic.Actions;
using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Logic.PetEffects.Element.Actives
{
    public class AE1560 : BasePetEffect
    {
        private int m_type = 0;
        private int m_count = 0;
        private int m_probability = 0;
        private int m_delay = 0;
        private int m_coldDown = 0;
        private int m_currentId;
        private int m_added = 0;
        private int BaseDamage = 0;

        public AE1560(int count, int probability, int type, int skillId, int delay, string elementID)
            : base(ePetEffectType.AE1560, elementID)
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
            AE1560 effect = living.PetEffectList.GetOfType(ePetEffectType.AE1560) as AE1560;
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
            player.PlayerBuffSkillPet += new PlayerEventHandle(Player_PlayerBuffSkillPet);
        }

        protected override void OnRemovedFromPlayer(Player player)
        {
            player.PlayerBuffSkillPet -= new PlayerEventHandle(Player_PlayerBuffSkillPet);
        }

        private void Player_PlayerBuffSkillPet(Player player)
        {
            List<Point> pointList = new List<Point>();
            if ((player.PetEffects.CurrentUseSkill != m_added ? 1 : (!(player.Game is PVPGame) ? 1 : 0)) == 0)
            {
                int x = player.X;
                int y = player.Y;
                foreach (Player enemy in player.Game.GetAllEnemyPlayers(player as Living))
                {
                    pointList.Add(new Point(enemy.X, enemy.Y));
                }
                if (pointList.Count >= 0)
                {
                    int index = rand.Next(pointList.Count);
                    player.SetXY(pointList[index]);
                    player.StartMoving();
                    Player fp = new Player(player.PlayerDetail, (player.Game as PVPGame).PhysicalId++, (player.Game as PVPGame), player.Team, player.PlayerDetail.PlayerCharacter.hp);
                    fp.Reset();
                    fp.Direction = player.Direction;
                    fp.IsShadown = true;
                    fp.SetXY(x, y);
                    fp.Delay = player.Delay + m_delay;
                    (player.Game as PVPGame).AddShadow(fp);
                }
            }
        }
    }
}
