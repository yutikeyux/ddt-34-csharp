using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class NPCKhiChuaAoAnh4 : ABrain
	{
		private int m_attackTurn = 0;

		private PhysicalObj moive;

		private int npcID = 71159;

		private int isSay = 0;

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			base.Body.CurrentShootMinus = 1f;
			this.isSay = 0;
		}

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			bool flag = false;
			PhysicalObj[] array = base.Game.FindPhysicalObjByName("dangerMoive");
			if (array != null)
			{
				PhysicalObj[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					PhysicalObj phy = array2[i];
					base.Game.RemovePhysicalObj(phy, true);
				}
			}
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X > 1150)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.KillAttack();
			}
			else if (this.m_attackTurn == 0)
			{
				this.TankA();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.TankB();
				this.m_attackTurn++;
			}
			else
			{
				this.TankC();
				this.m_attackTurn = 0;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void KillAttack()
		{
			Player player = base.Game.FindRandomPlayer();
			base.Body.CurrentDamagePlus = 50000f;
			base.Body.PlayMovie("beatA", 3000, 0);
			base.Body.RangeAttacking(player.X - 150, player.X + 150, "cry", 4000, null);
		}

		private void TankA()
		{
			Player player = base.Game.FindRandomPlayer();
			base.Body.CurrentDamagePlus = (float)base.Game.Random.Next(150, 200);
			base.Body.PlayMovie("beatA", 1700, 0);
			base.Body.RangeAttacking(player.X - 100, player.X + 100, "cry", 4000, null);
		}

		private void TankB()
		{
			Player player = base.Game.FindRandomPlayer();
			base.Body.CurrentDamagePlus = 100f;
			base.Body.PlayMovie("beatB", 1700, 0);
			int num = base.Game.Random.Next(200, 300);
			base.Body.RangeAttacking(player.X - num, player.X + num, "cry", 4000, null);
			base.Body.CallFuction(new LivingCallBack(this.PlayersMove), 3000);
		}

		private void TankC()
		{
			base.Body.PlayMovie("beatC", 1700, 0);
			base.Body.CallFuction(new LivingCallBack(this.Call), 3000);
		}

		private void PlayersMove()
		{
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				current.MoveTo(current.X - 100, current.Y, "run", 0, "", 12);
				base.Body.CallFuction(new LivingCallBack(this.Roi), 1000);
			}
		}

		private void Roi()
		{
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.X <= 375)
				{
					current.MoveTo(current.X - 30, current.Y, "run", 0, "", 12);
					current.FallFrom(current.X - 30, current.Y, null, 0, 0, 12);
				}
			}
		}

		private void Call()
		{
			((SimpleBoss)base.Body).CreateChild(this.npcID, 1040, 900, 1, 1, -1);
		}

		public override void OnKillPlayerSay()
		{
			base.OnKillPlayerSay();
		}

		private void GoOut()
		{
			if (this.moive != null)
			{
				base.Game.RemovePhysicalObj(this.moive, true);
				this.moive = null;
			}
		}

		public override void OnShootedSay()
		{
			if (this.isSay == 0 && base.Body.IsLiving)
			{
				this.isSay = 1;
			}
		}
	}
}
