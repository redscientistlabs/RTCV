namespace RTCV.CorruptCore.EventWarlock
{
    using System.Collections.Generic;
    using System.Linq;
    using RTCV.CorruptCore.Coroutines;

    /// <summary>
    /// I'm a wizard?? If you rename this you can't see the magic
    /// </summary>
    public static class Warlock
    {
        /// <summary>
        /// The list of grimoires
        /// </summary>
        private static List<Grimoire> Grimoires = new List<Grimoire>();

        /// <summary>
        /// Last result(conditionals), used for else logic within Spells. All elses will be after another spell
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
            if (grimoire.Name.Length != 0 && Grimoires.Any(x => x.Name == grimoire.Name))
            {
                //Grimoire name already taken
                return false;
            }
            else
            {
                //Unnamed grimoire or name is not taken
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

        public static List<Grimoire> GetGrimoiresForSaving()
        {
            Smallify();
            return Grimoires;
        }

        /// <summary>
        /// Makes the lists as small as possible. Only really saves a few bytes but hey
        /// </summary>
        public static void Smallify()
        {
            Grimoires.TrimExcess();
            for (int j = 0; j < Grimoires.Count; j++)
            {
                Grimoires[j].Smallify();
            }
        }

        //todo: rename
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
