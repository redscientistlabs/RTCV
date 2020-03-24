using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock
{

    /// <summary>
    /// All subclasses must be serializable
    /// </summary>
    [Serializable]
    public abstract class WarlockAction
    {
        //Add param data in subclasses
        public abstract void DoAction(Grimoire grimoire);
    }
}
