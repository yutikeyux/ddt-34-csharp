using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
    public class GCGC : ABrain
    {
        private int m_attackTurn = 0;

        private bool IsEixt = false;

        protected Player m_targer;

        private PhysicalObj moive;

        private PhysicalObj m_effect = null;

        private static string[] AllAttackChat = new string[]
		{
			"Cận thận cái đầu !!"
		};

        private static string[] ShootChat = new string[]
		{
			"Thịt đè người !",
			"Cảm nhận sức mạnh của ta !"
		};

        private static string[] KillAttackChat = new string[]
		{
			"Đến nộp mạng à ?? Sức mạnh tối cao!!..."
		};

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            this.m_body.CurrentDamagePlus = 1f;
            this.m_body.CurrentShootMinus = 1f;
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
            bool flag = false;
            int num = 0;
            foreach (Player current in base.Game.GetAllFightPlayers())
            {
                if (current.IsLiving && current.X > 0 && current.X < 350)
                {
                    int num2 = (int)base.Body.Distance(current.X, current.Y);
                    if (num2 > num)
                    {
                        num = num2;
                    }
                    flag = true;
                }
            }
            if (flag)
            {
                this.KillAttack(0, 350);
            }
            else if (this.m_attackTurn == 0)
            {
                if (this.IsEixt)
                {
                    base.Game.RemovePhysicalObj(this.m_effect, true);
                    this.IsEixt = false;
                }
                this.AttackingB();
                this.m_attackTurn++;
            }
            else if (this.m_attackTurn == 1)
            {
                this.AttackingA();
                this.m_attackTurn++;
            }
            else if (this.m_attackTurn == 2)
            {
                this.AttackingC();
                this.m_attackTurn++;
            }
            else if (this.m_attackTurn == 3)
            {
                this.AttackingD();
                this.m_attackTurn++;
            }
            else
            {
                this.AttackingC();
                this.m_attackTurn = 0;
            }
        }

        private void KillAttack(int fx, int tx)
        {
            int num = base.Game.Random.Next(0, GCGC.KillAttackChat.Length);
            base.Body.Say(GCGC.KillAttackChat[num], 1, 0);
            base.Body.PlayMovie("skill", 1900, 0);
            base.Body.RangeAttacking(0, 350, "cry", 4000, null);
            base.Body.CallFuction(new LivingCallBack(this.GoKillAttack), 4000);
        }

        private void GoKillAttack()
        {
            base.Body.CurrentDamagePlus *= 10f;
            this.m_targer = base.Game.FindNearestPlayer(base.Body.X, base.Body.Y);
            this.m_effect = ((PVEGame)base.Game).CreatePhysicalObj(this.m_targer.X, this.m_targer.Y, "skill", "asset.game.seven.jinqucd", "1", 1, 0);
        }

        private void AttackingB()
        {
            Player player = base.Game.FindRandomPlayer();
            if (player != null)
            {
                base.Body.MoveTo(player.X - 150, player.Y, "run", 1000, "", 25, new LivingCallBack(this.NextAttackB));
            }
        }

        private void NextAttackB()
        {
            base.Body.CurrentDamagePlus = 1.5f;
            base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
            base.Body.PlayMovie("beatB", 1500, 0);
            base.Body.RangeAttacking(base.Body.X, base.Body.X + 170, "cry", 3000, null);
            base.Body.CallFuction(new LivingCallBack(this.Comback), 3000);
        }

        private void Comback()
        {
            base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
            base.Body.MoveTo(181, base.Body.Y, "run", 1000, "", 25, new LivingCallBack(this.ChangeDirection));
        }

        private void ChangeDirection()
        {
            base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
        }

        private void AttackingA()
        {
            Player player = base.Game.FindRandomPlayer();
            if (player != null)
            {
                base.Body.MoveTo(player.X - 150, player.Y, "run", 1000, "", 25, new LivingCallBack(this.NextAttackA));
            }
        }

        private void NextAttackA()
        {
            base.Body.CurrentDamagePlus = 2.5f;
            base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
            base.Body.PlayMovie("beatA", 1500, 0);
            base.Body.RangeAttacking(base.Body.X, base.Body.X + 170, "cry", 3600, null);
            base.Body.CallFuction(new LivingCallBack(this.Comback), 3000);
        }

        private void AttackingC()
        {
            Player player = base.Game.FindRandomPlayer();
            if (player != null)
            {
                int x = player.X;
                int mau = player.Blood;
                if (base.Body.ShootPoint(x, player.Y, 83, 1000, 10000, 3, 1.5f, 3300))
                {
                    base.Body.PlayMovie("beatC", 1500, 0);
                }
            }
        }

        private void AttackingD()
        {
            base.Body.MoveTo(1477, base.Body.Y, "run", 1000, "", 25, new LivingCallBack(this.PersonalAttack));
        }

        private void PersonalAttack()
        {
            base.Body.Direction = -base.Body.Direction;
            base.Body.PlayMovie("beatD", 2500, 8100);
            base.Body.CallFuction(new LivingCallBack(this.GoMovie), 6100);
        }

        private void GoMovie()
        {
            Player player = base.Game.FindRandomPlayer();
            if (player != null)
            {
                int x = player.X;
                base.Body.RangeAttacking(x - 200, x + 200, "cry", 3000, null);
                this.moive = ((PVEGame)base.Game).Createlayer(x, player.Y, "moive", "asset.game.seven.choud", "out", 1, 0);
                base.Body.CallFuction(new LivingCallBack(this.GoAttacking), 2000);
            }
        }

        private void GoAttacking()
        {
            Player player = base.Game.FindRandomPlayer();
            if (!this.IsEixt && player != null)
            {
                this.m_effect = ((PVEGame)base.Game).CreatePhysicalObj(player.X, player.Y, "effect", "asset.game.seven.du", "1", 1, 0);
                this.IsEixt = true;
                List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
                foreach (Player current in allLivingPlayers)
                {
                    int num = 140;
                    if (current.X > player.X - num && current.X < player.X + num)
                    {
                        current.AddEffect(new ContinueReduceBlood(2, 200, current), 0);
                    }
                }
            }
            base.Body.CallFuction(new LivingCallBack(this.Comback), 1000);
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
    }
}
