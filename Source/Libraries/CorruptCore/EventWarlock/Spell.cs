using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock
{
    [System.Serializable]
    public class Spell
    {
        public string Name;
        public Spell(string name = "Unnamed")
        {
            Name = name;
        }


        public EWConditional Conditionals = null;
        public List<WarlockAction> Actions = new List<WarlockAction>();
        private bool isElse = false;

        public void SetConditional(EWConditional conditional, bool isElse = false)
        {
            this.isElse = isElse;
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

        /// <summary>
        /// Checks the conditionals and executes the actions if the conditionals evaluate to true. Returns the conditional result
        /// </summary>
        /// <returns>the conditional result</returns>
        public bool Execute()
        {
            bool res = true;

            bool doLogic = true;

            if (isElse && Warlock.LastResult)
            {
                doLogic = false;
                //return value will be true, prevents other elses from executing
            }

            if (doLogic)
            {
                if (Conditionals == null || (res = Conditionals.Evaluate()))
                {
                    for (int j = 0; j < Actions.Count; j++)
                    {
                        Actions[j].DoAction();
                    }
                }
            }

            return res;
        }
        //List<Wizard>
    }
}
