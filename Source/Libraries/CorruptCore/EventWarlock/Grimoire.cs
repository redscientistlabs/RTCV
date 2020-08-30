namespace RTCV.CorruptCore.EventWarlock
{
    using System;
    using System.Collections.Generic;

    [System.Serializable]
    public class Grimoire
    {
        public string Name = null;

        public List<Spell> LoadSpells = new List<Spell>();
        public List<Spell> PreExecuteSpells = new List<Spell>();
        public List<Spell> ExecuteSpells = new List<Spell>();
        public List<Spell> PostExecuteSpells = new List<Spell>();


        public BlastLayer Layer = null;
        //Todo: add other wanted data here

        //Static variables
        [NonSerialized]
        public static HashSet<string> StaticFlags = new HashSet<string>();
        [NonSerialized]
        public static Dictionary<string, object> StaticVariables = new Dictionary<string, object>();

        //Non static variables
        [NonSerialized]
        public HashSet<string> Flags = new HashSet<string>();
        [NonSerialized]
        public Dictionary<string, object> Variables = new Dictionary<string, object>();


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
