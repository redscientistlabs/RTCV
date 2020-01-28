using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock
{
    /// <summary>
    /// Name subclasses what you want them to appear as in the editor
    /// </summary>
    [Serializable]
    public abstract class EWConditional
    {
        public QuestionOp NextOp = QuestionOp.NONE;
        public EWConditional SetNextOp(QuestionOp op) { NextOp = op; return this; }
        public abstract bool Evaluate();
    }
}
