using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class LongClickCannon : ABrain
	{
		private int m_attackTurn = 0;

		private int m_turn = 0;

		private PhysicalObj m_wallLeft = null;

		private PhysicalObj m_wallRight = null;

		private int IsEixt = 0;

		private static string[] AllAttackChat = new string[]
		{
			"要地震喽！！<br/>各位请扶好哦",
			"把你武器震下来！",
			"看你们能还经得起几下！！"
		};

		private static string[] ShootChat = new string[]
		{
			"让你知道什么叫百发百中！",
			"送你一个球~你可要接好啦",
			"你们这群无知的低等庶民"
		};

		private static string[] ShootedChat = new string[]
		{
			"哎呀~~你们为什么要攻击我？<br/>我在干什么？",
			"噢~~好痛!我为什么要战斗？<br/>我必须战斗…"
		};

		private static string[] KillPlayerChat = new string[]
		{
			"马迪亚斯不要再控制我！",
			"这就是挑战我的下场！",
			"不！！这不是我的意愿… "
		};

		private static string[] AddBooldChat = new string[]
		{
			"扭啊扭~<br/>扭啊扭~~",
			"哈利路亚~<br/>路亚路亚~~",
			"呀呀呀，<br/>好舒服啊！"
		};

		private static string[] KillAttackChat = new string[]
		{
			"君临天下！！"
		};

		private static string[] FrostChat = new string[]
		{
			"来尝尝这个吧",
			"让你冷静一下",
			"你们激怒了我"
		};

		private static string[] WallChat = new string[]
		{
			"神啊，赐予我力量吧！",
			"绝望吧，看我的水晶防护墙！"
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
			else if (this.m_attackTurn == 0)
			{
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.ToA();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 2)
			{
				this.BeatA();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 3)
			{
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 4)
			{
				this.m_attackTurn++;
			}
			else
			{
				this.m_attackTurn = 0;
			}
		}

		private void KillAttack(int fx, int tx)
		{
			int num = base.Game.Random.Next(0, LongClickCannon.KillAttackChat.Length);
			if (this.m_turn == 0)
			{
				base.Body.CurrentDamagePlus = 10f;
				base.Body.Say(LongClickCannon.KillAttackChat[num], 1, 13000);
				base.Body.PlayMovie("beat1", 15000, 0);
				base.Body.RangeAttacking(fx, tx, "cry", 17000, null);
				this.m_turn++;
			}
			else
			{
				base.Body.CurrentDamagePlus = 10f;
				base.Body.Say(LongClickCannon.KillAttackChat[num], 1, 0);
				base.Body.PlayMovie("beat1", 2000, 0);
				base.Body.RangeAttacking(fx, tx, "cry", 4000, null);
			}
		}

		private void ToA()
		{
			base.Body.PlayMovie("toA", 1000, 4000);
		}

		private void BeatA()
		{
			base.Body.PlayMovie("beatA", 1000, 0);
			base.Body.RangeAttacking(base.Body.X + 9000, base.Body.X - 9000, "cry", 3000, null);
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}
