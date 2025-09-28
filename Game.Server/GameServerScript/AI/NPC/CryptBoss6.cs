using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class CryptBoss6 : ABrain
	{
		private int m_attackTurn = 0;

		private bool m_openBoss = false;

		public int currentCount = 0;

		public int Dander = 0;

		private static string[] AttackAChat = new string[]
		{
			"Ta sẽ thiêu đốt ngươi!!!!",
			"Hãy đỡ móng vuốt bóng tối của ta.",
			"Ta sẽ cho ngươi thấy ai giỏi hơn.",
			"Ngươi nghĩ ngươi đủ khả năng đánh bại ta sao?",
			"Ta sẽ đưa ngươi vào địa ngục.",
			"Đủ khả năng đỡ đòn này không?"
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
				base.Body.Say("Ta là bất khả chiến bại!!!", 0, 1000);
				base.Body.CallFuction(new LivingCallBack(this.AttackA), 3000);
			}
			else
			{
				Player player = base.Game.FindRandomPlayer();
				if (this.MetarX(player.X, base.Body.X) > 200)
				{
					int num = player.X + 200;
					if (num < 0)
					{
						num = 200;
					}
					else if (num > base.Game.Map.Info.DeadWidth)
					{
						num = base.Game.Map.Info.DeadWidth - 200;
					}
					int delay;
					if (this.MetarX(num, base.Body.X) <= 250)
					{
						delay = 1000;
					}
					else
					{
						delay = 3000;
					}
					base.Body.MoveTo(num, player.Y, "walk", 200, "", ((SimpleBoss)base.Body).NpcInfo.speed);
					base.Body.ChangeDirection(base.Game.FindlivingbyDir(base.Body), delay);
				}
				int num2 = base.Game.Random.Next(0, CryptBoss6.AttackAChat.Length - 1);
				base.Body.Say(CryptBoss6.AttackAChat[num2], 1, 500);
				if (this.m_attackTurn == 0)
				{
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
					this.AttackC();
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
			base.Body.CurrentDamagePlus = 5f;
			base.Body.PlayMovie("beatA", 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.RangeAttacking), 2000);
		}

		private void AttackB()
		{
			base.Body.CurrentDamagePlus = 3f;
			base.Body.PlayMovie("beatB", 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.RangeAttacking), 2000);
		}

		private void AttackC()
		{
			base.Body.CurrentDamagePlus = 8f;
			base.Body.PlayMovie("beatC", 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.RangeAttacking), 2000);
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
