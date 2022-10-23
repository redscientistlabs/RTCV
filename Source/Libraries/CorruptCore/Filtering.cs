namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using RTCV.Common;
    using RTCV.CorruptCore.Extensions;
    using RTCV.NetCore;

    public static class Filtering
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static ConcurrentDictionary<string, IListFilter> Hash2LimiterDico
        {
            get => (ConcurrentDictionary<string, IListFilter>)AllSpec.CorruptCoreSpec[RTCSPEC.FILTERING_HASH2LIMITERDICO];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.FILTERING_HASH2VALUEDICO, value);
        }

        public static ConcurrentDictionary<string, IListFilter> Hash2ValueDico
        {
            get => (ConcurrentDictionary<string, IListFilter>)AllSpec.CorruptCoreSpec[RTCSPEC.FILTERING_HASH2VALUEDICO];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.FILTERING_HASH2VALUEDICO, value);
        }

        public static ConcurrentDictionary<string, string> Hash2NameDico
        {
            get => (ConcurrentDictionary<string, string>)AllSpec.CorruptCoreSpec[RTCSPEC.FILTERING_HASH2NAMEDICO];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.FILTERING_HASH2NAMEDICO, value);
        }

        internal static int StockpileListCount = 0;

        public static PartialSpec getDefaultPartial()
        {
            var partial = new PartialSpec("RTCSpec");
            partial[RTCSPEC.FILTERING_HASH2LIMITERDICO] = new ConcurrentDictionary<string, IListFilter>();
            partial[RTCSPEC.FILTERING_HASH2VALUEDICO] = new ConcurrentDictionary<string, IListFilter>();
            partial[RTCSPEC.FILTERING_HASH2NAMEDICO] = new ConcurrentDictionary<string, string>();

            return partial;
        }

        internal static void LoadStockpileLists(Stockpile sks)
        {
            var lists = LoadListsFromPaths(Directory.GetFiles(Path.Combine(RtcCore.workingDir, "SKS"), "*.limiter"));

            Dictionary<string, string> allKnownLists = new Dictionary<string, string>();

            //Get the knownlists and add dummy entries for them
            foreach (var sk in sks.StashKeys)
            {
                foreach (var t in sk.KnownLists)
                {
                    allKnownLists[t.Key] = t.Value;
                }
            }

            //We don't have names for these lists so just add them with a generic name if they're not in the knownlists
            foreach (var list in lists)
            {
                //We have the name so use it
                if (allKnownLists.ContainsKey(list))
                {
                    RegisterListInUI(allKnownLists[list], list);
                }
                //Throw in a generic name
                else
                {
                    RegisterListInUI("SK_" + StockpileListCount, list);
                    StockpileListCount++;
                }
            }
            //Now add the dummy lists. Since a list doesn't register if the hash already exists, this works
            foreach (var key in allKnownLists.Keys)
            {
                RegisterListInUI("MISSING_" + allKnownLists[key], key);
            }
        }

        /// <summary>
        /// Loads lists and registers them as both limiter and value lists, then returns the hashes
        /// </summary>
        /// <param name="paths"></param>
        /// <returns>Hashes of the lists</returns>
        public static List<string> LoadListsFromPaths(string[] paths)
        {
            ConcurrentDictionary<string, string> h = new ConcurrentDictionary<string, string>();

            Parallel.ForEach(paths, (path) =>
            {
                //Load the lists and add their hashes to the returns
                var hash = loadListFromPath(path, false);
                var name = Path.GetFileNameWithoutExtension(path);
                if (!string.IsNullOrEmpty(hash))
                {
                    h[hash] = name;
                }
            });

            //We do this because we're adding to the lists not replacing them. It's a bit odd but it's needed for the spec system
            PartialSpec update = new PartialSpec("RTCSpec");
            update[RTCSPEC.FILTERING_HASH2LIMITERDICO] = Hash2LimiterDico;
            update[RTCSPEC.FILTERING_HASH2VALUEDICO] = Hash2ValueDico;
            AllSpec.CorruptCoreSpec.Update(update);

            return h.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value).Keys.ToList(); //Return the hashes ordered by their name
        }

        /// <summary>
        /// Loads a list from a path and registers it as a value and limiter list
        /// </summary>
        /// <param name="path"></param>
        /// <param name="syncListViaNetcore"></param>
        /// <returns>The hash of the list</returns>
        private static string loadListFromPath(string path, bool syncListViaNetcore)
        {
            //Load the list in
            string[] temp = File.ReadAllLines(path);
            if (temp.Length == 0)
            {
                return "";
            }

            //If the file is prefixed with '_', we assume it's stored as big endian and flip the bytes
            bool flipBytes = Path.GetFileName(path).StartsWith("_");

            IListFilter list;

            if (temp[0][0] == '@')
            {
                string typeRequested = temp[0].Substring(1).Trim();
                list = (IListFilter)S.BLINDMAKE(typeRequested);

                var temp2 = new string[temp.Length - 1];
                Array.Copy(temp, 1, temp2, 0, temp2.Length);
                temp = temp2;
            }
            else
            {
                //detect what kind of list it is
                if (temp.FirstOrDefault(it => it.Contains("?")) != null) //has wildcards, needs nullable array
                {
                    list = new NullableByteArrayList();
                }
                else //standard list, use value arrays
                {
                    list = new ValueByteArrayList();
                }
            }

            try
            {
                return list.Initialize(path, temp, flipBytes, syncListViaNetcore);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return "";
            }
        }

        /// <summary>
        /// Registers a list as a limiter and value list in the dictionaries
        /// </summary>
        /// <param name="list"></param>
        /// <param name="name"></param>
        /// <param name="syncListsViaNetcore"></param>
        /// <returns>The hash of the list being registereds</returns>
        public static string RegisterList(IListFilter list, string name, bool syncListsViaNetcore)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            string hashStr = list.GetHash(); //get it from object

            //Assuming the key doesn't already exist (we assume collions won't happen), add it.
            if (!Hash2ValueDico?.ContainsKey(hashStr) ?? false)
            {
                Hash2ValueDico[hashStr] = list;
            }

            if (!Hash2LimiterDico?.ContainsKey(hashStr) ?? false)
            {
                Hash2LimiterDico[hashStr] = list;
            }

            if (!Hash2NameDico?.ContainsKey(name) ?? false)
            {
                Hash2NameDico[hashStr] = name;
            }

            //Push it over netcore if we need to
            if (syncListsViaNetcore)
            {
                PartialSpec update = new PartialSpec("RTCSpec");
                update[RTCSPEC.FILTERING_HASH2LIMITERDICO] = Hash2LimiterDico;
                update[RTCSPEC.FILTERING_HASH2VALUEDICO] = Hash2ValueDico;
                update[RTCSPEC.FILTERING_HASH2NAMEDICO] = Hash2NameDico;
                AllSpec.CorruptCoreSpec.Update(update);
            }

            return hashStr;
        }

        public static bool LimiterPeekBytes(long startAddress, long endAddress, string hash, MemoryInterface mi)
        {
            if (mi == null)
            {
                throw new ArgumentNullException(nameof(mi));
            }

            //If we go outside of the domain, just return false
            if (endAddress > mi.Size)
            {
                return false;
            }

            //Find the precision
            long precision = endAddress - startAddress;
            byte[] values = new byte[precision];

            //Peek the memory
            for (long i = 0; i < precision; i++)
            {
                values[i] = mi.PeekByte(startAddress + i);
            }

            //The compare is done as little endian
            if (mi.BigEndian)
            {
                values = values.FlipBytes();
            }

            //If the limiter contains the value we peeked, return true
            if (LimiterContainsValue(values, hash))
            {
                return true;
            }

            return false;
        }

        public static byte[] LimiterPeekAndGetBytes(long startAddress, long endAddress, string hash, MemoryInterface mi)
        {
            if (mi == null)
            {
                throw new ArgumentNullException(nameof(mi));
            }

            //If we go outside of the domain, just return false
            if (endAddress > mi.Size)
            {
                return null;
            }

            //Find the precision
            long precision = endAddress - startAddress;
            //byte[] values = new byte[precision];

            //Peek the memory
            byte[] values = new byte[precision];
            for (long i = 0; i < precision; i++)
            {
                values[i] = mi.PeekByte(startAddress + i);
            }

            //The compare is done as little endian
            if (mi.BigEndian)
            {
                values = values.FlipBytes();
            }

            //If the limiter contains the value we peeked, return true
            if (LimiterContainsValue(values, hash))
            {
                return values;
            }

            return null;
        }

        /// <summary>
        /// Returns true if a limiter list contains the sequence of bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool LimiterContainsValue(byte[] bytes, string hash)
        {
            //Specifically log if any of these are null
            if (Hash2LimiterDico == null)
            {
                logger.Error("Hash2LimiterDico null");
                return false;
            }
            if (bytes == null)
            {
                logger.Error("Bytes null");
                return false;
            }
            if (hash == null)
            {
                logger.Error("Hash null");
                return false;
            }

            //If the limiter dictionary contains the hash, check if the hashset contains the byte sequence
            if (Hash2LimiterDico.TryGetValue(hash, out IListFilter list))
            {
                return list.ContainsValue(bytes);
            }

            return false;
        }

        internal static bool NullableByteArrayContains(HashSet<byte?[]> hs, byte[] bytes)
        {
            //checks nullable bytes lists against other byte lists, ignoring null collisions from both sides.

            foreach (var item in hs.ToArray())
            {
                bool found = true;

                for (int i = 0; i < item.Length; i++)
                {
                    if (item[i] == null) //ignoring wildcards (null values)
                    {
                        continue;
                    }

                    if (item[i].Value != bytes[i])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Gets precision used in a list
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static int GetPrecisionFromHash(string hash)
        {
            if (Hash2ValueDico == null)
            {
                return -1;
            }

            if (!Hash2ValueDico.ContainsKey(hash))
            {
                return -1;
            }

            return Hash2ValueDico[hash].GetPrecision();
        }

        /// <summary>
        /// Gets a random constant from a value list
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static byte[] GetRandomConstant(string hash, int precision, byte[] passthrough = null)
        {
            //If the value dico doesn't exist, return false
            if (Hash2ValueDico == null)
            {
                return null;
            }

            //If the dico doesn't contain the list, return null
            if (!Hash2ValueDico.ContainsKey(hash))
            {
                return null;
            }

            var list = Hash2ValueDico[hash];

            return list.GetRandomValue(hash, precision, passthrough);
        }

        /// <summary>
        /// Returns all the limiter lists from a stockpile as a list of string arrays (one value per line)
        /// </summary>
        /// <param name="sks"></param>
        /// <returns></returns>
        internal static Dictionary<string, List<string>> GetAllLimiterListsFromStockpile(Stockpile sks)
        {
            sks.MissingLimiter = false;

            var lists = new Dictionary<string, List<string>>();
            var hashList = new List<string>();

            //Build up a list of all the lists used by every blastunit
            foreach (StashKey sk in sks.StashKeys)
            {
                sk.PopulateKnownLists();
                foreach (BlastUnit bu in sk.BlastLayer.Layer)
                {
                    if (!hashList.Contains(bu.LimiterListHash))
                    {
                        hashList.Add(bu.LimiterListHash);
                    }
                }
            }

            //Iterate through our list of lists
            foreach (var s in hashList)
            {
                //If we have a value and the dictionary contains it, build up a String[] containing the values
                if (s != null && Hash2LimiterDico.ContainsKey(s))
                {
                    List<string> strList = Hash2LimiterDico[s].GetStringList();

                    Hash2NameDico.TryGetValue(s, out string name); //See if we can get the name
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        name = "UNKNOWN_" + s.Substring(0, 5); // Default name will use the first 5 chars of the hash
                    }

                    lists[(name.StartsWith("STOCKPILE_") ? "" : "STOCKPILE_") + name] = strList;
                }
                //If we have a value but the dictionary didn't have it, pop that we couldn't find the list
                else if (s != null)
                {
                    var name = "";
                    name = Hash2NameDico.ContainsKey(s) ? Hash2NameDico[s] : "UNKNOWN LIST OF HASH: " + s;

                    DialogResult dr = MessageBox.Show("Couldn't find Limiter List " + name +
                        " If you continue saving, any blastunit using this list will ignore the limiter on playback if the list still cannot be found.\nDo you want to continue?", "Couldn't Find Limiter List",
                        MessageBoxButtons.YesNo);

                    if (dr == DialogResult.No)
                    {
                        return null;
                    }

                    sks.MissingLimiter = true;
                }
            }

            return lists;
        }

        public static void ResetLoadedListsInUI()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                RtcCore.LimiterListBindingSource.Clear();
                RtcCore.ValueListBindingSource.Clear();
            });
        }

        public static bool RegisterListInUI(string name, string hash)
        {
            //Don't double-register the same name. For now, just iterate over the limiter lists and pull the names out.
            //In the future, we'll keep a proper dictionary when this code is re-written
            if (RtcCore.LimiterListBindingSource.Any(x => x.Value == hash))
            {
                return false;
            }

            if (RtcCore.LimiterListBindingSource.Any(x => x.Name == name))
            {
                name = name + "_1";
            }

            SyncObjectSingleton.FormExecute(() =>
            {
                RtcCore.LimiterListBindingSource.Add(new ComboBoxItem<string>(name, hash));
                RtcCore.ValueListBindingSource.Add((new ComboBoxItem<string>(name, hash)));
            });
            return true;
        }
    }
}
