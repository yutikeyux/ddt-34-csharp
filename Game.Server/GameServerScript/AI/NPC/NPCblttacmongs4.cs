using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class NPCblttacmongs4 : ABrain
	{
		private int attackingTurn = 1;

		private int npcID = 560019;

		private int npcID2 = 560027;

		private int npcID1 = 3313;

		public List<SimpleNpc> orchins = new List<SimpleNpc>();

		private static string[] AllAttackChat = new string[]
		{
			"Tiếng gầm của mảnh hổ !!...",
			"这招酷吧，<br/>想学不？",
			"消失吧！！！<br/>卑微的灰尘！",
			"你们会为此付出代价的！ "
		};

		private static string[] ShootChat = new string[]
		{
			"你是在给我挠痒痒吗？",
			"我可不会像刚才那个废物一样被你打败！",
			"哎哟，你打的我好疼啊，<br/>哈哈哈哈！",
			"啧啧啧，就这样的攻击力！",
			"看到我是你们的荣幸！"
		};

		private static string[] CallChat = new string[]
		{
			"Vũ điệu<br/>săn bắn...."
		};

		private static string[] AngryChat = new string[]
		{
			"是你们逼我使出绝招的！"
		};

		private static string[] KillAttackChat = new string[]
		{
			"Muốn xem lợi hại của cổ họng của ta!"
		};

		private static string[] SealChat = new string[]
		{
			"异次元放逐！"
		};

		private static string[] KillPlayerChat = new string[]
		{
			"灭亡是你唯一的归宿！",
			"太不堪一击了！"
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
		}

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
			bool flag = false;
			int num = 0;
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X > 592 && current.X < 872)
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
				this.KillAttack(592, 872);
			}
			else if (!flag)
			{
				if (this.attackingTurn == 1)
				{
					this.HalfAttack();
				}
				else if (this.attackingTurn == 2)
				{
					this.PersonalAttack();
				}
				else if (this.attackingTurn == 3)
				{
					this.Summon();
				}
				else if (this.attackingTurn == 4)
				{
					this.PersonalAttackDame();
				}
				else
				{
					this.SummonNpc();
					this.attackingTurn = 1;
				}
				this.attackingTurn++;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
			base.Game.RemoveLiving(this.npcID);
		}

		public void HalfAttack()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			int num = base.Game.Random.Next(0, NPCblttacmongs4.SealChat.Length);
			base.Body.Say(NPCblttacmongs4.AllAttackChat[num], 1, 500);
			base.Body.PlayMovie("beatC", 2500, 0);
			base.Body.RangeAttacking(base.Body.X - 2000, base.Body.Y + 2000, "cry", 3000, null);
		}

		private void PersonalAttackDame()
		{
			Player player = base.Game.FindRandomPlayer();
			Player player2 = base.Game.FindRandomPlayer();
			Player player3 = base.Game.FindRandomPlayer();
			if (player.X > base.Body.Y)
			{
				base.Body.ChangeDirection(1, 800);
			}
			else
			{
				base.Body.ChangeDirection(-1, 800);
			}
			if (player != null)
			{
				int num = base.Game.Random.Next(player.X, player.X);
				if (base.Body.ShootPoint(player.X, player.Y, 55, 1000, 10000, 1, 1.5f, 2550))
				{
					base.Body.PlayMovie("beatB", 1700, 0);
				}
			}
		}

		private void PersonalAttack()
		{
			int x = base.Game.Random.Next(700, 800);
			base.Body.MoveTo(x, base.Body.Y, "walk", 1000, "", 3, new LivingCallBack(this.NextAttack));
		}

		private void NextAttack()
		{
			Player player = base.Game.FindRandomPlayer();
			Player player2 = base.Game.FindRandomPlayer();
			Player player3 = base.Game.FindRandomPlayer();
			if (player.X > base.Body.Y)
			{
				base.Body.ChangeDirection(1, 800);
			}
			else
			{
				base.Body.ChangeDirection(-1, 800);
			}
			if (player != null)
			{
				int num = base.Game.Random.Next(player.X, player.X);
				if (base.Body.ShootPoint(player.X, player.Y, 54, 1000, 10000, 1, 1.5f, 2550))
				{
					base.Body.PlayMovie("beatA", 1700, 0);
				}
				if (base.Body.ShootPoint(player2.X, player2.Y, 54, 1000, 10000, 1, 1.5f, 4550))
				{
					base.Body.PlayMovie("beatA", 3700, 0);
				}
				if (base.Body.ShootPoint(player3.X, player3.Y, 54, 1000, 10000, 1, 1.5f, 6550))
				{
					base.Body.PlayMovie("beatA", 5700, 0);
				}
			}
		}

		public void Summon()
		{
			int num = base.Game.Random.Next(0, NPCblttacmongs4.CallChat.Length);
			base.Body.Say(NPCblttacmongs4.CallChat[num], 1, 0);
			base.Body.PlayMovie("callA", 100, 0);
			base.Body.CallFuction(new LivingCallBack(this.CreateChild2), 2500);
		}

		public void SummonNpc()
		{
			int num = base.Game.Random.Next(0, NPCblttacmongs4.CallChat.Length);
			base.Body.Say(NPCblttacmongs4.CallChat[num], 1, 0);
			base.Body.PlayMovie("callB", 100, 0);
			base.Body.CallFuction(new LivingCallBack(this.CreateChild), 2500);
		}

		public void KillAttack(int fx, int mx)
		{
			base.Body.CurrentDamagePlus = 10f;
			int num = base.Game.Random.Next(0, NPCblttacmongs4.KillAttackChat.Length);
			((SimpleBoss)base.Body).Say(NPCblttacmongs4.KillAttackChat[num], 1, 500);
			base.Body.PlayMovie("beatC", 2500, 0);
			base.Body.RangeAttacking(fx, mx, "cry", 3300, null);
		}

		public void CreateChild()
		{
			((SimpleBoss)base.Body).CreateChild(this.npcID, 520, 395, 50, 6, -1);
		}

		public void CreateChild2()
		{
			int x = base.Game.Random.Next(100, 400);
			int disToSecond = base.Game.Random.Next(150, 300);
			((SimpleBoss)base.Body).CreateChild(this.npcID2, x, 395, disToSecond, 6, -1);
		}
	}
}
