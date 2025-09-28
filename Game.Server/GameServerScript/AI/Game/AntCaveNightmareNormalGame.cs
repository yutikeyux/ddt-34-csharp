using Game.Logic.AI;

namespace GameServerScript.AI.Game
{
    public class AntCaveNightmareNormalGame : APVEGameControl
    {
        public override void OnCreated()
        {
			base.OnCreated();
			base.Game.SetupMissions("2501,2502");
			base.Game.TotalMissionCount = 2;
        }

        public override void OnPrepated()
        {
			base.OnPrepated();
        }

        public override int CalculateScoreGrade(int score)
        {
			base.CalculateScoreGrade(score);
			if (score > 900)
			{
				return 3;
			}
			if (score > 825)
			{
				return 2;
			}
			if (score > 725)
			{
				return 1;
			}
			return 0;
        }

        public override void OnGameOverAllSession()
        {
			base.OnGameOverAllSession();
        }
    }
}
