using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class WorldBatKingSecond : ABrain
	{
		private int m_attackTurn = 0;

		private PhysicalObj moive;

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
			if (this.m_attackTurn == 0)
			{
				this.AttackA(base.Body.X - 10000, base.Body.X + 10000);
				this.m_attackTurn++;
			}
			else
			{
				this.AttackB(base.Body.X - 10000, base.Body.X + 10000);
				this.m_attackTurn = 0;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void AttackA(int fx, int tx)
		{
			Player player = base.Game.FindRandomPlayer();
			if (player != null)
			{
				base.Body.CurrentDamagePlus = 10f;
				int num = base.Game.Random.Next(0, WorldBatKingSecond.ShootChat.Length);
				base.Body.Say(WorldBatKingSecond.ShootChat[num], 1, 0);
				base.Body.PlayMovie("beatA", 1700, 0);
				base.Body.RangeAttacking(fx, tx, "cry", 4000, null);
			}
		}

		private void AttackB(int fx, int tx)
		{
			Player player = base.Game.FindRandomPlayer();
			if (player != null)
			{
				base.Body.CurrentDamagePlus = 15f;
				int num = base.Game.Random.Next(0, WorldBatKingSecond.ShootChat.Length);
				base.Body.Say(WorldBatKingSecond.ShootChat[num], 1, 0);
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
				this.moive = ((PVEGame)base.Game).Createlayer(current.X, current.Y, "moive", "asset.game.zero.294b", "out", 1, 0);
			}
			base.Body.PlayMovie("in", 100, 0);
		}
	}
}
