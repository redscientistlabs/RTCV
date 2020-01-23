using RTCV.CorruptCore.EventWarlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializeTest.EventWarlock.WarlockConditions
{
    [Serializable]
    class FirstEqualsSecond : EWConditional
    {
        string a = "";
        string b = "";
        public FirstEqualsSecond(string a, string b)
        {
            this.a = a;
            this.b = b;
        }

        public override bool Evaluate()
        {
            return a == b;
        }
    }
}
