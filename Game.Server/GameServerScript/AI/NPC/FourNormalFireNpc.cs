using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class FourNormalFireNpc : ABrain
	{
		private int m_turn = 0;

		private int m_attackTurn = 0;

		private PhysicalObj m_moive = null;

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
			base.OnStartAttacking();
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
				this.Die();
				this.m_attackTurn = 0;
			}
		}

		private void Move()
		{
			base.Body.MoveTo(base.Game.Random.Next(300, 980), base.Game.Random.Next(300, 600), "fly", 500, "", 6, new LivingCallBack(this.CreateChild));
		}

		public void Die()
		{
			base.Body.PlayMovie("cry", 1000, 0);
			base.Body.PlayMovie("die", 2000, 0);
			base.Body.Die(3000);
		}

		private void CreateChild()
		{
			this.m_moive = ((PVEGame)base.Game).Createlayer(base.Body.X, base.Body.Y + 20, "moive", "game.living.Living141", "stand", 1, 0);
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
