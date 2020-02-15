using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using RTCV.CorruptCore.Coroutines;

namespace RTCV.CorruptCore.EventWarlock
{
    /// <summary>
    /// I'm a wizard?? If you rename this you can't see the magic
    /// </summary>
    public static class Warlock
    {
        /// <summary>
        /// Holds the execution lists and can be serialized
        /// </summary>
        public static Grimoire BookOfSpells = new Grimoire();
        /// <summary>
        /// Last result, used for else logic
        /// </summary>
        public static bool LastResult { get; private set; } = false;

        [NonSerialized]
        public static HashSet<string> StaticFlags = new HashSet<string>();
        [NonSerialized]
        public static Dictionary<string, object> StaticVariables = new Dictionary<string, object>();

        public static void ResetVariables()
        {
            StaticFlags.Clear();
            StaticVariables.Clear();
        }


        private static void ClearLists()
        {
            if (BookOfSpells != null)
            {
                BookOfSpells.LoadSpells.Clear();
                BookOfSpells.PreExecuteSpells.Clear();
                BookOfSpells.ExecuteSpells.Clear();
                BookOfSpells.PostExecuteSpells.Clear();
            }
        }


        public static void Reset()
        {
            ClearLists();
            ResetVariables();
            CoroutineEngine.Reset();
        }

        public static void LoadGrimoire(Grimoire grimoire)
        {
            Reset();
            BookOfSpells = grimoire;
        }


        //Can probably do these by just accessing the BookOfSpells
        public static void AddLoadSpell(Spell spell)
        {
            BookOfSpells.LoadSpells.Add(spell);
        }

        public static void AddPreExecuteSpell(Spell spell)
        {
            BookOfSpells.PreExecuteSpells.Add(spell);
        }

        public static void AddExecuteSpell(Spell spell)
        {
            BookOfSpells.ExecuteSpells.Add(spell);
        }

        public static void AddPostExecuteSpell(Spell spell)
        {
            BookOfSpells.PostExecuteSpells.Add(spell);
        }

        private static void ExecuteList(List<Spell> list)
        {
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j].Enabled)
                {
                    LastResult = list[j].Execute();
                }
            }
        }

        public static void Load()
        {
            ExecuteList(BookOfSpells.LoadSpells);
        }

        public static void PreExecute()
        {
            CoroutineEngine.PreExecute.Update();
            ExecuteList(BookOfSpells.PreExecuteSpells);
        }

        public static void Execute()
        {
            CoroutineEngine.Execute.Update();
            ExecuteList(BookOfSpells.ExecuteSpells);
        }

        public static void PostExecute()
        {
            CoroutineEngine.PostExecute.Update();
            ExecuteList(BookOfSpells.PostExecuteSpells);
        }
    }
}
