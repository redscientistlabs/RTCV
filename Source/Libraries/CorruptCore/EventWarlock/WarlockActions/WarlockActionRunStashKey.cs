using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ceras;
using RTCV.CorruptCore.EventWarlock.Editor;

namespace RTCV.CorruptCore.EventWarlock.WarlockActions
{
    /// <summary>
    /// Example action
    /// </summary>
    [Serializable]
    [WarlockEditable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class WarlockActionRunStashKey : WarlockAction
    {
        private StashKey sk;


        /// <summary>
        /// Parameterless consturctor for serialization. DON'T USE THIS.
        /// </summary>
        public WarlockActionRunStashKey() { }

        public WarlockActionRunStashKey(StashKey _sk)
        {
            sk = _sk;
        }

        public override void DoAction(Grimoire grimoire)
        {
            sk.Run();
        }
    }
}
