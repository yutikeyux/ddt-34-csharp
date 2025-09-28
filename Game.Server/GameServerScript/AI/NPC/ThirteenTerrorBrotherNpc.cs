using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class ThirteenTerrorBrotherNpc : ABrain
	{
		private int m_attackTurn = 0;

		private int isSay = 0;

		private int IsEixt = 0;

		private PhysicalObj m_moive;

		private PhysicalObj m_front;

		private PhysicalObj wallLeft = null;

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
				if (current.IsLiving && current.X > 0 && current.X < 0)
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
				this.KillAttack(0, 0);
			}
			else if (this.m_attackTurn == 0)
			{
				this.Jump();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.JumpPersonalAttack();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 2)
			{
				this.FallSummon();
				this.m_attackTurn++;
			}
			else
			{
				this.Healing();
				this.m_attackTurn = 0;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		public void JumpPersonalAttack()
		{
			base.Body.PlayMovie("walk", 0, 500);
			base.Body.JumpTo(base.Body.X, base.Body.Y - 150, "", 0, 1, new LivingCallBack(this.PersonalAttack));
			((SimpleBoss)base.Body).SetRelateDemagemRect(-41, -107, 83, 100);
		}

		private void PersonalAttack()
		{
			Player player = base.Game.FindRandomPlayer();
			if (player != null)
			{
				((SimpleBoss)base.Body).SetRelateDemagemRect(-41, -107, 83, 100);
				int num = base.Game.Random.Next(player.X - 10, player.Y + 20);
				if (base.Body.ShootPoint(player.X, player.Y, 54, 1000, 10000, 1, 3f, 2550))
				{
					base.Body.PlayMovie("beatA", 1700, 0);
				}
			}
		}

		public void Jump()
		{
			base.Body.PlayMovie("walk", 700, 0);
			base.Body.JumpTo(base.Body.X, base.Body.Y - 150, "", 1000, 1, new LivingCallBack(this.CreateChild));
			((SimpleBoss)base.Body).SetRelateDemagemRect(-41, -107, 83, 100);
		}

		public void FallSummon()
		{
			base.Body.PlayMovie("walk", 700, 0);
			base.Body.FallFrom(base.Body.X, base.Body.Y + 150, "", 1000, 0, 50, new LivingCallBack(this.Summon));
			((SimpleBoss)base.Body).SetRelateDemagemRect(-41, -107, 83, 100);
		}

		public void Summon()
		{
			base.Body.PlayMovie("call", 100, 0);
			this.wallLeft = ((PVEGame)base.Game).CreatePhysicalObj(1146, 566, "moive", "asset.game.ten.jitan", "beatA", 1, 0);
			base.Body.CallFuction(new LivingCallBack(this.Remove), 1000);
		}

		public void Remove()
		{
			if (this.m_moive != null)
			{
				base.Game.RemovePhysicalObj(this.m_moive, true);
				this.m_moive = null;
			}
			if (this.m_front != null)
			{
				base.Game.RemovePhysicalObj(this.m_front, true);
				this.m_front = null;
			}
		}

		public void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 10f;
			int num = base.Game.Random.Next(0, ThirteenTerrorBrotherNpc.KillAttackChat.Length);
			((SimpleBoss)base.Body).Say(ThirteenTerrorBrotherNpc.KillAttackChat[num], 1, 500);
			base.Body.PlayMovie("beatB", 2500, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 3300, null);
		}

		public void Healing()
		{
			base.Body.SyncAtTime = true;
			base.Body.AddBlood(5000);
			base.Body.PlayMovie("castA", 100, 0);
			base.Body.Say("Hồi phục sức mạnh", 1, 0);
		}

		public void CreateChild()
		{
			base.Body.PlayMovie("call", 100, 0);
			this.m_moive = ((PVEGame)base.Game).Createlayer(1146, 566, "moive", "asset.game.ten.jitan", "out", 1, 0);
			this.m_front = ((PVEGame)base.Game).Createlayer(1146, 566, "font", "asset.game.ten.jitan", "out", 1, 0);
		}
	}
}
