using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class CryptBoss1 : ABrain
	{
		private int m_attackTurn = 0;

		private bool m_openBoss = false;

		public int currentCount = 0;

		public int Dander = 0;

		private static string[] AttackAChat = new string[]
		{
			"Ta chém chết ngươi!",
			"Muốn giết ta à? Đỡ được không?",
			"Ngươi sẽ phải chết",
			"Gãi ngứa cho ta à? Tiếp chiêu.",
			"Chém phát chết ''nuôn''"
		};

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			base.Body.CurrentDamagePlus = 1f;
			base.Body.CurrentShootMinus = 1f;
			base.Body.SetRect(((SimpleBoss)base.Body).NpcInfo.X, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);
			if (base.Body.Direction == -1)
			{
				base.Body.SetRect(((SimpleBoss)base.Body).NpcInfo.X, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);
			}
			else
			{
				base.Body.SetRect(-((SimpleBoss)base.Body).NpcInfo.X - ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);
			}
		}

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			if (!this.m_openBoss)
			{
				this.m_openBoss = true;
				base.Body.PlayMovie("born", 500, 0);
				base.Body.Say("Người xông vào đây để hiến mạng cho ta hả?", 0, 1000);
				base.Body.CallFuction(new LivingCallBack(this.AttackB), 4000);
			}
			else if (this.m_attackTurn == 0)
			{
				int num = base.Game.Random.Next(0, CryptBoss1.AttackAChat.Length - 1);
				base.Body.Say(CryptBoss1.AttackAChat[num], 1, 500);
				this.AttackA();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.AttackB();
				this.m_attackTurn++;
			}
			else
			{
				if (base.Body.X != 1065)
				{
					this.MoveToDefaultPoint();
					base.Body.CallFuction(new LivingCallBack(this.AttackC), 3100);
				}
				else
				{
					this.AttackC();
				}
				this.m_attackTurn = 0;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void AttackA()
		{
			Player player = base.Game.FindRandomPlayer();
			int num = 0;
			if (this.MetarX(player.X, base.Body.X) > 200)
			{
				int num2 = player.X + 200;
				if (num2 < 0)
				{
					num2 = 200;
				}
				else if (num2 > base.Game.Map.Info.DeadWidth)
				{
					num2 = base.Game.Map.Info.DeadWidth - 200;
				}
				if (this.MetarX(num2, base.Body.X) <= 300)
				{
					num = 1000;
				}
				else
				{
					num = 3000;
				}
				base.Body.MoveTo(num2, player.Y, "walk", 200, "", ((SimpleBoss)base.Body).NpcInfo.speed);
				base.Body.ChangeDirection(base.Game.FindlivingbyDir(base.Body), num);
			}
			base.Body.CurrentDamagePlus = 3f;
			base.Body.PlayMovie("beatA", num + 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.RangeAttacking), num + 3000);
		}

		private void AttackB()
		{
			base.Body.CurrentDamagePlus = 6f;
			base.Body.PlayMovie("beatB", 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.RangeAttacking), 5000);
		}

		private void AttackC()
		{
			base.Body.CurrentDamagePlus = 5f;
			base.Body.Say("Nếm thử ma pháp quyền trượng của ta đây!", 0, 500);
			base.Body.PlayMovie("beatC", 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.RangeAttacking), 3000);
		}

		private void RangeAttacking()
		{
			base.Body.RangeAttacking(base.Body.X - 10000, base.Body.X + 10000, "cry", 0, null);
		}

		private void MoveToDefaultPoint()
		{
			base.Body.MoveTo(1065, 648, "walk", 200, "", ((SimpleBoss)base.Body).NpcInfo.speed);
			base.Body.ChangeDirection(base.Game.FindlivingbyDir(base.Body), 3000);
		}

		private int MetarX(int x1, int x2)
		{
			int result;
			if (x1 > x2)
			{
				result = x1 - x2;
			}
			else
			{
				result = x2 - x1;
			}
			return result;
		}
	}
}
