using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class FourTerrorGunNpc : ABrain
	{
		public int attackingTurn = 1;

		private int npcID = 4103;

		private static string[] AllAttackChat = new string[]
		{
			"看我的绝技！",
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
			"来啊，<br/>让他们尝尝炸弹的厉害！"
		};

		private static string[] AngryChat = new string[]
		{
			"是你们逼我使出绝招的！"
		};

		private static string[] KillAttackChat = new string[]
		{
			"你来找死吗？"
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
				if (current.IsLiving && current.X > 400 && current.X < 1600)
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
				this.KillAttack(400, 1600);
			}
			else if (!flag)
			{
				((PVEGame)base.Game).SendGameObjectFocus(1, "door", 1000, 0);
				if (this.attackingTurn == 1)
				{
					this.BestA();
				}
				else if (this.attackingTurn == 2)
				{
					this.BestC();
				}
				else
				{
					this.BestB();
					this.attackingTurn = 0;
				}
				this.attackingTurn++;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		public void BestB()
		{
			base.Body.PlayMovie("beatB", 0, 3000);
			List<SimpleNpc> list = new List<SimpleNpc>();
			foreach (Living current in list)
			{
				if (current is SimpleNpc)
				{
					list.Add(current as SimpleNpc);
					current.AddEffect(new ContinueReduceBloodEffect(2, 500, current), 0);
				}
			}
		}

		public void BestA()
		{
			base.Body.PlayMovie("beatA", 1000, 0);
		}

		public void BestC()
		{
			base.Body.PlayMovie("beatC", 1000, 0);
			int num = base.Game.Random.Next(400, 1680);
			base.Body.CallFuction(new LivingCallBack(this.CreateChild), 2500);
		}

		public void CreateChild()
		{
			int x = base.Game.Random.Next(470, 880);
			((SimpleBoss)base.Body).CreateChild(this.npcID, x, 700, 2, 400, -1);
		}

		private void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 10f;
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			base.Body.PlayMovie("beatB", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 4000, null);
		}
	}
}
