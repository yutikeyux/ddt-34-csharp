using Game.Logic;
using Game.Logic.AI;
using System.Collections.Generic;
using System.Drawing;

namespace GameServerScript.AI.NPC
{
    public class SixNormalSecondNpc : ABrain
    {
        private int turnCount;
        private int currentPathIndex;
        private List<Point> pathPoints;
        private List<Point> targetSlots;
        private int maxStepIndex;
        private bool isFirstTurn;

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
            if (base.Body.Blood != 1)
            {
                ((PVEGame)base.Game).SendLivingActionMapping(base.Body, "stand", "stand");
                base.Body.PlayMovie("stand", 0, 0);
            }
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            m_body.CurrentDamagePlus = 1f;
            m_body.CurrentShootMinus = 1f;
            if (base.Body.Blood == 1)
            {
                ((PVEGame)base.Game).SendLivingActionMapping(base.Body, "stand", "standB");
                base.Body.PlayMovie("standB", 0, 0);
            }
        }

        public override void OnCreated()
        {
            base.OnCreated();
            currentPathIndex = 0; // DÜZELTME: 1'den 0'a çevrildi. Yolun en başından başlamalı.
            maxStepIndex = base.Body.Config.MaxStepMove;
            base.Body.Config.CompleteStep = false;
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();

            // Görevi tamamladıysa işlem yapma
            if (base.Body.Config.CompleteStep) return;

            turnCount++;
            if (isFirstTurn)
            {
                maxStepIndex = base.Body.Config.FirstStepMove;
            }
            isFirstTurn = false;

            if (base.Body.Blood > 1)
            {
                ProcessMovement();
            }
        }

        private void ProcessMovement()
        {
            // Yolun sonuna geldiysek slotlara yerleş
            if (currentPathIndex >= pathPoints.Count)
            {
                if ((base.Game as PVEGame).CountMosterPlace < targetSlots.Count && !base.Body.Config.CompleteStep)
                {
                    Point targetSlot = targetSlots[(base.Game as PVEGame).CountMosterPlace];
                    (base.Game as PVEGame).CountMosterPlace++;
                    ((PVEGame)base.Game).SendLivingActionMapping(base.Body, "stand", "happy");
                    base.Body.BoltMove(targetSlot.X, targetSlot.Y, 0);
                    base.Body.PlayMovie("happy", 0, 0);
                    base.Body.Config.CompleteStep = true;
                }
                return;
            }

            if (currentPathIndex == pathPoints.Count - 1) maxStepIndex++;

            Point nextPoint = pathPoints[currentPathIndex];
            string action = "walk";

            if (nextPoint.X == base.Body.X && (nextPoint.Y == 920 || nextPoint.Y == 760)) action = "flyUp";
            else if (nextPoint.X >= 620) action = "flyLR";

            currentPathIndex++;

            if (currentPathIndex <= maxStepIndex && currentPathIndex <= pathPoints.Count)
            {
                base.Body.MoveTo(nextPoint.X, nextPoint.Y, action, 0, ProcessMovement, 5);
            }
            else
            {
                maxStepIndex = currentPathIndex + base.Body.Config.MaxStepMove;
                base.Body.MoveTo(nextPoint.X, nextPoint.Y, action, 0, 5);
            }
        }

        public SixNormalSecondNpc()
        {
            pathPoints = new List<Point>
            {
                new Point(620, 1080), new Point(620, 980), new Point(720, 980), new Point(820, 980),
                new Point(920, 980), new Point(1020, 980), new Point(1120, 980), new Point(1220, 980),
                new Point(1320, 980), new Point(1420, 980), new Point(1520, 980), new Point(1620, 980),
                new Point(1620, 830), new Point(1520, 830), new Point(1420, 830), new Point(1320, 830),
                new Point(1220, 830), new Point(1120, 830), new Point(1020, 830), new Point(920, 830),
                new Point(820, 830), new Point(720, 830), new Point(620, 830), new Point(620, 680),
                new Point(720, 680), new Point(820, 680), new Point(920, 680), new Point(1020, 680),
                new Point(1120, 680), new Point(1220, 680), new Point(1320, 680), new Point(1420, 680),
                new Point(1520, 680), new Point(1620, 680), new Point(1620, 530), new Point(1520, 530),
                new Point(1420, 530), new Point(1320, 530), new Point(1220, 530), new Point(1120, 530),
                new Point(1020, 530), new Point(920, 530), new Point(820, 530), new Point(720, 530),
                new Point(620, 530), new Point(620, 380), new Point(720, 380), new Point(820, 380),
                new Point(920, 380), new Point(1020, 380), new Point(1120, 380), new Point(1220, 380),
                new Point(1320, 380), new Point(1420, 380), new Point(1520, 380), new Point(1620, 380),
                new Point(1620, 260)
            };
            targetSlots = new List<Point>
            {
                new Point(700, 260), new Point(800, 260), new Point(900, 260), new Point(1000, 260),
                new Point(1100, 260), new Point(1200, 260), new Point(1300, 260), new Point(1400, 260),
                new Point(1400, 260), new Point(1500, 260)
            };
            isFirstTurn = true;
        }
    }
}