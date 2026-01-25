using System;
using System.Collections.Generic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    // Token: 0x0200025E RID: 606
    public class GD1071 : AMissionControl
    {
        // Token: 0x06001F0A RID: 7946 RVA: 0x000E3960 File Offset: 0x000E1B60
        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            bool flag = score > 600;
            int result;
            if (flag)
            {
                result = 3;
            }
            else
            {
                bool flag2 = score > 520;
                if (flag2)
                {
                    result = 2;
                }
                else
                {
                    bool flag3 = score > 450;
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

        // Token: 0x06001F0B RID: 7947 RVA: 0x000E39B0 File Offset: 0x000E1BB0
        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] npcIDleri = new int[]
            {
                1001
            };
            base.Game.LoadResources(npcIDleri);
            base.Game.LoadNpcGameOverResources(npcIDleri);
            base.Game.SetMap(1072);
        }

        // Token: 0x06001F0C RID: 7948 RVA: 0x000E3A00 File Offset: 0x000E1C00
        public override void OnStartGame()
        {
            base.OnStartGame();
            bool flag = base.Game.GetLivedLivings().Count == 0;
            if (flag)
            {
                base.Game.PveGameDelay = 0;
            }
            for (int i = 0; i < 4; i++)
            {
                this.bogolar++;
                bool flag2 = i < 1;
                if (flag2)
                {
                    this.Bogo_Liste.Add(base.Game.CreateNpc(this.mavi_bogo, 900 + (i + 1) * 100, 505, -1, 1));
                }
                else
                {
                    bool flag3 = i < 3;
                    if (flag3)
                    {
                        this.Bogo_Liste.Add(base.Game.CreateNpc(this.mavi_bogo, 920 + (i + 1) * 100, 505, -1, 1));
                    }
                    else
                    {
                        this.Bogo_Liste.Add(base.Game.CreateNpc(this.mavi_bogo, 1000 + (i + 1) * 100, 515, -1, 1));
                    }
                }
            }
            this.bogolar++;
            this.Bogo_Liste.Add(base.Game.CreateNpc(this.mavi_bogo, 1467, 495, -1, 1));
        }

        // Token: 0x06001F0D RID: 7949 RVA: 0x000E3B44 File Offset: 0x000E1D44
        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
            bool flag = base.Game.GetLivedLivings().Count == 0;
            if (flag)
            {
                base.Game.PveGameDelay = 0;
            }
            bool flag2 = base.Game.TurnIndex <= 1 || base.Game.CurrentPlayer.Delay <= base.Game.PveGameDelay;
            if (!flag2)
            {
                for (int i = 0; i < 4; i++)
                {
                    bool flag3 = this.bogolar < 15;
                    if (flag3)
                    {
                        this.bogolar++;
                        bool flag4 = i < 1;
                        if (flag4)
                        {
                            this.Bogo_Liste.Add(base.Game.CreateNpc(this.mavi_bogo, 900 + (i + 1) * 100, 505, -1, 1));
                        }
                        else
                        {
                            bool flag5 = i < 3;
                            if (flag5)
                            {
                                this.Bogo_Liste.Add(base.Game.CreateNpc(this.mavi_bogo, 920 + (i + 1) * 100, 505, -1, 1));
                            }
                            else
                            {
                                this.Bogo_Liste.Add(base.Game.CreateNpc(this.mavi_bogo, 1000 + (i + 1) * 100, 515, -1, 1));
                            }
                        }
                    }
                }
                bool flag6 = this.bogolar < 15;
                if (flag6)
                {
                    this.bogolar++;
                    this.Bogo_Liste.Add(base.Game.CreateNpc(this.mavi_bogo, 1467, 495, -1, 1));
                }
            }
        }

        // Token: 0x06001F0E RID: 7950 RVA: 0x000E3CEC File Offset: 0x000E1EEC
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        // Token: 0x06001F0F RID: 7951 RVA: 0x000E3CF8 File Offset: 0x000E1EF8
        public override bool CanGameOver()
        {
            bool flag = true;
            base.CanGameOver();
            this.pembe_bogo = 0;
            foreach (SimpleNpc item in this.Bogo_Liste)
            {
                bool isLiving = item.IsLiving;
                if (isLiving)
                {
                    flag = false;
                }
                else
                {
                    this.pembe_bogo++;
                }
            }
            bool flag2 = flag && this.pembe_bogo == 15;
            bool result;
            if (flag2)
            {
                base.Game.IsWin = true;
                result = true;
            }
            else
            {
                bool flag3 = base.Game.TurnIndex > base.Game.MissionInfo.TotalTurn - 1;
                result = flag3;
            }
            return result;
        }

        // Token: 0x06001F10 RID: 7952 RVA: 0x000E3DD4 File Offset: 0x000E1FD4
        public override int UpdateUIData()
        {
            return base.Game.TotalKillCount;
        }

        // Token: 0x06001F11 RID: 7953 RVA: 0x000E3DF4 File Offset: 0x000E1FF4
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

        // Token: 0x06001F12 RID: 7954 RVA: 0x000E3E3E File Offset: 0x000E203E
        public GD1071()
        {
            this.Bogo_Liste = new List<SimpleNpc>();
            this.mavi_bogo = 1001;
        }

        // Token: 0x04001141 RID: 4417
        private List<SimpleNpc> Bogo_Liste;

        // Token: 0x04001142 RID: 4418
        private int bogolar;

        // Token: 0x04001143 RID: 4419
        private int pembe_bogo;

        // Token: 0x04001144 RID: 4420
        private int mavi_bogo;
    }
}
