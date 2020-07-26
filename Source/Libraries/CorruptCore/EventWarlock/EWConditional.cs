namespace RTCV.CorruptCore.EventWarlock
{
    using System;

    [Serializable]
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
