using System.Runtime.CompilerServices;
using RTCV.Common;

namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Windows.Forms;
    using Ceras;
    using RTCV.CorruptCore.Extensions;

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class BitlogicListFilter : IListFilter
    {
        //Could be moved to a common location
        const char CHAR_WILD = '?';
        const char CHAR_PASS = '#';
        const char CHAR_FLAG = '@';
        const char CHAR_EXCLUDE = '!';

        //Valid chars for lines (not including prefixes)
        private static HashSet<char> validCharHashSetHex = new HashSet<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', CHAR_WILD, CHAR_PASS };
        private static HashSet<char> validCharHashSetBinary = new HashSet<char>() { '0', '1', CHAR_WILD, CHAR_PASS };

        List<BitlogicFilterEntry> entries = new List<BitlogicFilterEntry>();
        List<BitlogicFilterEntry> exclusions = new List<BitlogicFilterEntry>();
        private HashSet<string> options = new HashSet<string>();

        private int precision = 0;
        [Ceras.Exclude]
        private static readonly BitlogicFilterEntry AllFilter = new BitlogicFilterEntry(0UL, ulong.MaxValue, 0UL, 0UL, 8);

        public string Initialize(string filePath, string[] dataLines, bool flipBytes, bool syncListViaNetcore)
        {
            bool inHeader = true;
            bool doFlipBytes = flipBytes;

            if (dataLines == null)
            {
                throw new ArgumentNullException(nameof(dataLines));
            }

            try
            {
                for (int j = 0; j < dataLines.Length; j++)
                {
                    if (inHeader && ((!string.IsNullOrWhiteSpace(dataLines[j])) && dataLines[j].Length > 1 && dataLines[j][0] == CHAR_FLAG))
                    {
                        string flagOrig = dataLines[j];
                        string flag = dataLines[j].Substring(1).Trim().ToLower();

                        if (flag == "v1.0" || flag == "forceflip")
                        {
                            doFlipBytes = true;
                        }

                        options.Add(flagOrig); //add as flag to hashset
                    }
                    else
                    {
                        if (inHeader) { inHeader = false; }
                        var line = dataLines[j].Replace(" ", "").Replace("\t", "").Replace("_", ""); //trim and remove spaces and underscores

                        if (line.Length == 0) continue;

                        bool exclusion = false;
                        if (line[0] == CHAR_EXCLUDE)
                        {
                            exclusion = true;
                            line = line.Substring(1);
                        }

                        var e = ParseLine(j + 2, filePath, line, doFlipBytes); //Parse lines individually
                        if (e != null)
                        {
                            if (exclusion)
                            {
                                //Exclusion
                                exclusions.Add(e);
                            }
                            else
                            {
                                //Normal entry
                                entries.Add(e);
                            }
                        }
                    }
                }

                if ((entries.Count + exclusions.Count) == 0)
                {
                    throw new Exception($"Error reading list {Path.GetFileName(filePath)}, list was empty or contained no valid lines"); //show message to user
                }
            }
            catch (Exception e)
            {
                var logger = NLog.LogManager.GetCurrentClassLogger();
                logger.Error(e, "Error in loadListFromPath");
                MessageBox.Show(e.Message);
                return "";
            }

            if (entries.Count != 0) precision = entries[0].Precision;
            else precision = exclusions[0].Precision;

            var name = Path.GetFileNameWithoutExtension(filePath);
            string hash = Filtering.RegisterList(this, name, syncListViaNetcore);
            return hash;
        }

        public string GetHash()
        {
            List<byte> bList = new List<byte>();

            foreach (var e in entries)
            {
                bList.AddRange(e.GetBytesForHash());
            }

            foreach (var e in exclusions)
            {
                bList.AddRange(new byte[] { 0xAB, 0xCD, 0xEF });
                bList.AddRange(e.GetBytesForHash());
            }

            MD5 hash = MD5.Create();
            hash.ComputeHash(bList.ToArray());
            string hashStr = Convert.ToBase64String(hash.Hash);
            return hashStr;
        }

        public bool ContainsValue(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            try
            {
                UInt128 data = BytesToUInt128(bytes); //Convert bytes to ulong

                if (entries.Count > 0)
                {
                    //Check exclusions first
                    foreach (var excl in exclusions)
                    {
                        if (excl.Matches(data))
                        {
                            return false;
                        }
                    }

                    //Check entries
                    foreach (var entry in entries)
                    {
                        if (entry.Matches(data))
                        {
                            return true;
                        }
                    }

                    return false;
                }
                else if (exclusions.Count > 0)
                {
                    foreach (var excl in exclusions)
                    {
                        if (excl.Matches(data))
                        {
                            return false;
                        }
                    }
                    //Else
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public int GetPrecision()
        {
            return precision;
        }

        public byte[] GetRandomValue(string hash, int precision, byte[] passthrough)
        {
            byte[] outValue = null;
            BitlogicFilterEntry selectedEntry = null;

            if (exclusions.Count == 0) //No exclusions
            {
                selectedEntry = entries[RtcCore.RND.Next(entries.Count)];

                if (passthrough == null || passthrough.Length > 16)
                {
                    outValue = selectedEntry.GetRandomLegacy().ToByteArray(); //Bitconverter as little endian
                }
                else
                {
                    UInt128 passUInt128 = BytesToUInt128(passthrough);
                    outValue = selectedEntry.GetRandom(passUInt128).ToByteArray(); //Bitconverter as little endian
                }
            }
            else //Check exclusions
            {
                List<BitlogicFilterEntry> entries;
                if (this.entries.Count == 0) entries = new List<BitlogicFilterEntry>() { AllFilter };
                else entries = this.entries;

                var randomEntry = entries[RtcCore.RND.Next(entries.Count)];

                const int maxRetries = 1000;
                //bool matchedExclusion = false;
                bool foundSuitable = false;
                int cycleChange = maxRetries / entries.Count;
                int cycle = 0;

                if (passthrough == null || passthrough.Length > 8)
                {
                    //Legacy
                    for (int i = 0; i < maxRetries; i++)
                    {
                        bool matchedExclusion = false;
                        UInt128 data = randomEntry.GetRandomLegacy();

                        foreach (var e in exclusions)
                        {
                            if (e.Matches(data))
                            {
                                matchedExclusion = true;
                                break;
                            }
                        }

                        if (matchedExclusion)
                        {
                            cycle++;
                            if (cycle >= cycleChange)
                            {
                                //Try a different one
                                cycle = 0;
                                randomEntry = entries[RtcCore.RND.Next(entries.Count)];
                            }
                        }
                        else //Didn't match exclusions, good
                        {
                            outValue = data.ToByteArray();
                            if (this.entries.Count > 0) selectedEntry = randomEntry;
                            foundSuitable = true;
                            break;
                        }
                    }
                }
                else
                {
                    UInt128 passUInt128 = BytesToUInt128(passthrough);

                    for (int i = 0; i < maxRetries; i++)
                    {
                        bool matchedExclusion = false;
                        UInt128 data = randomEntry.GetRandom(passUInt128);

                        foreach (var e in exclusions)
                        {
                            if (e.Matches(data))
                            {
                                matchedExclusion = true;
                                break;
                            }
                        }

                        if (matchedExclusion)
                        {
                            cycle++;
                            if (cycle >= cycleChange)
                            {
                                //Try a different one
                                cycle = 0;
                                randomEntry = entries[RtcCore.RND.Next(entries.Count)];
                            }
                        }
                        else //Didn't match exclusions, good
                        {
                            outValue = data.ToByteArray();
                            if (this.entries.Count > 0) selectedEntry = randomEntry;
                            foundSuitable = true;
                            break;
                        }
                    }
                }

                if (!foundSuitable)
                {
                    //Give up and make sure no side effects
                    return passthrough;
                }
            } //End exclusion check


            //foundSuitable == true
            //If not using AllFilter
            if (selectedEntry != null) Array.Resize(ref outValue, selectedEntry.Precision); //discard the last bytes

            //Copied and pasted from other list implementations
            if (outValue.Length < precision)
            {
                outValue = outValue.PadLeft(precision);
            }
            else if (outValue.Length > precision)
            {
                byte[] newArr = new byte[precision];
                Array.Copy(outValue, outValue.Length - precision, newArr, 0, precision);
                outValue = newArr;
            }

            return outValue;
        }

        public List<string> GetStringList()
        {
            List<string> res = new List<string>();
            res.Add("@" + nameof(BitlogicListFilter)); //Add top line to specify class for reflection
            res.AddRange(this.options);
            foreach (var e in entries)
            {
                res.Add(e.OriginalLine);
            }
            foreach (var e in exclusions)
            {
                res.Add("!" + e.OriginalLine);
            }
            return res;
        }

        private static UInt128 BytesToUInt128(byte[] bytes)
        {
            ////Fun switch of fun, but is faster for most common paths than resizing to 8 bytes
            switch (bytes.Length)
            {
                case 1:
                    return bytes[0];
                case 2:
                    return BitConverter.ToUInt16(bytes, 0);
                case 3:
                    byte[] new4byte = new byte[4];
                    Array.Copy(bytes, 0, new4byte, 1, 3);
                    return BitConverter.ToUInt32(new4byte, 0);
                case 4:
                    return BitConverter.ToUInt32(bytes, 0);
                case 5:
                case 6:
                case 7:
                    //Flipping twice was too slow
                    byte[] new8byte = new byte[8];
                    Array.Copy(bytes, 0, new8byte, 8 - bytes.Length, bytes.Length);
                    return BitConverter.ToUInt64(new8byte, 0);
                case 8:
                    return BitConverter.ToUInt64(bytes, 0);
                case 9:
                    return new UInt128(BitConverter.ToUInt64(bytes, 0), bytes[8]);
                case 10:
                    return new UInt128(BitConverter.ToUInt64(bytes, 0), BitConverter.ToUInt16(bytes, 8));
                case 11:
                    byte[] new12byte = new byte[12];
                    Array.Copy(bytes, 0, new12byte, 4, 8);
                    return new UInt128(BitConverter.ToUInt64(new12byte, 0), BitConverter.ToUInt32(new12byte, 4));
                case 12:
                    return new UInt128(BitConverter.ToUInt64(bytes, 0), BitConverter.ToUInt32(bytes, 8));
                case 13:
                case 14:
                case 15:
                    byte[] new16byte = new byte[16];
                    Array.Copy(bytes, 0, new16byte, 16 - bytes.Length, bytes.Length);
                    return new UInt128(BitConverter.ToUInt64(new16byte, 0), BitConverter.ToUInt64(new16byte, 8));
                case 16:
                    return new UInt128(BitConverter.ToUInt64(bytes, 0), BitConverter.ToUInt64(bytes, 8));
                default:
                    throw new Exception("Invalid byte count in BitLogicListFilter. Limiter must be less than 64 bits (8 bytes)");
            }
        }



        private static BitlogicFilterEntry ParseLine(int lineNum, string filePath, string line, bool fileMarkedFlipBytes)
        {
            bool flipBytes = !fileMarkedFlipBytes; //bytes are already flipped in method below

            string originalLine = line;

            //Ignore empty lines and comments
            if (string.IsNullOrWhiteSpace(line) || (line.Length > 2 && (line.Substring(0, 2) == "//")))
            {
                return null;
            }


            line = line.ToUpper(); //All alphabetical characters to upper case for easier handling

            bool isHex = true; //Line defaults to hex

            //Check prefix
            if (line.Length > 2)
            {
                string prefix = line.Substring(0, 2);

                if (prefix == "0B")
                {
                    line = line.Substring(2); //remove 0b
                    isHex = false; //Set type to binary
                }
                else if (prefix == "0X")
                {
                    line = line.Substring(2); //Remove 0x
                    //Hex is default, don't need to set
                }
            }

            //Check for invalid characters
            var validChars = isHex ? validCharHashSetHex : validCharHashSetBinary; //Get ref to correct valid char list
            foreach (char c in line)
            {
                if (!validChars.Contains(c))
                {
                    //return null;
                    throw new Exception($"Error reading list {Path.GetFileName(filePath)} (Line {lineNum}), line contains invalid character"); //Warn user about invalid characters
                }
            }

            //Check for line sizes that may be too big
            //Note: May have to move and rework this depending on future formats
            if ((!isHex && line.Length > 128) || (isHex && line.Length > 32))
            {
                throw new Exception($"Error reading list {Path.GetFileName(filePath)} (Line {lineNum}), total number of bits must be 128 or less (16 bytes)"); //Warn user about line size too big
            }

            //Discard non-byte divisible lines
            if ((!isHex && (line.Length % 8 != 0)) || (isHex && (line.Length % 2 != 0)))
            {
                throw new Exception($"Error reading list {Path.GetFileName(filePath)} (Line {lineNum}), lines must be byte sized"); //Warn user about line not byte sized
            }


            //Flip bytes, simulating manual flipping
            if (flipBytes)
            {
                if (isHex)
                {
                    line = FlipBytesStr(line, 2);
                }
                else
                {
                    line = FlipBytesStr(line, 8);
                }
            }

            //=========================== At this point line should only contain valid chars===========================

            //Parse with the correct method
            if (isHex)
            {
                var ret = ParseHex(line);
                ret.OriginalLine = originalLine;
                return ret;
            }
            else
            {
                var ret = ParseBin(line);
                ret.OriginalLine = originalLine;
                return ret;
            }
        }


        //assumes s.Length is evenly divisible by chunksize, should be the case always
        private static string FlipBytesStr(string s, int chunkSize)
        {
            //StringBuilder sb = new StringBuilder();
            string res = "";
            int div = s.Length / chunkSize;
            for (int j = div - 1; j >= 0; j--)
            {
                res += s.Substring(j * chunkSize, chunkSize);
            }
            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)] //Inline hint (does it do anything here? idk)
        private static ulong CharToUlongHex(char c)
        {
            //Ascii format
            int i = c;
            return (ulong)(i - ((i <= 57) ? 48 : 55)); //optimized, values are guaranteed
        }

        ////Gets precision of a line, uncomment if non-byte sized lists become a thing
        //private int GetPrecision(string s, int incr = 1)
        //{
        //    int ret = s.Length * incr;
        //    if (ret % 8 > 0)
        //    {
        //        return (ret / 8) + 1;
        //    }
        //    else
        //    {
        //        return ret / 8;
        //    }
        //}

        private static int GetPrecision(string s, int incr = 1)
        {
            return (s.Length * incr) / 8;
        }

        private static BitlogicFilterEntry ParseHex(string line)
        {
            //Could be refactored and merged with ParseBin probably

            const ulong digitMask = 0b1111; //ulong mask for one hex digit

            UInt128 template = 0;
            UInt128 wildcard = 0;
            UInt128 passthrough = 0;
            UInt128 reserved = 0;

            //At this point we know only valid characters are in the line

            //Fill in the fields
            int curLeftShift = 0; //Additional variable to maintain my sanity
            for (int j = line.Length - 1; j >= 0; j--)
            {
                if (line[j] == CHAR_WILD) { wildcard |= UInt128.LeftShift(digitMask, curLeftShift); } //Wildcard
                else if (line[j] == CHAR_PASS) { passthrough |= UInt128.LeftShift(digitMask, curLeftShift); } //Passthrough
                else //is a Constant
                {
                    //line[j] is guaranteed to be Hex characters here
                    template |= UInt128.LeftShift(CharToUlongHex(line[j]), curLeftShift); //Convert char to ulong and shift
                    reserved |= UInt128.LeftShift(digitMask, curLeftShift); //Also add to reserved mask
                }
                curLeftShift += 4; //add half byte shift
            }
            return new BitlogicFilterEntry(template, wildcard, passthrough, reserved, GetPrecision(line, 4));
        }

        private static BitlogicFilterEntry ParseBin(string line)
        {
            UInt128 template = 0;
            UInt128 wildcard = 0;
            UInt128 passthrough = 0;
            UInt128 reserved = 0;

            //At this point we know only valid characters are in the line

            //Fill in the fields
            int curLeftShift = 0; //Additional variable to maintain my sanity
            for (int j = line.Length - 1; j >= 0; j--)
            {
                if (line[j] == CHAR_WILD) { wildcard |= UInt128.LeftShift(1UL, curLeftShift); } //Wildcard
                else if (line[j] == CHAR_PASS) { passthrough |= UInt128.LeftShift(1UL, curLeftShift); } //Passthrough
                else //Constant
                {
                    //line[j] is guaranteed to be '1' or '0' here
                    
                    template |= UInt128.LeftShift(line[j] - 48UL, curLeftShift); //Convert char to ulong and shift
                    reserved |= UInt128.LeftShift(1UL, curLeftShift); //Also add to reserved mask
                }

                curLeftShift++;
            }
            return new BitlogicFilterEntry(template, wildcard, passthrough, reserved, GetPrecision(line, 1));
        }
    }

    /// <summary>
    /// Represents an entry for bit filter list.
    /// </summary>
    [Serializable]
    [MemberConfig(TargetMember.All)]
    public class BitlogicFilterEntry
    {
        UInt128 template;
        UInt128 wildcard;
        UInt128 passthrough;
        UInt128 reserved;
        UInt128 unreserved;

        public int Precision { get; private set; }
        public string OriginalLine { get; set; }

        //Current random, slow, replace eventually
        static byte[] byteBuffer = new byte[Unsafe.SizeOf<UInt128>()];
        static UInt128 NextUInt128()
        {
            RtcCore.RND.NextBytes(byteBuffer);
            return new UInt128(byteBuffer);
        }

        public BitlogicFilterEntry(UInt128 template, UInt128 wildcard, UInt128 passthrough, UInt128 reserved, int precision)
        {
            this.template = template;
            this.wildcard = wildcard;
            this.passthrough = passthrough;
            this.reserved = reserved;
            this.unreserved = ~reserved; //Opposite of reserved for efficiency
            this.Precision = precision;
        }

        //Gotta do this to satisfy Ceras
        public BitlogicFilterEntry()
        {
            this.template = 0;
            this.wildcard = 0;
            this.passthrough = 0;
            this.reserved = 0;
            this.unreserved = 0;
            Precision = 0;
        }

        public bool Matches(UInt128 data)
        {
            //template == data and reserved mask
            return template == (data & reserved);
        }

        public UInt128 GetRandom(UInt128 data)
        {
            return (NextUInt128() & wildcard) | (data & passthrough) | template;
        }

        public UInt128 GetRandomLegacy()
        {
            return (NextUInt128() & unreserved) | template;
        }

        /// <summary>
        /// Gets bytes for hashing
        /// </summary>
        public byte[] GetBytesForHash()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(template.ToByteArray());
            bytes.AddRange(wildcard.ToByteArray());
            bytes.AddRange(passthrough.ToByteArray());
            bytes.AddRange(reserved.ToByteArray());
            //Don't need unreserved, it's just reserved flipped
            return bytes.ToArray();
        }
    }
}
