using Bussiness;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class WorldBatKing : ABrain
	{
		private int m_attackTurn = 0;

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
			if (base.Body.State == 10)
			{
				this.KillAttack(base.Body.X - 10000, base.Body.X + 10000);
			}
			else if (this.m_attackTurn == 0)
			{
				this.AttackA();
				this.m_attackTurn++;
			}
			else
			{
				this.AttackB(base.Body.X - 10000, base.Body.X + 10000);
				this.m_attackTurn = 0;
			}
		}

		private void KillAttack(int fx, int tx)
		{
			int num = base.Game.Random.Next(0, WorldBatKing.KillAttackChat.Length);
			base.Body.Say(WorldBatKing.KillAttackChat[num], 1, 1000);
			base.Body.CurrentDamagePlus = 100f;
			base.Body.PlayMovie("beatB", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 5000, null);
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void AttackA()
		{
			int x = base.Game.Random.Next(173, 1439);
			int y = base.Game.Random.Next(150, 700);
			base.Body.MoveTo(x, y, "fly", 3000, "", 12, new LivingCallBack(this.AllAttackA));
		}

		private void AllAttackA()
		{
			Player player = base.Game.FindRandomPlayer();
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			if (player != null)
			{
				base.Body.CurrentDamagePlus = 10f;
				int num = base.Game.Random.Next(0, WorldBatKing.ShootChat.Length);
				base.Body.Say(WorldBatKing.ShootChat[num], 1, 0);
				base.Body.PlayMovie("beatA", 1700, 0);
				base.Body.RangeAttacking(base.Body.X - 10000, base.Body.X + 10000, "cry", 4000, null);
			}
		}

		private void AttackB(int fx, int tx)
		{
			Player player = base.Game.FindRandomPlayer();
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			if (player != null)
			{
				base.Body.CurrentDamagePlus = 15f;
				int num = base.Game.Random.Next(0, WorldBatKing.ShootChat.Length);
				base.Body.Say(WorldBatKing.ShootChat[num], 1, 0);
				base.Body.PlayMovie("beatB", 1900, 0);
				base.Body.RangeAttacking(fx, tx, "cry", 4000, null);
				base.Body.CallFuction(new LivingCallBack(this.GoMovie), 4000);
			}
		}

		private void GoMovie()
		{
			List<Player> allFightPlayers = base.Game.GetAllFightPlayers();
			foreach (Player current in allFightPlayers)
			{
			}
			base.Body.CallFuction(new LivingCallBack(this.ShowIn), 2000);
		}

		private void ShowIn()
		{
			base.Body.PlayMovie("in", 100, 0);
		}

		public override void OnKillPlayerSay()
		{
			base.OnKillPlayerSay();
			int num = base.Game.Random.Next(0, WorldBatKing.KillPlayerChat.Length);
			base.Body.Say(WorldBatKing.KillPlayerChat[num], 1, 0, 2000);
		}
	}
}
