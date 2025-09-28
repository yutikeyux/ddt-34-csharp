using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Drawing;

namespace GameServerScript.AI.NPC
{
	public class SimpleQueenAntAi : ABrain
	{
		private int m_attackTurn = 0;

		private int npcID = 2004;

		private int isSay = 0;

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
			int num = base.Game.Random.Next(0, SimpleQueenAntAi.KillAttackChat.Length);
			base.Body.Say(SimpleQueenAntAi.KillAttackChat[num], 1, 1000);
			base.Body.CurrentDamagePlus = 10f;
			base.Body.PlayMovie("beatB", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 5000, null);
		}

		private void PersonalAttack()
		{
			Player player = base.Game.FindRandomPlayer();
			if (player != null)
			{
				base.Body.CurrentDamagePlus = 0.8f;
				int num = base.Game.Random.Next(0, SimpleQueenAntAi.ShootChat.Length);
				base.Body.Say(SimpleQueenAntAi.ShootChat[num], 1, 0);
				int num2 = base.Game.Random.Next(670, 880);
				int num3 = base.Game.Random.Next(player.X - 10, player.X + 10);
				if (base.Body.ShootPoint(player.X, player.Y, ((SimpleBoss)base.Body).NpcInfo.CurrentBallId, 1000, 10000, 1, 3f, 2550))
				{
					base.Body.PlayMovie("beatA", 1700, 0);
				}
			}
		}

		private void Summon()
		{
			int num = base.Game.Random.Next(0, SimpleQueenAntAi.CallChat.Length);
			base.Body.Say(SimpleQueenAntAi.CallChat[num], 1, 600);
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
			int num = base.Game.Random.Next(0, SimpleQueenAntAi.KillPlayerChat.Length);
			base.Body.Say(SimpleQueenAntAi.KillPlayerChat[num], 1, 0, 2000);
		}

		public override void OnDiedSay()
		{
		}

		private void CreateChild()
		{
		}

		public override void OnShootedSay()
		{
			int num = base.Game.Random.Next(0, SimpleQueenAntAi.ShootedChat.Length);
			if (this.isSay == 0 && base.Body.IsLiving)
			{
				base.Body.Say(SimpleQueenAntAi.ShootedChat[num], 1, 900, 0);
				this.isSay = 1;
			}
			if (!base.Body.IsLiving)
			{
				num = base.Game.Random.Next(0, SimpleQueenAntAi.DiedChat.Length);
				base.Body.Say(SimpleQueenAntAi.DiedChat[num], 1, 100, 2000);
			}
		}
	}
}
