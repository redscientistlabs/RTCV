using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock.WarlockActions
{
    [Serializable]
    class WarlockActionEcho : WarlockAction
    {
        string Data { get; set; }

        public WarlockActionEcho(string data)
        {
            Data = data;
        }

        public override void DoAction()
        {
            Console.WriteLine("Repeating: " + Data);
        }
    }
}
