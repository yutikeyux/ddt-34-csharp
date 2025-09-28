using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class FiveHardFourBoss : ABrain
	{
		private int m_attackTurn = 0;

		private int npcID = 5232;

		private int isSay = 0;

		private int m_maxBlood;

		private int m_blood;

		private PhysicalObj m_moive;

		private PhysicalObj m_front;

		private static string[] AllAttackChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg1", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg2", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg3", new object[0])
		};

		private static string[] ShootChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg4", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg5", new object[0])
		};

		private static string[] KillPlayerChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg6", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg7", new object[0])
		};

		private static string[] CallChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg8", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg9", new object[0])
		};

		private static string[] JumpChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg10", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg11", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg12", new object[0])
		};

		private static string[] KillAttackChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg13", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg14", new object[0])
		};

		private static string[] ShootedChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg15", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg16", new object[0])
		};

		private static string[] DiedChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg17", new object[0])
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
				if (current.IsLiving && current.X > 1500 && current.X < base.Game.Map.Info.ForegroundWidth + 1)
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
				this.KillAttack(base.Body.X - 10000, base.Body.X + 10000);
			}
			else if (this.m_attackTurn == 0)
			{
				this.BeatE();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.AllAttack();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 2)
			{
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 3)
			{
				this.AllAttack2();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 4)
			{
				this.Dame();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 5)
			{
				this.Summon();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 6)
			{
				this.AtoB();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 7)
			{
				this.AtoB();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 8)
			{
				this.Born();
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
			int num = base.Game.Random.Next(0, FiveHardFourBoss.KillAttackChat.Length);
			base.Body.Say(FiveHardFourBoss.KillAttackChat[num], 1, 1000);
			base.Body.CurrentDamagePlus = 10f;
			base.Body.PlayMovie("beatB", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 5000, null);
		}

		private void Born()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			base.Body.PlayMovie("born", 1000, 0);
			base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 3000, null);
		}

		private void BeatE()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			base.Body.PlayMovie("beatE", 1000, 0);
			base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 3000, null);
		}

		private void AllAttack()
		{
			base.Body.PlayMovie("beatA", 1000, 3000);
		}

		private void AllAttack2()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			base.Body.PlayMovie("beatB", 1000, 0);
			base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 3000, null);
		}

		private void Dame()
		{
			base.Body.PlayMovie("beatD", 1000, 3000);
			base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 3000, null);
			List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
			foreach (Player current in allLivingPlayers)
			{
				current.MoveTo(current.X - 400, base.Body.Y, "run", 0, "", 3);
				this.m_moive = ((PVEGame)base.Game).Createlayer(current.X, current.Y, "moive", "asset.game.4.tang", "out", 1, 0);
			}
		}

		private void PersonalAttack()
		{
			base.Body.PlayMovie("beatC", 3000, 5000);
			base.Body.CallFuction(new LivingCallBack(this.OnPersonalAttack), 3000);
		}

		private void OnPersonalAttack()
		{
			Player player = base.Game.FindRandomPlayer();
			if (player != null)
			{
				int num = base.Game.Random.Next(player.Y + 10, player.Y + 10);
                if (base.Body.Shoot(0, player.X, player.Y, 66, 66, 1, 2550))
				{
					this.m_moive = ((PVEGame)base.Game).Createlayer(player.X, player.Y, "moive", "asset.game.4.guang", "out", 1, 0);
				}
			}
		}

		private void Summon()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			base.Body.PlayMovie("beatE", 1000, 0);
			base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 3000, null);
			base.Body.CallFuction(new LivingCallBack(this.Call), 4000);
		}

		private void AtoB()
		{
			base.Body.PlayMovie("AtoB", 1700, 2000);
		}

		public void Call()
		{
			((SimpleBoss)base.Body).CreateChild(this.npcID, 1000, 530, 430, 1, -1);
		}
	}
}
