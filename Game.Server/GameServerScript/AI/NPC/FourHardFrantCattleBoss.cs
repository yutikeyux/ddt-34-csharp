using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class FourHardFrantCattleBoss : ABrain
	{
		public int attackingTurn = 1;

		public int orchinIndex = 1;

		public int currentCount = 0;

		public int Dander = 0;

		public List<SimpleNpc> orchins = new List<SimpleNpc>();

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
			else if (!flag)
			{
				if (this.attackingTurn == 1)
				{
					this.Jump();
				}
				else if (this.attackingTurn == 2)
				{
					this.PersonalAttack();
				}
				else
				{
					this.attackingTurn = 0;
				}
				this.attackingTurn++;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		public void Jump()
		{
			base.Body.PlayMovie("jump", 1000, 6000);
			Player player = base.Game.FindRandomPlayer();
			base.Body.JumpToSpeed(player.X, base.Body.Y - 1000, "", 2500, 1, 10, new LivingCallBack(this.Fall));
		}

		public void Fall()
		{
			base.Body.PlayMovie("fall", 0, 0);
			base.Body.RangeAttacking(base.Body.X - 2000, base.Body.X + 2000, "cry", 0, null);
		}

		private void PersonalAttack()
		{
			if (base.Body.X > base.Game.Map.Info.ForegroundWidth / 2)
			{
				base.Body.MoveTo(1, base.Body.Y, "walk", 1000, "", 24, new LivingCallBack(this.FallTo));
			}
			if (base.Body.X < base.Game.Map.Info.ForegroundWidth / 2)
			{
				base.Body.MoveTo(1600, base.Body.Y, "walk", 1000, "", 24, new LivingCallBack(this.FallTo));
			}
			base.Body.RangeAttacking(base.Body.X - 2000, base.Body.X + 2000, "cry", 1200, null);
		}

		private void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 10f;
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			int num = base.Game.Random.Next(0, FourHardFrantCattleBoss.KillAttackChat.Length);
			base.Body.Say(FourHardFrantCattleBoss.KillAttackChat[num], 1, 1000);
			base.Body.PlayMovie("beat", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 4000, null);
		}

		public void AllAttack()
		{
			base.Body.RangeAttacking(base.Body.X - 2000, base.Body.X + 2000, "cry", 0, null);
		}

		public void FallTo()
		{
			if (base.Body.X > 700)
			{
				base.Body.ChangeDirection(-1, 0);
				base.Body.FallFrom(1599, 900, "fallB", 10, 10, 10);
				base.Body.PlayMovie("fallB", 20, 0);
			}
			else
			{
				base.Body.ChangeDirection(1, 0);
				base.Body.FallFrom(1, 900, "fallB", 10, 10, 10);
				base.Body.PlayMovie("fallB", 20, 0);
			}
		}
	}
}
