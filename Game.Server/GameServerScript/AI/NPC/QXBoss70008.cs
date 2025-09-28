using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class QXBoss70008 : ABrain
	{
		private int m_attackTurn = 0;

		public int currentCount = 0;

		public int Dander = 0;

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			base.Body.CurrentDamagePlus = 1f;
			base.Body.CurrentShootMinus = 1f;
			base.Body.SetRect(((SimpleBoss)base.Body).NpcInfo.X, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);
			if (base.Body.Direction == -1)
			{
				base.Body.SetRect(((SimpleBoss)base.Body).NpcInfo.X, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);
			}
			else
			{
				base.Body.SetRect(-((SimpleBoss)base.Body).NpcInfo.X - ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);
			}
		}

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			bool flag = false;
			int num = 0;
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X > 670)
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
				this.KillAttack(base.Body.X - 10000, base.Body.X + 10000);
			}
			else if (this.m_attackTurn == 0)
			{
				this.AttackA();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.AttackB();
				this.m_attackTurn++;
			}
			else
			{
				this.AttackC();
				this.m_attackTurn = 0;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 1000f;
			base.Body.PlayMovie("beatA", 1000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 4000, null);
		}

		private void AttackA()
		{
			base.Body.CurrentDamagePlus = 1.5f;
			base.Body.PlayMovie("beatA", 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.RangeAttacking), 4000);
		}

		private void AttackB()
		{
			base.Body.PlayMovie("beatB", 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.RangeAttacking), 4000);
		}

		private void AttackC()
		{
			base.Body.CurrentDamagePlus = 3.1f;
			base.Body.PlayMovie("beatC", 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.RangeAttacking), 3500);
		}

		private void RangeAttacking()
		{
			base.Body.RangeAttacking(base.Body.X - 10000, base.Body.X + 10000, "cry", 0, null);
		}
	}
}
