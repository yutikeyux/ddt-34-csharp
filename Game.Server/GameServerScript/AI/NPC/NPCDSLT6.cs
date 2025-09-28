using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class NPCDSLT6 : ABrain
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
				if (current.IsLiving && Math.Abs(current.X - base.Body.X) < 150 && Math.Abs(current.Y - base.Body.Y) < 150)
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
					this.Move();
					this.m_turn++;
				}
				else if (this.m_turn == 1)
				{
					this.TankB();
					this.Move();
					this.m_turn++;
				}
				else
				{
					this.KillC();
					this.Move();
					this.m_turn = 0;
				}
			}
		}

		private void TankA()
		{
			this.dame = base.Game.Random.Next(200, 300);
			base.Body.CurrentDamagePlus = 100f;
			base.Body.PlayMovie("beatA", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - this.dame, this.m_targer.X + this.dame, "cry", 4000, null);
			base.Body.CallFuction(new LivingCallBack(this.KillA), 3000);
		}

		private void TankB()
		{
			this.dame = base.Game.Random.Next(200, 300);
			base.Body.CurrentDamagePlus = 100f;
			base.Body.PlayMovie("beatB", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - this.dame, this.m_targer.X + this.dame, "cry", 4000, null);
			base.Body.CallFuction(new LivingCallBack(this.KillB), 3000);
		}

		private void TankC()
		{
		}

		private void Move()
		{
            Random A = new Random();
            int x = A.Next(200, 800);
            int y = A.Next(200, 600);
            base.Body.MoveTo(x, y, "fly", 1000, "", ((SimpleBoss)base.Body).NpcInfo.speed);
		}

		private void KillA()
		{
			Player player = base.Game.FindRandomPlayer();
			this.moive = ((PVEGame)base.Game).Createlayer(player.X, player.Y, "moive", "asset.game.eleven.055a", "1", 1, 1);
			base.Body.CallFuction(new LivingCallBack(this.GoOut), 4000);
		}

		private void KillB()
		{
			Player player = base.Game.FindRandomPlayer();
			this.moive = ((PVEGame)base.Game).Createlayer(player.X, player.Y, "moive", "asset.game.eleven.055b", "out", 1, 1);
			base.Body.CallFuction(new LivingCallBack(this.GoOut), 4000);
		}

		private void KillC()
		{
			this.dame = base.Game.Random.Next(200, 300);
			base.Body.CurrentDamagePlus = 100f;
			base.Body.PlayMovie("beatC", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - this.dame, this.m_targer.X + this.dame, "cry", 4000, null);
		}

		private void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 1000f;
			base.Body.PlayMovie("beatA", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 3000, null);
			base.Body.CallFuction(new LivingCallBack(this.KillA), 3000);
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
