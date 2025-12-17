using Game.Logic.AI;

namespace GameServerScript.AI.Game
{
    public class YengecKralHardGame : APVEGameControl
    {
        
        

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1750)
            {
                return 3;
            }
            else if (score > 1675)
            {
                return 2;
            }
            else if (score > 1600)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override void OnCreated()
        {
			base.OnCreated();
			base.Game.SetupMissions("12014");
			base.Game.TotalMissionCount = 1;
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

