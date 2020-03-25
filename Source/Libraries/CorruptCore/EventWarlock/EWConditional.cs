using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ceras;

namespace RTCV.CorruptCore.EventWarlock
{
    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public abstract class EWConditional
    {
        public QuestionOp NextOp = QuestionOp.NONE;
        public EWConditional SetNextOp(QuestionOp op) { NextOp = op; return this; }
        public abstract bool Evaluate(Grimoire grimoire);

        public virtual void Smallify()
        {
            //Do nothing by default
        }
    }
}
