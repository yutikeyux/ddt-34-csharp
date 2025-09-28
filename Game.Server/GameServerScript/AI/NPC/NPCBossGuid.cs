using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using System.Threading.Tasks;
using System.Threading;

namespace GameServerScript.AI.NPC
{
    public class RetSaMac : ABrain
    {
        private int m_attackTurn = 0;

        public int currentCount = 0;

        public int Dander = 0;

        private PhysicalObj moive;

        Player target = null;

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }
		
		#region NPC Chat
		private static string[] AllAttackChat = new string[] 
		{
            "Thí chủ xin hãy tự trọng...!"	
        };
		
		private static string[] KillAttackChat = new string[]
		{
             "Ối Giờiiiiiiiiiiiiiii Ôiiiiiiiiiiiiiiiiiiii...." 
        };
		#endregion


        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            Body.CurrentDamagePlus = 1;
            Body.CurrentShootMinus = 1;
            Body.SetRect(((SimpleBoss)Body).NpcInfo.X, ((SimpleBoss)Body).NpcInfo.Y, ((SimpleBoss)Body).NpcInfo.Width, ((SimpleBoss)Body).NpcInfo.Height);

            if (Body.Direction == -1)
            {
                Body.SetRect(((SimpleBoss)Body).NpcInfo.X, ((SimpleBoss)Body).NpcInfo.Y, ((SimpleBoss)Body).NpcInfo.Width, ((SimpleBoss)Body).NpcInfo.Height);
            }
            else
            {
                Body.SetRect(-((SimpleBoss)Body).NpcInfo.X - ((SimpleBoss)Body).NpcInfo.Width, ((SimpleBoss)Body).NpcInfo.Y, ((SimpleBoss)Body).NpcInfo.Width, ((SimpleBoss)Body).NpcInfo.Height);
            }

        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            Body.Direction = Game.FindlivingbyDir(Body);
            bool result = false;
            int maxdis = 0;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && player.X > 857 && player.X < 1440)
                {
                    int dis = (int)Body.Distance(player.X, player.Y);
                    if (dis > maxdis)
                    {
                        maxdis = dis;
                    }
                    result = true;
                }
            }

            if (result)
            {
                KillAttack(Body.X - 10000, Body.X + 10000);

                return;
            }

            if (m_attackTurn == 0)
            {
                Moving();
				KillB();
				Healing();
                m_attackTurn++;
            }            
            else if (m_attackTurn == 1)
            {
                AllAttack();
				Healing();
                m_attackTurn++;
            }
			else
			{
				KillC();
				Healing();
                m_attackTurn = 0;
			}
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
		
        private void KillAttack(int fx, int tx)
        {
			int index = Game.Random.Next(0, KillAttackChat.Length); //Chat
            base.Body.Say(KillAttackChat[index], 1, 0);
			
            ChangeDirection(3);
            Body.PlayMovie("beatB", 1000, 0);
			Thread.Sleep(5000);
            target = Game.FindRandomPlayer();
            Body.CurrentDamagePlus = 100000f;
            Body.RangeAttacking(Body.X - 10000, Body.X + 10000, "cry", 3300, null);
            Body.CallFuction(CreateEffect, 3300);
            Body.CallFuction(Out, 4600);
        }
        private void AllAttack()
        {
			int index = Game.Random.Next(0, AllAttackChat.Length); //Chat
            base.Body.Say(AllAttackChat[index], 1, 0);
			
            ChangeDirection(3);
            Body.PlayMovie("beatA", 1000, 0);
            target = Game.FindRandomPlayer();
            Body.CurrentDamagePlus = 170f;
            Body.RangeAttacking(Body.X - 10000, Body.X + 10000, "cry", 3300, null);
            Body.CallFuction(CreateEffect, 3300);
            Body.CallFuction(Out, 4600);
        }
		
		private void KillC()
        {
			int index = Game.Random.Next(0, AllAttackChat.Length); //Chat
            base.Body.Say(AllAttackChat[index], 1, 0);
			
            ChangeDirection(3);
            Body.PlayMovie("beatC", 1000, 0);
            target = Game.FindRandomPlayer();
            Body.CurrentDamagePlus = 180f;
            Body.RangeAttacking(Body.X - 10000, Body.X + 10000, "cry", 3300, null);
            Body.CallFuction(CreateEffect, 3300);
            Body.CallFuction(Out, 4600);
        }
		
		private void KillB()
        {
			int index = Game.Random.Next(0, AllAttackChat.Length); //Chat
            base.Body.Say(AllAttackChat[index], 1, 0);
			
            ChangeDirection(3);
            Body.PlayMovie("beatB", 1000, 0);
            target = Game.FindRandomPlayer();
            Body.CurrentDamagePlus = 190f;
            Body.RangeAttacking(Body.X - 10000, Body.X + 10000, "cry", 3300, null);
            Body.CallFuction(CreateEffect, 3300);
            Body.CallFuction(Out, 4600);
        }
		
		
		public void Healing()
        {
            Body.SyncAtTime = true;            
            //Body.PlayMovie("cast", 2000, 0);
            Body.AddBlood(40000);
            Body.Say("Sao mà đỡ được hở cưng!", 1, 0);
        }
		
        public void CreateEffect()
        {
            if (target != null)
            {                
                if (target.X < 1000)
                {          
                    moive = ((PVEGame)Game).Createlayer(target.X, target.Y, "effect", "asset.game.eight.xiezi", "beatA", 1, 0);
                }
                else if (target.X < 1000)
                {
                    moive = ((PVEGame)Game).Createlayer(target.X, target.Y, "effect", "asset.game.eight.xiezi", "beatB", 1, 0);
                }
				else
				{
					moive = ((PVEGame)Game).Createlayer(target.X, target.Y, "effect", "asset.game.eight.xiezi", "beatC", 1, 0);
				}
            }
        }       

        private void Out()
        {
            ((PVEGame)Game).SendGameFocus(Body, 1000, 2000);
            Body.PlayMovie("in", 1000, 0);
            if (moive != null)
            {
                Game.RemovePhysicalObj(moive, true);
                moive = null;
            }
            
        }
        private void Moving()
        {
			
            ChangeDirection(3);
            int dis = Game.Random.Next(990, 1300);
            int direction = Body.Direction;
            Body.MoveTo(dis, Body.Y, "walk", 1000, "", ((SimpleBoss)Body).NpcInfo.speed);
            Body.ChangeDirection(Game.FindlivingbyDir(Body), 3000);
        } 
        private void ChangeDirection(int count)
        {
            int direction = Body.Direction;
            for (int i = 0; i < count; i++)
            {
                Body.ChangeDirection(-direction, i * 200 + 100);
                Body.ChangeDirection(direction, (i + 1) * 100 + i * 200);
            }
        }
    }
}
