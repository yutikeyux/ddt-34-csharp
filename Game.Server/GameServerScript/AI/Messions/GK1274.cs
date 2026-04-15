using System;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    public class GK1274 : AMissionControl
    {
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
        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }
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
        public override int UpdateUIData()
        {
            return this.m_kill;
        }
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
        public override void DoOther()
        {
            base.DoOther();
            int index = base.Game.Random.Next(0, GK1274.KillChat.Length);
            this.m_king.Say(GK1274.KillChat[index], 0, 0);
        }
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
        public GK1274()
        {
        }
        static GK1274()
        {
        }
        private SimpleBoss m_king = null;
        private SimpleBoss m_secondKing = null;
        private int m_kill;
        private int IsSay;
        private int bossID = 1207;
        private int npcID = 1204;
        private int turn;
        private PhysicalObj m_kingMoive;
        private PhysicalObj m_kingFront;
        private static string[] KillChat = new string[]
        {
            "Hepsi bu kadar mı?",
            "Ouch! Acıyor! Hahaha?",
            "Ah, oldukça iyi."
        };
        private static string[] ShootedChat = new string[]
        {
            "Kazandığını mı sandın? Henüz bitmedi! Geri dönüyorum!"
        };
    }
}
