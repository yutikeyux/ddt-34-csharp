using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
	public class MESGaacmongs3 : AMissionControl
	{
		private List<PhysicalObj> m_bord = new List<PhysicalObj>();

		private List<PhysicalObj> m_key = new List<PhysicalObj>();

		private PhysicalObj m_door = null;

		private string KeyIndex = null;

		private int m_count = 0;

		private int playerCount = 0;

		public override int CalculateScoreGrade(int score)
		{
			base.CalculateScoreGrade(score);
			int result;
			if (score > 900)
			{
				result = 3;
			}
			else if (score > 825)
			{
				result = 2;
			}
			else if (score > 725)
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
			base.Game.AddLoadingFile(2, "image/map/1075/objects/1075Object.swf", "game.crazytank.assetmap.Board001");
			base.Game.AddLoadingFile(2, "image/map/1075/objects/1075Object.swf", "game.crazytank.assetmap.CrystalDoor001");
			base.Game.AddLoadingFile(2, "image/map/1075/objects/1075Object.swf", "game.crazytank.assetmap.Key");
			base.Game.SetMap(1075);
		}

		public override void OnStartGame()
		{
			base.OnStartGame();
			base.Game.TotalCount = base.Game.PlayerCount;
			base.Game.TotalTurn = base.Game.PlayerCount * 6;
			base.Game.SendMissionInfo();
			this.m_bord.Add(base.Game.CreatePhysicalObj(76, 167, "board1", "game.crazytank.assetmap.Board001", "1", 1, 336));
			this.m_bord.Add(base.Game.CreatePhysicalObj(402, 159, "board2", "game.crazytank.assetmap.Board001", "1", 1, 23));
			this.m_bord.Add(base.Game.CreatePhysicalObj(699, 156, "board3", "game.crazytank.assetmap.Board001", "1", 1, 350));
			this.m_bord.Add(base.Game.CreatePhysicalObj(959, 148, "board4", "game.crazytank.assetmap.Board001", "1", 1, 325));
			this.m_bord.Add(base.Game.CreatePhysicalObj(177, 261, "board5", "game.crazytank.assetmap.Board001", "1", 1, 22));
			this.m_bord.Add(base.Game.CreatePhysicalObj(514, 277, "board6", "game.crazytank.assetmap.Board001", "1", 1, 336));
			this.m_bord.Add(base.Game.CreatePhysicalObj(782, 285, "board7", "game.crazytank.assetmap.Board001", "1", 1, 23));
			this.m_bord.Add(base.Game.CreatePhysicalObj(1061, 280, "board8", "game.crazytank.assetmap.Board001", "1", 1, 22));
			this.m_bord.Add(base.Game.CreatePhysicalObj(273, 406, "board9", "game.crazytank.assetmap.Board001", "1", 1, 350));
			this.m_bord.Add(base.Game.CreatePhysicalObj(620, 408, "board10", "game.crazytank.assetmap.Board001", "1", 1, 23));
			this.m_bord.Add(base.Game.CreatePhysicalObj(873, 414, "board11", "game.crazytank.assetmap.Board001", "1", 1, 336));
			this.m_bord.Add(base.Game.CreatePhysicalObj(1155, 428, "board12", "game.crazytank.assetmap.Board001", "1", 1, 336));
			this.m_door = base.Game.CreatePhysicalObj(1275, 556, "door", "game.crazytank.assetmap.CrystalDoor001", "start", 1, 0);
			int[] array = new int[]
			{
				12,
				12,
				12,
				12,
				12,
				12,
				12,
				12,
				12,
				12,
				12,
				12
			};
			for (int i = 0; i < base.Game.TotalCount; i++)
			{
				int num = base.Game.Random.Next(0, 12);
				if (array[num] == num)
				{
					i--;
				}
				else
				{
					array[num] = num;
					this.m_bord.ToArray()[num].PlayMovie("2", 0, 0);
					this.KeyIndex = string.Format("Key{0}", num);
					this.m_key.Add(base.Game.CreatePhysicalObj(this.m_bord.ToArray()[num].X, this.m_bord.ToArray()[num].Y - 8, this.KeyIndex, "game.crazytank.assetmap.Key", "1", 1, 0));
					base.Game.SendGameObjectFocus(1, this.m_bord.ToArray()[num].Name, 0, 0);
				}
			}
			base.Game.SendGameObjectFocus(1, "door", 1000, 0);
			List<LoadingFileInfo> list = new List<LoadingFileInfo>();
			list.Add(new LoadingFileInfo(2, "sound/Sound201.swf", "Sound201"));
			list.Add(new LoadingFileInfo(2, "sound/Sound202.swf", "Sound202"));
			base.Game.SendLoadResource(list);
			base.Game.GameOverResources.Add("game.crazytank.assetmap.CrystalDoor001");
		}

		public override void OnNewTurnStarted()
		{
			base.OnNewTurnStarted();
			if (base.Game.CurrentLiving != null)
			{
				((Player)base.Game.CurrentLiving).Seal((Player)base.Game.CurrentLiving, 0, 0);
			}
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			if (base.Game.CurrentLiving != null)
			{
				((Player)base.Game.CurrentLiving).SetBall(3);
			}
		}

		public override bool CanGameOver()
		{
			for (int i = 0; i < 12; i++)
			{
				foreach (Player current in base.Game.GetAllFightPlayers())
				{
					if (current.X > this.m_bord[i].X - 40 && current.X < this.m_bord[i].X + 40 && current.Y < this.m_bord[i].Y && current.Y > this.m_bord[i].Y - 40)
					{
						if (this.m_bord[i].CurrentAction == "2")
						{
							this.m_bord[i].PlayMovie("3", 0, 0);
							this.KeyIndex = string.Format("Key{0}", i);
							base.Game.RemovePhysicalObj(base.Game.FindPhysicalObjByName(this.KeyIndex)[0], true);
							this.m_count++;
						}
					}
				}
			}
			if (this.m_count == base.Game.TotalCount)
			{
				base.Game.SendGameObjectFocus(2, "door", 0, 6000);
				base.Game.SendPlaySound("201");
				this.m_door.PlayMovie("end", 4000, 3000);
				base.Game.SendPlaySound("202");
				base.Game.SendUpdateUiData();
				base.Game.TurnQueue.Clear();
			}
			return (base.Game.TurnIndex > base.Game.TotalTurn - 1 && this.m_count != base.Game.TotalCount) || this.m_door.CurrentAction == "end";
		}

		public override int UpdateUIData()
		{
			return this.m_count;
		}

		public override void OnGameOver()
		{
			base.OnGameOver();
			if (this.m_door.CurrentAction == "end")
			{
				foreach (Player current in base.Game.GetAllFightPlayers())
				{
					current.SetSeal(false);
				}
				base.Game.AddAllPlayerToTurn();
				base.Game.IsWin = true;
			}
			else
			{
				base.Game.IsWin = false;
			}
			List<LoadingFileInfo> list = new List<LoadingFileInfo>();
			list.Add(new LoadingFileInfo(2, "image/map/show6.jpg", ""));
			base.Game.SendLoadResource(list);
		}
	}
}
