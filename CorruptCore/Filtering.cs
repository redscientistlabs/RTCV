using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Ceras;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
	public static class Filtering
	{
		public static Dictionary<string, HashSet<byte[]>> Hash2LimiterDico
		{
			get => (Dictionary<string, HashSet<byte[]>>)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.FILTERING_HASH2LIMITERDICO.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.FILTERING_HASH2VALUEDICO.ToString(), value);
		}
		public static Dictionary<string, List<Byte[]>> Hash2ValueDico
		{
			get => (Dictionary<string, List<Byte[]>>) RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.FILTERING_HASH2VALUEDICO.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.FILTERING_HASH2VALUEDICO.ToString(), value);
		}

		public static Dictionary<string, string> Hash2NameDico
		{
			get => (Dictionary<string, string>)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.FILTERING_HASH2NAMEDICO.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.FILTERING_HASH2NAMEDICO.ToString(), value);
		}

		public static int StockpileListCount = 0;

		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("RTCSpec");
			partial[RTCSPEC.FILTERING_HASH2LIMITERDICO.ToString()] = new Dictionary<string, HashSet<byte[]>> ();
			partial[RTCSPEC.FILTERING_HASH2VALUEDICO.ToString()] = new Dictionary<string, List<Byte[]>>();
			partial[RTCSPEC.FILTERING_HASH2NAMEDICO.ToString()] = new Dictionary<string, string>();

			return partial;
		}

		public static void LoadStockpileLists(Stockpile sks)
		{
			var lists = LoadListsFromPaths(Directory.GetFiles(CorruptCore.workingDir + Path.DirectorySeparatorChar + "SKS" + Path.DirectorySeparatorChar, "*.limiter"));

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
					RegisterListInUI(allKnownLists[list], list);
				//Throw in a generic name
				else
				{
					RegisterListInUI("SK_" + StockpileListCount, list);
					Hash2NameDico[list] = "SK_" + StockpileListCount;
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
			List<string> md5s = new List<string>();

			foreach (string path in paths)
			{
				//Load the lists and add their hashes to the returns
				md5s.Add(loadListFromPath(path, false));
			}

			//We do this because we're adding to the lists not replacing them. It's a bit odd but it's needed for the spec system
			PartialSpec update = new PartialSpec("RTCSpec");
			update[RTCSPEC.FILTERING_HASH2LIMITERDICO.ToString()] = Hash2LimiterDico;
			update[RTCSPEC.FILTERING_HASH2VALUEDICO.ToString()] = Hash2ValueDico;
			RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(update);

			return md5s;
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
			//If the file is prefixed with '_', we assume it's stored as big endian and flip the bytes
			bool flipBytes = Path.GetFileName(path).StartsWith("_");

			List<Byte[]> byteList = new List<byte[]>();
			//For every line in the list, build up our list of bytes
			for (int i = 0; i < temp.Length; i++)
			{
				string t = temp[i];
				byte[] bytes = null;
				try
				{
					//Get the string as a byte array
					bytes = CorruptCore_Extensions.StringToByteArray(t);
				}
				catch (Exception e)
				{
					MessageBox.Show("Error reading list " + Path.GetFileName(path) + ". Error at line " + (i+1) + ".\nValue: " + t + "\n. Valid format is a list of raw byte values.");
					break;
				}

				//If it's big endian, flip it
				if (flipBytes)
				{
					bytes.FlipBytes();
				}

				byteList.Add(bytes);
			}

			var hash =  RegisterList(byteList.Distinct(new CorruptCore_Extensions.ByteArrayComparer()).ToList(), syncListViaNetcore);
			var name = Path.GetFileName(path);
			Hash2NameDico[hash] = name;
			return hash;
		}

		/// <summary>
		/// Registers a list as a limiter and value list in the dictionaries
		/// </summary>
		/// <param name="list"></param>
		/// <param name="syncListsViaNetcore"></param>
		/// <returns>The hash of the list being registereds</returns>
		public static string RegisterList(List<Byte[]> list, bool syncListsViaNetcore)
		{
			//Make one giant string to hash
			string concat = String.Empty;
			foreach (byte[] line in list)
			{
				StringBuilder sb = new StringBuilder();
				foreach (var b in line)
					sb.Append(b.ToString());

				concat = String.Concat(concat, sb.ToString());
			}

			//Hash it. We don't use GetHashCode because we want something consistent to hash to use as a key
			MD5 hash = MD5.Create();
			hash.ComputeHash(concat.GetBytes());
			string hashStr = Convert.ToBase64String(hash.Hash);

			//Assuming the key doesn't already exist (we assume collions won't happen), add it.
			if (!Hash2ValueDico?.ContainsKey(hashStr) ?? false)
				Hash2ValueDico[hashStr] = list;
			if (!Hash2LimiterDico?.ContainsKey(hashStr) ?? false)
				Hash2LimiterDico[hashStr] = new HashSet<byte[]>(list, new CorruptCore_Extensions.ByteArrayComparer());

			//Push it over netcore if we need to
			if (syncListsViaNetcore)
			{
				PartialSpec update = new PartialSpec("RTCSpec");
				update[RTCSPEC.FILTERING_HASH2LIMITERDICO.ToString()] = Hash2LimiterDico;
				update[RTCSPEC.FILTERING_HASH2VALUEDICO.ToString()] = Hash2ValueDico;
				update[RTCSPEC.FILTERING_HASH2NAMEDICO.ToString()] = Hash2NameDico;
				RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(update);
			}

			return hashStr;
		}

		public static bool LimiterPeekBytes(long startAddress, long endAddress, string domain, string hash, MemoryInterface mi)
		{
			//If we go outside of the domain, just return false
			if (endAddress > mi.Size)
				return false;

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
				values = values.FlipBytes();

			//If the limiter contains the value we peeked, return true
			if (LimiterContainsValue(values, hash))
				return true;

			return false;
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
				Console.WriteLine("Hash2LimiterDico null");
				return false;
			}
			if (bytes == null)
			{
				Console.WriteLine("Bytes null");
				return false;
			}
			if (hash == null)
			{
				Console.WriteLine("Hash null");
				return false;
			}

			//If the limiter dictionary contains the hash, check if the hashset contains the byte sequence
			if (Hash2LimiterDico.TryGetValue(hash, out HashSet<byte[]> hs))
			{
				return hs.Contains(bytes);
			}

			return false;
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
				return null;

			//If the dico doesn't contain the list, return null
			if (!Hash2ValueDico.ContainsKey(hash))
			{
				return null;
			}

			//Get a random line in the list and grab the value
			int line = CorruptCore.RND.Next(Hash2ValueDico[hash].Count);
			Byte[] value = Hash2ValueDico[hash][line];

			//Copy the value to a working array
			Byte[] outValue = new byte[value.Length];
			Array.Copy(value, outValue, value.Length);

			//If the list is shorter than the current precision, left pad it
			if (outValue.Length < precision)
				outValue = outValue.PadLeft(precision);
			//If the list is longer than the current precision, truncate it. Lists are stored little endian so truncate from the right
			else if (outValue.Length > precision)
			{
				//It'd probably be faster to do this via bitshifting but it's 4am and I want to be able to read this code in the future so...

				outValue = value.FlipBytes(); //Flip the bytes (stored as little endian)
				Array.Resize(ref outValue, precision); //Truncate
				outValue = outValue.FlipBytes(); //Flip them back
				return outValue;
			}
			return outValue;
		}

		/// <summary>
		/// Returns all the limiter lists from a stockpile as a list of string arrays (one value per line)
		/// </summary>
		/// <param name="sks"></param>
		/// <returns></returns>
		public static List<String[]> GetAllLimiterListsFromStockpile(Stockpile sks)
		{
			sks.MissingLimiter = false;
			List<String> hashList = new List<string>();
			List<String[]> returnList = new List<String[]>();

			//Build up a list of all the lists used by every blastunit
			foreach (StashKey sk in sks.StashKeys)
			{
				sk.PopulateKnownLists();
				foreach (BlastUnit bu in sk.BlastLayer.Layer)
				{
					if (!hashList.Contains(bu.LimiterListHash))
						hashList.Add(bu.LimiterListHash);
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
							string tmp = b.ToString("X");
							if (tmp.Length % 2 != 0)
								tmp = "0" + tmp;
							sb.Append(tmp);
						}
						strList.Add(sb.ToString());
					}
					returnList.Add(strList.ToArray());
				}
				//If we have a value but the dictionary didn't have it, pop that we couldn't find the list
				else if(s != null)
				{
					var name = "";
					name = Hash2NameDico.ContainsKey(s) ? Hash2NameDico[s] : "UNKNOWN LIST OF HASH: " + s;

					DialogResult dr = MessageBox.Show("Couldn't find Limiter List " + name +
						" If you continue saving, any blastunit using this list will ignore the limiter on playback if the list still cannot be found.\nDo you want to continue?", "Couldn't Find Limiter List",
						MessageBoxButtons.YesNo);

					if (dr == DialogResult.No)
						return null;

					sks.MissingLimiter = true;
				}
			}

			return returnList;
		}
		public static bool RegisterListInUI(string name, string hash)
		{
			//Don't double-register the same name. For now, just iterate over the limiter lists and pull the names out.
			//In the future, we'll keep a proper dictionary when this code is re-written
			if (CorruptCore.LimiterListBindingSource.Any(x => x.Value == hash))
				return false;

			if (CorruptCore.LimiterListBindingSource.Any(x => x.Name == name))
				name = name + "_1";

			CorruptCore.LimiterListBindingSource.Add(new ComboBoxItem<String>(name, hash));
			CorruptCore.ValueListBindingSource.Add((new ComboBoxItem<String>(name, hash)));
			return true;
		}

	}
}