using Bussiness;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Drawing;

namespace GameServerScript.AI.NPC
{
	public class DCSM40022Boss : ABrain
	{
		private int m_attackTurn = 0;

		private Point[] brithPoint = new Point[]
		{
			new Point(979, 630),
			new Point(1013, 630),
			new Point(1052, 630),
			new Point(1088, 630),
			new Point(1142, 630)
		};

		private static string[] AllAttackChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg1", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg2", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg3", new object[0])
		};

		private static string[] ShootChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg4", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg5", new object[0])
		};

		private static string[] KillPlayerChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg6", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg7", new object[0])
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
		}

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			int num = 0;
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X > 1000)
				{
					int num2 = (int)base.Body.Distance(current.X, current.Y);
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			if (this.m_attackTurn == 0)
			{
				this.PersonalAttackC();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.PersonalAttackE();
				this.m_attackTurn++;
			}
			else
			{
				this.KillAttack();
				this.m_attackTurn = 0;
			}
		}

		private void KillAttack(int fx, int tx)
		{
			int num = base.Game.Random.Next(0, DCSM40022Boss.KillAttackChat.Length);
			base.Body.Say(DCSM40022Boss.KillAttackChat[num], 1, 1000);
			base.Body.CurrentDamagePlus = 100f;
			base.Body.PlayMovie("beatF", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 5000, null);
		}

		private void KillAttack()
		{
			Player player = base.Game.FindRandomPlayer();
			if (player != null)
			{
				int num = base.Game.Random.Next(0, DCSM40022Boss.KillAttackChat.Length);
				base.Body.Say(DCSM40022Boss.KillAttackChat[num], 1, 1000);
				base.Body.CurrentDamagePlus = 15f;
				base.Body.PlayMovie("beatF", 3000, 0);
				base.Body.RangeAttacking(0, base.Body.X + 1000, "cry", 5000, null);
			}
		}

		private void PersonalAttackC()
		{
			Player player = base.Game.FindRandomPlayer();
			if (player != null)
			{
				base.Body.CurrentDamagePlus = 5f;
				int num = base.Game.Random.Next(0, DCSM40022Boss.ShootChat.Length);
				base.Body.Say(DCSM40022Boss.ShootChat[num], 1, 0);
				int num2 = base.Game.Random.Next(0, 1200);
				base.Body.PlayMovie("beatC", 1700, 0);
				base.Body.RangeAttacking(0, base.Body.X + 1000, "cry", 4000, null);
			}
		}

		private void PersonalAttackE()
		{
			Player player = base.Game.FindRandomPlayer();
			if (player != null)
			{
				base.Body.CurrentDamagePlus = 10f;
				int num = base.Game.Random.Next(0, DCSM40022Boss.ShootChat.Length);
				base.Body.Say(DCSM40022Boss.ShootChat[num], 1, 0);
				int num2 = base.Game.Random.Next(0, 1200);
				base.Body.PlayMovie("beatE", 1700, 0);
				base.Body.RangeAttacking(0, base.Body.X + 1000, "cry", 4000, null);
			}
		}
	}
}
