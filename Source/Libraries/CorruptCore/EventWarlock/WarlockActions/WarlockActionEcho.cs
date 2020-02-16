using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock.WarlockActions
{
    /// <summary>
    /// Example action
    /// </summary>
    [Serializable]
    public class WarlockActionEcho : WarlockAction
    {
        string Data { get; set; }

        public WarlockActionEcho(string data)
        {
            Data = data;
        }

        public override void DoAction(Grimoire grimoire)
        {
            Console.WriteLine("Repeating: " + Data);
        }
    }
}
