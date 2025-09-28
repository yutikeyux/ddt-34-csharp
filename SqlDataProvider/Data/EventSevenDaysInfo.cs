using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataProvider.Data
{
    public class EventSevenDaysInfo : DataObject
    {
        public int ServerID { get; set; }
        public int UserID { get; set; }
        public bool IsFirstStreng { get; set; }
        public bool IsFirstLv { get; set; }
    }
}
