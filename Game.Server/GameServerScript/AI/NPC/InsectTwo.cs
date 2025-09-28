using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class InsectTwo : ABrain
	{
		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			base.Body.CurrentDamagePlus = 1f;
			base.Body.CurrentShootMinus = 1f;
			base.Body.SetRect(((SimpleNpc)base.Body).NpcInfo.X, ((SimpleNpc)base.Body).NpcInfo.Y, ((SimpleNpc)base.Body).NpcInfo.Width, ((SimpleNpc)base.Body).NpcInfo.Height);
			if (base.Body.Direction == -1)
			{
				base.Body.SetRect(((SimpleNpc)base.Body).NpcInfo.X, ((SimpleNpc)base.Body).NpcInfo.Y, ((SimpleNpc)base.Body).NpcInfo.Width, ((SimpleNpc)base.Body).NpcInfo.Height);
			}
			else
			{
				base.Body.SetRect(-((SimpleNpc)base.Body).NpcInfo.X - ((SimpleNpc)base.Body).NpcInfo.Width, ((SimpleNpc)base.Body).NpcInfo.Y, ((SimpleNpc)base.Body).NpcInfo.Width, ((SimpleNpc)base.Body).NpcInfo.Height);
			}
		}

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
			int x = base.Game.Random.Next(233, 1222);
			int y = base.Game.Random.Next(130, 533);
			base.Body.MoveTo(x, y, "walk", 200, "", ((SimpleNpc)base.Body).NpcInfo.speed);
			base.Body.ChangeDirection(base.Game.FindlivingbyDir(base.Body), 2000);
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}
