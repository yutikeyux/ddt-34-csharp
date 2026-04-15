using Game.Logic.AI;

namespace GameServerScript.AI.Game
{
    public class DarkCastleTerrorGame : APVEGameControl //kale kahraman
    {
        public override void OnCreated()
        {
			base.Game.SetupMissions("4301,4302,4303"); //kale kahraman misson id leri etap 1, etap 2 ve etap 3
            base.Game.TotalMissionCount = 3; //kale kahraman etap sayısı 3
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
        }
    }
}
