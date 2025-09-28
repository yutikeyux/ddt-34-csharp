using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class FourHardShortNpc : ABrain
	{
		public int attackingTurn = 1;

		public int orchinIndex = 1;

		public int currentCount = 0;

		public int Dander = 0;

		protected List<Living> m_livings;

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
			bool flag = false;
			int num = 0;
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X > 0 && current.X < 0)
				{
					int num2 = (int)base.Body.Distance(current.X, current.Y);
					if (num2 > num)
					{
						num = num2;
					}
					flag = true;
				}
			}
			if (flag)
			{
				this.KillAttack(0, 0);
			}
			else if (!flag)
			{
				if (this.attackingTurn == 1)
				{
					this.MoveToPlayer();
				}
				else if (this.attackingTurn == 2)
				{
					this.MoveToPlayer();
				}
				else if (this.attackingTurn == 3)
				{
					this.MoveToPlayer();
				}
				else
				{
					this.MoveToPlayer();
					this.attackingTurn = 0;
				}
				this.attackingTurn++;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		public void MoveToPlayer()
		{
			base.Body.MoveTo(base.Game.Random.Next(370, 650), base.Body.Y, "walk", 2000, "", 4);
		}

		private void KillAttack(int fx, int tx)
		{
			base.Body.PlayMovie("beat", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 4000, null);
		}
	}
}
