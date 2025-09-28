using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class NPCVatToNo : ABrain
	{
		protected Player m_targer;

		private PhysicalObj moive;

		private int m_attackTurn = 0;

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			this.m_body.CurrentDamagePlus = 30f;
		}

		public override void OnStartAttacking()
		{
			if (base.Game.KichNo)
			{
				this.TankA();
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void TankA()
		{
			Player player = base.Game.FindRandomPlayer();
			base.Body.CurrentDamagePlus = (float)base.Game.Random.Next(100, 200);
			base.Body.PlayMovie("beatA", 1700, 0);
			base.Body.RangeAttacking(base.Body.X - 100, base.Body.X + 100, "cry", 4000, null);
			base.Body.CallFuction(new LivingCallBack(this.ClearAllNpc), 3000);
		}

		private void HieuUngA()
		{
			Player player = base.Game.FindRandomPlayer();
			this.moive = ((PVEGame)base.Game).Createlayer(player.X, player.Y, "Moive", "asset.game.ten.100a", "1", 1, 1);
		}

		private void HieuUngB()
		{
			Player player = base.Game.FindRandomPlayer();
			this.moive = ((PVEGame)base.Game).Createlayer(player.X, player.Y, "Moive", "asset.game.ten.100b", "out", 1, 1);
		}

		private void ClearAllNpc()
		{
			base.Game.ClearAllNpc();
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
