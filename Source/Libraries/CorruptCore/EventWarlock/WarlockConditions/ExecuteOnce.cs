using RTCV.CorruptCore.EventWarlock;
using RTCV.CorruptCore.EventWarlock.Editor;
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
    [WarlockEditable]
    public class ExecuteOnce : EWConditional
    {
        bool executed = false;
        
        public override bool Evaluate(Grimoire grimoire)
        {
            if (executed == false)
            {
                executed = true;
                return true;
            }
            return false;
        }
    }
}
