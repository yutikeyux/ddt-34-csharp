using Game.Logic.AI;
using System;

namespace GameServerScript.AI.Game
{
	public class QX70001 : APVEGameControl
	{
		public override void OnCreated()
		{
			base.Game.SetupMissions("70001");
			base.Game.TotalMissionCount = 1;
		}

		public override void OnPrepated()
		{
			base.Game.SessionId = 0;
		}

		public override int CalculateScoreGrade(int score)
		{
			int result;
			if (score > 800)
			{
				result = 3;
			}
			else if (score > 725)
			{
				result = 2;
			}
			else if (score > 650)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public override void OnGameOverAllSession()
		{
		}
	}
}
