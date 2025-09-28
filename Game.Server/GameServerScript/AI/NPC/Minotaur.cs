using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.NPC
{
    public class Minotaur : ABrain
    {
        private int m_attackTurn = 0;

        #region NPC 说话内容
        private static string[] AllAttackChat = new string[] { 
            "要地震喽！！<br/>各位请扶好哦",
       
            "把你武器震下来！",
       
            "看你们能还经得起几下！！"
        };

        private static string[] ShootChat = new string[]{
             "让你知道什么叫百发百中！",
                               
             "送你一个球~你可要接好啦",

             "你们这群无知的低等庶民"
        };

        private static string[] ShootedChat = new string[]{
           "哎呀~~你们为什么要攻击我？<br/>我在干什么？",
                   
            "噢~~好痛!我为什么要战斗？<br/>我必须战斗…"

        };

        private static string[] AddBooldChat = new string[]{
            "扭啊扭~<br/>扭啊扭~~",
               
            "哈利路亚~<br/>路亚路亚~~",
                
            "呀呀呀，<br/>好舒服啊！"
         
        };

        private static string[] KillAttackChat = new string[]{
            "君临天下！！"
        };

        #endregion

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            m_body.CurrentDamagePlus = 1;
            m_body.CurrentShootMinus = 1;
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            Body.Direction = Game.FindlivingbyDir(Body);
            bool result = false;
            int maxdis = 0;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && player.X > 390 && player.X < 1110)
                {
                    int dis = (int)Body.Distance(player.X, player.Y);
                    if (dis > maxdis)
                    {
                        maxdis = dis;
                    }
                    result = true;
                }
            }

            if (m_attackTurn == 0)
            {
                AllAttack();
                m_attackTurn++;
            }
            else if (m_attackTurn == 1)
            {
                PersonalAttack();
                m_attackTurn++;
            }
            else
            {
                Healing();
                m_attackTurn = 0;
            }
		}

        private void KillAttack(int fx, int tx)
        {
            Body.CurrentDamagePlus = 0.5f;

            int index = Game.Random.Next(0, AllAttackChat.Length);
            Body.Say(AllAttackChat[index], 1, 0);

            Body.PlayMovie("beatA", 1000, 0);
            Body.RangeAttacking(Body.X - 1000, Body.X + 1000, "in", 3000, null);
        }

        private void AllAttack()
        {
            Body.CurrentDamagePlus = 0.5f;

            int index = Game.Random.Next(0, AllAttackChat.Length);
            Body.Say(AllAttackChat[index], 1, 0);

            Body.PlayMovie("beatB", 1000, 0);
            Body.RangeAttacking(Body.X - 1000, Body.X + 1000, "cry", 3000, null);
        }

        private void PersonalAttack()
        {
			int dis = Game.Random.Next(670, 880);
            Body.MoveTo(dis, Body.Y, "walk", 1000,"", ((SimpleBoss)Body).NpcInfo.speed, new LivingCallBack(NextAttack));
        }

        private void Healing()
        {
            int index = Game.Random.Next(0, AddBooldChat.Length);
            Body.Say(AddBooldChat[index], 1, 0);
            Body.SyncAtTime = true;
            Body.AddBlood(5000);
            Body.PlayMovie("beatC", 1000, 4500);
        }


        private void NextAttack()
        {
            Body.CurrentDamagePlus = 0.5f;

            int index = Game.Random.Next(0, AllAttackChat.Length);
            Body.Say(AllAttackChat[index], 1, 0);

            Body.PlayMovie("beatE", 1000, 0);
            Body.RangeAttacking(Body.X - 1000, Body.X + 1000, "cry", 3000, null);
		}
		
		private void Healing1()
        {
            int index = Game.Random.Next(0, AddBooldChat.Length);
            Body.Say(AddBooldChat[index], 1, 0);
            Body.SyncAtTime = true;
            Body.AddBlood(8000);
            Body.PlayMovie("beatC", 1000, 4500);
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
    }
}
