using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Bussiness;
namespace GameServerScript.AI.Messions

    //Civciv Kolay Etap 2
{
    public class GCGCD1162 : AMissionControl
    {
        private SimpleBoss OrtadakiTavuk = null;

        private SimpleBoss ÜsttekiTavuk = null;

        private SimpleBoss AlttakiTavuk = null;

        private int SismanTavukID = 7011;

        private int CountBossTurn = 0;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1750)
            {
                return 3;
            }
            else if (score > 1675)
            {
                return 2;
            }
            else if (score > 1600)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] resources = { SismanTavukID };
            int[] gameOverResource = { SismanTavukID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResource);
            Game.AddLoadingFile(1, "bombs/84.swf", "tank.resource.bombs.Bomb84");
            Game.AddLoadingFile(2, "image/game/effect/7/cao.swf", "asset.game.seven.cao");
            Game.AddLoadingFile(2, "image/game/effect/7/jinquhd.swf", "asset.game.seven.jinquhd");
            Game.SetMap(1162);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            LivingConfig config = Game.BaseLivingConfig();
            config.IsTurn = true;
            config.HaveShield = false;
            LivingConfig config1 = Game.BaseLivingConfig();
            config1.IsTurn = true;
            config1.HaveShield = false;
            LivingConfig config2 = Game.BaseLivingConfig();
            config2.IsTurn = true;
            config2.HaveShield = false;
            ÜsttekiTavuk = Game.CreateBoss(SismanTavukID, 1680, 315, -1, 1, "", config);
            OrtadakiTavuk = Game.CreateBoss(SismanTavukID, 1615, 565, -1, 1, "", config1);
            AlttakiTavuk = Game.CreateBoss(SismanTavukID, 1600, 849, -1, 1, "", config2);
            OrtadakiTavuk.FallFrom(OrtadakiTavuk.X, OrtadakiTavuk.Y, "", 0, 0, 2000);
            AlttakiTavuk.FallFrom(AlttakiTavuk.X, AlttakiTavuk.Y, "", 0, 0, 2000);
            ÜsttekiTavuk.FallFrom(ÜsttekiTavuk.X, ÜsttekiTavuk.Y, "", 0, 0, 2000);
            ÜsttekiTavuk.SetRelateDemagemRect(ÜsttekiTavuk.NpcInfo.X, ÜsttekiTavuk.NpcInfo.Y, ÜsttekiTavuk.NpcInfo.Width, ÜsttekiTavuk.NpcInfo.Height);
            AlttakiTavuk.SetRelateDemagemRect(AlttakiTavuk.NpcInfo.X, AlttakiTavuk.NpcInfo.Y, AlttakiTavuk.NpcInfo.Width, AlttakiTavuk.NpcInfo.Height);
            OrtadakiTavuk.SetRelateDemagemRect(OrtadakiTavuk.NpcInfo.X, OrtadakiTavuk.NpcInfo.Y, OrtadakiTavuk.NpcInfo.Width, OrtadakiTavuk.NpcInfo.Height);
            ÜsttekiTavuk.Properties1 = 0;
            OrtadakiTavuk.Properties1 = 0;
            AlttakiTavuk.Properties1 = 2;
            //AlttakiTavuk.Delay += 2;
            //OrtadakiTavuk.Delay += 1;
            GirişKonuşmaları();
        }

        private void GirişKonuşmaları()
        {
            Game.SendObjectFocus(ÜsttekiTavuk, 1, 1000, 0);
            ÜsttekiTavuk.PlayMovie("speak", 1500, 0);
            ÜsttekiTavuk.Say("Bunlar neden buraya geldi?", 0, 1500);
            Game.SendObjectFocus(OrtadakiTavuk, 1, 4000, 0);
            OrtadakiTavuk.PlayMovie("speak", 4500, 0);
            OrtadakiTavuk.Say("Boşver onları. Yok edelim!", 0, 4500);
            Game.SendObjectFocus(AlttakiTavuk, 1, 8000, 0);
            AlttakiTavuk.PlayMovie("speak", 8500, 0);
            AlttakiTavuk.Say("Hâlâ kaçmak için vaktin var.", 0, 8500, 1500);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }


        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override bool CanGameOver()
        {
            if (ÜsttekiTavuk != null && ÜsttekiTavuk.IsLiving == false && OrtadakiTavuk != null && OrtadakiTavuk.IsLiving == false && AlttakiTavuk != null && AlttakiTavuk.IsLiving == false)
                return true;

            if (Game.TotalTurn > Game.MissionInfo.TotalTurn)
                return true;

            return false;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return Game.TotalKillCount;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (ÜsttekiTavuk != null && ÜsttekiTavuk.IsLiving == false && OrtadakiTavuk != null && OrtadakiTavuk.IsLiving == false && AlttakiTavuk != null && AlttakiTavuk.IsLiving == false)
            {
                Game.IsWin = true;
                //Game.SessionId++;
            }
            else
            {
                Game.IsWin = false;
            }
        }
    }
}