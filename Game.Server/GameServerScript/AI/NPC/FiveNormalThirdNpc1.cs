using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class FiveNormalThirdNpc1 : ABrain
	{
		private int m_attackTurn = 0;

		private static string[] AllAttackChat = new string[]
		{
			"Trận động đất, bản thân mình! ! <br/> bạn vui lòng Ay giúp đỡ",
			"Hạ vũ khí xuống!",
			"Xem nếu bạn có thể đủ khả năng, một số ít!！"
		};

		private static string[] ShootChat = new string[]
		{
			"Cho bạn biết những gì một cú sút vết nứt!",
			"Gửi cho bạn một quả bóng - bạn phải chọn Vâng",
			"Nhóm của bạn của những người dân thường ngu dốt và thấp"
		};

		private static string[] ShootedChat = new string[]
		{
			"Ah ~ ~ Tại sao bạn tấn công? <br/> tôi đang làm gì?",
			"Oh ~ ~ nó thực sự đau khổ! Tại sao tôi phải chiến đấu? <br/> tôi phải chiến đấu ..."
		};

		private static string[] AddBooldChat = new string[]
		{
			"Xoắn ah xoay ~ <br/>xoắn ah xoay ~ ~ ~",
			"~ Hallelujah <br/>Luyaluya ~ ~ ~",
			"Yeah Yeah Yeah, <br/> để thoải mái!"
		};

		private static string[] KillAttackChat = new string[]
		{
			"Con rồng trong thế giới! !"
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
		}

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
			base.OnStartAttacking();
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			bool flag = false;
			int num = 0;
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X > 0 && current.X < 0)
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
				this.KillAttack(0, 0);
			}
			else if (this.m_attackTurn == 0)
			{
				this.In();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.m_attackTurn++;
			}
			else
			{
				this.Beat();
				this.m_attackTurn = 0;
			}
		}

		private void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 10f;
			int num = base.Game.Random.Next(0, FiveNormalThirdNpc1.KillAttackChat.Length);
			base.Body.Say(FiveNormalThirdNpc1.KillAttackChat[num], 1, 1000);
			base.Body.PlayMovie("beat", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 4000, null);
		}

		private void In()
		{
			base.Body.PlayMovie("AtoB", 3000, 0);
			Player player = base.Game.FindRandomPlayer();
			player.MoveTo(base.Body.X, base.Body.Y, "", 0, "", 3);
			player.IsNoHole = true;
		}

		private void Beat()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			base.Body.PlayMovie("beatA", 1000, 4500);
			base.Body.RangeAttacking(base.Body.X - 10, base.Body.X + 10, "cry", 3000, null);
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}
