using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class ThirdNormalKingFour : ABrain
	{
		private int attackingTurn = 1;

		private int orchinIndex = 1;

		private int currentCount = 0;

		private int Dander = 0;

		private int npcID = 3103;

		private int IsEixt = 0;

		private PhysicalObj m_kingMoive;

		public List<SimpleNpc> orchins = new List<SimpleNpc>();

		private static string[] AllAttackChat = new string[]
		{
			"Hãy nhìn vào diễn viên đóng thế của tôi!",
			"Điều này thật tuyệt, bạn muốn học?",
			"Biến mất !!!!!!bụi khiêm nhường! ",
			"Bạn sẽ trả tiền cho nó!"
		};

		private static string[] ShootChat = new string[]
		{
			"Lửa địa ngục...",
			"Tôi sẽ không bị đánh bại bởi bạn như chất thải!",
			"Này, tôi làm tổn thương bạn, tôi đã hahaha!",
			"Này, đó là một cuộc tấn công như vậy!",
			"Nhìn thấy tôi là vinh dự của bạn!"
		};

		private static string[] CallChat = new string[]
		{
			"Nào, hãy để họ nếm thử quả bom!"
		};

		private static string[] AngryChat = new string[]
		{
			"Bạn buộc tôi phải thực hiện một mẹo!"
		};

		private static string[] KillAttackChat = new string[]
		{
			"Bạn sắp chết?"
		};

		private static string[] SealChat = new string[]
		{
			"Chạy đường nào đây?"
		};

		private static string[] KillPlayerChat = new string[]
		{
			"Lửa bất diệt cháy bừng lên đi!"
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
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X > 572 && current.X < 872)
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
				this.KillAttack(572, 872);
			}
			else if (!flag)
			{
				if (this.attackingTurn == 1)
				{
					this.Summon();
				}
				else if (this.attackingTurn == 2)
				{
					this.PersonalAttackDame();
				}
				else if (this.attackingTurn == 3)
				{
					this.HalfAttack();
				}
				else if (this.attackingTurn == 4)
				{
					this.MovePlayer();
				}
				else if (this.attackingTurn == 5)
				{
					this.PersonalNpc();
				}
				else
				{
					this.HalfAttack();
					this.attackingTurn = 1;
				}
				this.attackingTurn++;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		public new virtual void Dispose()
		{
			base.Dispose();
			base.Game.RemoveLiving(this.npcID);
		}

		public void HalfAttack()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			int num = base.Game.Random.Next(0, ThirdNormalKingFour.SealChat.Length);
			base.Body.Say(ThirdNormalKingFour.AllAttackChat[num], 1, 500);
			base.Body.PlayMovie("beatB", 2500, 0);
			base.Body.RangeAttacking(base.Body.X - 2000, base.Body.Y + 2000, "cry", 3000, null);
		}

		private void PersonalAttackDame()
		{
			Player player = base.Game.FindRandomPlayer();
			int num = base.Game.Random.Next(0, ThirdNormalKingFour.ShootChat.Length);
			((SimpleBoss)base.Body).Say(ThirdNormalKingFour.ShootChat[num], 1, 500);
			if (base.Body.X > player.X)
			{
				if (player.X > base.Body.Y)
				{
					base.Body.ChangeDirection(1, 50);
				}
				else
				{
					base.Body.ChangeDirection(-1, 50);
				}
				int num2 = base.Game.Random.Next(player.X - 10, player.X + 10);
				if (base.Body.ShootPoint(player.X, player.Y, 53, 1000, 10000, 3, 2.3f, 2600))
				{
					base.Body.PlayMovie("aim", 1000, 0);
					base.Body.PlayMovie("beatA", 1500, 0);
				}
			}
			else
			{
				if (player.X > base.Body.Y)
				{
					base.Body.ChangeDirection(1, 50);
				}
				else
				{
					base.Body.ChangeDirection(-1, 50);
				}
				int num2 = base.Game.Random.Next(player.X - 10, player.X + 10);
				if (base.Body.ShootPoint(player.X, player.Y, 53, 1000, 10000, 3, 1f, 2600))
				{
					base.Body.PlayMovie("aim", 1000, 0);
					base.Body.PlayMovie("beatA", 1500, 0);
				}
			}
		}

		private void PersonalNpc()
		{
			int num = base.Game.Random.Next(0, ThirdNormalKingFour.CallChat.Length);
			((SimpleBoss)base.Body).Say(ThirdNormalKingFour.CallChat[num], 1, 500);
			if (this.IsEixt == 1)
			{
				base.Body.ChangeDirection(1, 50);
				if (base.Body.ShootPoint(1000, 560, 53, 1000, 10000, 1, 2f, 2550))
				{
					base.Body.PlayMovie("aim", 1700, 0);
				}
				base.Body.PlayMovie("beatA", 2000, 0);
				this.IsEixt = 0;
			}
			else
			{
				base.Body.ChangeDirection(-1, 50);
				if (base.Body.ShootPoint(478, 550, 53, 1000, 10000, 1, 2f, 2550))
				{
					base.Body.PlayMovie("aim", 1700, 0);
				}
				base.Body.PlayMovie("beatA", 2000, 0);
				this.IsEixt = 1;
			}
		}

		public void Summon()
		{
			if (base.Body.State == 1)
			{
				base.Body.PlayMovie("beatC", 2500, 0);
			}
			else
			{
				base.Body.PlayMovie("beatC", 0, 2000);
				List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
				Player player = base.Game.FindRandomPlayer();
				int num = base.Game.Random.Next(0, ThirdNormalKingFour.KillPlayerChat.Length);
				((SimpleBoss)base.Body).Say(ThirdNormalKingFour.KillPlayerChat[num], 1, 500);
				foreach (Player current in allLivingPlayers)
				{
					current.AddEffect(new ContinueReduceBloodEffect(2, 500, current), 0);
				}
				base.Body.CallFuction(new LivingCallBack(this.In), 1300);
			}
		}

		public void In()
		{
			List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
			Player player = base.Game.FindRandomPlayer();
			foreach (Player current in allLivingPlayers)
			{
				this.m_kingMoive = ((PVEGame)base.Game).Createlayer(current.X, current.Y - 100, "moive", "asset.game.4.flame", "out", 1, 0);
			}
			base.Body.CallFuction(new LivingCallBack(this.Remove), 1000);
		}

		public void Remove()
		{
			if (this.m_kingMoive != null)
			{
				base.Game.RemovePhysicalObj(this.m_kingMoive, true);
				this.m_kingMoive = null;
			}
		}

		public void MovePlayer()
		{
			if (base.Body.State == 1)
			{
				base.Body.PlayMovie("beatC", 2500, 0);
			}
			else
			{
				base.Body.PlayMovie("beatC", 0, 2000);
				List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
				Player player = base.Game.FindRandomPlayer();
				int num = base.Game.Random.Next(0, ThirdNormalKingFour.SealChat.Length);
				((SimpleBoss)base.Body).Say(ThirdNormalKingFour.SealChat[num], 1, 500);
				foreach (Player current in allLivingPlayers)
				{
					int x = base.Game.Random.Next(200, 1550);
					current.BoltMove(x, 400, 500);
				}
			}
		}

		public void KillAttack(int fx, int mx)
		{
			base.Body.CurrentDamagePlus = 10f;
			int num = base.Game.Random.Next(0, ThirdNormalKingFour.KillAttackChat.Length);
			((SimpleBoss)base.Body).Say(ThirdNormalKingFour.KillAttackChat[num], 1, 500);
			base.Body.PlayMovie("beatC", 2500, 0);
			base.Body.RangeAttacking(fx, mx, "cry", 3300, null);
		}
	}
}
