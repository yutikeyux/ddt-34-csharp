using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using System.Drawing;
using Bussiness;

namespace GameServerScript.AI.NPC
{
    public class Ga : ABrain
    {
        private int m_attackTurn = 0;

        private int npcID = 12017;

        private int isSay = 0;

        private Point[] brithPoint = { new Point(979, 630), new Point(1013, 630), new Point(1052, 630), new Point(1088, 630), new Point(1142, 630) };

        #region NPC 说话内容
        private static string[] AllAttackChat = new string[] {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg1"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg2"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg3")
        };

        private static string[] ShootChat = new string[]{
            "Nguy quá!!! Các Dũng sĩ mau mau tiêu những tên gian ác và cứu chúng tôi với"
			
        };

        private static string[] KillPlayerChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg6"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg7")
        };

        private static string[] CallChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg8"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg9")

        };

        private static string[] JumpChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg10"),

             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg11"),

             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg12")
        };

        private static string[] KillAttackChat = new string[]{
             LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg13"),

              LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg14")
        };

        private static string[] ShootedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg15"),

            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg16")

        };

        private static string[] DiedChat = new string[]{
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NormalQueenAntAi.msg17")
        };

        #endregion

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            Body.CurrentDamagePlus = 1;
            Body.CurrentShootMinus = 1;

            isSay = 0;
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
                if (player.IsLiving && player.X > 1169 && player.X < Game.Map.Info.ForegroundWidth + 1)
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
                PersonalAttack();
                m_attackTurn++;
            }
			
			else if (m_attackTurn == 1)
            {
                PersonalAttack();
                m_attackTurn++;
            }
			
			else
            {
                PersonalAttack();
                m_attackTurn = 0;
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
        
        private void PersonalAttack()
        {
            int index = Game.Random.Next(0, ShootChat.Length);
            Body.Say(ShootChat[index], 1, 0);
            int dis = Game.Random.Next(670, 880);  
        }
	}
}