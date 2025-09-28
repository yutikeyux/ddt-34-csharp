using System.Collections.Generic;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
//using Game.Server.GameServerScript.AI;

namespace GameServerScript.AI.NPC
{
    public class CryptBoss40 : ABrain
    {
        public int m_attackTurn = 0;

        public List<Player> PlayerList = new List<Player>();

        public SimpleBoss m_living;

        public PhysicalObj m_PhysicalObj;

        public int npcID = 4207;

        //private BossChat BossChatRandom = new BossChat();

        private Player m_Player;
        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            Body.CurrentDamagePlus = 1;
            Body.CurrentShootMinus = 1;
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            Body.Direction = Game.FindlivingbyDir(Body);
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && (player.X > 600))
                {

                    base.Body.CallFuction(delegate
                    {
                        KillAttack(600, 1249);
                    }, 4000);
                }
             }
            //m_attackTurn = 3;
            if (m_attackTurn == 0)
            {
                beatA();
                m_attackTurn++;
            }
            else if (m_attackTurn == 1)
            {
                beatB();
                m_attackTurn++;
            }
            else if (m_attackTurn == 2)
            {
                WalkAndPlayBeatC();
                m_attackTurn++;
            }
            else if (m_attackTurn == 3)
            {
                Jump();
                m_attackTurn = 0;
            }
        }

        public void KillAttack(int tx, int mx)
        {
            Body.CurrentDamagePlus = 100000000f;
            Body.Say("bạn đến chết? ", 0, 1000);
            Body.PlayMovie("beatA", 500, 0);
            Body.RangeAttacking(tx, mx, "cry", 4000, null);
        }
        public void beatA()
        {
            //Body.Say(BossChatRandom.PlayBossChatRnd(), 1, 1000);
            Body.PlayMovie("beatA", 500, 0);
            Body.CurrentDamagePlus = 1.0f;
            Body.RangeAttacking(Body.X - 1500, Body.X + 1500, "cry", 4000, null);
        }

        public void beatB()
        {
            //Body.Say(BossChatRandom.PlayBossChatRnd(), 1, 1000);
            m_Player = Game.FindRandomPlayer();
            Body.CurrentDamagePlus = 1.5f;
            Body.PlayMovie("beatB", 2000, 0);
            Body.CallFuction(new LivingCallBack(CreateEffect), 6100);
            Body.RangeAttacking(m_Player.X - 50, m_Player.X + 50, "cry", 6000, true);
        }
        public void WalkAndPlayBeatC()
        {
            //ody.Say(BossChatRandom.PlayBossChatRnd(), 1, 1000);
            m_Player = Game.FindNearestPlayer(872, 822);
            Body.MoveTo(m_Player.X + 250, m_Player.Y, "walk", 500, 12);
            Body.PlayMovie("beatC", 3000, 0);
            Body.RangeAttacking(m_Player.X - 50, m_Player.X + 50, "cry", 7000, true);
            Body.CallFuction(new LivingCallBack(BackGo), 7500);
        }

        public void Jump()
        {
            //Body.Say(BossChatRandom.PlayBossChatRnd(), 1, 1000);
            m_Player = Game.FindRandomPlayer();
            if (Body.X < Game.Map.Info.DeadWidth / 2)
            {
                Body.CallFuction(new LivingCallBack(BackGo), 100);
            }
            int rnd = Game.Random.Next(0, 2);
            if (rnd == 0)
            {
                Body.PlayMovie("jump", 7000, 0);
                Body.ChangeDirection(-1, 7000);
                ((PVEGame)base.Game).SendObjectFocus(this.m_Player, 1, 8000, 0);
                base.Body.BoltMove(this.m_Player.X, this.m_Player.Y, 8500);
                base.Body.PlayMovie("fall", 8800, 0);
            }
            else
            {
                Body.PlayMovie("jump", 7000, 0);
                Body.ChangeDirection(-1, 7000);
                ((PVEGame)base.Game).SendObjectFocus(this.m_Player, 1, 8000, 0);
                base.Body.BoltMove(this.m_Player.X, this.m_Player.Y, 8500);
                base.Body.PlayMovie("fallB", 8800, 0);
            }
            Body.RangeAttacking(m_Player.X - 50, m_Player.X + 50, "cry", 9000, true);
            Body.CallFuction(new LivingCallBack(BackGo), 10000);
        }

        public void CreateEffect()
        {
            if (m_Player != null)
            {
                ((PVEGame)base.Game).Createlayer(m_Player.X, m_Player.Y, "", "asset.game.nine.057", "", 1, 1);
            }
        }

        public void PlaybeatB()
        {
            Body.PlayMovie("beatB", 3000, 0);
        }

        public void BackGo()
        {
            Body.MoveTo(872, 822, "walk", 1000, 12);
            Body.ChangeDirection(-1, 2000);
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        public override void OnDie()
        {
            base.OnDie();
        }
    }
}
