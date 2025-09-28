using System;
using System.Collections.Generic;
using System.Linq;
using Bussiness;
using SqlDataProvider.Data;

namespace Game.Server.ConsortiaTask
{
    public class ConsortiaTaskData
    {
        private readonly List<ConsortiaTaskConditions>[,] consortiaTaskConditions = new List<ConsortiaTaskConditions>[5, 10];

        public void AddTask(ConsortiaTaskConditions consortiaTask)
        {
            int typeIndex = consortiaTask.Type - 1;
            int levelIndex = consortiaTask.Level - 1;

            // Validate indices
            if (typeIndex < 0 || typeIndex >= 5 || levelIndex < 0 || levelIndex >= 10)
            {
                // Log invalid parameters
                return;
            }

            var taskList = consortiaTaskConditions[typeIndex, levelIndex];
            if (taskList == null)
            {
                taskList = new List<ConsortiaTaskConditions>();
                consortiaTaskConditions[typeIndex, levelIndex] = taskList;
            }
            taskList.Add(consortiaTask);
        }

        public ConsortiaTaskConditions GetTaskConditionDataInfo(int type, int level)
        {
            int typeIndex = type - 1;
            int levelIndex = level - 1;

            // Validate indices
            if (typeIndex < 0 || typeIndex >= 5 || levelIndex < 0 || levelIndex >= 10)
            {
                return null;
            }

            var taskList = consortiaTaskConditions[typeIndex, levelIndex];

            // Handle null or empty lists
            if (taskList == null || taskList.Count == 0)
            {
                return null;
            }

            taskList.Shuffle();
            return taskList.Random();
        }
    }
}