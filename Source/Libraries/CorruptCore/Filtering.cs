using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
    public static class Filtering
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static ConcurrentDictionary<string, HashSet<byte?[]>> Hash2LimiterDico
        {
            get => (ConcurrentDictionary<string, HashSet<byte?[]>>)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.FILTERING_HASH2LIMITERDICO];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.FILTERING_HASH2VALUEDICO, value);
        }

        public static ConcurrentDictionary<string, List<byte?[]>> Hash2ValueDico
        {
            get => (ConcurrentDictionary<string, List<byte?[]>>)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.FILTERING_HASH2VALUEDICO];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.FILTERING_HASH2VALUEDICO, value);
        }

        public static ConcurrentDictionary<string, string> Hash2NameDico
        {
            get => (ConcurrentDictionary<string, string>)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.FILTERING_HASH2NAMEDICO];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.FILTERING_HASH2NAMEDICO, value);
        }

        public static int StockpileListCount = 0;

        public static PartialSpec getDefaultPartial()
        {
            var partial = new PartialSpec("RTCSpec");
            partial[RTCSPEC.FILTERING_HASH2LIMITERDICO] = new ConcurrentDictionary<string, HashSet<byte?[]>>();
            partial[RTCSPEC.FILTERING_HASH2VALUEDICO] = new ConcurrentDictionary<string, List<byte?[]>>();
            partial[RTCSPEC.FILTERING_HASH2NAMEDICO] = new ConcurrentDictionary<string, string>();

            return partial;
        }

        public static void LoadStockpileLists(Stockpile sks)
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
            RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(update);

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

            List<byte?[]> byteList = new List<byte?[]>();
            //For every line in the list, build up our list of bytes
            for (int i = 0; i < temp.Length; i++)
            {
                string t = temp[i];
                byte?[] bytes = null;
                try
                {
                    //Get the string as a byte array
                    if ((bytes = CorruptCore_Extensions.StringToByteArray(t)) == null)
                    {
                        throw new Exception($"Error reading list {Path.GetFileName(path)}. Valid format is a list of raw hexadecimal values.\nLine{(i + 1)}.\nValue: {t}\n");
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e, "Error in loadListFromPath");
                    MessageBox.Show(e.Message);
                    return "";
                }

                //If it's big endian, flip it
                if (flipBytes)
                {
                    bytes.FlipBytes();
                }

                byteList.Add(bytes);
            }

            var name = Path.GetFileNameWithoutExtension(path);

            var hash = RegisterList(byteList, name, syncListViaNetcore);
            //var hash = RegisterList(byteList.Distinct(new CorruptCore_Extensions.ByteArrayComparer()).ToList(), name, syncListViaNetcore);
            
            return hash;
        }

        /// <summary>
        /// Registers a list as a limiter and value list in the dictionaries
        /// </summary>
        /// <param name="list"></param>
        /// <param name="syncListsViaNetcore"></param>
        /// <returns>The hash of the list being registereds</returns>
        public static string RegisterList(List<byte?[]> list, string name, bool syncListsViaNetcore)
        {
            List<byte?> bList = new List<byte?>();
            foreach (byte?[] line in list)
            {
                bList.AddRange(line);
            }

            //Hash it. We don't use GetHashCode because we want something consistent to hash to use as a key
            MD5 hash = MD5.Create();
            hash.ComputeHash(bList.ToArray().Flatten69());
            string hashStr = Convert.ToBase64String(hash.Hash);

            //Assuming the key doesn't already exist (we assume collions won't happen), add it.
            if (!Hash2ValueDico?.ContainsKey(hashStr) ?? false)
            {
                Hash2ValueDico[hashStr] = list;
            }

            if (!Hash2LimiterDico?.ContainsKey(hashStr) ?? false)
            {
                Hash2LimiterDico[hashStr] = new HashSet<byte?[]>(list, new CorruptCore_Extensions.ByteArrayComparer());
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
                RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(update);
            }

            return hashStr;
        }

        public static bool LimiterPeekBytes(long startAddress, long endAddress, string domain, string hash, MemoryInterface mi)
        {
            //If we go outside of the domain, just return false
            if (endAddress > mi.Size)
            {
                return false;
            }

            //Find the precision
            long precision = endAddress - startAddress;
            byte?[] values = new byte?[precision];

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

        /// <summary>
        /// Returns true if a limiter list contains the sequence of bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool LimiterContainsValue(byte?[] bytes, string hash)
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
            if (Hash2LimiterDico.TryGetValue(hash, out HashSet<byte?[]> hs))
            {
                return NullableByteArrayContains(hs,bytes);
            }

            return false;
        }

        public static bool NullableByteArrayContains(HashSet<byte?[]> hs, byte?[] bytes)
        {
            //checks nullable bytes lists against other byte lists, ignoring null collisions from both sides.

            foreach(var item in hs.ToArray())
            {
                bool found = true;

                for(int i = 0; i<item.Length;i++)
                {
                    if (item[i] == null || bytes[i] == null) //ignoring wildcards (null values)
                        continue;

                    if(item[i].Value != bytes[i].Value)
                    {
                        found = false;
                        break;
                    }
                }

                if(found)
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

            byte?[] value = Hash2ValueDico[hash][0];
            return value.Length;
        }

        /// <summary>
        /// Gets a random constant from a value list
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static byte[] GetRandomConstant(string hash, int precision)
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

            //Get a random line in the list and grab the value
            int line = RtcCore.RND.Next(Hash2ValueDico[hash].Count);
            byte?[] value = Hash2ValueDico[hash][line];

            byte[] outValue = new byte[value.Length];

            for (int i=0;i<value.Length;i++)
            {
                if (value[i] == null)
                    outValue[i] = (byte)RtcCore.RND.Next(255); //filling wildcards with random(255)
                else
                    outValue[i] = value[i].Value;
            }

            //Copy the value to a working array

            //Array.Copy(value, outValue, value.Length);

            //If the list is shorter than the current precision, left pad it
            if (outValue.Length < precision)
            {
                outValue = outValue.PadLeft(precision);
            }
            //If the list is longer than the current precision, truncate it. Lists are stored little endian so truncate from the right
            else if (outValue.Length > precision)
            {
                //It'd probably be faster to do this via bitshifting but it's 4am and I want to be able to read this code in the future so...
                outValue.FlipBytes(); //Flip the bytes (stored as little endian)
                Array.Resize(ref outValue, precision); //Truncate
                outValue.FlipBytes(); //Flip them back
            }
            return outValue;
        }

        /// <summary>
        /// Returns all the limiter lists from a stockpile as a list of string arrays (one value per line)
        /// </summary>
        /// <param name="sks"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetAllLimiterListsFromStockpile(Stockpile sks)
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
                    List<String> strList = new List<string>();
                    foreach (var line in Hash2LimiterDico[s])
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var b in line)
                        {
                            //If the string isn't of an even length, pad it
                            string tmp = b?.ToString("X");

                            if(tmp == null)
                            {
                                tmp = "**";
                            }
                            else if (tmp.Length % 2 != 0)
                            {
                                tmp = "0" + tmp;
                            }

                            sb.Append(tmp);
                        }
                        strList.Add(sb.ToString());
                    }
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
