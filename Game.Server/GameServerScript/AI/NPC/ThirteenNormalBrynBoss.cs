using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class ThirteenNormalBrynBoss : ABrain
	{
		private int m_attackTurn = 0;

		private PhysicalObj m_npc;

		private PhysicalObj m_moive;

		private int isSay = 0;

		private static string[] AllAttackChat = new string[]
		{
			LanguageMgr.GetTranslation("Sư tử rống...", new object[0]),
			LanguageMgr.GetTranslation("Sức mạnh của chúa rừng !", new object[0]),
			LanguageMgr.GetTranslation("Sự đau đớn tột độ !", new object[0])
		};

		private static string[] ShootChat = new string[]
		{
			LanguageMgr.GetTranslation("Nén đá dấu tay ...!", new object[0]),
			LanguageMgr.GetTranslation("Vũ khí của thần bộ lạc !", new object[0])
		};

		private static string[] KillPlayerChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg6", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg7", new object[0])
		};

		private static string[] CallChat = new string[]
		{
			LanguageMgr.GetTranslation("Vật tổ ...", new object[0]),
			LanguageMgr.GetTranslation("Kill !", new object[0])
		};

		private static string[] JumpChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg10", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg11", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg12", new object[0])
		};

		private static string[] KillAttackChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg13", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg14", new object[0])
		};

		private static string[] ShootedChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg15", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg16", new object[0])
		};

		private static string[] DiedChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg17", new object[0])
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
			this.isSay = 0;
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
				if (current.IsLiving && current.X > 950 && current.X < 1250 && current.Y > 666)
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
				this.KillAttack(950, 1250);
			}
			else if (this.m_attackTurn == 0)
			{
				this.SummonA();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.CreateMovie();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 2)
			{
				this.PersonalAttack();
				this.m_attackTurn++;
			}
			else
			{
				this.PersonalAttack2();
				this.m_attackTurn = 0;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void KillAttack(int fx, int tx)
		{
			int num = base.Game.Random.Next(0, ThirteenNormalBrynBoss.KillAttackChat.Length);
			base.Body.Say(ThirteenNormalBrynBoss.KillAttackChat[num], 1, 1000);
			base.Body.CurrentDamagePlus = 20f;
			base.Body.PlayMovie("beatC", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 5000, null);
		}

		private void PersonalAttack()
		{
			this.m_moive.PlayMovie("beatA", 1000, 1000);
			base.Body.CallFuction(new LivingCallBack(this.CallEffectB), 1000);
		}

		private void CallEffectB()
		{
			bool flag = false;
			Player[] allPlayers = base.Game.GetAllPlayers();
			List<Player> list = new List<Player>();
			Player[] array = allPlayers;
			for (int i = 0; i < array.Length; i++)
			{
				Player player = array[i];
				if (player.X > 990 && player.X < 1315 && player.Y < 606)
				{
					flag = true;
					list.Add(player);
					break;
				}
			}
			if (flag)
			{
				array = allPlayers;
				for (int i = 0; i < array.Length; i++)
				{
					Player player2 = array[i];
					player2.AddEffect(new DamageEffect(2), 0);
					player2.AddEffect(new GuardEffect(2), 0);
				}
			}
			base.Body.RangeAttacking(base.Body.X - 1500, base.Body.X + 1500, "cry", 1500, list);
		}

		private void PersonalAttack2()
		{
			int num = base.Game.Random.Next(0, ThirteenNormalBrynBoss.ShootChat.Length);
			base.Body.Say(ThirteenNormalBrynBoss.ShootChat[num], 1, 0);
			base.Body.CallFuction(new LivingCallBack(this.DoudbleAttack), 2600);
			base.Body.CallFuction(new LivingCallBack(this.DoudbleAttack), 6500);
		}

		private void DoudbleAttack()
		{
			Player player = base.Game.FindRandomPlayer();
			base.Body.CurrentDamagePlus = 1.8f;
			if (player != null)
			{
				int num = base.Game.Random.Next(player.X - 10, player.X + 10);
				if (base.Body.ShootPoint(player.X, player.Y, 55, 1000, 10000, 1, 1.5f, 2550))
				{
					base.Body.PlayMovie("beatB", 1700, 0);
				}
			}
		}

		private void SummonA()
		{
			base.Body.PlayMovie("callA", 3500, 0);
			base.Body.RangeAttacking(base.Body.X - 1500, base.Body.X + 1500, "cry", 5500, null);
			base.Body.CallFuction(new LivingCallBack(this.GoMovie), 5500);
			base.Body.CallFuction(new LivingCallBack(this.MovingPlayer), 6500);
		}

		private void MovingPlayer()
		{
			Player[] allPlayers = base.Game.GetAllPlayers();
			Player[] array = allPlayers;
			for (int i = 0; i < array.Length; i++)
			{
				Player player = array[i];
				int x = base.Game.Random.Next(900, 1350);
				player.StartSpeedMult(x, player.Y);
			}
		}

		private void GoMovie()
		{
			List<Player> allFightPlayers = base.Game.GetAllFightPlayers();
			foreach (Player current in allFightPlayers)
			{
				((PVEGame)base.Game).Createlayer(current.X, current.Y, "boom", "game.living.Living126", "beatA", 1, 0);
			}
		}

		public void CreateMovie()
		{
			this.m_moive = ((PVEGame)base.Game).CreatePhysicalObj(1146, 566, "moiveA", "asset.game.ten.jitan", "born", 1, 0);
			base.Body.CallFuction(new LivingCallBack(this.SummonB), 1000);
		}

		private void SummonB()
		{
			base.Body.PlayMovie("callA", 3500, 0);
			base.Body.RangeAttacking(base.Body.X - 1500, base.Body.X + 1500, "cry", 5500, null);
			base.Body.CallFuction(new LivingCallBack(this.GoMovie), 5500);
			base.Body.CallFuction(new LivingCallBack(this.CallEffectA), 6500);
		}

		private void CallEffectA()
		{
			List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
			int num = 0;
			foreach (Player current in allLivingPlayers)
			{
				if (num == 0)
				{
					current.AddEffect(new ReduceStrengthEffect(3, 110), 0);
				}
				if (num == 1)
				{
					current.AddEffect(new LockDirectionEffect(3), 0);
				}
				num++;
			}
		}

		public override void OnKillPlayerSay()
		{
			base.OnKillPlayerSay();
			int num = base.Game.Random.Next(0, ThirteenNormalBrynBoss.KillPlayerChat.Length);
			base.Body.Say(ThirteenNormalBrynBoss.KillPlayerChat[num], 1, 0, 2000);
		}

		public override void OnShootedSay()
		{
			int num = base.Game.Random.Next(0, ThirteenNormalBrynBoss.ShootedChat.Length);
			if (this.isSay == 0 && base.Body.IsLiving)
			{
				base.Body.Say(ThirteenNormalBrynBoss.ShootedChat[num], 1, 900, 0);
				this.isSay = 1;
			}
			if (!base.Body.IsLiving)
			{
				num = base.Game.Random.Next(0, ThirteenNormalBrynBoss.DiedChat.Length);
				base.Body.Say(ThirteenNormalBrynBoss.DiedChat[num], 1, 100, 2000);
			}
		}
	}
}
