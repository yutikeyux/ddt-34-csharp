using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class NormalKingSecond : ABrain
	{
		private int m_attackTurn = 0;

		private int m_turn = 0;

		private PhysicalObj m_wallLeft = null;

		private PhysicalObj m_wallRight = null;

		private int IsEixt = 0;

		private int npcID = 1110;

		private static string[] AllAttackChat = new string[]
		{
			"Tôi muốn có một trận động đất !! <br/> Xin hãy giúp tôi",
			"Sốc vũ khí của bạn!",
			"Hãy nhìn xem bạn vẫn đủ khả năng bao nhiêu !!"
		};

		private static string[] ShootChat = new string[]
		{
			"Hãy cho bạn biết một trăm trăm là gì!",
			"Đưa cho bạn một quả bóng ~ Bạn phải nhặt nó lên",
			"Bạn là một nhóm những người cấp thấp không biết gì"
		};

		private static string[] ShootedChat = new string[]
		{
			"Ồ ~ ~ Tại sao bạn tấn công tôi? <br/> Tôi đang làm gì vậy?",
			"Này ~ ~ Đau quá! Tại sao tôi phải chiến đấu? <br/> Tôi phải chiến đấu ..."
		};

		private static string[] KillPlayerChat = new string[]
		{
			"Madias không còn kiểm soát tôi nữa!",
			"Đây là thử thách cuối cùng của tôi!",
			"Không !! Đây không phải là mong muốn của tôi ..."
		};

		private static string[] AddBooldChat = new string[]
		{
			"Xoắn và xoắn ~ <br/> xoắn ~ ~",
			"GunnyV2 ~ Thật tuyệt phải không nào ^^! ~ ~",
			"Yeah, thật thoải mái!"
		};

		private static string[] KillAttackChat = new string[]
		{
			"Jun Lin là cả thế giới !!"
		};

		private static string[] FrostChat = new string[]
		{
			"Hãy đến và thử cái này",
			"Hãy để bạn bình tĩnh",
			"Bạn đã chọc giận tôi"
		};

		private static string[] WallChat = new string[]
		{
			"Chúa ơi, cho con sức mạnh!",
			"Tuyệt vọng, nhìn vào bức tường bảo vệ pha lê của tôi!"
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
				if (current.IsLiving && current.X > 390 && current.X < 1110)
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
				this.KillAttack(390, 1110);
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
				this.FrostAttack();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 2)
			{
				this.ProtectingWall();
				this.m_attackTurn++;
			}
			else
			{
				this.CriticalStrikes();
				this.m_attackTurn = 0;
			}
		}

		private void CriticalStrikes()
		{
			Player frostPlayerRadom = base.Game.GetFrostPlayerRadom();
			List<Player> allFightPlayers = base.Game.GetAllFightPlayers();
			List<Player> list = new List<Player>();
			foreach (Player current in allFightPlayers)
			{
				if (!current.IsFrost)
				{
					list.Add(current);
				}
			}
			((SimpleBoss)base.Body).CurrentDamagePlus = 30f;
			if (list.Count != allFightPlayers.Count)
			{
				if (list.Count != 0)
				{
					base.Body.PlayMovie("beat1", 0, 0);
					base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "beat1", 1500, list);
				}
				else
				{
					base.Body.PlayMovie("beat1", 0, 0);
					base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "beat1", 1500, null);
				}
			}
			else
			{
				base.Body.Say("Đưa chúng cho tôi, và dạy kẻ thù!", 1, 3300);
				base.Body.PlayMovie("renew", 3500, 0);
				base.Body.CallFuction(new LivingCallBack(this.CreateChild), 6000);
			}
		}

		private void FrostAttack()
		{
			int x = base.Game.Random.Next(850, 920);
			base.Body.MoveTo(x, base.Body.Y, "walk", 0, "", ((SimpleBoss)base.Body).NpcInfo.speed, new LivingCallBack(this.NextAttack));
		}

		private void AllAttack()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			if (this.m_turn == 0)
			{
				int num = base.Game.Random.Next(0, NormalKingSecond.AllAttackChat.Length);
				base.Body.Say(NormalKingSecond.AllAttackChat[num], 1, 13000);
				base.Body.PlayMovie("beat1", 15000, 0);
				base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 17000, null);
				this.m_turn++;
			}
			else
			{
				int num = base.Game.Random.Next(0, NormalKingSecond.AllAttackChat.Length);
				base.Body.Say(NormalKingSecond.AllAttackChat[num], 1, 0);
				base.Body.PlayMovie("beat1", 1000, 0);
				base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 3000, null);
			}
		}

		private void KillAttack(int fx, int tx)
		{
			int num = base.Game.Random.Next(0, NormalKingSecond.KillAttackChat.Length);
			if (this.m_turn == 0)
			{
				base.Body.CurrentDamagePlus = 10f;
				base.Body.Say(NormalKingSecond.KillAttackChat[num], 1, 13000);
				base.Body.PlayMovie("beat1", 15000, 0);
				base.Body.RangeAttacking(fx, tx, "cry", 17000, null);
				this.m_turn++;
			}
			else
			{
				base.Body.CurrentDamagePlus = 10f;
				base.Body.Say(NormalKingSecond.KillAttackChat[num], 1, 0);
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
			int num = base.Game.Random.Next(0, NormalKingSecond.WallChat.Length);
			base.Body.Say(NormalKingSecond.WallChat[num], 1, 0);
		}

		public void CreateChild()
		{
			base.Body.PlayMovie("renew", 100, 2000);
			((SimpleBoss)base.Body).CreateChild(this.npcID, 520, 530, 400, 6, 1);
		}

		private void NextAttack()
		{
			int num = base.Game.Random.Next(1, 2);
			for (int i = 0; i < num; i++)
			{
				Player player = base.Game.FindRandomPlayer();
				int num2 = base.Game.Random.Next(0, NormalKingSecond.ShootChat.Length);
				base.Body.Say(NormalKingSecond.ShootChat[num2], 1, 0);
				if (player.X > base.Body.X)
				{
					base.Body.ChangeDirection(1, 500);
				}
				else
				{
					base.Body.ChangeDirection(-1, 500);
				}
				if (player != null && !player.IsFrost)
				{
					if (base.Body.ShootPoint(player.X, player.Y, ((SimpleBoss)base.Body).NpcInfo.CurrentBallId, 1000, 10000, 1, 1.5f, 2000))
					{
						base.Body.PlayMovie("beat2", 1500, 0);
					}
				}
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}
