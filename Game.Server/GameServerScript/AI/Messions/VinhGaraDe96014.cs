using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;

namespace GameServerScript.AI.Messions
{
	public class VinhGaraDe96014 : AMissionControl
	{
		private SimpleBoss boss = null;

		private int npcID = 0;

		private int bossID = 96014; 
		//private int bossID = 12014;

		private int kill = 0;

		private PhysicalObj m_moive;

		private PhysicalObj m_front;

		public override int CalculateScoreGrade(int score)
		{
			base.CalculateScoreGrade(score);
			int result;
			if (score > 1750)
			{
				result = 3;
			}
			else if (score > 1675)
			{
				result = 2;
			}
			else if (score > 1600)
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
				this.bossID
			};
			int[] npcIds2 = new int[]
			{
				this.bossID
			};
			base.Game.LoadResources(npcIds);
			base.Game.LoadNpcGameOverResources(npcIds2);
			base.Game.AddLoadingFile(1, "bombs/51.swf", "tank.resource.bombs.Bomb51");
			base.Game.AddLoadingFile(1, "bombs/99.swf", "tank.resource.bombs.Bomb99");
			base.Game.AddLoadingFile(2, "image/game/effect/10/jianyu.swf", "asset.game.ten.jianyu");
			base.Game.AddLoadingFile(1, "bombs/61.swf", "tank.resource.bombs.Bomb61");
			base.Game.SetMap(11034); 
		}

		public override void OnStartGame()
		{
			base.OnStartGame();
			base.Game.IsBossWar = "May Ha Buoi =))";
			this.boss = base.Game.CreateBoss(this.bossID, 1120, 385, -1, 1, "");
			this.boss.SetRelateDemagemRect(this.boss.NpcInfo.X, this.boss.NpcInfo.Y, this.boss.NpcInfo.Width, this.boss.NpcInfo.Height);
		}

		public override void OnNewTurnStarted()
		{
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			if (base.Game.TurnIndex > 1)
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
			if (this.boss != null && !this.boss.IsLiving)
			{
				this.kill++;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public override int UpdateUIData()
		{
			base.UpdateUIData();
			return this.kill;
		}

		public override void OnGameOver()
		{
			base.OnGameOver();
			if (this.boss != null && !this.boss.IsLiving)
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
