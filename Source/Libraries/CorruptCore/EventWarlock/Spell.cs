using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock
{
    [System.Serializable]
    class Spell
    {
        EWConditional Conditionals = null;
        List<WarlockAction> Actions = new List<WarlockAction>();

        public void SetConditional(EWConditional conditional)
        {
            Conditionals = conditional;
        }
        public void SetActions(List<WarlockAction> action)
        {
            Actions = action;
        }
        public void AddAction(WarlockAction action)
        {
            Actions.Add(action);
        }

        public void Execute()
        {
            //if debug
            if(Actions.Count == 0)
            {
                Console.WriteLine("Actions are empty dummy");
            }

            if (Conditionals == null || Conditionals.Evaluate())
            {
                
                for (int j = 0; j < Actions.Count; j++)
                {
                    Actions[j].DoAction();
                }
            }
        }

        //List<Wizard>
    }
}
