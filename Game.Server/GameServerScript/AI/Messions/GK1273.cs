using System;
using System.Collections.Generic;
using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    // Token: 0x02000262 RID: 610
    public class GK1273 : AMissionControl
    {
        // Token: 0x06001F34 RID: 7988 RVA: 0x000E5104 File Offset: 0x000E3304
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

        // Token: 0x06001F35 RID: 7989 RVA: 0x000E5154 File Offset: 0x000E3354
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

        // Token: 0x06001F36 RID: 7990 RVA: 0x000E523C File Offset: 0x000E343C
        public override void OnStartGame()
        {
            base.OnStartGame();
            this.m_kingMoive = base.Game.Createlayer(0, 0, "kingmoive", "game.asset.living.BossBgAsset", "out", 1, 1);
            this.m_kingFront = base.Game.Createlayer(720, 495, "font", "game.asset.living.boguoKingAsset", "out", 1, 1);
            this.m_king = base.Game.CreateBoss(this.m_state, 888, 590, -1, 1, "");
            this.m_king.FallFrom(this.m_king.X, 0, "", 0, 2, 2000);
            this.m_king.SetRelateDemagemRect(-21, -87, 72, 59);
            this.m_king.AddDelay(10);
            this.m_king.Say(LanguageMgr.GetTranslation("Tất cả các bạn dân thường thấp hèn, dám tự tin trong cung điện của tôi!", Array.Empty<object>()), 0, 3000);
            this.m_kingMoive.PlayMovie("in", 9000, 0);
            this.m_kingFront.PlayMovie("in", 9000, 0);
            this.m_kingMoive.PlayMovie("out", 13000, 0);
            this.m_kingFront.PlayMovie("out", 13400, 0);
            this.turn = base.Game.TurnIndex;
            base.Game.BossCardCount = 1;
        }

        // Token: 0x06001F37 RID: 7991 RVA: 0x000E53AA File Offset: 0x000E35AA
        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        // Token: 0x06001F38 RID: 7992 RVA: 0x000E53B4 File Offset: 0x000E35B4
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

        // Token: 0x06001F39 RID: 7993 RVA: 0x000E5438 File Offset: 0x000E3638
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
                this.m_secondKing.Say(LanguageMgr.GetTranslation("Bạn tức giận tôi, tôi không tha thứ cho bạn!", Array.Empty<object>()), 0, 3000);
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

        // Token: 0x06001F3A RID: 7994 RVA: 0x000E5654 File Offset: 0x000E3854
        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return this.m_kill;
        }

        // Token: 0x06001F3B RID: 7995 RVA: 0x000E5674 File Offset: 0x000E3874
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

        // Token: 0x06001F3C RID: 7996 RVA: 0x000E5784 File Offset: 0x000E3984
        public override void DoOther()
        {
            base.DoOther();
            bool flag = this.m_king != null;
            if (flag)
            {
                bool isLiving = this.m_king.IsLiving;
                if (isLiving)
                {
                    int index = base.Game.Random.Next(0, GK1273.KillChat.Length);
                    this.m_king.Say(GK1273.KillChat[index], 0, 0);
                }
                else
                {
                    int index2 = base.Game.Random.Next(0, GK1273.KillChat.Length);
                    this.m_king.Say(GK1273.KillChat[index2], 0, 0);
                }
            }
        }

        // Token: 0x06001F3D RID: 7997 RVA: 0x000E581C File Offset: 0x000E3A1C
        public override void OnShooted()
        {
            base.OnShooted();
            bool flag = this.IsSay == 0;
            if (flag)
            {
                bool isLiving = this.m_king.IsLiving;
                if (isLiving)
                {
                    int index = base.Game.Random.Next(0, GK1273.ShootedChat.Length);
                    this.m_king.Say(GK1273.ShootedChat[index], 0, 1500);
                }
                else
                {
                    int index2 = base.Game.Random.Next(0, GK1273.ShootedChat.Length);
                    this.m_secondKing.Say(GK1273.ShootedChat[index2], 0, 1500);
                }
                this.IsSay = 1;
            }
        }

        // Token: 0x06001F3E RID: 7998 RVA: 0x000E58C3 File Offset: 0x000E3AC3
        public GK1273()
        {
        }

        // Token: 0x06001F3F RID: 7999 RVA: 0x000E58F8 File Offset: 0x000E3AF8
        // Note: this type is marked as 'beforefieldinit'.
        static GK1273()
        {
        }

        // Token: 0x0400115F RID: 4447
        private PhysicalObj m_kingMoive;

        // Token: 0x04001160 RID: 4448
        private PhysicalObj m_kingFront;

        // Token: 0x04001161 RID: 4449
        private SimpleBoss m_king;

        // Token: 0x04001162 RID: 4450
        private SimpleBoss m_secondKing;

        // Token: 0x04001163 RID: 4451
        private PhysicalObj[] m_leftWall;

        // Token: 0x04001164 RID: 4452
        private PhysicalObj[] m_rightWall;

        // Token: 0x04001165 RID: 4453
        private int m_kill;

        // Token: 0x04001166 RID: 4454
        private int m_state = 1205;

        // Token: 0x04001167 RID: 4455
        private int turn;

        // Token: 0x04001168 RID: 4456
        private int IsSay;

        // Token: 0x04001169 RID: 4457
        private int firstBossID = 1205;

        // Token: 0x0400116A RID: 4458
        private int secondBossID = 1206;

        // Token: 0x0400116B RID: 4459
        private int npcID = 1209;

        // Token: 0x0400116C RID: 4460
        private int direction;

        // Token: 0x0400116D RID: 4461
        private static string[] KillChat = new string[]
        {
            "Tôi cuối cùng cũng thoát <br/> khỏi khống chế của <br/> Matthias, thật nhức đầu! "
        };

        // Token: 0x0400116E RID: 4462
        private static string[] ShootedChat = new string[]
        {
            "Ai ya, các bạn <br/> sao lại đánh tôi? <br/> Tôi làm gì ?... ",
            "Ui~đau quá, sao phải đánh nhau, mình phải chiến đấu ?"
        };
    }
}
