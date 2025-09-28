using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class FiveNormalThirdBoss : ABrain
	{
		private int m_attackTurn = 0;

		private int npcID = 5122;

		private int npcID2 = 5123;

		private int npcID3 = 5124;

		private PhysicalObj m_moive;

		private PhysicalObj m_front;

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
			if (this.m_attackTurn == 0)
			{
				this.BeatE();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.CallNpc();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 2)
			{
				base.Body.MoveTo(base.Game.Random.Next(400, 1300), 600, "fly", 0, "", 10, new LivingCallBack(this.AllAttack));
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 3)
			{
				this.BeatE();
				this.m_attackTurn++;
			}
			else
			{
				this.CallNpc();
				this.m_attackTurn = 0;
			}
		}

		private void BeatE()
		{
			Player player = base.Game.FindRandomPlayer();
			base.Body.MoveTo(base.Game.Random.Next(player.X - 50, player.X + 50), base.Game.Random.Next(player.Y - 100, player.Y - 100), "fly", 1000, "", 10, new LivingCallBack(this.BeatOneKill));
		}

		private void BeatOneKill()
		{
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			base.Body.PlayMovie("beatE", 3000, 0);
			base.Body.RangeAttacking(base.Body.X - 100, base.Body.X + 100, "cry", 5000, null);
		}

		private void CallNpc()
		{
			base.Body.MoveTo(base.Game.Random.Next(500, 1200), base.Game.Random.Next(400, 600), "fly", 1000, "", 10, new LivingCallBack(this.CallMohang));
		}

		private void CallMohang()
		{
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			base.Body.PlayMovie("beatB", 3300, 4000);
			base.Body.CallFuction(new LivingCallBack(this.GoCallMohang), 3500);
		}

		private void GoCallMohang()
		{
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			int x = base.Game.Random.Next(700, 1300);
			if (base.Game.GetLivedLivings().Count <= 1)
			{
				((SimpleBoss)base.Body).CreateChild(this.npcID, x, 680, -1, 1, 1);
			}
			if (base.Game.GetLivedLivings().Count > 1)
			{
				((SimpleBoss)base.Body).CreateChild(this.npcID, x, 680, -1, 100, 2);
			}
		}

		private void AllAttack()
		{
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			base.Body.PlayMovie("beatA", 3200, 0);
			base.Body.CallFuction(new LivingCallBack(this.In), 3400);
		}

		private void In()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			this.m_moive = ((PVEGame)base.Game).CreatePhysicalObj(1000, 400, "moive", "asset.game.4.heip", "out", 2, 0);
			base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 3000, null);
			base.Body.CallFuction(new LivingCallBack(this.Out), 2500);
		}

		private void Out()
		{
			this.m_moive.CanPenetrate = true;
			base.Game.RemovePhysicalObj(this.m_moive, true);
		}
	}
}
