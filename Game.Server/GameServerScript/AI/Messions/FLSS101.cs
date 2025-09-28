using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
    public class FLSS101 : AMissionControl
    {
        private SimpleNpc someNpc;

        private bool isBeginResult;

        private bool isCreateNpcFirst = true;

        private int createNpcCount;

        private int arightResultCount;

        private int needArightResult = 3;

        private int timeOut = 30;

        private int currQuizID;

        private int enterQuizID;

        private int redNpcID = 4;

        private int maxQuizSize = 10;

        private string[,] resultStr = new string[10, 3] {
            {"","",""},
            {"","",""},
            {"","",""},
            {"","",""},
            {"","",""},
            {"","",""},
            {"","",""},
            {"","",""},
            {"","",""},
            {"","",""},
        };

        string[] distanceStr = new string[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", };

        private int[] arightResult = new int[10];

        private int[] createNpcX = new int[10];

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1870)
            {
                return 3;
            }
            if (score > 1825)
            {
                return 2;
            }
            if (score > 1780)
            {
                return 1;
            }
            return 0;
        }

        public override void OnPrepareNewGame()
        {
            base.OnPrepareNewGame();
            InitNpcCoor();
        }

        public void InitNpcCoor()
        {
            List<int> m_tempList = new List<int>();
            int last = -1;
            int x = 0;
            for (int i = 0; i < maxQuizSize; i++)
            {
                m_tempList.Clear();
                do
                {
                    x = base.Game.Random.Next(0, 10);
                    createNpcX[i] = (x + 1) * 100 + 75;
                }
                while (x == last);
                last = x;
                int n = base.Game.Random.Next(0, 3);
                resultStr[i, n] = distanceStr[x];
                arightResult[i] = n;
                if (x - 1 >= 0 && x + 1 < 10)
                {
                    m_tempList.Add(x - 1);
                    m_tempList.Add(x + 1);
                }
                else if (x - 1 < 0)
                {
                    m_tempList.Add(x + 1);
                    m_tempList.Add(x + 2);
                }
                else if (x + 1 >= 10)
                {
                    m_tempList.Add(x - 1);
                    m_tempList.Add(x - 2);
                }
                for (int j = 0; j < 3; j++)
                {
                    if (resultStr[i, j] != distanceStr[x])
                    {
                        resultStr[i, j] = distanceStr[m_tempList[0]];
                        m_tempList.RemoveAt(0);
                    }
                }
            }
        }

        public override void OnGeneralCommand(GSPacketIn packet)
        {
            switch (packet.ReadInt())
            {
                case 0:
                    CreateNpc();
                    break;
                case 1:
                    Reset(isFirstNpc: true);
                    KillNpc();
                    break;
                case 2:
                    SendQuizWindow();
                    break;
                case 3:
                    if (isBeginResult)
                    {
                        Reset(isFirstNpc: true);
                    }
                    break;
                case 4:
                    {
                        if (!isBeginResult)
                        {
                            break;
                        }
                        int quizID = packet.ReadInt();
                        int result = packet.ReadInt();
                        if (quizID == currQuizID)
                        {
                            if (result == arightResult[quizID - 1])
                            {
                                arightResultCount++;
                                SendQuizWindow();
                            }
                            else
                            {
                                SendQuizWindow();
                            }
                            enterQuizID = quizID;
                            base.Game.WaitTime(0);
                        }
                        break;
                    }
                case 5:
                    if (isBeginResult)
                    {
                        Reset(isFirstNpc: false);
                        SendQuizWindow();
                    }
                    break;
            }
        }

        public void CreateNpc()
        {
            if (createNpcCount <= maxQuizSize)
            {
                if (isCreateNpcFirst)
                {
                    someNpc = base.Game.CreateNpc(redNpcID, 575, 505, 2);
                    someNpc.SetRelateDemagemRect(-25, -55, 45, 55);
                    isCreateNpcFirst = false;
                    createNpcCount++;
                }
                else
                {
                    someNpc = base.Game.CreateNpc(redNpcID, createNpcX[currQuizID], 505, 2);
                    someNpc.SetRelateDemagemRect(-25, -55, 45, 55);
                    createNpcCount++;
                }
            }
        }

        public void KillNpc()
        {
            if (someNpc != null && someNpc.IsLiving)
            {
                base.Game.RemoveLiving(someNpc, sendToClient: true);
                someNpc = null;
            }
        }

        public void Reset(bool isFirstNpc)
        {
            currQuizID = 0;
            enterQuizID = 0;
            arightResultCount = 0;
            createNpcCount = 0;
            if (isFirstNpc)
            {
                isCreateNpcFirst = true;
            }
            else
            {
                isCreateNpcFirst = false;
            }
            isBeginResult = false;
            resultStr = new string[10, 3] {
                                {"","",""},
                                {"","",""},
                                {"","",""},
                                {"","",""},
                                {"","",""},
                                {"","",""},
                                {"","",""},
                                {"","",""},
                                {"","",""},
                                {"","",""},};
            InitNpcCoor();
        }

        private void SendQuizWindow()
        {
            if (currQuizID <= maxQuizSize - 1 && arightResultCount < needArightResult)
            {
                KillNpc();
                CreateNpc();
                isBeginResult = true;
                base.Game.SendOpenPopupQuestionFrame(currQuizID + 1, arightResultCount, needArightResult, maxQuizSize, timeOut, LanguageMgr.GetTranslation("FightLab.caption"), LanguageMgr.GetTranslation("FightLab.question"), resultStr[currQuizID, 0], resultStr[currQuizID, 1], resultStr[currQuizID, 2]);
                currQuizID++;
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] resources = { redNpcID };
            int[] gameOverResources = { redNpcID, redNpcID, redNpcID };
            base.Game.LoadResources(resources);
            base.Game.LoadNpcGameOverResources(gameOverResources);
            base.Game.SetMap(1136);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
            if (base.Game.CurrentLiving != null)
            {
                ((Player)base.Game.CurrentLiving).Seal((Player)base.Game.CurrentLiving, 0, 0);
            }
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override bool CanGameOver()
        {
            if (arightResultCount >= needArightResult)
            {
                base.Game.SendClosePopupQuestionFrame();
                base.Game.WaitTime(1000);
                base.Game.IsWin = true;
                return true;
            }
            if (enterQuizID == maxQuizSize)
            {
                base.Game.SendClosePopupQuestionFrame();
                base.Game.WaitTime(1000);
                if (arightResultCount >= needArightResult)
                {
                    base.Game.IsWin = true;
                }
                else
                {
                    base.Game.IsWin = false;
                }
                return true;
            }
            return false;
        }

        public override int UpdateUIData()
        {
            return base.Game.TotalKillCount;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (arightResultCount >= needArightResult)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
            new List<LoadingFileInfo>();
        }
    }
}