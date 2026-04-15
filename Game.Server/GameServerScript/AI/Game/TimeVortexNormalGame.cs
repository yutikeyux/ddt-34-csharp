using Game.Logic.AI;

namespace GameServerScript.AI.Game
{
    public class TimeVortexNormalGame : APVEGameControl //zaman anaforu normal
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
			base.Game.SetupMissions("12101,12102,12104"); //zaman anaforu normal misson id leri etap 1, etap 2 ve etap 3
            base.Game.TotalMissionCount = 3; //zaman anaforu normal etap sayısı 3
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
