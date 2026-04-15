using Game.Logic.AI;

namespace Game.Server.GameServerScript.AI.Game
{
    public class IcyWinterAreaSimpleGame : APVEGameControl // Buz ve Kar Dünyası Kolay
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
            base.Game.SetupMissions("26001,26002"); //Buz ve Kar Dünyası kolay mession idleri
            base.Game.TotalMissionCount = 2; //Buz ve Kar Dünyası kolay toplam etap sayısı 2
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