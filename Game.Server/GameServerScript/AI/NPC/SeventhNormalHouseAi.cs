using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class SeventhNormalHouseAi : ABrain
	{
		private int m_attackTurn = 0;

		private PhysicalObj moive;

		private List<SimpleNpc> Children = new List<SimpleNpc>();

		private int npcID2 = 7122;

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
				if (current.IsLiving && current.X < 650)
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
				this.KillAttack(0, 650);
			}
			else if (this.m_attackTurn == 0)
			{
				this.Summon();
				this.m_attackTurn++;
			}
			else
			{
				this.m_attackTurn = 0;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void KillAttack(int fx, int tx)
		{
			int num = base.Game.Random.Next(0, SeventhNormalHouseAi.KillAttackChat.Length);
			base.Body.Say(SeventhNormalHouseAi.KillAttackChat[num], 1, 1000);
			base.Body.CurrentDamagePlus = 10f;
			base.Body.RangeAttacking(fx, tx, "cry", 5000, null);
			base.Body.CallFuction(new LivingCallBack(this.GoMovie), 4000);
		}

		private void GoMovie()
		{
			List<Player> allFightPlayers = base.Game.GetAllFightPlayers();
			foreach (Player current in allFightPlayers)
			{
				if (current.IsLiving && current.X < 700)
				{
					this.moive = ((PVEGame)base.Game).Createlayer(current.X, current.Y, "moive", "asset.game.seven.jinquhd", "out", 1, 0);
				}
			}
		}

		private void Summon()
		{
			base.Body.CallFuction(new LivingCallBack(this.CreateChild), 4000);
		}

		public void CreateChild()
		{
			((SimpleBoss)base.Body).CreateChild(this.npcID2, 880, 900, 20, 6, 1);
		}
	}
}
