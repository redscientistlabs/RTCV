namespace RTCV.CorruptCore.EventWarlock
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Holds conditionals and actions to be executed if conditionals evaluates to true
    /// </summary>
    [System.Serializable]
    public class Spell
    {
        //encapsulation will need to be figured out later

        public bool Enabled = true;
        public string Name;
        private EWConditional conditional = null;
        public EWConditional Conditional { get => conditional; }
        public List<WarlockAction> Actions = new List<WarlockAction>();
        public bool isElse = false;

        public Spell(string name = "Unnamed")
        {
            this.Name = name;
        }

        public void SetEnabled(bool enabled = true)
        {
            this.Enabled = enabled;
        }

        public void SetConditional(EWConditional conditional, bool isElse = false)
        {
            this.isElse = isElse;
            this.conditional = conditional;
        }

        public void SetActions(List<WarlockAction> action)
        {
            Actions = action;
        }

        public void AddAction(WarlockAction action)
        {
            Actions.Add(action);
        }

        public void ClearActions()
        {
            Actions.Clear();
        }

        public void ClearConditional()
        {
            conditional = null;
        }

        public void Smallify()
        {
            Conditional.Smallify();
            Actions.TrimExcess();
        }


        /// <summary>
        /// Checks the conditionals and executes the actions if the conditionals evaluate to true. Returns the conditional result
        /// </summary>
        /// <returns>the conditional result</returns>
        public bool Execute(Grimoire grimoire)
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
                if (conditional == null || (res = conditional.Evaluate(grimoire)))
                {
                    for (int j = 0; j < Actions.Count; j++)
                    {
                        Actions[j].DoAction(grimoire);
                    }
                }
            }

            return res;
        }
    }
}
