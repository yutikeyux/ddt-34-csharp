using Game.Logic.AI;

namespace GameServerScript.AI.Game
{
    public class GuluKingdomHardGame : APVEGameControl //bogo zor
    {
        public override void OnCreated()
        {
			base.OnCreated();
			base.Game.SetupMissions("1271,1272,1273,1274"); //bogo zor misson id leri etap 1, etap 2, etap 3 ve etap 4
            base.Game.TotalMissionCount = 4; //etap sayısı 4
        }

        public override void OnPrepated()
        {
			base.OnPrepated();
        }

        public override int CalculateScoreGrade(int score)
        {
			base.CalculateScoreGrade(score);
			if (score > 800)
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
