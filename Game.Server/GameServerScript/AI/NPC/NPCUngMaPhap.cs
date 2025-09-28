using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using System.Drawing;
using Game.Logic.Actions;
using Bussiness;

namespace GameServerScript.AI.NPC
{
    public class NPCUngMaPhap : ABrain
    {
        private int m_attackTurn = 0;
        protected Player m_targer;
        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            Body.CurrentDamagePlus = 10;
            Body.CurrentShootMinus = 1;
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            if (this.m_attackTurn == 0)
            {
                this.MoveBeat();
                ++this.m_attackTurn;
            }
            else if (this.m_attackTurn == 1)
            {
                this.MoveBeat();
                ++this.m_attackTurn;
            }
            else
            {
                this.MoveBeat();
                this.m_attackTurn = 0;
            }
        }

        private void Move()
        {
            this.Body.MoveTo(this.Game.Random.Next(318, 1300), this.Game.Random.Next(416, 765), "fly", 500, "", 12, null);
        }


        private void MoveBeat()
        {
            this.Body.MoveTo(this.Game.Random.Next(318, 1300), this.Game.Random.Next(416, 765), "fly", 500, "", 12, new LivingCallBack(this.Beating));
        }

        public void Beating()
        {
            this.Body.PlayMovie("beatA", 2000, 0);
            this.Body.CallFuction(new LivingCallBack(this.RangeAttacking), 3000);
        }

        private void RangeAttacking()
        {
            this.Body.RangeAttacking(0, this.Body.Game.Map.Info.ForegroundWidth + 1, "cry", 1000, (List<Player>)null);
        }


        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
    }
}
