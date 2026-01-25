using System;
using System.Collections.Generic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    // Token: 0x02000260 RID: 608
    public class GK1271 : AMissionControl
    {
        // Token: 0x06001F1F RID: 7967 RVA: 0x000E4374 File Offset: 0x000E2574
        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            bool flag = score > 930;
            int result;
            if (flag)
            {
                result = 3;
            }
            else
            {
                bool flag2 = score > 850;
                if (flag2)
                {
                    result = 2;
                }
                else
                {
                    bool flag3 = score > 775;
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

        // Token: 0x06001F20 RID: 7968 RVA: 0x000E43C4 File Offset: 0x000E25C4
        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] resources = new int[]
            {
                this.redNpcID,
                this.blueNpcID
            };
            base.Game.LoadResources(resources);
            base.Game.SetMap(1072);
        }

        // Token: 0x06001F21 RID: 7969 RVA: 0x000E4410 File Offset: 0x000E2610
        public override void OnStartGame()
        {
            base.OnStartGame();
            for (int i = 0; i < 4; i++)
            {
                this.redTotalCount++;
                bool flag = i < 1;
                if (flag)
                {
                    this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 900 + (i + 1) * 100, 505, -1, 1));
                }
                else
                {
                    bool flag2 = i < 3;
                    if (flag2)
                    {
                        this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 920 + (i + 1) * 100, 505, -1, 1));
                    }
                    else
                    {
                        this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 1000 + (i + 1) * 100, 515, -1, 1));
                    }
                }
            }
            this.blueTotalCount++;
            this.blueNpc.Add(base.Game.CreateNpc(this.blueNpcID, 1467, 495, -1, 1));
        }

        // Token: 0x06001F22 RID: 7970 RVA: 0x000E452C File Offset: 0x000E272C
        public override void OnNewTurnStarted()
        {
            this.redCount = this.redTotalCount - this.dieRedCount;
            this.blueCount = this.blueTotalCount - this.dieBlueCount;
            bool flag = base.Game.GetLivedLivings().Count == 0;
            if (flag)
            {
                base.Game.PveGameDelay = 0;
            }
            bool flag2 = base.Game.TurnIndex <= 1 || base.Game.CurrentPlayer.Delay <= base.Game.PveGameDelay || (this.blueCount == 3 && this.redCount == 12);
            if (!flag2)
            {
                bool flag3 = this.redTotalCount < 12 && this.blueTotalCount < 3;
                if (flag3)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        this.redTotalCount++;
                        bool flag4 = i < 1;
                        if (flag4)
                        {
                            this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 900 + (i + 1) * 100, 505, -1, 1));
                        }
                        else
                        {
                            bool flag5 = i < 3;
                            if (flag5)
                            {
                                this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 920 + (i + 1) * 100, 505, -1, 1));
                            }
                            else
                            {
                                this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 1000 + (i + 1) * 100, 515, -1, 1));
                            }
                        }
                    }
                    this.blueTotalCount++;
                    this.blueNpc.Add(base.Game.CreateNpc(this.blueNpcID, 1467, 495, -1, 1));
                }
                else
                {
                    bool flag6 = this.redCount >= 12;
                    if (!flag6)
                    {
                        bool flag7 = 12 - this.redCount >= 4;
                        if (flag7)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                bool flag8 = this.redTotalCount < 15 && this.redCount != 12;
                                if (flag8)
                                {
                                    this.redTotalCount++;
                                    bool flag9 = j < 1;
                                    if (flag9)
                                    {
                                        this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 900 + (j + 1) * 100, 505, -1, 1));
                                    }
                                    else
                                    {
                                        bool flag10 = j < 3;
                                        if (flag10)
                                        {
                                            this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 920 + (j + 1) * 100, 505, -1, 1));
                                        }
                                        else
                                        {
                                            this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 1000 + (j + 1) * 100, 515, -1, 1));
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            bool flag11 = 12 - this.redCount > 0;
                            if (flag11)
                            {
                                for (int k = 0; k < 12 - this.redCount; k++)
                                {
                                    bool flag12 = this.redTotalCount < 15 && this.redCount != 12;
                                    if (flag12)
                                    {
                                        this.redTotalCount++;
                                        bool flag13 = k < 1;
                                        if (flag13)
                                        {
                                            this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 900 + (k + 1) * 100, 505, -1, 1));
                                        }
                                        else
                                        {
                                            bool flag14 = k < 3;
                                            if (flag14)
                                            {
                                                this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 920 + (k + 1) * 100, 505, -1, 1));
                                            }
                                            else
                                            {
                                                this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 1000 + (k + 1) * 100, 515, -1, 1));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        bool flag15 = this.blueCount < 3 && this.blueTotalCount < 5;
                        if (flag15)
                        {
                            this.blueTotalCount++;
                            this.blueNpc.Add(base.Game.CreateNpc(this.blueNpcID, 1467, 495, -1, 1));
                        }
                    }
                }
            }
        }

        // Token: 0x06001F23 RID: 7971 RVA: 0x000E49BA File Offset: 0x000E2BBA
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        // Token: 0x06001F24 RID: 7972 RVA: 0x000E49C4 File Offset: 0x000E2BC4
        public override bool CanGameOver()
        {
            bool result = true;
            this.dieRedCount = 0;
            this.dieBlueCount = 0;
            foreach (SimpleNpc item in this.redNpc)
            {
                bool isLiving = item.IsLiving;
                if (isLiving)
                {
                    result = false;
                }
                else
                {
                    this.dieRedCount++;
                }
            }
            foreach (SimpleNpc item2 in this.blueNpc)
            {
                bool isLiving2 = item2.IsLiving;
                if (isLiving2)
                {
                    result = false;
                }
                else
                {
                    this.dieBlueCount++;
                }
            }
            bool flag = result && this.redTotalCount == 15 && this.blueTotalCount == 5;
            bool result2;
            if (flag)
            {
                base.Game.IsWin = true;
                result2 = true;
            }
            else
            {
                bool flag2 = base.Game.TurnIndex > base.Game.MissionInfo.TotalTurn - 1;
                result2 = flag2;
            }
            return result2;
        }

        // Token: 0x06001F25 RID: 7973 RVA: 0x000E4B0C File Offset: 0x000E2D0C
        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return base.Game.TotalKillCount;
        }

        // Token: 0x06001F26 RID: 7974 RVA: 0x000E4B30 File Offset: 0x000E2D30
        public override void OnGameOver()
        {
            base.OnGameOver();
            bool flag = base.Game.GetLivedLivings().Count == 0;
            if (flag)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
        }

        // Token: 0x06001F27 RID: 7975 RVA: 0x000E4B7A File Offset: 0x000E2D7A
        public GK1271()
        {
        }

        // Token: 0x0400114D RID: 4429
        private List<SimpleNpc> redNpc = new List<SimpleNpc>();

        // Token: 0x0400114E RID: 4430
        private List<SimpleNpc> blueNpc = new List<SimpleNpc>();

        // Token: 0x0400114F RID: 4431
        private int redCount;

        // Token: 0x04001150 RID: 4432
        private int blueCount;

        // Token: 0x04001151 RID: 4433
        private int redTotalCount;

        // Token: 0x04001152 RID: 4434
        private int blueTotalCount;

        // Token: 0x04001153 RID: 4435
        private int dieRedCount;

        // Token: 0x04001154 RID: 4436
        private int dieBlueCount;

        // Token: 0x04001155 RID: 4437
        private int redNpcID = 1201;

        // Token: 0x04001156 RID: 4438
        private int blueNpcID = 1202;
    }
}
