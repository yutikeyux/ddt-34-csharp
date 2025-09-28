using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic.Effects;

namespace GameServerScript.AI.NPC
{
	public class BuomMaNpc: ABrain
	{
		protected Player m_targer;

		private int attackingTurn = 0;
		
		private int m_turn = 0;
		
		#region NPC Chat
		private static string[] AllAttackChat = new string[] 
		{
            "Bộ em hết dễ thương rồi haaaaaa~"	
        };
		
		private static string[] CallChat = new string[]
		{
            "Anh nghĩ em sợ hông........."
        };
		
		private static string[] KillPlayerChat = new string[]
		{
            "Dỗiiiiiiiiii......"
        };
		
		private static string[] KillAttackChat = new string[]
		{
             "Thứ kém sang.........."
        };
        #endregion

        //public override void OnBeginSelfTurn()
        //{
        //base.OnBeginSelfTurn();
        //}

        //public override void OnBeginNewTurn()
        //{
        //base.OnBeginNewTurn();
        //base.Body.CurrentDamagePlus = 1f;
        //base.Body.CurrentShootMinus = 1f;
        //}



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
					//Healing();
					this.m_turn++;
				}
				else if (this.m_turn == 1)
				{
					this.KillB();
					Healing();
					this.m_turn++;
				}
				else
				{
					this.KillC();
					Healing();
					this.m_turn = 0;
				}
			}
		}

		private void KillA()
		{	
			int index = Game.Random.Next(0, AllAttackChat.Length); //Chat
            base.Body.Say(AllAttackChat[index], 1, 0);

			base.Body.CurrentDamagePlus = 130;
			base.Body.PlayMovie("beatA", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - 500, this.m_targer.X + 500, "cry", 4000, null);
		}

		private void KillB()
		{	
			int index = Game.Random.Next(0, CallChat.Length); //Chat
            base.Body.Say(CallChat[index], 1, 0);
			
			base.Body.CurrentDamagePlus = 140;
			base.Body.PlayMovie("beatB", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - 500, this.m_targer.X + 500, "cry", 4000, null);
		}

		private void KillC()
		{	
			int index = Game.Random.Next(0, KillPlayerChat.Length); //Chat
            base.Body.Say(KillPlayerChat[index], 1, 0);

			base.Body.CurrentDamagePlus = 150;
			base.Body.PlayMovie("beatC", 2400, 0);
			base.Body.RangeAttacking(this.m_targer.X - 500, this.m_targer.X + 500, "cry", 4000, null);
		}

		
		private void KillAttack(int fx, int tx) 
		{	
			int index = Game.Random.Next(0, KillAttackChat.Length); //Chat
            base.Body.Say(KillAttackChat[index], 1, 0);
			
			base.Body.CurrentDamagePlus = 100000000f;
			base.Body.PlayMovie("beatA", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 5000, null); 
		}
		
		public void Healing()
        {
            Body.SyncAtTime = true;            
            //Body.PlayMovie("cast", 2000, 0);
            Body.AddBlood(30000);
            Body.Say("Sao mà đỡ được hỏ cưng!", 1, 0);
        }

		//public override void OnStopAttacking()
		//{
			//base.OnStopAttacking();
		//}
	}
}
