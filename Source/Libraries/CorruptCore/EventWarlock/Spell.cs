using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock
{
    /// <summary>
    /// Holds conditionals and actions to be executed if conditionals evaluates to true
    /// </summary>
    [System.Serializable]
    public class Spell
    {
        //encapsulation will need to be figured out later

        public bool Enabled = true;
        public string Name;
        //private EWConditional conditional = null;
        //public EWConditional Conditional { get => conditional; }
        public List<WarlockAction> Actions = new List<WarlockAction>();
        public bool IsElse = false;

        public List<List<EWConditional>> ConditionalGroups { get; private set; } = new List<List<EWConditional>>();
        //public List<List<EWConditional>> ConditionalGroups => conditionalGroups;
        private int maxExecutions = -1;
        private int executionsLeft = -1;
        public int MaxExecutions { get { return maxExecutions; } set { maxExecutions = value; executionsLeft = value; } }
        public int ExecutionsLeft => executionsLeft;
        public Spell(string name = "Unnamed")
        {
            this.Name = name;
        }

        public void SetEnabled(bool enabled = true)
        {
            this.Enabled = enabled;
        }

        /// <summary>
        /// Sets the conditional to a single conditional, overrides any others. 
        /// </summary>
        /// <param name="conditional"></param>
        /// <param name="isElse"></param>
        public void SetConditional(EWConditional conditional, bool isElse = false)
        {
            this.IsElse = isElse;
            this.ConditionalGroups.Clear();
            this.ConditionalGroups.Add(new List<EWConditional>() { conditional });
        }

        public void AddConditionalGroup(List<EWConditional> conditionals)
        {
            ConditionalGroups.Add(conditionals);
        }

        public void AddNewGroup()
        {
            ConditionalGroups.Add(new List<EWConditional>());
        }

        public void AddConditionalToLastGroup(EWConditional conditional)
        {
            if (ConditionalGroups.Count == 0)
            {
                ConditionalGroups.Add(new List<EWConditional>() { conditional });
            }
            else
            {
                ConditionalGroups[ConditionalGroups.Count - 1].Add(conditional);
            }
        }

        public void AddConditionalToGroup(EWConditional conditional, int index)
        {
            ConditionalGroups[index].Add(conditional);
        }

        public void RemoveConditional(EWConditional conditional)
        {
            for (int j = 0; j < ConditionalGroups.Count; j++)
            {
                if (ConditionalGroups[j].Remove(conditional)) break;
            }
            CleanGroups();
        }

        public void CleanGroups()
        {
            var groupsToClean = new List<List<EWConditional>>();
            for (int j = 0; j < ConditionalGroups.Count; j++)
            {
                if (ConditionalGroups[j].Count == 0)
                {
                    groupsToClean.Add(ConditionalGroups[j]);
                }
            }

            for (int j = 0; j < groupsToClean.Count; j++)
            {
                ConditionalGroups.Remove(groupsToClean[j]);
            }
        }


        public void SetActions(List<WarlockAction> action)
        {
            Actions = action;
        }

        public void AddAction(WarlockAction action)
        {
            Actions.Add(action);
        }

        public void RemoveAction(WarlockAction action)
        {
            Actions.Remove(action);
        }

        public void ClearActions()
        {
            Actions.Clear();
        }

        public void ClearConditionals()
        {
            ConditionalGroups.Clear();
        }

        public void Smallify()
        {
            Actions.TrimExcess();
        }


        /// <summary>
        /// Checks the conditionals and executes the actions if the conditionals evaluate to true. Returns the conditional result
        /// </summary>
        /// <returns>the conditional result</returns>
        public bool Execute(Grimoire grimoire)
        {
            bool elseCheck = true;

            //Local function to run actions
            void DoLogic()
            {
                if(maxExecutions > -1 && executionsLeft != 0)
                {
                    executionsLeft--;
                }

                for (int j = 0; j < Actions.Count; j++)
                {
                    Actions[j].DoAction(grimoire);
                }
            }

            //Else logic
            if (IsElse && Warlock.LastResult)
            {
                //return value will be true, prevents other elses from executing
                return true;
            }

            if(maxExecutions > -1)
            {
                if(executionsLeft == 0)
                {
                    //elses after will run
                    return false;
                }
            }


            if (elseCheck)
            {
                //If no conditionals just do the logic
                if (ConditionalGroups.Count == 0)
                {
                    DoLogic();
                    return true;
                }
                else
                {

                    bool curRes = true;
                    for (int j = 0; j < ConditionalGroups.Count; j++)
                    {
                        curRes = true;
                        //AND logic per group
                        for (int k = 0; k < ConditionalGroups[j].Count; k++)
                        {
                            if (!ConditionalGroups[j][k].Evaluate(grimoire))
                            {
                                curRes = false;
                                //Don't check other conditionals
                                break;
                            }
                        }

                        //OR logic, if any single group is all true, execute
                        if (curRes == true)
                        {
                            DoLogic();
                            return true;
                        }
                    }
                }
            }
            //satisfy compiler / return false if all groups fail
            return false;
        }
    }
}
