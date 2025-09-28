using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class TerrorKingLast : ABrain
	{
		private int attackingTurn = 1;

		private int Dander = 0;

		private int npcID = 1304;

		public List<SimpleNpc> orchins = new List<SimpleNpc>();

		private static string[] AllAttackChat = new string[]
		{
			"Nghiên cứu kỹ năng của tôi!",
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
			bool flag = false;
			int num = 0;
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X > 620 && current.X < 1160)
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
			else if (!flag)
			{
				if (this.attackingTurn == 1)
				{
					base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
					this.HalfAttack();
				}
				else if (this.attackingTurn == 2)
				{
					base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
					this.Summon();
				}
				else if (this.attackingTurn == 3)
				{
					base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
					this.Seal();
				}
				else if (this.attackingTurn == 4)
				{
					base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
					this.Angger();
				}
				else
				{
					base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
					this.GoOnAngger();
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
			int num = base.Game.Random.Next(0, TerrorKingLast.SealChat.Length);
			base.Body.Say(TerrorKingLast.AllAttackChat[num], 1, 500);
			base.Body.PlayMovie("beatB", 2500, 0);
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
			int num = base.Game.Random.Next(0, TerrorKingLast.CallChat.Length);
			base.Body.Say(TerrorKingLast.CallChat[num], 1, 0);
			base.Body.PlayMovie("beatA", 100, 0);
			base.Body.CallFuction(new LivingCallBack(this.CreateChild), 2500);
		}

		public void Seal()
		{
			int num = base.Game.Random.Next(0, TerrorKingLast.SealChat.Length);
			((SimpleBoss)base.Body).Say(TerrorKingLast.SealChat[num], 1, 0);
			Player player = base.Game.FindRandomPlayer();
			base.Body.PlayMovie("mantra", 2000, 2000);
			base.Body.Seal(player, 1, 3000);
		}

		public void Angger()
		{
			int num = base.Game.Random.Next(0, TerrorKingLast.AngryChat.Length);
			base.Body.Say(TerrorKingLast.AngryChat[num], 1, 0);
			base.Body.State = 1;
			this.Dander += 100;
			base.Body.PlayMovie("angry", 1000, 0);
			((SimpleBoss)base.Body).SetDander(this.Dander);
			if (base.Body.Direction == -1)
			{
				((SimpleBoss)base.Body).SetRelateDemagemRect(8, -252, 74, 50);
			}
			else
			{
				((SimpleBoss)base.Body).SetRelateDemagemRect(-82, -252, 74, 50);
			}
		}

		public void GoOnAngger()
		{
			if (base.Body.State == 1)
			{
				base.Body.CurrentDamagePlus = 1000f;
				base.Body.PlayMovie("beatC", 3500, 0);
				base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 5600, null);
				base.Body.Die(5600);
			}
			else
			{
				((SimpleBoss)base.Body).SetRelateDemagemRect(-41, -187, 83, 140);
				base.Body.PlayMovie("mantra", 0, 2000);
				List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
				foreach (Player current in allLivingPlayers)
				{
					current.AddEffect(new ContinueReduceBloodEffect(2, 150, current), 0);
				}
			}
		}

		public void KillAttack(int fx, int mx)
		{
			base.Body.CurrentDamagePlus = 10f;
			int num = base.Game.Random.Next(0, TerrorKingLast.KillAttackChat.Length);
			((SimpleBoss)base.Body).Say(TerrorKingLast.KillAttackChat[num], 1, 500);
			base.Body.PlayMovie("beatB", 2500, 0);
			base.Body.RangeAttacking(fx, mx, "cry", 3300, null);
		}

		public void CreateChild()
		{
			((SimpleBoss)base.Body).CreateChild(this.npcID, 800, 665, 180, 6, -1);
		}
	}
}
