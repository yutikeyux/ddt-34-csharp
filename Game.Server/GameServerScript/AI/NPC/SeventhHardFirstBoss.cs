using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class SeventhHardFirstBoss : ABrain
        {
            private int m_attackTurn = 0;

            private PhysicalObj moive;

            private static string[] AllAttackChat = new string[]
            {
            LanguageMgr.GetTranslation("Ddtank super là số 1", new object[0])
            };

            private static string[] ShootChat = new string[]
            {
            LanguageMgr.GetTranslation("Anh em tiến lên !", new object[0])
            };

            private static string[] KillPlayerChat = new string[]
            {
            LanguageMgr.GetTranslation("Anh em tiến lên !", new object[0])
            };

            private static string[] CallChat = new string[]
            {
            LanguageMgr.GetTranslation("Ai giết được chúng sẻ được ban thưởng !", new object[0])
            };

            private static string[] JumpChat = new string[]
            {
            LanguageMgr.GetTranslation("Ai giết được chúng sẻ được ban thưởng !", new object[0])
            };

            private static string[] KillAttackChat = new string[]
            {
            LanguageMgr.GetTranslation("Gebermeyi seviyorsun heralde!", new object[0]),
            LanguageMgr.GetTranslation("İntihar etmeyi bu kadar istiyosan burda ne işin var?", new object[0])
            };

            private static string[] ShootedChat = new string[]
            {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg15", new object[0]),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg16", new object[0])
            };

            private static string[] DiedChat = new string[]
            {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg17", new object[0])
            };

            public override void OnBeginSelfTurn()
            {
                base.OnBeginSelfTurn();
            }

            public override void OnBeginNewTurn()
            {
                base.OnBeginNewTurn();
                base.Body.CurrentDamagePlus = 1f;
                base.Body.CurrentShootMinus = 1f;
            }

            public override void OnCreated()
            {
                base.OnCreated();
            }

            public override void OnStartAttacking()
            {
                base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
                bool flag = false;
                int num = 0;
                foreach (Player current in base.Game.GetAllFightPlayers())
                {
                    if (current.IsLiving && current.X > 1344)
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
                    this.KillAttack(1344, base.Game.Map.Info.ForegroundWidth + 1);
                }
                else if (this.m_attackTurn == 0)
                {
                    this.Summon(0);
                    this.m_attackTurn++;
                }
                else if (this.m_attackTurn == 1)
                {
                    this.Shield();
                    this.m_attackTurn++;
                }
                else if (this.m_attackTurn == 2)
                {
                    this.Summon(1);
                    this.m_attackTurn++;
                }
                else if (this.m_attackTurn == 3)
                {
                    this.Shield();
                    this.m_attackTurn++;
                }
                else if (this.m_attackTurn == 4)
                {
                    this.Summon(2);
                    this.m_attackTurn++;
                }
                else
                {
                    this.Shield();
                    this.m_attackTurn = 0;
                }
            }

            public override void OnStopAttacking()
            {
                base.OnStopAttacking();
            }

            private void KillAttack(int fx, int tx)

            {

                int num = base.Game.Random.Next(0, SeventhHardFirstBoss.KillAttackChat.Length);
                base.Body.Say(SeventhHardFirstBoss.KillAttackChat[num], 1, 1000);
                base.Body.CurrentDamagePlus = 10f;
            base.Body.Config.CanTakeDamage = true;
            base.Body.PlayMovie("beatB", 3000, 0);
                base.Body.RangeAttacking(fx, tx, "cry", 5000, null);
            }

            public void Summon(int type)
            {
                base.Body.PlayMovie("Ato", 100, 0);
            base.Body.Config.CanTakeDamage = true;
            ((SimpleBoss)base.Body).SetRelateDemagemRect(-56, -122, 124, 129);
                switch (type)
                {
                    case 1:
                        base.Body.CallFuction(new LivingCallBack(this.PersonalAttackDame), 2500);
                        break;
                    case 2:
                        base.Body.CallFuction(new LivingCallBack(this.AllAttack), 2500);
                        break;
                    default:
                        base.Body.CallFuction(new LivingCallBack(this.PersonalAttack), 2500);
                        break;
                }
            }

            private void AllAttack()
            {
                base.Body.PlayMovie("beatB", 3000, 0);
            base.Body.Config.CanTakeDamage = true;
            base.Body.RangeAttacking(0, base.Body.X, "cry", 6000, null);
                base.Body.CallFuction(new LivingCallBack(this.GoMovie), 5000);
            }

            private void GoMovie()
            {
                List<Player> allFightPlayers = base.Game.GetAllFightPlayers();
                foreach (Player current in allFightPlayers)
                {
                    this.moive = ((PVEGame)base.Game).Createlayer(current.X, current.Y, "moive", "asset.game.seven.cao", "out", 1, 0);
                    this.moive.PlayMovie("in", 1000, 0);
                }
            }

            private void PersonalAttack()
            {
                Player player = base.Game.FindRandomPlayer();
                if (player != null)
                {
                    base.Body.CurrentDamagePlus = 0.8f;
                    base.Body.Config.CanTakeDamage = true;
                if (base.Body.ShootPoint(player.X, player.Y, 84, 1200, 10000, 1, 3f, 2550))
                    {
                        base.Body.PlayMovie("beatA", 1700, 0);
                    }
                }
            }

            public void Shield()
            {
                base.Body.State = 1;
                base.Body.Config.CanTakeDamage = false;
                base.Body.PlayMovie("toA", 2700, 0);
                
            }

            private void PersonalAttackDame()
            {
                Player player = base.Game.FindRandomPlayer();
                if (player != null)
                {
                    base.Body.CurrentDamagePlus = 1f;
                    base.Body.Config.CanTakeDamage = true;
                int num = base.Game.Random.Next(player.X, player.X);
                    if (base.Body.ShootPoint(player.X, player.Y, 84, 1200, 10000, 1, 3f, 2650))
                    {
                        base.Body.PlayMovie("beat", 1700, 0);
                    }
                }
            }
        }
    }