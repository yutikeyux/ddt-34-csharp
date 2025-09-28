using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
	public class LoopholeSpaceHardBoss : AMissionControl
	{
		private SimpleBoss boss = null;

		private SimpleBoss boss2 = null;

		private SimpleBoss m_secondboss = null;

		private PhysicalObj m_moive;

		private PhysicalObj m_front;

		private int m_state = 71089;

		private int turn = 0;

		private int firstBossID = 71089;

		private int secondBossID = 71090;

		private int direction;

		private int m_kill = 0;

		public override int CalculateScoreGrade(int score)
		{
			base.CalculateScoreGrade(score);
			int result;
			if (score > 1150)
			{
				result = 3;
			}
			else if (score > 925)
			{
				result = 2;
			}
			else if (score > 700)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public override void OnPrepareNewSession()
		{
			base.OnPrepareNewSession();
			int[] npcIds = new int[]
			{
				this.firstBossID,
				this.secondBossID
			};
			int[] npcIds2 = new int[]
			{
				this.firstBossID,
				this.secondBossID
			};
			base.Game.LoadResources(npcIds);
			base.Game.LoadNpcGameOverResources(npcIds2);
			base.Game.AddLoadingFile(2, "image/bomb/blastOut/blastOut51.swf", "shootMovie51");
			base.Game.AddLoadingFile(2, "image/bomb/bullet/bullet51.swf", "bullet51");
			base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
			base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.AntQueenAsset");
			base.Game.SetMap(1469);
		}

		public override void OnStartGame()
		{
			base.OnStartGame();
			this.boss = base.Game.CreateBoss(this.m_state, 910, 800, -1, 1, "");
			this.boss.SetRelateDemagemRect(this.boss.NpcInfo.X, this.boss.NpcInfo.Y, this.boss.NpcInfo.Width, this.boss.NpcInfo.Height);
			//this.m_moive.PlayMovie("in", 4000, 0);
			this.m_front.PlayMovie("in", 4100, 0);
			this.m_moive.PlayMovie("out", 80000, 1000);
			this.m_front.PlayMovie("out", 9900, 0);
			this.turn = base.Game.TurnIndex;
		}

		public override void OnNewTurnStarted()
		{
			base.OnNewTurnStarted();
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			if (base.Game.TurnIndex > this.turn + 1)
			{
				if (this.m_moive != null)
				{
					base.Game.RemovePhysicalObj(this.m_moive, true);
					this.m_moive = null;
				}
				if (this.m_front != null)
				{
					base.Game.RemovePhysicalObj(this.m_front, true);
					this.m_front = null;
				}
			}
		}

		public override bool CanGameOver()
		{
			bool result;
			if (base.Game.TurnIndex > base.Game.MissionInfo.TotalTurn - 1)
			{
				result = true;
			}
			else
			{
				base.CanGameOver();
				if (!this.boss.IsLiving)
				{
					if (this.m_state == this.firstBossID)
					{
						this.m_state++;
					}
				}
				if (this.m_state == this.secondBossID && this.m_secondboss == null)
				{
					LivingConfig livingConfig = base.Game.BaseLivingConfig();
					livingConfig.IsFly = true;
					this.m_secondboss = base.Game.CreateBoss(this.m_state, this.boss.X, this.boss.Y, this.boss.Direction, 2, "", livingConfig);
					this.m_secondboss.SetRelateDemagemRect(this.m_secondboss.NpcInfo.X, this.m_secondboss.NpcInfo.Y, this.m_secondboss.NpcInfo.Width, this.m_secondboss.NpcInfo.Height);
					base.Game.RemoveLiving(this.boss.Id);
					List<Player> allFightPlayers = base.Game.GetAllFightPlayers();
					Player player = base.Game.FindRandomPlayer();
					int num = 0;
					if (player != null)
					{
						num = player.Delay;
					}
					foreach (Player current in allFightPlayers)
					{
						if (current.Delay < num)
						{
							num = current.Delay;
						}
					}
					this.m_secondboss.AddDelay(num - 2000);
					this.turn = base.Game.TurnIndex;
				}
				if (this.m_secondboss != null && !this.m_secondboss.IsLiving)
				{
					this.direction = this.m_secondboss.Direction;
					this.m_kill++;
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		public override int UpdateUIData()
		{
			base.UpdateUIData();
			return this.m_kill;
		}

		public override void OnGameOver()
		{
			base.OnGameOver();
			if (this.m_state == this.secondBossID && !this.m_secondboss.IsLiving)
			{
				base.Game.IsWin = true;
			}
			else
			{
				base.Game.IsWin = false;
			}
		}
	}
}
