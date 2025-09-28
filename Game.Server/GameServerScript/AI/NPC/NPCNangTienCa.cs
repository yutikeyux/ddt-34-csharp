using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.NPC
{
	public class NPCNangTienCa: ABrain
	{
		protected Player m_targer;

		private int m_turn = 0;
		
		#region NPC Chat
		private static string[] AllAttackChat = new string[] 
		{
            "Bắn tim nà~"	
        };
		
		private static string[] CallChat = new string[]
		{
            "Thiện Style..."
        };
		
		private static string[] KillPlayerChat = new string[]
		{
            "Bổn cung tới trể , náo nhiệt quá ta!"
        };
		
		private static string[] KillAttackChat = new string[]
		{
             "Tôi năm nay 70 tuổi , mà tôi chưa gặp trường hợp này bao giờ."
        };
		#endregion

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
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			bool flag = false;
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && Math.Abs(current.X - base.Body.X) < 150)
				{
					flag = true;
				}
			}
			if (flag)
			{
				//this.KillAttack(base.Body.X - 100, base.Body.X + 100);
				this.KillAttack(base.Body.X - 500, base.Body.X + 500);
			}
			else
			{
				this.m_targer = base.Game.FindRandomPlayer();
				if (this.m_turn == 0)
				{
					this.KillA();
					this.m_turn++;
				}
				else if (this.m_turn == 1)
				{
					this.KillB();
					this.m_turn++;
				}
				else
				{
					this.KillC();
					this.m_turn = 0;
				}
			}
		}

		private void KillA()
		{	
			int index = Game.Random.Next(0, AllAttackChat.Length); //Chat
            base.Body.Say(AllAttackChat[index], 1, 0);

			base.Body.CurrentDamagePlus = 80;
			base.Body.PlayMovie("beatA", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - 500, this.m_targer.X + 500, "cry", 4000, null);
		}

		private void KillB()
		{	
			int index = Game.Random.Next(0, CallChat.Length); //Chat
            base.Body.Say(CallChat[index], 1, 0);
			
			base.Body.CurrentDamagePlus = 90;
			base.Body.PlayMovie("beatB", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - 500, this.m_targer.X + 500, "cry", 4000, null);
		}

		private void KillC()
		{	
			int index = Game.Random.Next(0, KillPlayerChat.Length); //Chat
            base.Body.Say(KillPlayerChat[index], 1, 0);

			base.Body.CurrentDamagePlus = 100;
			base.Body.PlayMovie("beatC", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - 500, this.m_targer.X + 500, "cry", 4000, null);
		}

		
		private void KillAttack(int fx, int tx) 
		{	
			int index = Game.Random.Next(0, KillAttackChat.Length); //Chat
            base.Body.Say(KillAttackChat[index], 1, 0);
			
			base.Body.CurrentDamagePlus = 1000000f;
			base.Body.PlayMovie("beatA", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 5000, null); 
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}
