using RTCV.CorruptCore.EventWarlock;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ceras;

namespace RTCV.CorruptCore.EventWarlock
{
    [System.Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class Grimoire
    {
        public string Name = null;

        public IEnumerable<Spell> AllSpells => LoadSpells.Concat(PreExecuteSpells).Concat(PreExecuteSpells).Concat(ExecuteSpells).Concat(PostExecuteSpells);
        public List<Spell> LoadSpells = new List<Spell>();
        public List<Spell> PreExecuteSpells = new List<Spell>();
        public List<Spell> ExecuteSpells = new List<Spell>();
        public List<Spell> PostExecuteSpells = new List<Spell>();


        public BlastLayer Layer = null;
        //Todo: add other wanted data here

        //Static variables
        [NonSerialized, Ceras.Exclude]
        public static HashSet<string> StaticFlags = new HashSet<string>();
        [NonSerialized, Ceras.Exclude]
        public static Dictionary<string, object> StaticVariables = new Dictionary<string, object>();

        //Non static variables
        [NonSerialized, Ceras.Exclude]
        public HashSet<string> Flags = new HashSet<string>();
        [NonSerialized, Ceras.Exclude]
        public Dictionary<string, object> Variables = new Dictionary<string, object>();


        /// <summary>
        /// Parameterless consturctor for serialization
        /// </summary>
        public Grimoire()
        {
            this.Name = RtcCore.GetRandomKey();
        }
        public Grimoire(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Resets all static variables and flags
        /// </summary>
        public static void ResetStaticVariables()
        {
            StaticFlags.Clear();
            StaticVariables.Clear();
        }

        //Likely not needed
        public void ResetVariables()
        {
            Flags.Clear();
            Variables.Clear();
        }


        public void Smallify()
        {
            //local functions yay
            void SmallifyList(List<Spell> spellList)
            {
                spellList.TrimExcess();
                for (int j = 0; j < spellList.Count; j++)
                {
                    spellList[j].Smallify();
                }
            }

            SmallifyList(LoadSpells);
            SmallifyList(PreExecuteSpells);
            SmallifyList(ExecuteSpells);
            SmallifyList(PostExecuteSpells);
        }

    }
}
