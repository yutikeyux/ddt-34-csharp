using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class HardKingFirst : ABrain
	{
		private int m_attackTurn = 0;

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
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
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
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.PersonalAttack();
				this.m_attackTurn++;
			}
			else
			{
				this.Healing();
				this.m_attackTurn = 0;
			}
		}

		private void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 10f;
			int num = base.Game.Random.Next(0, HardKingFirst.KillAttackChat.Length);
			base.Body.Say(HardKingFirst.KillAttackChat[num], 1, 1000);
			base.Body.PlayMovie("beat", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 4000, null);
		}

		private void AllAttack()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			int num = base.Game.Random.Next(0, HardKingFirst.AllAttackChat.Length);
			base.Body.Say(HardKingFirst.AllAttackChat[num], 1, 0);
			base.Body.PlayMovie("beat", 1000, 0);
			base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 3000, null);
		}

		private void PersonalAttack()
		{
			int x = base.Game.Random.Next(800, 980);
			base.Body.MoveTo(x, base.Body.Y, "walk", 1000, "", ((SimpleBoss)base.Body).NpcInfo.speed, new LivingCallBack(this.NextAttack));
		}

		private void Healing()
		{
			int num = base.Game.Random.Next(0, HardKingFirst.AddBooldChat.Length);
			base.Body.Say(HardKingFirst.AddBooldChat[num], 1, 0);
			base.Body.SyncAtTime = true;
			base.Body.AddBlood(10000);
			base.Body.PlayMovie("renew", 1000, 4500);
		}

		private void NextAttack()
		{
			Player player = base.Game.FindRandomPlayer();
			if (player.X > base.Body.Y)
			{
				base.Body.ChangeDirection(1, 800);
			}
			else
			{
				base.Body.ChangeDirection(-1, 800);
			}
			base.Body.CurrentDamagePlus = 0.8f;
			int num = base.Game.Random.Next(0, HardKingFirst.ShootChat.Length);
			base.Body.Say(HardKingFirst.ShootChat[num], 1, 0);
			if (player != null)
			{
				int x = base.Game.Random.Next(player.X - 50, player.X + 50);
				if (base.Body.ShootPoint(x, player.Y, 61, 1000, 10000, 1, 1f, 2300))
				{
					base.Body.PlayMovie("beat2", 1500, 0);
				}
				if (base.Body.ShootPoint(x, player.Y, 61, 1000, 10000, 1, 1f, 4100))
				{
					base.Body.PlayMovie("beat2", 3300, 0);
				}
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}
