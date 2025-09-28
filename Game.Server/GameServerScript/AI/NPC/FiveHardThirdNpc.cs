using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class FiveHardThirdNpc : ABrain
	{
		protected Player m_targer;

		private static Random random = new Random();

		private static string[] listChat = new string[]
		{
			"为了荣誉！为了胜利！！",
			"握紧手中的武器，不要发抖呀～",
			"为了国王而战！",
			"敌人就在眼前，大家做好战斗准备！",
			"感觉最近国王的行为举止越来越反常......",
			"为了啵咕的胜利！！兄弟们冲啊！",
			"快消灭敌人！",
			"大家一起上,人多力量大！",
			"大家一起速战速决！",
			"包围敌人，歼灭他们。",
			"增援！增援！我们需要更多的增援！！",
			"就算牺牲自己，也不会让你们轻易得逞。",
			"不要轻视啵咕的力量，否则你会为此付出代价。"
		};

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			this.m_body.CurrentDamagePlus = 1f;
			this.m_body.CurrentShootMinus = 1f;
			if (this.m_body.IsSay)
			{
				string oneChat = FiveHardThirdNpc.GetOneChat();
				int delay = base.Game.Random.Next(0, 5000);
				this.m_body.Say(oneChat, 0, delay);
			}
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
						}
					}
					else if (player.X > base.Body.X)
					{
					}
				}
				else if (base.Body.Y < 420)
				{
					if (base.Body.X + num > 200)
					{
					}
				}
				else if (player.X > base.Body.X)
				{
				}
			}
		}

		public void MoveBeat()
		{
			base.Body.Beat(this.m_targer, "beatA", 100, 0, 0, 1, 1);
		}

		public void FallBeat()
		{
			base.Body.Beat(this.m_targer, "beatA", 100, 0, 2000, 1, 1);
		}

		public void Jump()
		{
			base.Body.Direction = 1;
			base.Body.JumpTo(base.Body.X, base.Body.Y - 240, "Jump", 0, 2, 3, new LivingCallBack(this.Beating));
		}

		public void Beating()
		{
			if (this.m_targer != null && !base.Body.Beat(this.m_targer, "beatA", 100, 0, 0, 1, 1))
			{
				this.MoveToPlayer(this.m_targer);
			}
		}

		public void Fall()
		{
			base.Body.FallFrom(base.Body.X, base.Body.Y + 240, null, 0, 0, 12, new LivingCallBack(this.Beating));
		}

		public static string GetOneChat()
		{
			int num = FiveHardThirdNpc.random.Next(0, FiveHardThirdNpc.listChat.Length);
			return FiveHardThirdNpc.listChat[num];
		}

		public static void LivingSay(List<Living> livings)
		{
			if (livings != null && livings.Count != 0)
			{
				int count = livings.Count;
				foreach (Living current in livings)
				{
					current.IsSay = false;
				}
				int num;
				if (count <= 5)
				{
					num = FiveHardThirdNpc.random.Next(0, 2);
				}
				else if (count > 5 && count <= 10)
				{
					num = FiveHardThirdNpc.random.Next(1, 3);
				}
				else
				{
					num = FiveHardThirdNpc.random.Next(1, 4);
				}
				if (num > 0)
				{
					int[] array = new int[num];
					int i = 0;
					while (i < num)
					{
						int index = FiveHardThirdNpc.random.Next(0, count);
						if (!livings[index].IsSay)
						{
							livings[index].IsSay = true;
							int num2 = FiveHardThirdNpc.random.Next(0, 5000);
							i++;
						}
					}
				}
			}
		}
	}
}
