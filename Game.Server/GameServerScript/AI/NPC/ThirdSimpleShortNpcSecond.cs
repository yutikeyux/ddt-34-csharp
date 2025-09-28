using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class ThirdSimpleShortNpcSecond : ABrain
	{
		protected Player m_targer;

		private static Random random = new Random();

		private static string[] listChat = new string[]
		{
			"Để tôn vinh! Để giành chiến thắng! !",
			"Tổ chức cướp vũ khí của họ, không run sợ!",
			"Super Ddtank muôn năm !",
			"Kẻ thù ở phía trước, sẵn sàng chiến đấu!",
			"Cảm thấy hành vi của nhà vua và bất thường hơn ...",
			"Để Boo Goo chiến thắng! ! Brothers phí!",
			"Nhanh chóng để tiêu diệt kẻ thù!",
			"Sức mạnh số 1 !",
			"Với một sửa chữa nhanh chóng!",
			"Vây quanh kẻ thù và tiêu diệt chúng.",
			"Quân tiếp viện! Quân tiếp viện! Chúng tôi cần thêm quân tiếp viện! !",
			"Hy sinh bản thân, sẽ không cho phép bạn có được đi với.",
			"Đừng đánh giá thấp sức mạnh của Boo Goo, nếu không bạn sẽ phải trả cho việc này."
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
				string oneChat = ThirdSimpleShortNpcSecond.GetOneChat();
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
							base.Body.MoveTo(25, base.Body.Y, "walk", 1200, "", 3, new LivingCallBack(this.Jump));
						}
						else
						{
							base.Body.MoveTo(base.Body.X - num, base.Body.Y, "walk", 1200, "", 3, new LivingCallBack(this.MoveBeat));
						}
					}
					else if (player.X > base.Body.X)
					{
						base.Body.MoveTo(base.Body.X + num, base.Body.Y, "walk", 1200, "", 3, new LivingCallBack(this.MoveBeat));
					}
					else
					{
						base.Body.MoveTo(base.Body.X - num, base.Body.Y, "walk", 1200, "", 3, new LivingCallBack(this.MoveBeat));
					}
				}
				else if (base.Body.Y < 420)
				{
					if (base.Body.X + num > 200)
					{
						base.Body.MoveTo(200, base.Body.Y, "walk", 1200, "", 3, new LivingCallBack(this.Fall));
					}
				}
				else if (player.X > base.Body.X)
				{
					base.Body.MoveTo(base.Body.X + num, base.Body.Y, "walk", 1200, "", 3, new LivingCallBack(this.MoveBeat));
				}
				else
				{
					base.Body.MoveTo(base.Body.X - num, base.Body.Y, "walk", 1200, "", 3, new LivingCallBack(this.MoveBeat));
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
			int num = ThirdSimpleShortNpcSecond.random.Next(0, ThirdSimpleShortNpcSecond.listChat.Length);
			return ThirdSimpleShortNpcSecond.listChat[num];
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
					num = ThirdSimpleShortNpcSecond.random.Next(0, 2);
				}
				else if (count > 5 && count <= 10)
				{
					num = ThirdSimpleShortNpcSecond.random.Next(1, 3);
				}
				else
				{
					num = ThirdSimpleShortNpcSecond.random.Next(1, 4);
				}
				if (num > 0)
				{
					int[] array = new int[num];
					int i = 0;
					while (i < num)
					{
						int index = ThirdSimpleShortNpcSecond.random.Next(0, count);
						if (!livings[index].IsSay)
						{
							livings[index].IsSay = true;
							int delay = ThirdSimpleShortNpcSecond.random.Next(0, 5000);
							livings[index].Say(ThirdSimpleShortNpcSecond.GetOneChat(), 0, delay);
							i++;
						}
					}
				}
			}
		}
	}
}
