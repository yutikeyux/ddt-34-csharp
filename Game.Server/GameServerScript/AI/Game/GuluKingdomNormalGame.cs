using Game.Logic.AI;

namespace GameServerScript.AI.Game
{
    public class GuluKingdomNormalGame : APVEGameControl //bogo normal
    {
        public override void OnCreated()
        {
			base.OnCreated();
			base.Game.SetupMissions("1171,1172,1173"); //bogo normal misson id leri etap 1, etap 2 ve etap 3
            base.Game.TotalMissionCount = 3; //etap sayısı 3
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
