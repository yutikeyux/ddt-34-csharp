using Game.Logic.AI;

namespace GameServerScript.AI.Game
{
    public class GuluKingdomTerrorGame : APVEGameControl //bogo kahraman
    {
        public override void OnCreated()
        {
			base.OnCreated();
			base.Game.SetupMissions("1371,1372,1373,1374,1375"); //bogo kahraman misson id leri etap 1, etap 2, etap 3, etap 4 ve etap 5
            base.Game.TotalMissionCount = 5; //etap sayısı 5
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
