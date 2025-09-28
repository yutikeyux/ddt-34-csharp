using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class ThirteenTerrorAntBoss : ABrain
	{
		private int m_attackTurn = 0;

		private PhysicalObj moive;

		private PhysicalObj k_moive;

		private PhysicalObj m_moive;

		private int isSay = 0;

		private static string[] AllAttackChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg1", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg2", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg3", new object[0])
		};

		private static string[] ShootChat = new string[]
		{
			LanguageMgr.GetTranslation("Nén nhẹ mũi tên của ta đây !", new object[0]),
			LanguageMgr.GetTranslation("Hãy nén thử mũi tên băng này đi !", new object[0])
		};

		private static string[] KillPlayerChat = new string[]
		{
			LanguageMgr.GetTranslation("Nén nhẹ mũi tên của ta đây !", new object[0]),
			LanguageMgr.GetTranslation("Đón nhận mũi tên thần kì !", new object[0])
		};

		private static string[] CallChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg8", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg9", new object[0])
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
				if (current.IsLiving && current.X > 1169 && current.X < base.Game.Map.Info.ForegroundWidth + 1)
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
				this.KillAttack(1169, base.Game.Map.Info.ForegroundWidth + 1);
			}
			else if (this.m_attackTurn == 0)
			{
				this.PersonalAttack2();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.Healing();
				this.Plain();
				this.m_attackTurn++;
			}
			else
			{
				this.PersonalAttack();
				this.m_attackTurn = 0;
			}
		}

		private void Healing()
		{
			base.Body.SyncAtTime = true;
			if (base.Game.GetDiedBossCount() == 0)
			{
				base.Body.AddBlood(15000);
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void KillAttack(int fx, int tx)
		{
			int num = base.Game.Random.Next(0, ThirteenTerrorAntBoss.KillAttackChat.Length);
			base.Body.Say(ThirteenTerrorAntBoss.KillAttackChat[num], 1, 1000);
			base.Body.CurrentDamagePlus = 10f;
			base.Body.PlayMovie("beatB", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 3000, null);
		}

		private void PersonalAttack()
		{
			Player player = base.Game.FindRandomPlayer();
			if (player != null)
			{
				base.Body.CurrentDamagePlus = 1.3f;
				int num = base.Game.Random.Next(0, ThirteenTerrorAntBoss.KillPlayerChat.Length);
				base.Body.Say(ThirteenTerrorAntBoss.KillPlayerChat[num], 1, 0);
				if (base.Body.ShootPoint(player.X, player.Y, 51, 1400, 10000, 1, 3f, 2550))
				{
					base.Body.PlayMovie("beatA", 1700, 0);
				}
			}
		}

		private void Plain()
		{
			int num = base.Game.Random.Next(0, ThirteenTerrorAntBoss.ShootChat.Length);
			base.Body.Say(ThirteenTerrorAntBoss.ShootChat[num], 1, 0);
			base.Body.CurrentDamagePlus = 1.1f;
			Player[] allPlayers = base.Game.GetAllPlayers();
			int num2 = 0;
			Player[] array = allPlayers;
			for (int i = 0; i < array.Length; i++)
			{
				Player player = array[i];
				if (player != null)
				{
					if (base.Body.ShootPoint(player.X, player.Y, 99, 1000, 10000, 1, 2.7f, 3000))
					{
						base.Body.PlayMovie("beatD", 1500, 0);
					}
				}
				num2++;
				if (num2 == 2)
				{
					break;
				}
			}
		}

		private void PersonalAttack2()
		{
			int num = base.Game.Random.Next(0, ThirteenTerrorAntBoss.KillPlayerChat.Length);
			base.Body.Say(ThirteenTerrorAntBoss.KillPlayerChat[num], 1, 0);
			base.Body.PlayMovie("beatC", 3500, 0);
			base.Body.CallFuction(new LivingCallBack(this.GoShoot), 4000);
		}

		private void GoShoot()
		{
			base.Body.CurrentDamagePlus = 1.2f;
			Player[] allPlayers = base.Game.GetAllPlayers();
			Player[] array = allPlayers;
			for (int i = 0; i < array.Length; i++)
			{
				Player player = array[i];
				this.moive = ((PVEGame)base.Game).Createlayer(player.X, player.Y, "moive", "asset.game.ten.jianyu", "out", 1, 1);
				base.Body.RangeAttacking(base.Body.X - 10000, base.Body.X + 10000, "cry", 1000, null);
			}
			base.Body.CallFuction(new LivingCallBack(this.GoOut), 2000);
		}

		private void GoOut()
		{
			if (this.moive != null)
			{
				base.Game.RemovePhysicalObj(this.moive, true);
				this.moive = null;
			}
		}

		public override void OnKillPlayerSay()
		{
			base.OnKillPlayerSay();
			int num = base.Game.Random.Next(0, ThirteenTerrorAntBoss.KillPlayerChat.Length);
			base.Body.Say(ThirteenTerrorAntBoss.KillPlayerChat[num], 1, 0, 2000);
		}

		private void CreateChild()
		{
		}

		public override void OnShootedSay()
		{
			int num = base.Game.Random.Next(0, ThirteenTerrorAntBoss.ShootedChat.Length);
			if (this.isSay == 0 && base.Body.IsLiving)
			{
				base.Body.Say(ThirteenTerrorAntBoss.ShootedChat[num], 1, 900, 0);
				this.isSay = 1;
			}
			if (!base.Body.IsLiving)
			{
				num = base.Game.Random.Next(0, ThirteenTerrorAntBoss.DiedChat.Length);
				base.Body.Say(ThirteenTerrorAntBoss.DiedChat[num], 1, 100, 2000);
			}
		}
	}
}
