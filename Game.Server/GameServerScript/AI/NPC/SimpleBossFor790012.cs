using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class SimpleBossFor790012 : ABrain
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
					this.TankB();
					this.m_turn++;
				}
				else if (this.m_turn == 1)
				{
					this.TankE();
					this.m_turn++;
				}
				else if (this.m_turn == 2)
				{
					this.m_turn++;
					foreach (Player current in base.Game.GetAllFightPlayers())
					{
						if (current.IsFrost)
						{
							this.m_targer = current;
							this.TruMau();
							return;
						}
					}
					this.TankD();
				}
				else if (this.m_turn == 3)
				{
					this.TankA();
					this.m_turn++;
				}
				else
				{
					this.TankC();
					this.m_turn = 0;
				}
			}
		}

		private void TankA()
		{
			this.dame = base.Game.Random.Next(200, 300);
			base.Body.CurrentDamagePlus = 370f;
			base.Body.PlayMovie("beatA", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - this.dame, this.m_targer.X + this.dame, "cry", 4000, null);
		}

		private void TankB()
		{
			this.dame = base.Game.Random.Next(200, 300);
			base.Body.CurrentDamagePlus = 370f;
			base.Body.PlayMovie("beatB", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - this.dame, this.m_targer.X + this.dame, "cry", 4000, null);
		}

		private void TankC()
		{
			this.dame = base.Game.Random.Next(200, 300);
			base.Body.CurrentDamagePlus = 370f;
			base.Body.PlayMovie("beatC", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - this.dame, this.m_targer.X + this.dame, "cry", 4000, null);
		}

		private void TankD()
		{
			this.dame = base.Game.Random.Next(200, 300);
			base.Body.CurrentDamagePlus = 370f;
			base.Body.PlayMovie("beatD", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - this.dame, this.m_targer.X + this.dame, "cry", 4000, null);
			base.Body.CallFuction(new LivingCallBack(this.HieuUngA), 3000);
			base.Body.CallFuction(new LivingCallBack(this.PhaBang), 4000);
		}

		private void TankE()
		{
			Player player = base.Game.FindRandomPlayer();
			base.Body.ShootPoint(player.X, player.Y, 1, 1, 1, 1, 1f, 3000);
		}

		private void HieuUngA()
		{
			Player player = base.Game.FindRandomPlayer();
			this.moive = ((PVEGame)base.Game).Createlayer(player.X, player.Y, "moive", "asset.game.fifteen.424a", "out", 1, 1);
			base.Body.CallFuction(new LivingCallBack(this.GoOut), 4000);
		}

		private void TruMau()
		{
			Player player = base.Game.FindRandomPlayer();
			base.Body.PlayMovie("beatD", 2400, 0);
			((PVEGame)base.Game).SendGameFocus(player.X, player.Y, 1, 4000, 1000);
			base.Body.CallFuction(new LivingCallBack(this.HieuUngA), 3000);
			//base.Body.CallFuction(new LivingCallBack(this.PhaBang), 4000);
			base.Body.CallFuction(new LivingCallBack(this.AddBloodA), 4000);
		}

		private void AddBloodA()
		{
			Player player = base.Game.FindRandomPlayer();
			player.AddBlood(-500000, 6000);
			if (this.m_targer.Blood <= 0)
			{
				this.m_targer.Die();
			}
		}

		private void PhaBang()
		{
			Player player = base.Game.FindRandomPlayer();
			player.IsFrost = false;
		}

		private void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 100000000f;
			base.Body.PlayMovie("beatD", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 3000, null);
			base.Body.CallFuction(new LivingCallBack(this.HieuUngA), 3000);
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
