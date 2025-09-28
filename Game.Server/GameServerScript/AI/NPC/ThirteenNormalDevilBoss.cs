using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class ThirteenNormalDevilBoss : ABrain
	{
		private int m_attackTurn = 0;

		private int npcID = 5322;

		private int npcID2 = 5323;

		private PhysicalObj m_moive;

		private PhysicalObj m_front;

		private PhysicalObj moive;

		private PhysicalObj front;

		private PhysicalObj wallLeft = null;

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
			else if (base.Body.State != 1)
			{
				if (this.m_attackTurn == 0)
				{
					this.CallBell();
					this.m_attackTurn++;
				}
				else if (this.m_attackTurn == 1)
				{
					this.BeatE();
					this.m_attackTurn++;
				}
				else
				{
					this.BlackAttack();
					this.m_attackTurn = 0;
				}
			}
		}

		private void CallBell()
		{
			base.Body.PlayMovie("beatB", 3300, 0);
		}

		private void BlackAttack()
		{
			base.Body.PlayMovie("beatA", 0, 3000);
			this.moive = ((PVEGame)base.Game).Createlayer(base.Body.X, base.Body.Y, "top", "asset.game.4.heip", "out", 2, 1);
			base.Body.CallFuction(new LivingCallBack(this.AllAttack), 2500);
		}

		private void AllAttack()
		{
			base.Body.CurrentDamagePlus = 2.5f;
			base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 500, null);
			base.Body.CallFuction(new LivingCallBack(this.RemoveMove), 0);
		}

		private void RemoveMove()
		{
			if (this.moive != null)
			{
				base.Game.RemovePhysicalObj(this.moive, true);
				this.moive = null;
			}
			if (this.front != null)
			{
				base.Game.RemovePhysicalObj(this.front, true);
				this.front = null;
			}
		}

		private void BeatE()
		{
			Player player = base.Game.FindRandomPlayer();
			int x = player.X;
			int y = player.Y - 95;
			base.Body.MoveTo(x, y, "fly", 1000, "", 16, new LivingCallBack(this.PersonalAttack));
		}

		private void PersonalAttack()
		{
			base.Body.CurrentDamagePlus = 5.5f;
			base.Body.PlayMovie("beatE", 3500, 0);
			Player player = base.Game.FindRandomPlayer();
			base.Body.RangeAttacking(player.X - 50, player.X + 50, "cry", 5000, null);
			base.Body.CallFuction(new LivingCallBack(this.Run), 5000);
		}

		private void Run()
		{
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			base.Body.MoveTo(850, 770, "fly", 1000, "", 16, new LivingCallBack(this.ChangeDirection));
		}

		private void ChangeDirection()
		{
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
		}

		private void CallHopquaidi()
		{
			base.Body.CurrentDamagePlus = 0.8f;
			int num = base.Game.Random.Next(0, ThirteenNormalDevilBoss.ShootChat.Length);
			base.Body.Say(ThirteenNormalDevilBoss.ShootChat[num], 1, 0);
			int num2 = base.Game.Random.Next(400, 1300);
			Player player = base.Game.FindRandomPlayer();
			int x = player.X;
			int y = player.Y - 150;
			base.Body.MoveTo(x, y, "fly", 3500, "", 16, new LivingCallBack(this.CallHopquaidi2));
			base.Body.PlayMovie("beatD", 3300, 2000);
		}

		private void BeatDame()
		{
			Player player = base.Game.FindRandomPlayer();
			int x = player.X;
			int y = player.Y - 150;
			base.Body.MoveTo(x, y, "fly", 3500, "", 16, new LivingCallBack(this.GoBeatDame));
		}

		private void GoBeatDame()
		{
			int num = base.Game.Random.Next(0, ThirteenNormalDevilBoss.AddBooldChat.Length);
			base.Body.Say(ThirteenNormalDevilBoss.AddBooldChat[num], 1, 0);
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			base.Body.SyncAtTime = true;
			base.Body.PlayMovie("beatE", 3300, 5000);
			base.Body.RangeAttacking(base.Body.X - 100, base.Body.X + 100, "cry", 5000, null);
		}

		private void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 10f;
			int num = base.Game.Random.Next(0, ThirteenNormalDevilBoss.KillAttackChat.Length);
			base.Body.Say(ThirteenNormalDevilBoss.KillAttackChat[num], 1, 1000);
			base.Body.PlayMovie("beat", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 4000, null);
		}

		private void CallHopquaidi2()
		{
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			int x = base.Game.Random.Next(700, 1300);
			base.Body.SetXY(base.Body.X, 600);
			((SimpleBoss)base.Body).CreateChild(this.npcID2, x, 900, 1000, 1, -1);
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}
