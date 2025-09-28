using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class SimpleDSLT3 : ABrain
	{
		protected Player m_targer;

		private int m_turn = 0;

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
				if (current.IsLiving && Math.Abs(current.X - base.Body.X) < 150)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.KillAttack(base.Body.X - 100, base.Body.X + 100);
			}
			else
			{
				this.m_targer = base.Game.FindRandomPlayer();
				if (this.m_turn == 0)
				{
					this.KillA();
					this.m_turn++;
				}
				else if (this.m_turn == 1)
				{
					this.KillB();
					this.m_turn++;
				}
				else
				{
					this.KillC();
					this.m_turn = 0;
				}
			}
		}

		private void KillA()
		{
			base.Body.CurrentDamagePlus = 1f;
			base.Body.PlayMovie("beatA", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - 100, this.m_targer.X + 100, "cry", 4000, null);
		}

		private void KillB()
		{
			base.Body.CurrentDamagePlus = 1f;
			base.Body.PlayMovie("beatB", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - 300, this.m_targer.X + 300, "cry", 4000, null);
		}

		private void KillC()
		{
			base.Body.CurrentDamagePlus = 1f;
			base.Body.PlayMovie("beatC", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - 300, this.m_targer.X + 300, "cry", 4000, null);
		}

		private void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 1000f;
			base.Body.PlayMovie("beatA", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 5000, null);
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}
