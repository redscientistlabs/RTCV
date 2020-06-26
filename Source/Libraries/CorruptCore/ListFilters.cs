namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows.Forms;
    using Ceras;

    public interface IListFilter
    {
        public string GetHash();
        public bool ContainsValue(byte[] bytes);
        public byte[] GetRandomValue(string hash, int precision, byte[] passthrough = null);
        public string Initialize(string filePath, string[] dataLines, bool flipBytes, bool syncListViaNetcore);

        public List<string> GetStringList();

        public int GetPrecision();
    }

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class ValueByteArrayList : IListFilter
    {
        List<byte[]> byteList { get; set; } = null;
        HashSet<byte[]> hashSet { get; set; } = null;

        public string Initialize(string filePath, string[] dataLines, bool flipBytes, bool syncListViaNetcore)
        {
            byteList = new List<byte[]>();

            //For every line in the list, build up our list of bytes
            for (int i = 0; i < dataLines.Length; i++)
            {
                string t = dataLines[i];
                byte[] bytes = null;
                try
                {
                    //Get the string as a byte array
                    if ((bytes = CorruptCore_Extensions.StringToByteArray(t)) == null)
                    {
                        throw new Exception($"Error reading list {Path.GetFileName(filePath)}. Valid format is a list of raw hexadecimal values.\nLine{(i + 1)}.\nValue: {t}\n");
                    }
                }
                catch (Exception e)
                {
                    var logger = NLog.LogManager.GetCurrentClassLogger();
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

            var name = Path.GetFileNameWithoutExtension(filePath);

            //var hash = Filtering.RegisterList(byteList, name, syncListViaNetcore);
            byteList = byteList.Distinct(new CorruptCore_Extensions.ByteArrayComparer()).ToList();

            hashSet = new HashSet<byte[]>(byteList, new CorruptCore_Extensions.ByteArrayComparer());
            string hash = Filtering.RegisterList(this, name, syncListViaNetcore);

            return hash;
        }
        public string GetHash()
        {
            List<byte> bList = new List<byte>();
            foreach (byte[] line in byteList)
            {
                bList.AddRange(line);
            }

            //Hash it. We don't use GetHashCode because we want something consistent to hash to use as a key
            MD5 hash = MD5.Create();
            hash.ComputeHash(bList.ToArray());
            string hashStr = Convert.ToBase64String(hash.Hash);
            return hashStr;
        }
        public bool ContainsValue(byte[] bytes)
        {
            return hashSet.Contains(bytes);
        }
        public byte[] GetRandomValue(string hash, int precision, byte[] passthrough)
        {
            //Get a random line in the list and grab the value
            int line = RtcCore.RND.Next(byteList.Count);
            byte[] value = byteList[line];

            byte[] outValue = new byte[value.Length];

            //for (int i = 0; i < value.Length; i++)
            //{
            //    if (value[i] == null)
            //        outValue[i] = (byte)RtcCore.RND.Next(255); //filling wildcards with random(255)
            //    else
            //        outValue[i] = value[i].Value;
            //}

            //Copy the value to a working array
            Array.Copy(value, outValue, value.Length);

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
        public int GetPrecision()
        {
            var value = byteList[0];
            return value.Length;
        }
        public List<string> GetStringList()
        {
            List<string> strList = new List<string>();

            foreach (var line in hashSet)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var b in line)
                {
                    //If the string isn't of an even length, pad it
                    string tmp = b.ToString("X");

                    if (tmp.Length % 2 != 0)
                    {
                        tmp = "0" + tmp;
                    }

                    sb.Append(tmp);
                }
                strList.Add(sb.ToString());
            }
            return strList;
        }
    }

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class NullableByteArrayList : IListFilter
    {
        List<byte?[]> byteList { get; set; } = null;
        HashSet<byte?[]> hashSet { get; set; } = null;

        public string Initialize(string filePath, string[] dataLines, bool flipBytes, bool syncListViaNetcore)
        {
            byteList = new List<byte?[]>();

            //For every line in the list, build up our list of bytes
            for (int i = 0; i < dataLines.Length; i++)
            {
                string t = dataLines[i];
                byte?[] bytes = null;
                try
                {
                    //Get the string as a byte array
                    if ((bytes = CorruptCore_Extensions.StringToNullableByteArray(t)) == null)
                    {
                        throw new Exception($"Error reading list {Path.GetFileName(filePath)}. Valid format is a list of raw hexadecimal values.\nLine{(i + 1)}.\nValue: {t}\n");
                    }
                }
                catch (Exception e)
                {
                    var logger = NLog.LogManager.GetCurrentClassLogger();
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

            var name = Path.GetFileNameWithoutExtension(filePath);

            string hash = GetHash();
            hashSet = new HashSet<byte?[]>(byteList, new CorruptCore_Extensions.NullableByteArrayComparer());

            Filtering.RegisterList(this, name, syncListViaNetcore);

            return hash;
        }
        public string GetHash()
        {
            List<byte?> bList = new List<byte?>();
            foreach (byte?[] line in byteList)
            {
                bList.AddRange(line);
            }

            //Hash it. We don't use GetHashCode because we want something consistent to hash to use as a key
            MD5 hash = MD5.Create();
            hash.ComputeHash(bList.ToArray().Flatten69());
            string hashStr = Convert.ToBase64String(hash.Hash);
            return hashStr;
        }
        public bool ContainsValue(byte[] bytes)
        {
            return Filtering.NullableByteArrayContains(hashSet, bytes);
        }
        public byte[] GetRandomValue(string hash, int precision, byte[] passthrough)
        {
            //Get a random line in the list and grab the value
            int line = RtcCore.RND.Next(byteList.Count);
            byte?[] value = byteList[line];

            byte[] outValue = new byte[value.Length];

            for (int i = 0; i < value.Length; i++)
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
        public List<string> GetStringList()
        {
            List<string> strList = new List<string>();

            foreach (var line in hashSet)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var b in line)
                {
                    //If the string isn't of an even length, pad it
                    string tmp = b?.ToString("X");

                    if (tmp == null)
                    {
                        tmp = "??";
                    }
                    else if (tmp.Length % 2 != 0)
                    {
                        tmp = "0" + tmp;
                    }

                    sb.Append(tmp);
                }
                strList.Add(sb.ToString());
            }
            return strList;
        }
        public int GetPrecision()
        {
            var value = byteList[0];
            return value.Length;
        }
    }
}
