using Game.Logic.AI;

namespace GameServerScript.AI.Game
{
    public class DarkCastleNormalGame : APVEGameControl //kale normal
    {
        public override void OnCreated()
        {
			base.Game.SetupMissions("4101,4102,4103"); //kale normal misson id leri etap 1, etap 2 ve etap 3
            base.Game.TotalMissionCount = 3; //kale normal etap sayısı 3
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
