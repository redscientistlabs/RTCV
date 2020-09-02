namespace RTCV.CorruptCore.EventWarlock
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Grimoire
    {
        public string Name { get; private set; } = null;

        public List<Spell> LoadSpells { get; private set; } = new List<Spell>();
        public List<Spell> PreExecuteSpells { get; private set; } = new List<Spell>();
        public List<Spell> ExecuteSpells { get; private set; } = new List<Spell>();
        public List<Spell> PostExecuteSpells { get; private set; } = new List<Spell>();


        // public BlastLayer Layer = null;
        //Todo: add other wanted data here

        //Static variables
        [Ceras.Exclude]
        public static HashSet<string> StaticFlags = new HashSet<string>();
        [Ceras.Exclude]
        public static Dictionary<string, object> StaticVariables = new Dictionary<string, object>();

        //Non static variables
        // [NonSerialized]
        // public HashSet<string> Flags = new HashSet<string>();
        // [NonSerialized]
        // public Dictionary<string, object> Variables = new Dictionary<string, object>();


        public Grimoire(string name = "") {
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
        public static void ResetVariables()
        {
            // Flags.Clear();
            // Variables.Clear();
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
