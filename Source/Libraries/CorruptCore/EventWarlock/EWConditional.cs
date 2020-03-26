using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock
{
    [Serializable]
    public abstract class EWConditional
    {
        public abstract bool Evaluate(Grimoire grimoire);
    }
}
