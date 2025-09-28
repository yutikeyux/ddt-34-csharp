using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDataProvider.Data
{
    public class SubActiveConditionInfo
    {
        public int ID { get; set; }
        public int ActiveID { get; set; }
        public int SubID { get; set; }
        public int ConditionID { get; set; }
        public int Type { get; set; }
        public string Value { get; set; }
        public int AwardType { get; set; }
        public string AwardValue { get; set; }
        public bool IsValid { get; set; }

        public int GetValue(int index)
        {
            string[] valueArr = Value.Split('-');
            int place = (index * 2) - 1;
            if (string.IsNullOrEmpty(Value) || place < 0 || place > valueArr.Length)
            {
                Console.WriteLine("SubActiveCondition GetValue: Key {0}, valueArr.Length {1},  place {2}. Outside the bounds of the array", index, valueArr.Length, place);
                return 0;
            }
            try
            {
                string value = valueArr[place];

                return int.Parse(value);
            }
            catch
            {
                Console.WriteLine("SubActiveCondition GetValue: Key {0}, valueArr.Length {1},  place {2} error", index, valueArr.Length, place);
                return 0;
            }
        }

        public int OnStrengThen()
        {
            return GetValue(10);
        }

        public bool OnGold()
        {
            return GetValue(11) == 1;
        }

        public bool OnAvaible(int value1, int value3)
        {
            return GetValue(12) == value1 && ConditionID == value3;
        }
    }
}
