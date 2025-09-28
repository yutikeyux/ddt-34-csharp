using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class NormalKingFirst : ABrain
	{
		private int m_attackTurn = 0;

		private static string[] AllAttackChat = new string[]
		{
			"Tôi muốn có một trận động đất !! <br/> Xin hãy giúp tôi",
			"Sốc vũ khí của bạn!",
			"Hãy nhìn xem bạn vẫn đủ khả năng bao nhiêu !!"
		};

		private static string[] ShootChat = new string[]
		{
			"Hãy cho bạn biết một trăm trăm là gì!",
			"Đưa cho bạn một quả bóng ~ Bạn phải nhặt nó lên",
			"Bạn là một nhóm những người cấp thấp không biết gì"
		};

		private static string[] ShootedChat = new string[]
		{
			"Ồ ~ ~ Tại sao bạn tấn công tôi? <br/> Tôi đang làm gì vậy?",
			"Này ~ ~ Đau quá! Tại sao tôi phải chiến đấu? <br/> Tôi phải chiến đấu ..."
		};

		private static string[] AddBooldChat = new string[]
		{
			"Xoắn và xoắn ~ <br/> xoắn ~ ~",
			"GunnyV2 ~ Thật tuyệt phải không nào ^^! ~ ~",
			"Yeah, thật thoải mái!"
		};

		private static string[] KillAttackChat = new string[]
		{
			"Jun Lin là cả thế giới !!"
		};

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			this.m_body.CurrentDamagePlus = 1f;
			this.m_body.CurrentShootMinus = 1f;
		}

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
			base.OnStartAttacking();
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			bool flag = false;
			int num = 0;
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X > 390 && current.X < 1110)
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
				this.KillAttack(390, 1110);
			}
			else if (this.m_attackTurn == 0)
			{
				this.AllAttack();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.PersonalAttack();
				this.m_attackTurn++;
			}
			else
			{
				this.Healing();
				this.m_attackTurn = 0;
			}
		}

		private void KillAttack(int fx, int tx)
		{
			base.Body.CurrentDamagePlus = 10f;
			int num = base.Game.Random.Next(0, NormalKingFirst.KillAttackChat.Length);
			base.Body.Say(NormalKingFirst.KillAttackChat[num], 1, 1000);
			base.Body.PlayMovie("beat", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 4000, null);
		}

		private void AllAttack()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			int num = base.Game.Random.Next(0, NormalKingFirst.AllAttackChat.Length);
			base.Body.Say(NormalKingFirst.AllAttackChat[num], 1, 0);
			base.Body.PlayMovie("beat", 1000, 0);
			base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 3000, null);
		}

		private void PersonalAttack()
		{
			int x = base.Game.Random.Next(850, 920);
			base.Body.MoveTo(x, base.Body.Y, "walk", 1000, "", ((SimpleBoss)base.Body).NpcInfo.speed, new LivingCallBack(this.NextAttack));
		}

		private void Healing()
		{
			int num = base.Game.Random.Next(0, NormalKingFirst.AddBooldChat.Length);
			base.Body.Say(NormalKingFirst.AddBooldChat[num], 1, 0);
			base.Body.SyncAtTime = true;
			base.Body.AddBlood(7000);
			base.Body.PlayMovie("renew", 1000, 4500);
		}

		private void NextAttack()
		{
			Player player = base.Game.FindRandomPlayer();
			if (player.X > base.Body.Y)
			{
				base.Body.ChangeDirection(1, 800);
			}
			else
			{
				base.Body.ChangeDirection(-1, 800);
			}
			base.Body.CurrentDamagePlus = 0.8f;
			int num = base.Game.Random.Next(0, NormalKingFirst.ShootChat.Length);
			base.Body.Say(NormalKingFirst.ShootChat[num], 1, 0);
			if (player != null)
			{
				int x = base.Game.Random.Next(player.X - 50, player.X + 50);
				if (base.Body.ShootPoint(x, player.Y, ((SimpleBoss)base.Body).NpcInfo.CurrentBallId, 1000, 10000, 1, 1f, 2300))
				{
					base.Body.PlayMovie("beat2", 1500, 0);
				}
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}
