using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock
{
    [Serializable]
    public abstract class WarlockAction
    {
        protected static HashSet<string> Flags = new HashSet<string>();
        protected static Dictionary<string, object> Variables = new Dictionary<string, object>();

        public static void Reset()
        {
            Flags.Clear();
            Variables.Clear();
        }

        //Add param data in subclasses
        public abstract void DoAction();
    }
}
