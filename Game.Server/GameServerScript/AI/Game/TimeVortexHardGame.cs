using Game.Logic.AI;

namespace GameServerScript.AI.Game
{
    public class TimeVortexHardGame : APVEGameControl //zaman anaforu zor
    {
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

        public override void OnCreated()
        {
			base.OnCreated();
			base.Game.SetupMissions("12201,12202,12204"); //zaman anaforu zor misson id leri etap 1, etap 2 ve etap 3
            base.Game.TotalMissionCount = 3; //zaman anaforu zor etap sayısı 3
        }

        public override void OnGameOverAllSession()
        {
			base.OnGameOverAllSession();
        }

        public override void OnPrepated()
        {
			base.OnPrepated();
        }
    }
}
