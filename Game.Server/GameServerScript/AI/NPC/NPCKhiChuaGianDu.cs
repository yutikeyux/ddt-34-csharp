using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class ActivePve10Boss2 : ABrain
	{
		protected Player m_targer;

		private int m_turn = 0;

		private int dame;

		private PhysicalObj moive;

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
			bool flag = false;
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X > 1150)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.KillAttack(base.Body.X - 150, base.Body.X + 150);
			}
			else
			{
				this.m_targer = base.Game.FindRandomPlayer();
				if (this.m_turn == 0)
				{
					this.TankA();
					this.m_turn++;
				}
				else
				{
					this.TankB();
					this.m_turn = 0;
				}
			}
		}

		private void TankA()
		{
			base.Body.CurrentDamagePlus = (float)base.Game.Random.Next(150, 200);
			base.Body.PlayMovie("beatA", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - 100, this.m_targer.X + 100, "cry", 4000, null);
		}

		private void TankB()
		{
			base.Body.CurrentDamagePlus = (float)base.Game.Random.Next(150, 200);
			base.Body.PlayMovie("beatB", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - 100, this.m_targer.X + 100, "cry", 4000, null);
			base.Body.CallFuction(new LivingCallBack(this.SkillBong), 3000);
		}

		private void SkillBong()
		{
			List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
			int blood = base.Game.Random.Next(5000, 10000);
			foreach (Player current in allLivingPlayers)
			{
				current.AddEffect(new ContinueReduceBloodEffect(2, blood, current), 0);
			}
		}

		private void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 10000f;
			base.Body.PlayMovie("beatA", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 3000, null);
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void GoOut()
		{
			if (this.moive != null)
			{
				base.Game.RemovePhysicalObj(this.moive, true);
				this.moive = null;
			}
		}
	}
}
