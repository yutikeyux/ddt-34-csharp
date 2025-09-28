using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class NPCDSLT5 : ABrain
	{
		protected Player m_targer;

		private int m_turn = 0;

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
				if (current.IsLiving && Math.Abs(current.X - base.Body.X) < 300)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.KillAttack(base.Body.X - 300, base.Body.X + 300);
			}
			else if (this.m_turn == 0)
			{
				this.KillA();
				this.m_turn++;
			}
			else if (this.m_turn == 1)
			{
				this.Attack2();
				this.m_turn++;
			}
			else
			{
				this.Attack();
				this.m_turn = 0;
			}
		}

		private void KillA()
		{
			Player player = base.Game.FindRandomPlayer();
			base.Body.CurrentDamagePlus = 70f;
			base.Body.PlayMovie("beatA", 2400, 0);
			base.Body.RangeAttacking(player.X - 300, player.X + 300, "cry", 4000, null);
		}

		public void Attack2()
		{
			base.Body.PlayMovie("beatB", 1200, 0);
			base.Body.CallFuction(new LivingCallBack(this.KillB), 1000);
		}

		private void KillB()
		{
			Player player = base.Game.FindRandomPlayer();
			base.Body.CurrentDamagePlus = 70f;
			this.moive = ((PVEGame)base.Game).Createlayer(player.X, player.Y, "moive", "asset.game.ten.jianyu", "out", 1, 1);
			base.Body.RangeAttacking(player.X - 300, player.X + 300, "cry", 4000, null);
			base.Body.CallFuction(new LivingCallBack(this.GoOut), 4000);
		}

		private void GoOut()
		{
			if (this.moive != null)
			{
				base.Game.RemovePhysicalObj(this.moive, true);
				this.moive = null;
			}
		}

		public void Attack()
		{
			Player player = base.Game.FindRandomPlayer();
			if (player.X < base.Body.X)
			{
				int num = base.Body.X - (player.X + 289);
				base.Body.MoveTo(base.Body.X - num, player.Y, "walk", 2400, "", 5, new LivingCallBack(this.KillC));
			}
			else
			{
				int num = player.X - 289 - base.Body.X;
				base.Body.MoveTo(base.Body.X + num, player.Y, "walk", 2400, "", 5, new LivingCallBack(this.KillC));
			}
		}

		private void KillC()
		{
			base.Body.CurrentDamagePlus = 70f;
			base.Body.PlayMovie("beatC", 2400, 0);
			base.Body.RangeAttacking(base.Body.X - 300, base.Body.X + 300, "cry", 4000, null);
		}

		private void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 1000f;
			base.Body.PlayMovie("beatC", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 5000, null);
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}
