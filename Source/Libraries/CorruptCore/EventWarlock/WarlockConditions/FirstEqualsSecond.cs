using RTCV.CorruptCore.EventWarlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock.WarlockConditions
{
    /// <summary>
    /// Example conditional
    /// </summary>
    [Serializable]
    public class FirstEqualsSecond : EWConditional
    {
        string a = "";
        string b = "";
        public FirstEqualsSecond(string a, string b)
        {
            this.a = a;
            this.b = b;
        }

        public override bool Evaluate(Grimoire grimoire)
        {
            return a == b;
        }
    }
}
