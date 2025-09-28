using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class ZYSimpleNpc60018 : ABrain
	{
		private int attackingTurn = 1;

		private int npcID = 1311;

		private static string[] AllAttackChat = new string[]
		{
			"Xem tuyệt chiêu nè!",
			"Di chuyển mát mẻ!<br/>Bạn muốn tìm hiểu không?",
			"Chụi không nỗi!",
			"Bạn sẽ trả giá cho việc này! "
		};

		private static string[] CallChat = new string[]
		{
			"Nào, <br/>cho thử sức mạnh của lựu đạn!"
		};

		private static string[] AngryChat = new string[]
		{
			"Bạn buộc tôi để lừa!"
		};

		private static string[] KillAttackChat = new string[]
		{
			"Bạn đến chết?"
		};

		private static string[] SealChat = new string[]
		{
			"Chầu Diêm Vương!"
		};

		private static string[] KillPlayerChat = new string[]
		{
			"Địa ngục là điểm đến duy nhất của bạn!",
			"Quá dễ bị tổn thương."
		};

		public List<SimpleNpc> orchins = new List<SimpleNpc>();

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
			base.OnStartAttacking();
			bool flag = false;
			int num = 0;
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X > 740 && current.X < 1040)
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
				this.KillAttack(620, 1160);
			}
			else
			{
				if (this.attackingTurn == 1)
				{
					this.HalfAttack();
				}
				else if (this.attackingTurn == 2)
				{
					this.Summon();
				}
				else
				{
					this.KillAttack(620, 1160);
					this.attackingTurn = 0;
				}
				this.attackingTurn++;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		public void HalfAttack()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			int num = base.Game.Random.Next(0, ZYSimpleNpc60018.SealChat.Length);
			base.Body.Say(ZYSimpleNpc60018.AllAttackChat[num], 1, 500);
			base.Body.PlayMovie("beatC", 2500, 0);
			if (base.Body.Direction == 1)
			{
				base.Body.RangeAttacking(base.Body.X, base.Body.X + 1000, "cry", 3300, null);
			}
			else
			{
				base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X, "cry", 3300, null);
			}
		}

		public void Summon()
		{
			int num = base.Game.Random.Next(0, ZYSimpleNpc60018.CallChat.Length);
			base.Body.Say(ZYSimpleNpc60018.CallChat[num], 1, 0);
			base.Body.PlayMovie("beatA", 100, 0);
			base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X, "cry", 3300, null);
		}

		public void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 10f;
			int num = base.Game.Random.Next(0, ZYSimpleNpc60018.KillAttackChat.Length);
			((SimpleBoss)base.Body).Say(ZYSimpleNpc60018.KillAttackChat[num], 1, 500);
			base.Body.PlayMovie("beatB", 2500, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 3300, null);
		}

		public void CreateChild()
		{
			((SimpleBoss)base.Body).CreateChild(this.npcID, 680, 680, 405, 6, -1);
		}
	}
}
