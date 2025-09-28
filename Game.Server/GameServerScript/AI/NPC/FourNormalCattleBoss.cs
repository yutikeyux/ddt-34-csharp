using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class FourNormalCattleBoss : ABrain
	{
		private int m_attackTurn = 0;

		private PhysicalObj m_moive;

		private int Dander = 0;

		private int npcID = 4107;

		private static string[] AllAttackChat = new string[]
		{
			"你们这是自寻死路！",
			"你惹毛我了!",
			"超级无敌大地震……<br/>震……震…… "
		};

		private static string[] ShootChat = new string[]
		{
			"砸你家玻璃。",
			"看哥打的可比你们准多了"
		};

		private static string[] KillPlayerChat = new string[]
		{
			"送你回老家！",
			"就凭你还妄想能够打败我？"
		};

		private static string[] CallChat = new string[]
		{
			"卫兵！ <br/>卫兵！！ ",
			"啵咕们！！<br/>给我些帮助！"
		};

		private static string[] ShootedChat = new string[]
		{
			"哎呦！很痛…",
			"我还顶的住…"
		};

		private static string[] JumpChat = new string[]
		{
			"为了你们的胜利，<br/>向我开炮！",
			"你再往前半步我就把你给杀了！",
			"高！<br/>实在是高！"
		};

		private static string[] KillAttackChat = new string[]
		{
			"超级肉弹！！"
		};

		private List<SimpleNpc> m_child = new List<SimpleNpc>();

		public List<SimpleNpc> Child
		{
			get
			{
				return this.m_child;
			}
		}

		public int CurrentLivingNpcNum
		{
			get
			{
				int num = 0;
				foreach (Physics current in this.Child)
				{
					if (!current.IsLiving)
					{
						num++;
					}
				}
				return this.Child.Count - num;
			}
		}

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
			else if (this.m_attackTurn == 1)
			{
				if (this.CurrentLivingNpcNum > 1)
				{
					base.Body.AddBlood(15000);
				}
				this.AllAttack2();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 2)
			{
				this.PersonalAttack();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 3)
			{
				this.Jump();
				this.m_attackTurn++;
			}
			else
			{
				this.Physicallyinjured();
				this.m_attackTurn = 0;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void Star()
		{
			int num = base.Game.Random.Next(0, FourNormalCattleBoss.CallChat.Length);
			base.Body.Say(FourNormalCattleBoss.CallChat[num], 1, 500);
			this.m_moive = ((PVEGame)base.Game).Createlayer(base.Body.X, base.Body.Y - 150, "moive", "game.crazytank.assetmap.Buff_powup", "", 1, 0);
			base.Body.CallFuction(new LivingCallBack(this.CreateChild), 2000);
		}

		private void Physicallboss()
		{
			this.m_moive = ((PVEGame)base.Game).Createlayer(base.Body.X, base.Body.Y - 150, "moive", "game.crazytank.assetmap.Buff_powup", "", 1, 0);
		}

		public void CreateChild()
		{
		}

		private void Physicallyinjured()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			base.Body.PlayMovie("AtoB", 1000, 0);
		}

		private void AllAttack()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			base.Body.PlayMovie("beatA", 1000, 0);
			base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 3000, null);
		}

		private void AllAttack2()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			base.Body.PlayMovie("beatB", 2000, 0);
			base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 3000, null);
		}

		private void Healing()
		{
			base.Body.PlayMovie("beatC", 500, 0);
			base.Body.AddBlood(5000);
		}

		public void Jump()
		{
			base.Body.PlayMovie("jump", 1000, 6000);
			Player player = base.Game.FindRandomPlayer();
			base.Body.JumpToSpeed(player.X, base.Body.Y - 1000, "", 2500, 1, 10, new LivingCallBack(this.Jump2));
		}

		public void Jump2()
		{
			base.Body.PlayMovie("fall", 0, 0);
			base.Body.RangeAttacking(base.Body.X - 2000, base.Body.X + 2000, "cry", 0, null);
		}

		private void PersonalAttack()
		{
			base.Body.MoveTo(base.Body.X - 100, base.Body.Y, "walk", 1000, "", 6, new LivingCallBack(this.AllAttack));
		}

		public void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 10f;
			int num = base.Game.Random.Next(0, FourNormalCattleBoss.KillAttackChat.Length);
			((SimpleBoss)base.Body).Say(FourNormalCattleBoss.KillAttackChat[num], 1, 500);
			base.Body.PlayMovie("beatB", 2500, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 3300, null);
		}
	}
}
