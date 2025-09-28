using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class ThirteenSimpleBrotherNpc : ABrain
	{
		private int m_attackTurn = 0;

		private PhysicalObj m_moive;

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
			if (this.m_attackTurn == 0)
			{
				this.CallBuff();
				this.m_attackTurn++;
			}
			else
			{
				this.PersonalAttack();
				this.m_attackTurn = 0;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void PersonalAttack()
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
			base.Body.RangeAttacking(920, 1370, "cry", 1000, list);
			base.Body.PlayMovie("call", 1000, 1000);
			base.Body.CallFuction(new LivingCallBack(this.Remove), 1000);
		}

		public void CallBuff()
		{
			base.Body.JumpTo(base.Body.X, base.Body.Y - 300, "walk", 2000, -1);
			base.Body.PlayMovie("call", 1000, 1000);
			base.Body.CallFuction(new LivingCallBack(this.CreateMovie), 1000);
		}

		public void Remove()
		{
			if (this.m_moive != null)
			{
				this.m_moive.PlayMovie("beatA", 1000, 1000);
			}
		}

		public void CreateMovie()
		{
			this.m_moive = ((PVEGame)base.Game).CreatePhysicalObj(1146, 566, "moiveA", "asset.game.ten.jitan", "born", 1, 0);
		}
	}
}
