using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class YearMonster : ABrain
	{
		private int m_attackTurn = 0;

		public int currentCount = 0;

		public int Dander = 0;

		private PhysicalObj moive;

		private Player target;

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
				if (current.IsLiving && current.X > 1000)
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
			else if (this.m_attackTurn == 2)
			{
				this.AttackC();
				this.m_attackTurn++;
			}
			else
			{
				this.AttackD();
				this.target = base.Game.FindRandomPlayer();
				if (this.target != null)
				{
					if (this.target.X < 400)
					{
						this.m_attackTurn = 0;
					}
					else
					{
						this.m_attackTurn = 1;
					}
				}
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 1000f;
			base.Body.PlayMovie("beatE", 2000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 3000, null);
		}

		private void AttackA()
		{
			base.Body.CurrentDamagePlus = 1.5f;
			base.Body.PlayMovie("beatA", 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.MovingPlayer), 3000);
		}

		private void MovingPlayer()
		{
			Player[] allPlayers = base.Game.GetAllPlayers();
			Player[] array = allPlayers;
			for (int i = 0; i < array.Length; i++)
			{
				Player player = array[i];
				player.StartSpeedMult(player.X - 200, player.Y);
			}
			base.Body.CallFuction(new LivingCallBack(this.RangeAttacking), 1000);
		}

		private void AttackB()
		{
			base.Body.PlayMovie("beatB", 3000, 0);
			base.Body.CallFuction(new LivingCallBack(this.GoShootB), 4000);
		}

		private void AttackC()
		{
			base.Body.CurrentDamagePlus = 3.1f;
			base.Body.PlayMovie("beatC", 3000, 0);
			base.Body.CallFuction(new LivingCallBack(this.RangeAttacking), 4500);
		}

		private void AttackD()
		{
			base.Body.PlayMovie("beatD", 3000, 0);
			base.Body.CallFuction(new LivingCallBack(this.GoShootD), 4000);
		}

		private void GoShootB()
		{
			base.Body.CurrentDamagePlus = 2.5f;
			this.target = base.Game.FindRandomPlayer();
			if (this.target != null)
			{
				((PVEGame)base.Game).SendGameFocus(this.target, 0, 1000);
				this.moive = ((PVEGame)base.Game).Createlayer(this.target.X, this.target.Y, "moive", "asset.game.fifteen.305b", "out", 1, 1);
				base.Body.CallFuction(new LivingCallBack(this.GoOutB), 2000);
				base.Body.CallFuction(new LivingCallBack(this.RangeAttacking), 1000);
			}
		}

		private void GoOutB()
		{
			if (this.moive != null)
			{
				base.Game.RemovePhysicalObj(this.moive, true);
				this.moive = null;
			}
		}

		private void GoShootD()
		{
			base.Body.CurrentDamagePlus = 7.5f;
			this.target = base.Game.FindRandomPlayer();
			if (this.target != null)
			{
				((PVEGame)base.Game).SendGameFocus(this.target, 0, 1000);
				this.moive = ((PVEGame)base.Game).Createlayer(this.target.X, this.target.Y, "moive", "asset.game.fifteen.305d", "out", 1, 1);
				base.Body.CallFuction(new LivingCallBack(this.GoOutD), 2000);
				base.Body.CallFuction(new LivingCallBack(this.RangeAttacking), 1000);
			}
		}

		private void GoOutD()
		{
			((PVEGame)base.Game).SendGameFocus(base.Body, 0, 1000);
			base.Body.PlayMovie("born", 1000, 0);
			if (this.moive != null)
			{
				base.Game.RemovePhysicalObj(this.moive, true);
				this.moive = null;
			}
		}

		private void RangeAttacking()
		{
			base.Body.RangeAttacking(base.Body.X - 10000, base.Body.X + 10000, "cry", 0, null);
		}
	}
}
