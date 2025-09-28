using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class DCSM40006Boss : ABrain
	{
		private int m_attackTurn = 0;

		private int m_turn = 0;

		private PhysicalObj m_wallLeft = null;

		private PhysicalObj m_wallRight = null;

		private int IsEixt = 0;

		private int npcID = 1310;

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

		private static string[] KillPlayerChat = new string[]
		{
			"Mathias không kiểm soát tôi!",
			"Đây là thách thức số phận của tôi!",
			"Không! !Đây không phải là ý chí của tôi ..."
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

		private static string[] FrostChat = new string[]
		{
			"Hương vị này",
			"Hãy để bạn bình tĩnh",
			"Bạn đã giận dữ với tôi."
		};

		private static string[] WallChat = new string[]
		{
			"Chúa, cho tôi sức mạnh!",
			"Tuyệt vọng, xem tường thủy tinh của tôi!"
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
			bool flag = false;
			int num = 0;
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X > 620 && current.X < 1160)
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
				this.KillAttack(620, 1160);
			}
			else if (this.m_attackTurn == 0)
			{
				this.AllAttack();
				if (this.IsEixt == 1)
				{
					this.m_wallLeft.CanPenetrate = true;
					this.m_wallRight.CanPenetrate = true;
					base.Game.RemovePhysicalObj(this.m_wallLeft, true);
					base.Game.RemovePhysicalObj(this.m_wallRight, true);
					this.IsEixt = 0;
				}
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.ProtectingWall();
				this.m_attackTurn++;
			}
			else
			{
				this.CallBaby();
				this.m_attackTurn = 0;
			}
		}

		private void CallBaby()
		{
			base.Body.PlayMovie("renew", 3500, 0);
			base.Body.CallFuction(new LivingCallBack(this.CreateChild), 6000);
		}

		private void AllAttack()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			if (this.m_turn == 0)
			{
				int num = base.Game.Random.Next(0, DCSM40006Boss.AllAttackChat.Length);
				base.Body.Say(DCSM40006Boss.AllAttackChat[num], 1, 13000);
				base.Body.PlayMovie("beat1", 15000, 0);
				base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 17000, null);
				this.m_turn++;
			}
			else
			{
				int num = base.Game.Random.Next(0, DCSM40006Boss.AllAttackChat.Length);
				base.Body.Say(DCSM40006Boss.AllAttackChat[num], 1, 0);
				base.Body.PlayMovie("beat1", 1000, 0);
				base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 3000, null);
			}
		}

		private void KillAttack(int fx, int tx)
		{
			int num = base.Game.Random.Next(0, DCSM40006Boss.KillAttackChat.Length);
			if (this.m_turn == 0)
			{
				base.Body.CurrentDamagePlus = 10f;
				base.Body.Say(DCSM40006Boss.KillAttackChat[num], 1, 13000);
				base.Body.PlayMovie("beat1", 15000, 0);
				base.Body.RangeAttacking(fx, tx, "cry", 17000, null);
				this.m_turn++;
			}
			else
			{
				base.Body.CurrentDamagePlus = 10f;
				base.Body.Say(DCSM40006Boss.KillAttackChat[num], 1, 0);
				base.Body.PlayMovie("beat1", 2000, 0);
				base.Body.RangeAttacking(fx, tx, "cry", 4000, null);
			}
		}

		private void ProtectingWall()
		{
			if (this.IsEixt == 0)
			{
				this.m_wallLeft = ((PVEGame)base.Game).CreatePhysicalObj(base.Body.X - 65, 620, "wallLeft", "com.mapobject.asset.WaveAsset_01_left", "1", 1, 0);
				this.m_wallRight = ((PVEGame)base.Game).CreatePhysicalObj(base.Body.X + 65, 620, "wallLeft", "com.mapobject.asset.WaveAsset_01_right", "1", 1, 0);
				this.m_wallLeft.SetRect(-165, -169, 43, 330);
				this.m_wallRight.SetRect(128, -165, 41, 330);
				this.IsEixt = 1;
			}
			int num = base.Game.Random.Next(0, DCSM40006Boss.WallChat.Length);
			base.Body.Say(DCSM40006Boss.WallChat[num], 1, 0);
		}

		public void CreateChild()
		{
			base.Body.PlayMovie("renew", 100, 2000);
			((SimpleBoss)base.Body).CreateChild(this.npcID, 520, 530, 400, 6, -1);
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}
