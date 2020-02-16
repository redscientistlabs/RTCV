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

        private static List<Grimoire> Grimoires = new List<Grimoire>();

        /// <summary>
        /// Last result, used for else logic
        /// </summary>
        public static bool LastResult { get; private set; } = false;


        public static Grimoire GetByName(string name)
        {
            return Grimoires.FirstOrDefault(x => x.Name == name);
        }

        /// <summary>
        /// Add new grimoire
        /// </summary>
        /// <param name="grimoire"></param>
        /// <returns></returns>
        public static bool AddGrimoire(Grimoire grimoire)
        {
            if(grimoire.Name != null && Grimoires.Any(x => x.Name == grimoire.Name))
            {
                return false;
            }
            else
            {
                Grimoires.Add(grimoire);
                return true;
            }
        }

        public static void RemoveGrimoire(Grimoire g)
        {
            Grimoires.Remove(g);
        }


        public static void Reset()
        {
            Grimoires.Clear();
            Grimoire.ResetStaticVariables();
            CoroutineEngine.Reset();
            LastResult = false;
        }

        public static void LoadGrimoires(List<Grimoire> grimoires)
        {
            Reset();
            Grimoires = grimoires;
        }


        //Can probably do these by just accessing the BookOfSpells

        private static void ExecuteList(Grimoire grimoire, List<Spell> list)
        {
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j].Enabled)
                {
                    LastResult = list[j].Execute(grimoire);
                }
            }
        }

        public static void Load()
        {
            for (int j = 0; j < Grimoires.Count; j++)
            {
                ExecuteList(Grimoires[j], Grimoires[j].LoadSpells); 
            }
           
        }

        public static void PreExecute()
        {
            CoroutineEngine.PreExecute.Update();
            for (int j = 0; j < Grimoires.Count; j++)
            {
                ExecuteList(Grimoires[j], Grimoires[j].PreExecuteSpells);
            }
        }

        public static void Execute()
        {
            CoroutineEngine.Execute.Update();
            for (int j = 0; j < Grimoires.Count; j++)
            {
                ExecuteList(Grimoires[j], Grimoires[j].ExecuteSpells);
            }
        }

        public static void PostExecute()
        {
            CoroutineEngine.PostExecute.Update();
            for (int j = 0; j < Grimoires.Count; j++)
            {
                ExecuteList(Grimoires[j], Grimoires[j].PostExecuteSpells);
            }
        }
    }
}
