using Game.Logic.AI;

namespace GameServerScript.AI.Game
{
    public class DragonWarTerrorGame : APVEGameControl //ejderha savaşı kahraman
    {
        public override void OnCreated()
        {
			base.Game.SetupMissions("5301,5302,5303,5304"); //ejderha savaşı kahraman misson id leri etap 1, etap 2, etap 3 ve etap 4
            base.Game.TotalMissionCount = 4;//ejderha savaşı kahraman etap sayısı 4
        }

        public override void OnPrepated()
        {
			base.OnPrepated(); 
        }

        public override int CalculateScoreGrade(int score)
        {
			if (score > 800)
			{
				return 3;
			}
			if (score > 725)
			{
				return 2;
			}
			if (score > 650)
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
