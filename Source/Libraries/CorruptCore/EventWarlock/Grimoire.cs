using RTCV.CorruptCore.EventWarlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock
{
    [System.Serializable]
    public class Grimoire
    {
        public string Name = null;

        public List<Spell> LoadSpells = new List<Spell>();
        public List<Spell> PreExecuteSpells = new List<Spell>();
        public List<Spell> ExecuteSpells = new List<Spell>();
        public List<Spell> PostExecuteSpells = new List<Spell>();


        public BlastLayer Layer = null;
        //Add other data here


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


    }
}
