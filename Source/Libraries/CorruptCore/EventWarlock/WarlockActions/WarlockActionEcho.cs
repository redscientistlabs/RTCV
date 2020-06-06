using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTCV.CorruptCore.EventWarlock.Editor;

namespace RTCV.CorruptCore.EventWarlock.WarlockActions
{
    /// <summary>
    /// Example action
    /// </summary>
    [Serializable]
    [WarlockEditable]
    public class WarlockActionEcho : WarlockAction
    {
        [WarlockEditorField("Data")] string data;

        public WarlockActionEcho(string data)
        {
            this.data = data;
        }

        public override void DoAction(Grimoire grimoire)
        {
            Console.WriteLine("Repeating: " + data);
        }
    }
}
