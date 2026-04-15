using Game.Logic.AI;

namespace GameServerScript.AI.Game
{
    public class DragonWarNormalGame : APVEGameControl //ejderha savaşı normal
    {
        public override void OnCreated()
        {
			base.Game.SetupMissions("5101,5102,5103,5104"); //ejderha savaşı normal misson id leri etap 1, etap 2, etap 3 ve etap 4
            base.Game.TotalMissionCount = 4; //ejderha savaşı normal etap sayısı 4
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
