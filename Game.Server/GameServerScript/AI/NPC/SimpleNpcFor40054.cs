using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class SimpleNpcFor40054 : ABrain
	{
		protected Player m_targer;

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			this.m_body.CurrentDamagePlus = 1f;
			this.m_body.CurrentShootMinus = 1f;
		}

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
			base.OnStartAttacking();
			this.m_targer = base.Game.FindNearestPlayer(base.Body.X, base.Body.Y);
			this.Beating();
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		public void MoveToPlayer(Player player)
		{
			int num = (int)player.Distance(base.Body.X, base.Body.Y);
			int num2 = base.Game.Random.Next(((SimpleNpc)base.Body).NpcInfo.MoveMin, ((SimpleNpc)base.Body).NpcInfo.MoveMax);
			if (num > 97)
			{
				if (num > ((SimpleNpc)base.Body).NpcInfo.MoveMax)
				{
					num = num2;
				}
				else
				{
					num -= 90;
				}
				if (player.Y < 420 && player.X < 210)
				{
					if (base.Body.Y > 420)
					{
						if (base.Body.X - num < 50)
						{
							base.Body.MoveTo(25, base.Body.Y, "fly", 1200, "", 3, new LivingCallBack(this.MoveBeat));
						}
						else
						{
							base.Body.MoveTo(base.Body.X - num, base.Body.Y, "fly", 1200, "", 3, new LivingCallBack(this.MoveBeat));
						}
					}
					else if (player.X > base.Body.X)
					{
						base.Body.MoveTo(base.Body.X + num, base.Body.Y, "fly", 1200, "", 3, new LivingCallBack(this.MoveBeat));
					}
					else
					{
						base.Body.MoveTo(base.Body.X - num, base.Body.Y, "fly", 1200, "", 3, new LivingCallBack(this.MoveBeat));
					}
				}
				else if (base.Body.Y < 420)
				{
					if (base.Body.X + num > 200)
					{
						base.Body.MoveTo(200, base.Body.Y, "fly", 1200, "", 3, new LivingCallBack(this.MoveBeat));
					}
				}
				else if (player.X > base.Body.X)
				{
					base.Body.MoveTo(base.Body.X + num, base.Body.Y, "fly", 1200, "", 3, new LivingCallBack(this.MoveBeat));
				}
				else
				{
					base.Body.MoveTo(base.Body.X - num, base.Body.Y, "fly", 1200, "", 3, new LivingCallBack(this.MoveBeat));
				}
			}
		}

		public void MoveBeat()
		{
			base.Body.Beat(this.m_targer, "beatA", 100, 0, 0, 1, 1);
		}

		public void Beating()
		{
			if (this.m_targer != null && !base.Body.Beat(this.m_targer, "beatA", 100, 0, 0, 1, 1))
			{
				this.MoveToPlayer(this.m_targer);
			}
		}
	}
}
