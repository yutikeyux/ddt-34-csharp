using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class FourTerrorFireNpc : ABrain
	{
		private int m_attackTurn = 0;

		private PhysicalObj m_moive = null;

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
			if (this.m_attackTurn == 0)
			{
				this.Move();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.Move();
				this.m_attackTurn++;
			}
			else
			{
				this.m_attackTurn = 0;
			}
		}

		private void Move()
		{
			base.Body.MoveTo(base.Game.Random.Next(225, 1115), base.Game.Random.Next(113, 354), "fly", 500, "", 6, null);
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			if (this.m_moive != null)
			{
				base.Game.RemovePhysicalObj(this.m_moive, true);
				this.m_moive = null;
			}
		}
	}
}
