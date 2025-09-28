using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class ConsortiaScorpionBoss : ABrain
	{
		private int m_attackTurn = 0;

		public int currentCount = 0;

		public int Dander = 0;

		private PhysicalObj moive;

		private Player target = null;

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
				if (current.IsLiving && current.X > 857 && current.X < 1440)
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
				this.Moving();
				this.m_attackTurn++;
			}
			else
			{
				this.AllAttack();
				this.m_attackTurn = 0;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void KillAttack(int fx, int tx)
		{
			this.ChangeDirection(3);
			base.Body.PlayMovie("beatA", 1000, 0);
			this.target = base.Game.FindRandomPlayer();
			base.Body.CurrentDamagePlus = 308f;
			base.Body.RangeAttacking(base.Body.X - 10000, base.Body.X + 10000, "cry", 3300, null);
			base.Body.CallFuction(new LivingCallBack(this.CreateEffect), 3300);
			base.Body.CallFuction(new LivingCallBack(this.Out), 4600);
		}

		private void AllAttack()
		{
			this.ChangeDirection(3);
			base.Body.PlayMovie("beatA", 1000, 0);
			this.target = base.Game.FindRandomPlayer();
			base.Body.CurrentDamagePlus = 3.8f;
			base.Body.RangeAttacking(base.Body.X - 10000, base.Body.X + 10000, "cry", 3300, null);
			base.Body.CallFuction(new LivingCallBack(this.CreateEffect), 3300);
			base.Body.CallFuction(new LivingCallBack(this.Out), 4600);
		}

		public void CreateEffect()
		{
			if (this.target != null)
			{
				if (this.target.X < 1000)
				{
					this.moive = ((PVEGame)base.Game).Createlayer(this.target.X, this.target.Y, "effect", "asset.game.eight.xiezi", "beatA", 1, 0);
				}
				else
				{
					this.moive = ((PVEGame)base.Game).Createlayer(this.target.X, this.target.Y, "effect", "asset.game.eight.xiezi", "beatB", 1, 0);
				}
			}
		}

		private void Out()
		{
			((PVEGame)base.Game).SendGameFocus(base.Body, 1000, 2000);
			base.Body.PlayMovie("in", 1000, 0);
			if (this.moive != null)
			{
				base.Game.RemovePhysicalObj(this.moive, true);
				this.moive = null;
			}
		}

		private void Moving()
		{
			this.ChangeDirection(3);
			int x = base.Game.Random.Next(990, 1300);
			int direction = base.Body.Direction;
			base.Body.MoveTo(x, base.Body.Y, "walk", 1000, "", ((SimpleBoss)base.Body).NpcInfo.speed);
			base.Body.ChangeDirection(base.Game.FindlivingbyDir(base.Body), 3000);
		}

		private void ChangeDirection(int count)
		{
			int direction = base.Body.Direction;
			for (int i = 0; i < count; i++)
			{
				base.Body.ChangeDirection(-direction, i * 200 + 100);
				base.Body.ChangeDirection(direction, (i + 1) * 100 + i * 200);
			}
		}
	}
}
