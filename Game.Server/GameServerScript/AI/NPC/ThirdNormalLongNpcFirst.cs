using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class ThirdNormalLongNpcFirst : ABrain
	{
		private int m_attackTurn = 0;

		private int isSay = 0;

		protected Player m_targer;

		private static string[] AllAttackChat = new string[]
		{
			LanguageMgr.GetTranslation("Ddtank super là số 1", new object[0])
		};

		private static string[] ShootChat = new string[]
		{
			LanguageMgr.GetTranslation("Anh em tiến lên !", new object[0])
		};

		private static string[] KillPlayerChat = new string[]
		{
			LanguageMgr.GetTranslation("Anh em tiến lên !", new object[0])
		};

		private static string[] CallChat = new string[]
		{
			LanguageMgr.GetTranslation("Ai giết được chúng sẻ được ban thưởng !", new object[0])
		};

		private static string[] JumpChat = new string[]
		{
			LanguageMgr.GetTranslation("Ai giết được chúng sẻ được ban thưởng !", new object[0])
		};

		private static string[] KillAttackChat = new string[]
		{
			LanguageMgr.GetTranslation("Đến gần ta, chỉ có chết!!", new object[0]),
			LanguageMgr.GetTranslation("Tiêu diệt họ sẻ có thưởng!!", new object[0])
		};

		private static string[] ShootedChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg15", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg16", new object[0])
		};

		private static string[] DiedChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg17", new object[0])
		};

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
			base.Body.CurrentDamagePlus = 1f;
			base.Body.CurrentShootMinus = 1f;
			this.isSay = 0;
		}

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			bool flag = false;
			int num = 0;
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X > 1269 && current.X < base.Game.Map.Info.ForegroundWidth + 1)
				{
					int num2 = (int)base.Body.Distance(current.X, current.Y);
					if (num2 > num)
					{
						num = num2;
					}
					flag = true;
				}
			}
			if (flag)
			{
				this.KillAttack(1269, base.Game.Map.Info.ForegroundWidth + 1);
			}
			else if (this.m_attackTurn == 0)
			{
				if (((PVEGame)base.Game).GetLivedLivings().Count == 9)
				{
					this.PersonalAttack();
				}
				else
				{
					this.PersonalAttack();
				}
				this.m_attackTurn++;
			}
			else
			{
				this.PersonalAttack();
				this.m_attackTurn = 0;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void KillAttack(int fx, int tx)
		{
			int num = base.Game.Random.Next(0, ThirdNormalLongNpcFirst.KillAttackChat.Length);
			base.Body.Say(ThirdNormalLongNpcFirst.KillAttackChat[num], 0, 1000);
			base.Body.CurrentDamagePlus = 1f;
			Player player = base.Game.FindRandomPlayer();
			int x = base.Game.Random.Next(player.X - 70, player.X + 70);
			base.Body.MoveTo(x, player.Y, "walk", 1000, "", 3);
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			base.Body.PlayMovie("beatB", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 5000, null);
		}

		private void PersonalAttack()
		{
			int x = base.Game.Random.Next(1200, 1550);
			base.Body.MoveTo(x, base.Body.Y, "walk", 1000, "", 3, new LivingCallBack(this.NextAttack));
		}

		private void NextAttack()
		{
			Player player = base.Game.FindRandomPlayer();
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			if (player != null)
			{
				base.Body.CurrentDamagePlus = 0.8f;
				int num = base.Game.Random.Next(player.X, player.X);
				if (base.Body.ShootPoint(player.X, player.Y, 58, 1000, 10000, 1, 3f, 2550))
				{
					base.Body.PlayMovie("beatA", 1700, 0);
				}
			}
		}

		public override void OnKillPlayerSay()
		{
			base.OnKillPlayerSay();
			int num = base.Game.Random.Next(0, ThirdNormalLongNpcFirst.KillPlayerChat.Length);
			base.Body.Say(ThirdNormalLongNpcFirst.KillPlayerChat[num], 1, 0, 2000);
		}

		public override void OnDiedSay()
		{
		}

		private void CreateChild()
		{
		}

		public override void OnShootedSay()
		{
			int num = base.Game.Random.Next(0, ThirdNormalLongNpcFirst.ShootedChat.Length);
			if (this.isSay == 0 && base.Body.IsLiving)
			{
				base.Body.Say(ThirdNormalLongNpcFirst.ShootedChat[num], 1, 900, 0);
				this.isSay = 1;
			}
			if (!base.Body.IsLiving)
			{
				num = base.Game.Random.Next(0, ThirdNormalLongNpcFirst.DiedChat.Length);
				base.Body.Say(ThirdNormalLongNpcFirst.DiedChat[num], 1, 100, 2000);
			}
		}

		public static string GetOneChat()
		{
			int num = ThirdNormalLongNpcFirst.random.Next(0, ThirdNormalLongNpcFirst.listChat.Length);
			return ThirdNormalLongNpcFirst.listChat[num];
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
					num = ThirdNormalLongNpcFirst.random.Next(0, 2);
				}
				else if (count > 5 && count <= 10)
				{
					num = ThirdNormalLongNpcFirst.random.Next(1, 3);
				}
				else
				{
					num = ThirdNormalLongNpcFirst.random.Next(1, 4);
				}
				if (num > 0)
				{
					int[] array = new int[num];
					int i = 0;
					while (i < num)
					{
						int index = ThirdNormalLongNpcFirst.random.Next(0, count);
						if (!livings[index].IsSay)
						{
							livings[index].IsSay = true;
							int delay = ThirdNormalLongNpcFirst.random.Next(0, 5000);
							livings[index].Say(ThirdNormalLongNpcFirst.GetOneChat(), 0, delay);
							i++;
						}
					}
				}
			}
		}
	}
}
