using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock
{
    [Serializable]
    abstract class WarlockEffect
    {
        public virtual void Load() { }
        public virtual void PreExecute() { }
        public virtual void PostExecute() { }
    }
}
