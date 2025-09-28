using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class FiveNormalSecondBoss : ABrain
	{
		private int m_attackTurn = 0;

		private int m_turn = 0;

		private PhysicalObj m_moive;

		private PhysicalObj m_wallLeft = null;

		private PhysicalObj m_wallRight = null;

		private int IsEixt = 0;

		private PhysicalObj m_NPC;

		private PhysicalObj n_NPC;

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
				if (current.IsLiving && current.X > 1200 && current.X < 1984)
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
				this.KillAttack(1200, 1984);
			}
			else if (this.m_attackTurn == 0)
			{
				this.m_NPC = ((PVEGame)base.Game).Createlayer(1550, 650, "NPC", "game.living.Living154", "stand", 1, 0);
				this.n_NPC = ((PVEGame)base.Game).Createlayer(1367, 845, "NPC", "game.living.Living147", "stand", 1, 0);
				this.Goblinhunghan();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.BeatA();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 2)
			{
				this.Goblinxaotra();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 3)
			{
				this.BeatB();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 4)
			{
				this.Goblinhunghan();
				this.m_attackTurn++;
			}
			else
			{
				this.BeatD();
				this.m_attackTurn = 0;
			}
		}

		private void BeatD()
		{
			base.Body.PlayMovie("beatC", 1000, 1000);
			base.Body.CallFuction(new LivingCallBack(this.NpcDame2), 3000);
		}

		private void NpcDame2()
		{
			if (this.n_NPC != null)
			{
				base.Game.RemovePhysicalObj(this.n_NPC, true);
				this.n_NPC = null;
			}
			this.n_NPC = ((PVEGame)base.Game).Createlayer(1367, 845, "NPC", "game.living.Living147", "beatA", 1, 0);
			((PVEGame)base.Game).SendGameFocus(this.n_NPC, 0, 4000);
		}

		private void KillAttack(int fx, int tx)
		{
			int num = base.Game.Random.Next(0, FiveNormalSecondBoss.KillAttackChat.Length);
			if (this.m_turn == 0)
			{
				base.Body.CurrentDamagePlus = 10f;
				base.Body.Say(FiveNormalSecondBoss.KillAttackChat[num], 1, 13000);
				base.Body.PlayMovie("beat1", 15000, 0);
				base.Body.RangeAttacking(fx, tx, "cry", 17000, null);
				this.m_turn++;
			}
			else
			{
				base.Body.CurrentDamagePlus = 10f;
				base.Body.Say(FiveNormalSecondBoss.KillAttackChat[num], 1, 0);
				base.Body.PlayMovie("beat1", 2000, 0);
				base.Body.RangeAttacking(fx, tx, "cry", 4000, null);
			}
		}

		private void Goblinhunghan()
		{
			int num = base.Game.Random.Next(0, FiveNormalSecondBoss.AllAttackChat.Length);
			base.Body.Say(FiveNormalSecondBoss.AllAttackChat[num], 1, 0);
			base.Body.PlayMovie("beatD", 1000, 1000);
		}

		private void BeatB()
		{
			base.Body.PlayMovie("beatB", 1000, 1000);
			base.Body.CallFuction(new LivingCallBack(this.NpcDame), 3000);
		}

		private void NpcDame()
		{
			if (this.m_NPC != null)
			{
				base.Game.RemovePhysicalObj(this.m_NPC, true);
				this.m_NPC = null;
			}
			this.m_NPC = ((PVEGame)base.Game).Createlayer(1550, 650, "NPC", "game.living.Living154", "beatA", 1, 0);
			base.Body.CallFuction(new LivingCallBack(this.DameBlood), 4000);
		}

		private void DameBlood()
		{
			base.Body.CallFuction(new LivingCallBack(this.GoAtck), 1000);
		}

		private void GoAtck()
		{
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				int num = base.Game.Random.Next(321, 515);
				current.AddBlood(-num, 1);
				current.AddBlood(-num, 1);
				current.AddBlood(-num, 1);
				current.AddBlood(-num, 1);
				current.AddBlood(-num, 1);
				current.AddBlood(-num, 1);
			}
		}

		private void Goblinxaotra()
		{
			int num = base.Game.Random.Next(0, FiveNormalSecondBoss.AllAttackChat.Length);
			base.Body.Say(FiveNormalSecondBoss.AllAttackChat[num], 1, 0);
			base.Body.PlayMovie("beatC", 1000, 1000);
		}

		private void BeatA()
		{
			base.Body.PlayMovie("beatA", 1000, 4000);
			base.Body.CallFuction(new LivingCallBack(this.GoAttack), 3000);
		}

		private void GoAttack()
		{
			Player player = base.Game.FindRandomPlayer();
			((PVEGame)base.Game).SendGameFocus(player, 0, 1500);
			int num = base.Game.Random.Next(321, 515);
			player.AddBlood(-num, 1);
			this.m_moive = ((PVEGame)base.Game).Createlayer(player.X, player.Y, "wallLeft", "asset.game.4.xiaopao", "1", 1, 0);
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
			if (this.m_moive != null)
			{
				base.Game.RemovePhysicalObj(this.m_moive, true);
				this.m_moive = null;
			}
		}
	}
}
