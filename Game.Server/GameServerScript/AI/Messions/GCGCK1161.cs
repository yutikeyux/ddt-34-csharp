using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;
using System.Drawing;
//civciv zor etap 1
namespace GameServerScript.AI.Messions
{
    public class GCGCK1161 : AMissionControl
    {
		private List<SimpleNpc> CivcivListe;

		private List<SimpleNpc> DoðdurListe;

		private List<Point> KabukluListe;

		private List<Point> KabuksuzListe;

		private PhysicalObj efekt;

		private int MaxDoðmaSayýsý;

		private int ÝdealDoðmaSayýsý;

		private int KabuksuzDoðmaSayýsý;

		private int KabukluDoðmaSayýsý;

		private int KabukluArttýr;

		private int KabuksuzArttýr;

		private int KabukluCivciv;

		private int KabuksuzCivciv;

		public override int CalculateScoreGrade(int score)
		{
			base.CalculateScoreGrade(score);
			if (score > 930)
			{
				return 3;
			}
			if (score > 850)
			{
				return 2;
			}
			if (score > 775)
			{
				return 1;
			}
			return 0;
		}

		public override void OnPrepareNewSession()
		{
			base.OnPrepareNewSession();
			int[] npcIDleri = new int[2]
			{
				KabukluCivciv,
				KabuksuzCivciv
			};
			base.Game.AddLoadingFile(2, "image/game/living/living176.swf", "game.living.Living176");
			base.Game.LoadResources(npcIDleri);
			base.Game.LoadNpcGameOverResources(npcIDleri);
			base.Game.SetMap(1161);
		}

		public override void OnStartGame()
		{
			base.OnStartGame();
			efekt = base.Game.Createlayer(1200, 955, "kingmoive", "game.living.Living176", "in", 1, 0);
			DoðdurCivciv2(KabukluDoðmaSayýsý);
			DoðdurCivciv1(KabuksuzDoðmaSayýsý);
		}

		private void DoðdurCivciv1(int int_8)
		{
			for (int i = 0; i < int_8; i++)
			{
				Point point = ((i < KabukluListe.Count) ? KabukluListe[i] : KabukluListe[base.Game.Random.Next(KabukluListe.Count)]);
				CivcivListe.Add(base.Game.CreateNpc(KabukluCivciv, point.X, point.Y, 0, -1));
			}
		}

		private void DoðdurCivciv2(int int_8)
		{
			for (int i = 0; i < int_8; i++)
			{
				Point point = ((i < KabuksuzListe.Count) ? KabuksuzListe[i] : KabuksuzListe[base.Game.Random.Next(KabuksuzListe.Count)]);
				DoðdurListe.Add(base.Game.CreateNpc(KabukluCivciv, point.X, point.Y, 0, -1));
			}
		}

		public override void OnNewTurnStarted()
		{
			base.OnNewTurnStarted();
			if (KabukluArttýr < KabuksuzDoðmaSayýsý && CivcivListe.Count < MaxDoðmaSayýsý)
			{
				int num = ((KabuksuzDoðmaSayýsý - KabukluArttýr > MaxDoðmaSayýsý - CivcivListe.Count) ? (MaxDoðmaSayýsý - CivcivListe.Count) : (KabuksuzDoðmaSayýsý - KabukluArttýr));
				if (num > 0)
				{
					DoðdurCivciv1(num);
				}
			}
			if (KabuksuzArttýr < KabukluDoðmaSayýsý && DoðdurListe.Count < ÝdealDoðmaSayýsý)
			{
				int num2 = ((KabukluDoðmaSayýsý - KabuksuzArttýr > ÝdealDoðmaSayýsý - DoðdurListe.Count) ? (ÝdealDoðmaSayýsý - DoðdurListe.Count) : (KabukluDoðmaSayýsý - KabuksuzArttýr));
				if (num2 > 0)
				{
					DoðdurCivciv2(num2);
				}
			}
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
		}

		public override bool CanGameOver()
		{
			base.CanGameOver();
			if (base.Game.GetLivedLivings().Count == 0)
			{
				base.Game.PveGameDelay = 0;
			}
			KabuksuzArttýr = 0;
			KabukluArttýr = 0;
			foreach (SimpleNpc item in CivcivListe)
			{
				if (item.IsLiving)
				{
					KabukluArttýr++;
				}
			}
			foreach (SimpleNpc item2 in DoðdurListe)
			{
				if (item2.IsLiving)
				{
					KabuksuzArttýr++;
				}
			}
			if (DoðdurListe.Count >= ÝdealDoðmaSayýsý && CivcivListe.Count >= MaxDoðmaSayýsý && base.Game.GetLivedLivings().Count <= 0)
			{
				return true;
			}
			if (base.Game.TurnIndex > base.Game.MissionInfo.TotalTurn)
			{
				return true;
			}
			return false;
		}

		public override int UpdateUIData()
		{
			base.UpdateUIData();
			return base.Game.TotalKillCount;
		}

		public override void OnGameOver()
		{
			base.OnGameOver();
			if (base.Game.GetLivedLivings().Count == 0)
			{
				base.Game.IsWin = true;
			}
			else
			{
				base.Game.IsWin = false;
			}
		}

		public GCGCK1161()
        {
			CivcivListe = new List<SimpleNpc>();
			DoðdurListe = new List<SimpleNpc>();
			KabukluListe = new List<Point>
			{
				new Point(958, 950),
				new Point(1400, 950),
				new Point(1034, 950),
				new Point(1472, 950)
			};
			KabuksuzListe = new List<Point>
			{
				new Point(1150, 950),
				new Point(1346, 950)
			};
			MaxDoðmaSayýsý = 20;
			ÝdealDoðmaSayýsý = 10;
			KabuksuzDoðmaSayýsý = 10;
			KabukluDoðmaSayýsý = 5;
			KabukluCivciv = 7202;
			KabuksuzCivciv = 7201;
        }
    }
}
