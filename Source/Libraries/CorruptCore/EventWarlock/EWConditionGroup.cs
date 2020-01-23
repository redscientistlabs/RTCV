using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock
{
    [System.Serializable]
    class EWConditionGroup : EWConditional
    {

        List<EWConditional> Questions = new List<EWConditional>(2);

        public void AddConditional(EWConditional w)
        {
            Questions.Add(w);
        }

        public void AddOperator(QuestionOp op)
        {
            if (Questions.Count == 0) return;
            else { Questions[Questions.Count - 1].NextOp = op; }
        }

        public override bool Evaluate()
        {
            int ct = Questions.Count;
            bool res = Questions[0].Evaluate();
            //bypassed if only one
            for (int j = 1; j < ct; j++)
            {
                var next = Questions[j - 1].NextOp;
                if (next == QuestionOp.AND) {
                    res = res && Questions[j].Evaluate();
                }
                else if(next == QuestionOp.OR)
                {
                    res = res || Questions[j].Evaluate();
                }
                else
                {
                    //throw new Exception("Question NextOp not set");
                    Console.WriteLine("Question NextOp not set");
                    break;//testing
                }
            }
            return res;
        }
    }
}
