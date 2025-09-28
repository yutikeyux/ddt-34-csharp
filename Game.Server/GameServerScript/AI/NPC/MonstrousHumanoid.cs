using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class MonstrousHumanoid : ABrain
	{
		private Player m_target;

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			base.Body.CurrentDamagePlus = 1f;
			base.Body.CurrentShootMinus = 1f;
		}

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
			base.OnStartAttacking();
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			bool flag = this.ShootLowestBooldPlayer();
			if (!flag)
			{
				this.RandomShootPlayer();
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private bool ShootLowestBooldPlayer()
		{
			List<Player> list = new List<Player>();
			foreach (Player current in base.Game.GetAllLivingPlayers())
			{
				if ((double)current.Blood < (double)current.MaxBlood * 0.2)
				{
					list.Add(current);
				}
			}
			bool result;
			if (list.Count > 0)
			{
				int index = base.Game.Random.Next(0, list.Count);
				this.m_target = list[index];
				this.NpcAttack();
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		private void RandomShootPlayer()
		{
			List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
			int index = base.Game.Random.Next(0, allLivingPlayers.Count);
			this.m_target = allLivingPlayers[index];
			this.NpcAttack();
		}

		private void NpcAttack()
		{
			int num;
			if (this.m_target.X > base.Body.X)
			{
				num = 1;
				base.Body.ChangeDirection(1, 0);
			}
			else
			{
				num = -1;
				base.Body.ChangeDirection(-1, 0);
			}
			int num2 = Math.Abs(this.m_target.X - base.Body.X);
			if (num2 < 300)
			{
				this.ShootAttack();
			}
			else
			{
				int num3 = base.Game.Random.Next(((SimpleBoss)base.Body).NpcInfo.MoveMin, ((SimpleBoss)base.Body).NpcInfo.MoveMax) * 3;
				if (num3 > num2)
				{
					num3 = num2 - 300;
				}
				num3 *= num;
				if (!base.Body.MoveTo(base.Body.X + num3, this.m_target.Y - 20, "walk", 0, "", ((SimpleBoss)base.Body).NpcInfo.speed, new LivingCallBack(this.ShootAttack)))
				{
					this.ShootAttack();
				}
			}
		}

		private void ShootAttack()
		{
			int num = Math.Abs(this.m_target.X - base.Body.X);
			int num2;
			if (num < 200)
			{
				num2 = 10;
			}
			else if (num < 500)
			{
				num2 = 30;
			}
			else
			{
				num2 = 50;
			}
			int x = base.Game.Random.Next(this.m_target.X - num2, this.m_target.X + num2);
			if (base.Body.ShootPoint(x, this.m_target.Y, ((SimpleBoss)base.Body).NpcInfo.CurrentBallId, 1000, 10000, 1, 2f, 1700))
			{
				base.Body.PlayMovie("beat", 1700, 0);
			}
		}
	}
}
