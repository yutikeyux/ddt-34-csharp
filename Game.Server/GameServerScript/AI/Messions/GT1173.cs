using System;
using System.Collections.Generic;
using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    // Token: 0x02000266 RID: 614
    public class GT1173 : AMissionControl
    {
        // Token: 0x06001F61 RID: 8033 RVA: 0x000E6B5C File Offset: 0x000E4D5C
        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            bool flag = score > 1150;
            int result;
            if (flag)
            {
                result = 3;
            }
            else
            {
                bool flag2 = score > 925;
                if (flag2)
                {
                    result = 2;
                }
                else
                {
                    bool flag3 = score > 700;
                    if (flag3)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            return result;
        }

        // Token: 0x06001F62 RID: 8034 RVA: 0x000E6BAC File Offset: 0x000E4DAC
        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            base.Game.AddLoadingFile(1, "bombs/61.swf", "tank.resource.bombs.Bomb61");
            base.Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_left");
            base.Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_right");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.boguoLeaderAsset");
            int[] resources = new int[]
            {
                this.firstBossID,
                this.secondBossID,
                this.npcID
            };
            base.Game.LoadResources(resources);
            int[] gameOverResources = new int[]
            {
                this.firstBossID
            };
            base.Game.LoadNpcGameOverResources(gameOverResources);
            base.Game.SetMap(1076);
        }

        // Token: 0x06001F63 RID: 8035 RVA: 0x000E6C94 File Offset: 0x000E4E94
        public override void OnStartGame()
        {
            base.OnStartGame();
            this.m_kingMoive = base.Game.Createlayer(0, 0, "kingmoive", "game.asset.living.BossBgAsset", "out", 1, 1);
            this.m_kingFront = base.Game.Createlayer(720, 495, "font", "game.asset.living.boguoKingAsset", "out", 1, 1);
            this.m_king = base.Game.CreateBoss(this.m_state, 888, 590, -1, 1, "");
            this.m_king.FallFrom(this.m_king.X, 0, "", 0, 2, 2000);
            this.m_king.SetRelateDemagemRect(-21, -87, 72, 59);
            this.m_king.AddDelay(10);
            this.m_king.Say(LanguageMgr.GetTranslation("Seni zavallı! Nasıl cürret edersiniz?", Array.Empty<object>()), 0, 3000);
            this.m_kingMoive.PlayMovie("in", 9000, 0);
            this.m_kingFront.PlayMovie("in", 9000, 0);
            this.m_kingMoive.PlayMovie("out", 13000, 0);
            this.m_kingFront.PlayMovie("out", 13400, 0);
            this.turn = base.Game.TurnIndex;
            base.Game.BossCardCount = 1;
        }

        // Token: 0x06001F64 RID: 8036 RVA: 0x000E6E02 File Offset: 0x000E5002
        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        // Token: 0x06001F65 RID: 8037 RVA: 0x000E6E0C File Offset: 0x000E500C
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            bool flag = base.Game.TurnIndex > this.turn + 1;
            if (flag)
            {
                bool flag2 = this.m_kingMoive != null;
                if (flag2)
                {
                    base.Game.RemovePhysicalObj(this.m_kingMoive, true);
                    this.m_kingMoive = null;
                }
                bool flag3 = this.m_kingFront != null;
                if (flag3)
                {
                    base.Game.RemovePhysicalObj(this.m_kingFront, true);
                    this.m_kingFront = null;
                }
            }
        }

        // Token: 0x06001F66 RID: 8038 RVA: 0x000E6E90 File Offset: 0x000E5090
        public override bool CanGameOver()
        {
            base.CanGameOver();
            bool flag = !this.m_king.IsLiving && this.m_state == this.firstBossID;
            if (flag)
            {
                this.m_state++;
            }
            bool flag2 = this.m_state == this.secondBossID && this.m_secondKing == null;
            if (flag2)
            {
                this.m_secondKing = base.Game.CreateBoss(this.m_state, this.m_king.X, this.m_king.Y, this.m_king.Direction, 1, "");
                base.Game.RemoveLiving(this.m_king.Id);
                bool flag3 = this.m_secondKing.Direction == 1;
                if (flag3)
                {
                    this.m_secondKing.SetRect(-21, -87, 72, 59);
                }
                this.m_secondKing.SetRelateDemagemRect(-21, -87, 72, 59);
                this.m_secondKing.Say(LanguageMgr.GetTranslation("Bana bu yaptığınızı unutmayacağım!", Array.Empty<object>()), 0, 3000);
                List<Player> allFightPlayers = base.Game.GetAllFightPlayers();
                Player player = base.Game.FindRandomPlayer();
                int minDelay = 0;
                bool flag4 = player != null;
                if (flag4)
                {
                    minDelay = player.Delay;
                }
                foreach (Player item in allFightPlayers)
                {
                    bool flag5 = item.Delay < minDelay;
                    if (flag5)
                    {
                        minDelay = item.Delay;
                    }
                }
                this.m_secondKing.AddDelay(minDelay - 2000);
                this.turn = base.Game.TurnIndex;
            }
            bool flag6 = this.m_secondKing != null && !this.m_secondKing.IsLiving;
            bool result;
            if (flag6)
            {
                this.direction = this.m_secondKing.Direction;
                this.m_kill++;
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        // Token: 0x06001F67 RID: 8039 RVA: 0x000E70AC File Offset: 0x000E52AC
        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return this.m_kill;
        }

        // Token: 0x06001F68 RID: 8040 RVA: 0x000E70CC File Offset: 0x000E52CC
        public override void OnGameOver()
        {
            base.OnGameOver();
            bool flag = this.m_state == this.secondBossID && !this.m_secondKing.IsLiving;
            if (flag)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
            List<LoadingFileInfo> list = new List<LoadingFileInfo>();
            list.Add(new LoadingFileInfo(2, "image/map/show7.jpg", ""));
            base.Game.SendLoadResource(list);
            this.m_leftWall = base.Game.FindPhysicalObjByName("wallLeft");
            this.m_rightWall = base.Game.FindPhysicalObjByName("wallRight");
            for (int i = 0; i < this.m_leftWall.Length; i++)
            {
                base.Game.RemovePhysicalObj(this.m_leftWall[i], true);
            }
            for (int j = 0; j < this.m_rightWall.Length; j++)
            {
                base.Game.RemovePhysicalObj(this.m_rightWall[j], true);
            }
        }

        // Token: 0x06001F69 RID: 8041 RVA: 0x000E71DC File Offset: 0x000E53DC
        public override void DoOther()
        {
            base.DoOther();
            bool flag = this.m_king != null;
            if (flag)
            {
                bool isLiving = this.m_king.IsLiving;
                if (isLiving)
                {
                    int index = base.Game.Random.Next(0, GT1173.KillChat.Length);
                    this.m_king.Say(GT1173.KillChat[index], 0, 0);
                }
                else
                {
                    int index2 = base.Game.Random.Next(0, GT1173.KillChat.Length);
                    this.m_king.Say(GT1173.KillChat[index2], 0, 0);
                }
            }
        }

        // Token: 0x06001F6A RID: 8042 RVA: 0x000E7274 File Offset: 0x000E5474
        public override void OnShooted()
        {
            base.OnShooted();
            bool flag = this.IsSay == 0;
            if (flag)
            {
                bool isLiving = this.m_king.IsLiving;
                if (isLiving)
                {
                    int index = base.Game.Random.Next(0, GT1173.ShootedChat.Length);
                    this.m_king.Say(GT1173.ShootedChat[index], 0, 1500);
                }
                else
                {
                    int index2 = base.Game.Random.Next(0, GT1173.ShootedChat.Length);
                    this.m_secondKing.Say(GT1173.ShootedChat[index2], 0, 1500);
                }
                this.IsSay = 1;
            }
        }

        // Token: 0x0400118C RID: 4492
        private PhysicalObj m_kingMoive;

        // Token: 0x0400118D RID: 4493
        private PhysicalObj m_kingFront;

        // Token: 0x0400118E RID: 4494
        private SimpleBoss m_king;

        // Token: 0x0400118F RID: 4495
        private SimpleBoss m_secondKing;

        // Token: 0x04001190 RID: 4496
        private PhysicalObj[] m_leftWall;

        // Token: 0x04001191 RID: 4497
        private PhysicalObj[] m_rightWall;

        // Token: 0x04001192 RID: 4498
        private int m_kill;

        // Token: 0x04001193 RID: 4499
        private int m_state = 1105;

        // Token: 0x04001194 RID: 4500
        private int turn;

        // Token: 0x04001195 RID: 4501
        private int IsSay;

        // Token: 0x04001196 RID: 4502
        private int firstBossID = 1105;

        // Token: 0x04001197 RID: 4503
        private int secondBossID = 1106;

        // Token: 0x04001198 RID: 4504
        private int npcID = 1110;

        // Token: 0x04001199 RID: 4505
        private int direction;

        // Token: 0x0400119A RID: 4506
        private static string[] KillChat = new string[]
        {
            "Sonunda Matthias'ın kontrolünden kurtuldum, ne büyük bir baş ağrısıydı!"
        };

        // Token: 0x0400119B RID: 4507
        private static string[] ShootedChat = new string[]
        {
            "Aman Tanrım, neden bana vuruyorsunuz? Ne yaptım ben?... ",
            "Ah, canım acıyor! Neden savaşmak zorundayız? Savaşmak zorundayız!"
        };
    }
}
