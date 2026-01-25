using System;
using System.Collections.Generic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    // Token: 0x02000264 RID: 612
    public class GT1171 : AMissionControl
    {
        // Token: 0x06001F4C RID: 8012 RVA: 0x000E5DFC File Offset: 0x000E3FFC
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

        // Token: 0x06001F4D RID: 8013 RVA: 0x000E5E4C File Offset: 0x000E404C
        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] NpcIDleri = new int[]
            {
                this.Pembe_Bogolu,
                this.Mavi_Bogolu
            };
            base.Game.LoadResources(NpcIDleri);
            base.Game.LoadNpcGameOverResources(NpcIDleri);
            base.Game.SetMap(1072);
        }

        // Token: 0x06001F4E RID: 8014 RVA: 0x000E5EA8 File Offset: 0x000E40A8
        public override void OnStartGame()
        {
            base.OnStartGame();
            for (int i = 0; i < 4; i++)
            {
                this.Toplam_Pembe_Bogo++;
                bool flag = i < 1;
                if (flag)
                {
                    this.Pembe_Bogo_Listesi.Add(base.Game.CreateNpc(this.Pembe_Bogolu, 900 + (i + 1) * 100, 505, -1, 1));
                }
                else
                {
                    bool flag2 = i < 3;
                    if (flag2)
                    {
                        this.Pembe_Bogo_Listesi.Add(base.Game.CreateNpc(this.Pembe_Bogolu, 920 + (i + 1) * 100, 505, -1, 1));
                    }
                    else
                    {
                        this.Pembe_Bogo_Listesi.Add(base.Game.CreateNpc(this.Pembe_Bogolu, 1000 + (i + 1) * 100, 515, -1, 1));
                    }
                }
            }
            this.Toplam_Mavi_Bogo++;
            this.Mavi_Bogo_Listesi.Add(base.Game.CreateNpc(this.Mavi_Bogolu, 1467, 495, -1, 1));
        }

        // Token: 0x06001F4F RID: 8015 RVA: 0x000E5FC4 File Offset: 0x000E41C4
        public override void OnNewTurnStarted()
        {
            this.Pembe_Bogo_Sayýlar = this.Toplam_Pembe_Bogo - this.Kalan_Pembe_Bogo;
            this.Mavi_Bogo_Sayýlar = this.Toplam_Mavi_Bogo - this.Kalan_Mavi_Bogo;
            bool flag = base.Game.GetLivedLivings().Count == 0;
            if (flag)
            {
                base.Game.PveGameDelay = 0;
            }
            bool flag2 = base.Game.TurnIndex <= 1 || base.Game.CurrentPlayer.Delay <= base.Game.PveGameDelay || (this.Mavi_Bogo_Sayýlar == 3 && this.Pembe_Bogo_Sayýlar == 12);
            if (!flag2)
            {
                bool flag3 = this.Toplam_Pembe_Bogo < 12 && this.Toplam_Mavi_Bogo < 3;
                if (flag3)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        this.Toplam_Pembe_Bogo++;
                        bool flag4 = i < 1;
                        if (flag4)
                        {
                            this.Pembe_Bogo_Listesi.Add(base.Game.CreateNpc(this.Pembe_Bogolu, 900 + (i + 1) * 100, 505, -1, 1));
                        }
                        else
                        {
                            bool flag5 = i < 3;
                            if (flag5)
                            {
                                this.Pembe_Bogo_Listesi.Add(base.Game.CreateNpc(this.Pembe_Bogolu, 920 + (i + 1) * 100, 505, -1, 1));
                            }
                            else
                            {
                                this.Pembe_Bogo_Listesi.Add(base.Game.CreateNpc(this.Pembe_Bogolu, 1000 + (i + 1) * 100, 515, -1, 1));
                            }
                        }
                    }
                    this.Toplam_Mavi_Bogo++;
                    this.Mavi_Bogo_Listesi.Add(base.Game.CreateNpc(this.Mavi_Bogolu, 1467, 495, -1, 1));
                }
                else
                {
                    bool flag6 = this.Pembe_Bogo_Sayýlar >= 12;
                    if (!flag6)
                    {
                        bool flag7 = 12 - this.Pembe_Bogo_Sayýlar >= 4;
                        if (flag7)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                bool flag8 = this.Toplam_Pembe_Bogo < 15 && this.Pembe_Bogo_Sayýlar != 12;
                                if (flag8)
                                {
                                    this.Toplam_Pembe_Bogo++;
                                    bool flag9 = j < 1;
                                    if (flag9)
                                    {
                                        this.Pembe_Bogo_Listesi.Add(base.Game.CreateNpc(this.Pembe_Bogolu, 900 + (j + 1) * 100, 505, -1, 1));
                                    }
                                    else
                                    {
                                        bool flag10 = j < 3;
                                        if (flag10)
                                        {
                                            this.Pembe_Bogo_Listesi.Add(base.Game.CreateNpc(this.Pembe_Bogolu, 920 + (j + 1) * 100, 505, -1, 1));
                                        }
                                        else
                                        {
                                            this.Pembe_Bogo_Listesi.Add(base.Game.CreateNpc(this.Pembe_Bogolu, 1000 + (j + 1) * 100, 515, -1, 1));
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            bool flag11 = 12 - this.Pembe_Bogo_Sayýlar > 0;
                            if (flag11)
                            {
                                for (int k = 0; k < 12 - this.Pembe_Bogo_Sayýlar; k++)
                                {
                                    bool flag12 = this.Toplam_Pembe_Bogo < 15 && this.Pembe_Bogo_Sayýlar != 12;
                                    if (flag12)
                                    {
                                        this.Toplam_Pembe_Bogo++;
                                        bool flag13 = k < 1;
                                        if (flag13)
                                        {
                                            this.Pembe_Bogo_Listesi.Add(base.Game.CreateNpc(this.Pembe_Bogolu, 900 + (k + 1) * 100, 505, -1, 1));
                                        }
                                        else
                                        {
                                            bool flag14 = k < 3;
                                            if (flag14)
                                            {
                                                this.Pembe_Bogo_Listesi.Add(base.Game.CreateNpc(this.Pembe_Bogolu, 920 + (k + 1) * 100, 505, -1, 1));
                                            }
                                            else
                                            {
                                                this.Pembe_Bogo_Listesi.Add(base.Game.CreateNpc(this.Pembe_Bogolu, 1000 + (k + 1) * 100, 515, -1, 1));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        bool flag15 = this.Mavi_Bogo_Sayýlar < 3 && this.Toplam_Mavi_Bogo < 5;
                        if (flag15)
                        {
                            this.Toplam_Mavi_Bogo++;
                            this.Mavi_Bogo_Listesi.Add(base.Game.CreateNpc(this.Mavi_Bogolu, 1467, 495, -1, 1));
                        }
                    }
                }
            }
        }

        // Token: 0x06001F50 RID: 8016 RVA: 0x000E6452 File Offset: 0x000E4652
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        // Token: 0x06001F51 RID: 8017 RVA: 0x000E645C File Offset: 0x000E465C
        public override bool CanGameOver()
        {
            bool flag = true;
            this.Kalan_Pembe_Bogo = 0;
            this.Kalan_Mavi_Bogo = 0;
            foreach (SimpleNpc item in this.Pembe_Bogo_Listesi)
            {
                bool isLiving = item.IsLiving;
                if (isLiving)
                {
                    flag = false;
                }
                else
                {
                    this.Kalan_Pembe_Bogo++;
                }
            }
            foreach (SimpleNpc item2 in this.Mavi_Bogo_Listesi)
            {
                bool isLiving2 = item2.IsLiving;
                if (isLiving2)
                {
                    flag = false;
                }
                else
                {
                    this.Kalan_Mavi_Bogo++;
                }
            }
            bool flag2 = flag && this.Toplam_Pembe_Bogo == 15 && this.Toplam_Mavi_Bogo == 5;
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

        // Token: 0x06001F52 RID: 8018 RVA: 0x000E65A4 File Offset: 0x000E47A4
        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return base.Game.TotalKillCount;
        }

        // Token: 0x06001F53 RID: 8019 RVA: 0x000E65C8 File Offset: 0x000E47C8
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

        // Token: 0x06001F54 RID: 8020 RVA: 0x000E6612 File Offset: 0x000E4812
        public GT1171()
        {
            this.Pembe_Bogo_Listesi = new List<SimpleNpc>();
            this.Mavi_Bogo_Listesi = new List<SimpleNpc>();
            this.Pembe_Bogolu = 1101;
            this.Mavi_Bogolu = 1102;
        }

        // Token: 0x0400117A RID: 4474
        private List<SimpleNpc> Pembe_Bogo_Listesi;

        // Token: 0x0400117B RID: 4475
        private List<SimpleNpc> Mavi_Bogo_Listesi;

        // Token: 0x0400117C RID: 4476
        private int Pembe_Bogo_Sayýlar;

        // Token: 0x0400117D RID: 4477
        private int Mavi_Bogo_Sayýlar;

        // Token: 0x0400117E RID: 4478
        private int Toplam_Pembe_Bogo;

        // Token: 0x0400117F RID: 4479
        private int Toplam_Mavi_Bogo;

        // Token: 0x04001180 RID: 4480
        private int Kalan_Pembe_Bogo;

        // Token: 0x04001181 RID: 4481
        private int Kalan_Mavi_Bogo;

        // Token: 0x04001182 RID: 4482
        private int Pembe_Bogolu;

        // Token: 0x04001183 RID: 4483
        private int Mavi_Bogolu;
    }
}
