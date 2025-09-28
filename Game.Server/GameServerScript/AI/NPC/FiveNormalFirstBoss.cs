using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
	public class FiveNormalFirstBoss : ABrain
	{
		private int m_attackTurn = 0;

		private PhysicalObj m_moive;

		private PhysicalObj m_wallRight = null;

		private static string[] AllAttackChat = new string[]
		{
			"要地震喽！！<br/>各位请扶好哦",
			"把你武器震下来！",
			"看你们能还经得起几下！！"
		};

		private static string[] ShootChat = new string[]
		{
			"让你知道什么叫百发百中！",
			"送你一个球~你可要接好啦",
			"你们这群无知的低等庶民"
		};

		private static string[] ShootedChat = new string[]
		{
			"哎呀~~你们为什么要攻击我？<br/>我在干什么？",
			"噢~~好痛!我为什么要战斗？<br/>我必须战斗…"
		};

		private static string[] AddBooldChat = new string[]
		{
			"扭啊扭~<br/>扭啊扭~~",
			"哈利路亚~<br/>路亚路亚~~",
			"呀呀呀，<br/>好舒服啊！"
		};

		private static string[] KillAttackChat = new string[]
		{
			"君临天下！！"
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
			bool flag = false;
			int num = 0;
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X > 0 && current.X < 0)
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
				this.KillAttack(1400, 1600);
			}
			else if (this.m_attackTurn == 0)
			{
				this.BeatA();
				base.Body.SetXY(base.Body.X, 659);
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				base.Body.SetXY(base.Body.X, 559);
				this.BeatB();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 2)
			{
				base.Body.SetXY(base.Body.X, 459);
				this.BeatC();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 3)
			{
				base.Body.SetXY(base.Body.X, 359);
				this.BeatD();
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 4)
			{
				if (base.Body.Y == 758)
				{
					this.m_attackTurn = 0;
				}
				else
				{
					this.BeatE();
					base.Body.SetXY(base.Body.X, 259);
					this.m_attackTurn++;
				}
			}
			else if (this.m_attackTurn == 5)
			{
				if (base.Body.Y == 758)
				{
					this.m_attackTurn = 0;
				}
				else
				{
					this.BeatG();
					base.Body.SetXY(base.Body.X, 259);
				}
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void KillAttack(int fx, int tx)
		{
			int num = base.Game.Random.Next(0, FiveNormalFirstBoss.KillAttackChat.Length);
			base.Body.Say(FiveNormalFirstBoss.KillAttackChat[num], 1, 1000);
			base.Body.CurrentDamagePlus = 10f;
			base.Body.PlayMovie("beat", 3000, 10000);
			base.Body.RangeAttacking(fx, tx, "cry", 5000, null);
		}

		private void BeatA()
		{
			base.Body.PlayMovie("beatA", 3000, 11000);
			base.Body.CallFuction(new LivingCallBack(this.CallBeatA), 11000);
		}

		private void CallBeatA()
		{
			base.Body.PlayMovie("standA", 2000, 0);
			List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
			int blood = base.Game.Random.Next(1000, 2510);
			foreach (Player current in allLivingPlayers)
			{
				current.AddEffect(new ContinueReduceBloodEffect(2, blood, current), 0);
			}
		}

		private void BeatB()
		{
			base.Body.PlayMovie("beatB", 3000, 10000);
			Player player = base.Game.FindRandomPlayer();
			((SimpleBoss)base.Body).NpcInfo.FireY = 0;
			if (player != null)
			{
				base.Body.ShootPoint(player.X, player.Y, 56, 1000, 10000, 1, 2f, 10000);
			}
			base.Body.CallFuction(new LivingCallBack(this.CallBeatB), 11000);
		}

		private void CallBeatB()
		{
			base.Body.PlayMovie("standB", 3000, 0);
		}

		private void BeatC()
		{
			base.Body.PlayMovie("beatC", 3000, 10000);
			base.Body.CallFuction(new LivingCallBack(this.CallBeatC), 11000);
		}

		private void CallBeatC()
		{
			base.Body.PlayMovie("standC", 3000, 0);
			List<Player> allFightPlayers = base.Game.GetAllFightPlayers();
			foreach (Player current in allFightPlayers)
			{
				int num = base.Game.Random.Next(200, 510);
				this.m_wallRight = ((PVEGame)base.Game).CreatePhysicalObj(current.X, current.Y, "wallLeft", "asset.game.4.zap", "1", 1, 1);
				current.AddEffect(new ReduceStrengthEffect(2, 5), 0);
				current.AddBlood(-num, 1);
			}
			List<Player> list = new List<Player>();
			foreach (Player current in allFightPlayers)
			{
				if (!current.IsFrost)
				{
					list.Add(current);
				}
			}
		}

		private void BeatD()
		{
			base.Body.PlayMovie("beatD", 3000, 10000);
			base.Body.CallFuction(new LivingCallBack(this.CallBeatD), 11000);
		}

		private void CallBeatD()
		{
			base.Body.PlayMovie("standD", 3000, 0);
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				int num = base.Game.Random.Next(100, 515);
				this.m_moive = ((PVEGame)base.Game).Createlayer(current.X, current.Y, "moive", "asset.game.4.minigun", "", 1, 1);
				current.AddBlood(-num, 1);
				current.AddBlood(-num, 1);
				current.AddBlood(-num, 1);
				current.AddBlood(-num, 1);
				current.AddBlood(-num, 1);
				current.AddBlood(-num, 1);
			}
		}

		private void BeatE()
		{
			base.Body.PlayMovie("DtoE", 3000, 10000);
			base.Body.CallFuction(new LivingCallBack(this.BeatG), 10000);
		}

		private void CallBeatE()
		{
			base.Body.PlayMovie("standE", 3000, 0);
		}

		private void BeatG()
		{
			Player player = base.Game.FindRandomPlayer();
			base.Body.PlayMovie("beatE", 3000, 10000);
			((SimpleBoss)base.Body).NpcInfo.FireY = 20;
			base.Body.ShootPoint(player.X, player.Y, 72, 1000, 10000, 1, 1f, 5500);
			base.Body.CallFuction(new LivingCallBack(this.CallBeatE), 10000);
		}
	}
}
