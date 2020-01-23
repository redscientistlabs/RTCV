using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock
{
    /// <summary>
    /// Abstract due to serialization
    /// </summary>
    [Serializable]
    abstract class EWConditional
    {
        public QuestionOp NextOp = QuestionOp.NONE;
        public EWConditional SetNextOp(QuestionOp op) { NextOp = op; return this; }
        public abstract bool Evaluate();
    }
}
