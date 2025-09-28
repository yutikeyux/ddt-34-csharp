using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Drawing;

namespace GameServerScript.AI.NPC
{
	public class HardDSLT : ABrain
	{
		private int m_attackTurn = 0;

		private int npcID = 71124;

		private int isSay = 0;

		private Point[] brithPoint = new Point[]
		{
			new Point(420, 420),
			new Point(570, 420),
			new Point(700, 420),
			new Point(800, 420),
			new Point(1000, 420)
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
				if (((PVEGame)base.Game).GetLivedLivings().Count == 9)
				{
					this.PersonalAttack();
				}
				else
				{
					this.Summon();
				}
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

		private void KillAttack(int fx, int tx)
		{
			int num = base.Game.Random.Next(0, HardDSLT.KillAttackChat.Length);
			base.Body.Say(HardDSLT.KillAttackChat[num], 1, 1000);
			base.Body.CurrentDamagePlus = 100f;
			base.Body.PlayMovie("beatB", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 5000, null);
		}

		private void PersonalAttack()
		{
			Player player = base.Game.FindRandomPlayer();
			if (player != null)
			{
				base.Body.CurrentDamagePlus = 1f;
				int num = base.Game.Random.Next(0, HardDSLT.ShootChat.Length);
				base.Body.Say(HardDSLT.ShootChat[num], 1, 0);
				base.Body.PlayMovie("beatA", 1700, 0);
				base.Body.RangeAttacking(player.X - 10, player.X + 10, "cry", 4000, null);
			}
		}

		private void Summon()
		{
			int num = base.Game.Random.Next(0, HardDSLT.CallChat.Length);
			base.Body.Say(HardDSLT.CallChat[num], 1, 600);
			base.Body.PlayMovie("call", 1700, 2000, new LivingCallBack(this.Call));
			base.Body.CallFuction(new LivingCallBack(this.Call), 2000);
		}

		private void Call()
		{
			((SimpleBoss)base.Body).CreateChild(this.npcID, this.brithPoint, 9, 3, 9, -1);
		}

		public override void OnKillPlayerSay()
		{
			base.OnKillPlayerSay();
			int num = base.Game.Random.Next(0, HardDSLT.KillPlayerChat.Length);
			base.Body.Say(HardDSLT.KillPlayerChat[num], 1, 0, 2000);
		}

		public override void OnDiedSay()
		{
		}

		private void CreateChild()
		{
		}

		public override void OnShootedSay()
		{
			int num = base.Game.Random.Next(0, HardDSLT.ShootedChat.Length);
			if (this.isSay == 0 && base.Body.IsLiving)
			{
				base.Body.Say(HardDSLT.ShootedChat[num], 1, 900, 0);
				this.isSay = 1;
			}
			if (!base.Body.IsLiving)
			{
				num = base.Game.Random.Next(0, HardDSLT.DiedChat.Length);
				base.Body.Say(HardDSLT.DiedChat[num], 1, 100, 2000);
			}
		}
	}
}
