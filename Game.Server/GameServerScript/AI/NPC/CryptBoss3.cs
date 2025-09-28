using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class CryptBoss3 : ABrain
	{
		private int m_attackTurn = 0;

		private bool m_openBoss = false;

		public int currentCount = 0;

		public int Dander = 0;

		private static string[] AttackAChat = new string[]
		{
			"Ta sẽ nhai xương ngươi!",
			"Nghĩ ta yếu hả? Đừng có mơ.",
			"Tốt lắm, giờ đến lượt ta.",
			"Hạ ngục ngươi là điều dễ như trở bàn tay.",
			"Khá lắm. Ngươi sẽ biết thế nào là đau đớn.",
			"Buông tay chịu chết đê!",
			"Ta sẽ không nương tay đâu",
			"Thật sự ngươi muốn chết sao?"
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
				base.Body.Say("Hôm nay có bữa ăn ngon rồi.", 0, 1000);
				base.Body.CallFuction(new LivingCallBack(this.AttackA), 3000);
			}
			else
			{
				int num = base.Game.Random.Next(0, CryptBoss3.AttackAChat.Length - 1);
				base.Body.Say(CryptBoss3.AttackAChat[num], 1, 500);
				if (this.m_attackTurn == 0)
				{
					this.AttackA();
					this.m_attackTurn++;
				}
				else
				{
					this.AttackB();
					this.m_attackTurn = 0;
				}
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
			if (this.MetarX(player.X, base.Body.X) > 450)
			{
				int num2 = player.X + 450;
				if (num2 < 0)
				{
					num2 = 200;
				}
				else if (num2 > base.Game.Map.Info.DeadWidth)
				{
					num2 = base.Game.Map.Info.DeadWidth - 450;
				}
				if (this.MetarX(num2, base.Body.X) <= 450)
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
			base.Body.CurrentDamagePlus = 8f;
			base.Body.PlayMovie("beatA", num + 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.RangeAttacking), num + 2000);
		}

		private void AttackB()
		{
			base.Body.CurrentDamagePlus = 3f;
			base.Body.PlayMovie("beatB", 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.RangeAttacking), 5000);
		}

		private void RangeAttacking()
		{
			base.Body.RangeAttacking(base.Body.X - 10000, base.Body.X + 10000, "cry", 0, null);
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
