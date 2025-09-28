using System;
using Game.Logic.AI;

namespace GameServerScript.AI.Game
{
    // Token: 0x020001B6 RID: 438
    public class UngMaPhapSimpleGame : APVEGameControl
    {
        // Token: 0x060016FE RID: 5886 RVA: 0x000B1721 File Offset: 0x000AF921
        public override void OnCreated()
        {
            base.OnCreated();
            base.Game.SetupMissions("77150");
            base.Game.TotalMissionCount = 1;
        }

        // Token: 0x060016FF RID: 5887 RVA: 0x000B1748 File Offset: 0x000AF948
        public override void OnPrepated()
        {
            base.OnPrepated();
            base.Game.SessionId = 0;
        }

        // Token: 0x06001700 RID: 5888 RVA: 0x000B1760 File Offset: 0x000AF960
        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            int result;
            if (score > 900)
            {
                result = 3;
            }
            else
            {
                if (score > 825)
                {
                    result = 2;
                }
                else
                {
                    if (score > 725)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            return result;
        }

        // Token: 0x06001701 RID: 5889 RVA: 0x000B17C7 File Offset: 0x000AF9C7
        public override void OnGameOverAllSession()
        {
            base.OnGameOverAllSession();
        }
    }
}
