using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class HardKingZhen : ABrain
	{
		private int attackingTurn = 1;

		private int Dander = 0;

		private int npcID = 1211;

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

		public List<SimpleNpc> orchins = new List<SimpleNpc>();

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
			else
			{
				if (this.attackingTurn == 1)
				{
					this.Healing();
					this.HalfAttack();
				}
				else if (this.attackingTurn == 2)
				{
					this.Healing();
					this.Summon();
				}
				else if (this.attackingTurn == 3)
				{
					this.Healing();
					this.Seal();
				}
				else if (this.attackingTurn == 4)
				{
					this.Healing();
					this.Angger();
				}
				else
				{
					this.GoOnAngger();
					this.attackingTurn = 0;
				}
				this.attackingTurn++;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		public void HalfAttack()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			int num = base.Game.Random.Next(0, HardKingZhen.SealChat.Length);
			base.Body.Say(HardKingZhen.AllAttackChat[num], 1, 500);
			base.Body.PlayMovie("beatB", 2500, 0);
			if (base.Body.Direction == 1)
			{
				base.Body.RangeAttacking(base.Body.X, base.Body.X + 1000, "cry", 3300, null);
			}
			else
			{
				base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X, "cry", 3300, null);
			}
		}

		public void Summon()
		{
			int num = base.Game.Random.Next(0, HardKingZhen.CallChat.Length);
			base.Body.Say(HardKingZhen.CallChat[num], 1, 0);
			base.Body.PlayMovie("beatA", 100, 0);
			base.Body.CallFuction(new LivingCallBack(this.CreateChild), 2500);
		}

		public void Seal()
		{
			int num = base.Game.Random.Next(0, HardKingZhen.SealChat.Length);
			((SimpleBoss)base.Body).Say(HardKingZhen.SealChat[num], 1, 0);
			Player player = base.Game.FindRandomPlayer();
			base.Body.PlayMovie("mantra", 2000, 2000);
			base.Body.Seal(player, 1, 3000);
		}

		public void Angger()
		{
			int num = base.Game.Random.Next(0, HardKingZhen.AngryChat.Length);
			base.Body.Say(HardKingZhen.AngryChat[num], 1, 0);
			base.Body.State = 1;
			this.Dander += 100;
			((SimpleBoss)base.Body).SetDander(this.Dander);
			if (base.Body.Direction == -1)
			{
				((SimpleBoss)base.Body).SetRelateDemagemRect(8, -252, 74, 50);
			}
			else
			{
				((SimpleBoss)base.Body).SetRelateDemagemRect(-82, -252, 74, 50);
			}
		}

		public void GoOnAngger()
		{
			if (base.Body.State == 1)
			{
				base.Body.CurrentDamagePlus = 1000f;
				base.Body.PlayMovie("beatC", 3500, 0);
				base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 5600, null);
				base.Body.Die(5600);
			}
			else
			{
				((SimpleBoss)base.Body).SetRelateDemagemRect(-41, -187, 83, 140);
				base.Body.PlayMovie("mantra", 0, 2000);
				List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
				foreach (Player current in allLivingPlayers)
				{
					current.AddEffect(new ContinueReduceBloodEffect(2, 50, current), 0);
				}
			}
		}

		public void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 10f;
			int num = base.Game.Random.Next(0, HardKingZhen.KillAttackChat.Length);
			((SimpleBoss)base.Body).Say(HardKingZhen.KillAttackChat[num], 1, 500);
			base.Body.PlayMovie("beatB", 2500, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 3300, null);
		}

		public void Healing()
		{
			base.Body.SyncAtTime = true;
			base.Body.AddBlood(5000);
			base.Body.Say("哈哈,我又充满力量了!", 1, 0);
		}

		public void CreateChild()
		{
			((SimpleBoss)base.Body).CreateChild(this.npcID, 520, 530, 400, 6, 1);
		}
	}
}
