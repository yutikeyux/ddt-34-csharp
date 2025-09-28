using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class ThirteenNormalNioBoss : ABrain
	{
		private int m_attackTurn = 0;

		private static string[] AllAttackChat = new string[]
		{
			"Địa chấn ! <br/> Thật đáng sợ",
			"Đặt hết vũ khí xuống !",
			"Nhìn bạn cũng có thể có thể chịu được một số ít!"
		};

		private static string[] ShootChat = new string[]
		{
			"Cảm nhận sức mạnh của ta !",
			"Gửi cho cậu nhưng viên kheo đau khổ",
			"Cho các ngươi nén mùi lợi hại "
		};

		private static string[] ShootedChat = new string[]
		{
			"哎呀~~你们为什么要攻击我？<br/>我在干什么？",
			"噢~~好痛!我为什么要战斗？<br/>我必须战斗…"
		};

		private static string[] AddBooldChat = new string[]
		{
			"Xoay xoay ~ <br/> xoay ah xoay ~ ~",
			"Hallelujah ~ <br/> Luyaluya ~ ~",
			"Kì diệu quá! Đã đem đến cho ta sức mạnh siêu phàm !"
		};

		private static string[] KillAttackChat = new string[]
		{
			"君临天下！！"
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
				this.NextAttack();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.Healing();
				this.m_attackTurn++;
			}
			else
			{
				this.AllAttack();
				this.m_attackTurn = 0;
			}
		}

		private void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 10f;
			int num = base.Game.Random.Next(0, ThirteenNormalNioBoss.KillAttackChat.Length);
			base.Body.Say(ThirteenNormalNioBoss.KillAttackChat[num], 1, 1000);
			base.Body.PlayMovie("beatA", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 4000, null);
		}

		private void AllAttack()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			int num = base.Game.Random.Next(0, ThirteenNormalNioBoss.AllAttackChat.Length);
			base.Body.Say(ThirteenNormalNioBoss.AllAttackChat[num], 1, 0);
			base.Body.PlayMovie("beat", 1000, 0);
			base.Body.RangeAttacking(base.Body.X - 4000, base.Body.X + 4000, "cry", 3000, null);
		}

		private void Healing()
		{
			int num = base.Game.Random.Next(0, ThirteenNormalNioBoss.AddBooldChat.Length);
			base.Body.Say(ThirteenNormalNioBoss.AddBooldChat[num], 1, 0);
			base.Body.SyncAtTime = true;
			base.Body.AddBlood(7500);
			base.Body.PlayMovie("renew", 1000, 4500);
		}

		private void NextAttack()
		{
			Player player = base.Game.FindRandomPlayer();
			if (player.X > base.Body.Y)
			{
				base.Body.ChangeDirection(1, 800);
			}
			else
			{
				base.Body.ChangeDirection(-1, 800);
			}
			base.Body.CurrentDamagePlus = 0.8f;
			int num = base.Game.Random.Next(0, ThirteenNormalNioBoss.ShootChat.Length);
			base.Body.Say(ThirteenNormalNioBoss.ShootChat[num], 1, 0);
			if (player != null)
			{
				int x = base.Game.Random.Next(player.X - 30, player.X + 30);
				if (base.Body.ShootPoint(x, player.Y, 61, 1400, 10000, 1, 1.5f, 2300))
				{
					base.Body.PlayMovie("beat2", 1500, 0);
				}
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}
