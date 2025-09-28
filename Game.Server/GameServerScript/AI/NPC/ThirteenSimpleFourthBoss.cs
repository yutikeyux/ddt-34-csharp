using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class ThirteenSimpleFourthBoss : ABrain
	{
		public int attackingTurn = 0;

		private Player target = null;

		private PhysicalObj m_moive;

		private PhysicalObj m_front;

		private static string[] AllAttackChat = new string[]
		{
			"Bạn sẽ trả giá cho việc này ! "
		};

		private static string[] ShootChat = new string[]
		{
			"Tôi không muốn chỉ là lãng phí khi bạn đánh bại!",
			"Oh, bạn chơi tốt đấy, <br/>ha ha ha ha!",
			"Xem tôi là danh dự của bạn!"
		};

		private static string[] CallChat = new string[]
		{
			"Các, <br/>Boom con của ta !"
		};

		private static string[] AngryChat = new string[]
		{
			"Sức mạnh cuối cùng !"
		};

		private static string[] KillAttackChat = new string[]
		{
			"I want kill you ?"
		};

		private static string[] SealChat = new string[]
		{
			"Hãy đón nhận cái nón đến tột cùng !"
		};

		private static string[] KillPlayerChat = new string[]
		{
			"Địa ngục là điểm đến duy nhất của ngươi !",
			"Quá dễ để ta tiêu diệt."
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
			base.OnStartAttacking();
			bool flag = false;
			int num = 0;
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X < 158)
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
				this.KillAttack(0, 300);
			}
			else if (this.attackingTurn == 0)
			{
				this.CallDeadZone();
				this.attackingTurn++;
			}
			else if (this.attackingTurn == 1)
			{
				this.AttackInDeadZone();
				this.attackingTurn++;
			}
			else if (this.attackingTurn == 2)
			{
				this.PersonalActack();
				this.attackingTurn++;
			}
			else
			{
				this.AllAttack();
				this.attackingTurn = 0;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void CallDeadZone()
		{
			base.Body.PlayMovie("beatA", 2000, 0);
			base.Body.CallFuction(new LivingCallBack(this.CreateMovie), 4000);
		}

		public void CreateMovie()
		{
			this.m_moive = ((PVEGame)base.Game).Createlayer(800, base.Body.Y, "moive", "asset.game.ten.tedabiaoji", "out", 1, 0);
		}

		public void CreateEffect()
		{
			if (this.target != null)
			{
				this.m_front = ((PVEGame)base.Game).Createlayer(this.target.X, this.target.Y, "effect", "asset.game.ten.qunbao", "out", 1, 0);
			}
		}

		public void AllAttack()
		{
			base.Body.PlayMovie("beatA", 1000, 1000);
			base.Body.RangeAttacking(base.Body.X - 1500, base.Body.X + 1500, "cry", 4000, null);
			base.Body.CallFuction(new LivingCallBack(this.MultiMovie), 4000);
			base.Body.CallFuction(new LivingCallBack(this.Out), 5000);
		}

		private void MultiMovie()
		{
			List<Player> allFightPlayers = base.Game.GetAllFightPlayers();
			foreach (Player current in allFightPlayers)
			{
				this.m_moive = ((PVEGame)base.Game).Createlayer(current.X, current.Y, "boom", "asset.game.ten.qunbao", "out", 1, 0);
			}
		}

		public void Out()
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

		public void AttackInDeadZone()
		{
			int num = base.Game.Random.Next(0, ThirteenSimpleFourthBoss.AngryChat.Length);
			base.Body.Say(ThirteenSimpleFourthBoss.AngryChat[num], 1, 0);
			base.Body.PlayMovie("beatD", 3000, 0);
			base.Body.CurrentDamagePlus = 10f;
			base.Body.RangeAttacking(base.Body.X, 1400, "cry", 4000, null);
			base.Body.CallFuction(new LivingCallBack(this.Out), 5000);
		}

		public void PersonalActack()
		{
			base.Body.PlayMovie("beatA", 1000, 1000);
			this.target = base.Game.FindRandomPlayer();
			base.Body.CurrentDamagePlus = 2f;
			base.Body.RangeAttacking(this.target.X - 20, this.target.X + 20, "cry", 2000, null);
			base.Body.CallFuction(new LivingCallBack(this.CreateEffect), 2000);
			base.Body.CallFuction(new LivingCallBack(this.Out), 3000);
		}

		public void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 100f;
			int num = base.Game.Random.Next(0, ThirteenSimpleFourthBoss.KillAttackChat.Length);
			((SimpleBoss)base.Body).Say(ThirteenSimpleFourthBoss.KillAttackChat[num], 1, 500);
			base.Body.PlayMovie("beatB", 2500, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 3300, null);
		}
	}
}
