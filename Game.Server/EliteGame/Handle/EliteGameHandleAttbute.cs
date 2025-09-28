using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Server.EliteGame.Handle
{
    class EliteGameHandleAttbute : Attribute
    {
        public byte Code
        {
            get;
            private set;
        }
        public EliteGameHandleAttbute(byte code)
        {
            Code = code;
        }
    }
}
