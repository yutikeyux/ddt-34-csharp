using System.Drawing;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;
using GameServerScript.AI.Messions;

namespace GameServerScript.AI.NPC
{
    public class FourNormalBlowArmsNpc : ABrain
    {
        private int yürümeyeri;

        private PhysicalObj KapýyýKýr;

        private SimpleNpc BOMBAFIÇI;

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
            (base.Body.EffectList.GetOfType(eEffectType.IceFronzeEffect) as IceFronzeEffect)?.Stop();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            m_body.CurrentDamagePlus = 1f;
            m_body.CurrentShootMinus = 1f;
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            Point point = default(Point);
            switch (yürümeyeri)
            {
                case 0:
                    point = new Point(672, 746);
                    break;
                case 1:
                    point = new Point(1059, 749);
                    break;
                case 2:
                    point = new Point(1412, 751);
                    break;
            }
            int num = int.MaxValue;
            SimpleNpc[] array = base.Game.FindAllNpcLiving();
            SimpleNpc[] array2 = array;
            foreach (SimpleNpc simpleNpc in array2)
            {
                if (simpleNpc.IsLiving && simpleNpc.X >= base.Body.X && simpleNpc.X <= base.Body.X + point.X)
                {
                    int num2 = (int)base.Body.Distance(simpleNpc.X, simpleNpc.Y);
                    if (num2 < num)
                    {
                        BOMBAFIÇI = simpleNpc;
                        num = num2;
                    }
                }
            }
            if (BOMBAFIÇI != null)
            {
                yürü(BOMBAFIÇI.X - 20, BOMBAFIÇI.Y, FýçýÖldü);
            }
            else if (yürümeyeri < 2)
            {
                yürü(point.X, point.Y, null);
            }
            else
            {
                yürü(point.X, point.Y, zýpla);
                yürümeyeri = 0;
            }
            yürümeyeri++;
        }

        private void yürü(int int_1, int int_2, LivingCallBack livingCallBack_0)
        {
            base.Body.MoveTo(int_1, int_2, "walk", 1000, livingCallBack_0, 5);
        }

        public void FýçýÖldü()
        {
            base.Body.Beat(BOMBAFIÇI, "die", 5000, 5000, 800);
            base.Body.Die(3000);
        }

        private void zýpla()
        {
            base.Body.PlayMovie("beatA", 2000, 6000);
            base.Body.CallFuction(kapýyýkýr, 4500);
        }

        private void kapýyýkýr()
        {
            switch (((PVEGame)base.Game).MissionAI.UpdateUIData())
            {
                case 0:
                    if (KapýyýKýr == null)
                    {
                        KapýyýKýr = ((PVEGame)base.Game).Createlayer(1590, 750, "", "game.asset.Gate", "cryA", 1, 0);
                    }
                    else
                    {
                        KapýyýKýr.PlayMovie("cryA", 0, 0);
                    }
                    break;
                case 1:
                    if (KapýyýKýr == null)
                    {
                        KapýyýKýr = ((PVEGame)base.Game).Createlayer(1590, 750, "", "game.asset.Gate", "cryB", 1, 0);
                    }
                    else
                    {
                        KapýyýKýr.PlayMovie("cryB", 0, 0);
                    }
                    break;
                case 2:
                    if (KapýyýKýr == null)
                    {
                        KapýyýKýr = ((PVEGame)base.Game).Createlayer(1590, 750, "", "game.asset.Gate", "cryC", 1, 0);
                    }
                    else
                    {
                        KapýyýKýr.PlayMovie("cryC", 0, 0);
                    }
                    break;
            }
            (((PVEGame)base.Game).MissionAI as PDHAT1142).DuvarBoþluðu++;
            base.Body.Die();
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
    }
}