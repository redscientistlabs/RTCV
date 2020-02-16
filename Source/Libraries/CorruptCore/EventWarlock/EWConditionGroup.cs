using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock
{
    [System.Serializable]
    public class EWConditionGroup : EWConditional
    {
        /// <summary>
        /// The list of conditionals
        /// </summary>
        public List<EWConditional> Conditionals = new List<EWConditional>(2);

        /// <summary>
        /// Adds a conditional. If an operator wasn't set on the last conditional it is automatically assigned a QuestionOp.AND
        /// </summary>
        /// <param name="w"></param>
        public void AddConditional(EWConditional w)
        {
            Conditionals.Add(w);
            if(Conditionals.Count > 1 && Conditionals[Conditionals.Count-1].NextOp == QuestionOp.NONE)
            {
                Conditionals[Conditionals.Count - 1].NextOp = QuestionOp.AND;
            }
        }

        public void AddOperator(QuestionOp op)
        {
            if (Conditionals.Count == 0) { return; }
            else { Conditionals[Conditionals.Count - 1].NextOp = op; }
        }

        //Could use optimization
        public override bool Evaluate(Grimoire grimoire)
        {
            int ct = Conditionals.Count;
            bool res = Conditionals[0].Evaluate(grimoire);
            //bypassed if only one
            for (int j = 1; j < ct; j++)
            {
                var next = Conditionals[j - 1].NextOp;
                if (next == QuestionOp.AND) {
                    res = res && Conditionals[j].Evaluate(grimoire);
                }
                else if(next == QuestionOp.OR)
                {
                    res = res || Conditionals[j].Evaluate(grimoire);
                }
                else
                {
                    break;
                }
            }
            return res;
        }
    }
}
