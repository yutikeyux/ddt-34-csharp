using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;
using System.Drawing;

namespace GameServerScript.AI.Messions
{
    public class GCGCD1161 : AMissionControl
    {
		//civciv kolay etap 1
		private List<SimpleNpc> CivcivListe;

		private List<SimpleNpc> Do�durListe;

		private List<Point> KabukluListe;

		private List<Point> KabuksuzListe;

		private PhysicalObj efekt;

		private int MaxDo�maSay�s�;

		private int �dealDo�maSay�s�;

		private int KabuksuzDo�maSay�s�;

		private int KabukluDo�maSay�s�;

		private int KabukluArtt�r;

		private int KabuksuzArtt�r;

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
			Do�durCivciv2(KabukluDo�maSay�s�);
			Do�durCivciv1(KabuksuzDo�maSay�s�);
		}

		private void Do�durCivciv1(int int_8)
		{
			for (int i = 0; i < int_8; i++)
			{
				Point point = ((i < KabukluListe.Count) ? KabukluListe[i] : KabukluListe[base.Game.Random.Next(KabukluListe.Count)]);
				CivcivListe.Add(base.Game.CreateNpc(KabukluCivciv, point.X, point.Y, 0, -1));
			}
		}

		private void Do�durCivciv2(int int_8)
		{
			for (int i = 0; i < int_8; i++)
			{
				Point point = ((i < KabuksuzListe.Count) ? KabuksuzListe[i] : KabuksuzListe[base.Game.Random.Next(KabuksuzListe.Count)]);
				Do�durListe.Add(base.Game.CreateNpc(KabukluCivciv, point.X, point.Y, 0, -1));
			}
		}

		public override void OnNewTurnStarted()
		{
			base.OnNewTurnStarted();
			if (KabukluArtt�r < KabuksuzDo�maSay�s� && CivcivListe.Count < MaxDo�maSay�s�)
			{
				int num = ((KabuksuzDo�maSay�s� - KabukluArtt�r > MaxDo�maSay�s� - CivcivListe.Count) ? (MaxDo�maSay�s� - CivcivListe.Count) : (KabuksuzDo�maSay�s� - KabukluArtt�r));
				if (num > 0)
				{
					Do�durCivciv1(num);
				}
			}
			if (KabuksuzArtt�r < KabukluDo�maSay�s� && Do�durListe.Count < �dealDo�maSay�s�)
			{
				int num2 = ((KabukluDo�maSay�s� - KabuksuzArtt�r > �dealDo�maSay�s� - Do�durListe.Count) ? (�dealDo�maSay�s� - Do�durListe.Count) : (KabukluDo�maSay�s� - KabuksuzArtt�r));
				if (num2 > 0)
				{
					Do�durCivciv2(num2);
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
			KabuksuzArtt�r = 0;
			KabukluArtt�r = 0;
			foreach (SimpleNpc item in CivcivListe)
			{
				if (item.IsLiving)
				{
					KabukluArtt�r++;
				}
			}
			foreach (SimpleNpc item2 in Do�durListe)
			{
				if (item2.IsLiving)
				{
					KabuksuzArtt�r++;
				}
			}
			if (Do�durListe.Count >= �dealDo�maSay�s� && CivcivListe.Count >= MaxDo�maSay�s� && base.Game.GetLivedLivings().Count <= 0)
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

		public GCGCD1161()
        {
			CivcivListe = new List<SimpleNpc>();
			Do�durListe = new List<SimpleNpc>();
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
			MaxDo�maSay�s� = 11;
			�dealDo�maSay�s� = 5;
			KabuksuzDo�maSay�s� = 4;
			KabukluDo�maSay�s� = 2;
			KabukluCivciv = 7002;
			KabuksuzCivciv = 7001;
        }
    }
}
//civciv kolay etap 1