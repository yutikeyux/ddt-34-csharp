using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class SimpleBomblingNpc : ABrain
	{
		private Player m_target = null;

		private int m_targetDis = 0;

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
			this.m_target = base.Game.FindNearestPlayer(base.Body.X, base.Body.Y);
			this.m_targetDis = (int)this.m_target.Distance(base.Body.X, base.Body.Y);
			if (this.m_targetDis < 50)
			{
				base.Body.PlayMovie("beatA", 100, 0);
				base.Body.RangeAttacking(base.Body.X - 100, base.Body.X + 100, "cry", 1500, null);
				base.Body.Die(1000);
			}
			else
			{
				this.MoveToPlayer(this.m_target);
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		public void MoveToPlayer(Player player)
		{
			int num = base.Game.Random.Next(((SimpleNpc)base.Body).NpcInfo.MoveMin, ((SimpleNpc)base.Body).NpcInfo.MoveMax);
			if (player.X > base.Body.X)
			{
				if (base.Body.X + num >= player.X)
				{
					base.Body.MoveTo(player.X - 10, base.Body.Y, "walk", 2000, "", ((SimpleNpc)base.Body).NpcInfo.speed, new LivingCallBack(this.Beat));
				}
				else
				{
					base.Body.MoveTo(base.Body.X + num, base.Body.Y, "walk", 2000, "", ((SimpleNpc)base.Body).NpcInfo.speed, new LivingCallBack(this.Beat));
				}
			}
			else if (base.Body.X - num <= player.X)
			{
				base.Body.MoveTo(player.X + 10, base.Body.Y, "walk", 2000, "", ((SimpleNpc)base.Body).NpcInfo.speed, new LivingCallBack(this.Beat));
			}
			else
			{
				base.Body.MoveTo(base.Body.X - num, base.Body.Y, "walk", 2000, "", ((SimpleNpc)base.Body).NpcInfo.speed, new LivingCallBack(this.Beat));
			}
		}

		public void Beat()
		{
			this.m_targetDis = (int)this.m_target.Distance(base.Body.X, base.Body.Y);
			if (this.m_targetDis <= 50)
			{
				base.Body.PlayMovie("beatA", 100, 0);
				base.Body.RangeAttacking(base.Body.X - 100, base.Body.X + 100, "cry", 1500, null);
				base.Body.Die(1000);
			}
		}
	}
}
