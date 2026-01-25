using System;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    // Token: 0x02000263 RID: 611
    public class GK1274 : AMissionControl
    {
        // Token: 0x06001F40 RID: 8000 RVA: 0x000E5928 File Offset: 0x000E3B28
        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            bool flag = score > 900;
            int result;
            if (flag)
            {
                result = 3;
            }
            else
            {
                bool flag2 = score > 825;
                if (flag2)
                {
                    result = 2;
                }
                else
                {
                    bool flag3 = score > 725;
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

        // Token: 0x06001F41 RID: 8001 RVA: 0x000E5978 File Offset: 0x000E3B78
        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] resources = new int[]
            {
                this.npcID,
                this.bossID
            };
            base.Game.LoadResources(resources);
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BombKingAsset");
            base.Game.LoadNpcGameOverResources(resources);
            base.Game.SetMap(1076);
        }

        // Token: 0x06001F42 RID: 8002 RVA: 0x000E5A00 File Offset: 0x000E3C00
        public override void OnStartGame()
        {
            base.OnStartGame();
            this.m_kingMoive = base.Game.Createlayer(0, 0, "kingmoive", "game.asset.living.BossBgAsset", "out", 1, 1);
            this.m_kingFront = base.Game.Createlayer(720, 455, "font", "game.asset.living.boguoKingAsset", "out", 1, 1);
            this.m_king = base.Game.CreateBoss(this.bossID, 888, 510, -1, 1, "");
            this.m_king.FallFrom(888, 510, "fall", 0, 2, 1000);
            this.m_king.SetRelateDemagemRect(-41, -187, 83, 140);
            this.m_kingMoive.PlayMovie("in", 9000, 0);
            this.m_kingFront.PlayMovie("in", 9000, 0);
            this.m_kingMoive.PlayMovie("out", 13000, 0);
            this.m_kingFront.PlayMovie("out", 13400, 0);
            this.m_king.AddDelay(16);
        }

        // Token: 0x06001F43 RID: 8003 RVA: 0x000E5B33 File Offset: 0x000E3D33
        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        // Token: 0x06001F44 RID: 8004 RVA: 0x000E5B40 File Offset: 0x000E3D40
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            this.IsSay = 0;
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

        // Token: 0x06001F45 RID: 8005 RVA: 0x000E5BCC File Offset: 0x000E3DCC
        public override bool CanGameOver()
        {
            bool flag = !this.m_king.IsLiving;
            bool result;
            if (flag)
            {
                this.m_kill++;
                result = true;
            }
            else
            {
                bool flag2 = base.Game.TurnIndex > base.Game.MissionInfo.TotalTurn - 1;
                result = flag2;
            }
            return result;
        }

        // Token: 0x06001F46 RID: 8006 RVA: 0x000E5C2C File Offset: 0x000E3E2C
        public override int UpdateUIData()
        {
            return this.m_kill;
        }

        // Token: 0x06001F47 RID: 8007 RVA: 0x000E5C44 File Offset: 0x000E3E44
        public override void OnGameOver()
        {
            base.OnGameOver();
            bool result = true;
            foreach (Player allFightPlayer in base.Game.GetAllFightPlayers())
            {
                bool isLiving = allFightPlayer.IsLiving;
                if (isLiving)
                {
                    result = false;
                }
            }
            bool flag = !this.m_king.IsLiving && !result;
            if (flag)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
        }

        // Token: 0x06001F48 RID: 8008 RVA: 0x000E5CEC File Offset: 0x000E3EEC
        public override void DoOther()
        {
            base.DoOther();
            int index = base.Game.Random.Next(0, GK1274.KillChat.Length);
            this.m_king.Say(GK1274.KillChat[index], 0, 0);
        }

        // Token: 0x06001F49 RID: 8009 RVA: 0x000E5D30 File Offset: 0x000E3F30
        public override void OnShooted()
        {
            bool flag = this.m_king.IsLiving && this.IsSay == 0;
            if (flag)
            {
                int index = base.Game.Random.Next(0, GK1274.ShootedChat.Length);
                this.m_king.Say(GK1274.ShootedChat[index], 0, 1500);
                this.IsSay = 1;
            }
        }

        // Token: 0x06001F4A RID: 8010 RVA: 0x000E5D96 File Offset: 0x000E3F96
        public GK1274()
        {
        }

        // Token: 0x06001F4B RID: 8011 RVA: 0x000E5DC3 File Offset: 0x000E3FC3
        // Note: this type is marked as 'beforefieldinit'.
        static GK1274()
        {
        }

        // Token: 0x0400116F RID: 4463
        private SimpleBoss m_king = null;

        // Token: 0x04001170 RID: 4464
        private SimpleBoss m_secondKing = null;

        // Token: 0x04001171 RID: 4465
        private int m_kill;

        // Token: 0x04001172 RID: 4466
        private int IsSay;

        // Token: 0x04001173 RID: 4467
        private int bossID = 1207;

        // Token: 0x04001174 RID: 4468
        private int npcID = 1204;

        // Token: 0x04001175 RID: 4469
        private int turn;

        // Token: 0x04001176 RID: 4470
        private PhysicalObj m_kingMoive;

        // Token: 0x04001177 RID: 4471
        private PhysicalObj m_kingFront;

        // Token: 0x04001178 RID: 4472
        private static string[] KillChat = new string[]
        {
            "Chỉ được zậy thôi sao ?",
            "Ai ya~đánh đau quá! Ah hahahaha ?",
            "A~cũng được lấm."
        };

        // Token: 0x04001179 RID: 4473
        private static string[] ShootedChat = new string[]
        {
            "Tưởng thắng rồi sao ? Chưa kết thúc đâu! Tôi còn quay lại!"
        };
    }
}
