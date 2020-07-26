namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Numerics;
    using System.Security.Cryptography;
    using System.Windows.Forms;
    using Ceras;

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class BitlogicListFilter : IListFilter
    {
        List<BitlogicFilterEntry> entries = new List<BitlogicFilterEntry>();
        private HashSet<string> options = new HashSet<string>();

        private int precision = 0;

        //Could be moved to a common location
        const char CHAR_WILD = '?';
        const char CHAR_PASS = '#';
        const char CHAR_FLAG = '@';

        public string Initialize(string filePath, string[] dataLines, bool flipBytes, bool syncListViaNetcore)
        {
            bool inHeader = true;
            bool doFlipBytes = flipBytes;

            try
            {
                for (int j = 0; j < dataLines.Length; j++)
                {
                    if (inHeader && ((!string.IsNullOrWhiteSpace(dataLines[j])) && dataLines[j].Length > 1 && dataLines[j][0] == CHAR_FLAG))
                    {
                        string flagOrig = dataLines[j];
                        string flag = dataLines[j].Substring(1).Trim().ToLower();

                        if (flag == "v1.0")
                        {
                            doFlipBytes = true;
                        }

                        options.Add(flagOrig); //add as flag to hashset
                    }
                    else
                    {
                        if (inHeader) { inHeader = false; }
                        var e = ParseLine(j + 2, filePath, dataLines[j], doFlipBytes); //Parse lines individually
                        if (e != null)
                        {
                            entries.Add(e);
                        }
                    }
                }

                if (entries.Count == 0)
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

            //Bad, but keeps individual precision
            precision = entries[0].Precision;

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
            MD5 hash = MD5.Create();
            hash.ComputeHash(bList.ToArray());
            string hashStr = Convert.ToBase64String(hash.Hash);
            return hashStr;
        }

        public bool ContainsValue(byte[] bytes)
        {
            try
            {
                ulong data = BytesToUlong(bytes); //Convert bytes to ulong
                foreach (var e in entries)
                {
                    if (e.Matches(data))
                    {
                        return true;
                    }
                }
                return false;
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
            //Uniform distribution is mega slow so none of that
            var randomEntry = entries[RtcCore.RND.Next(entries.Count)];
            byte[] outValue = null;

            if (passthrough == null || passthrough.Length > 8)
            {
                outValue = BitConverter.GetBytes(randomEntry.GetRandomLegacy()); //Bitconverter as little endian
            }
            else
            {
                ulong passUlong = BytesToUlong(passthrough);
                outValue = BitConverter.GetBytes(randomEntry.GetRandom(passUlong)); //Bitconverter as little endian
            }

            Array.Resize(ref outValue, randomEntry.Precision); //discard the last bytes
            //Copied and pasted from other list implementations
            if (outValue.Length < precision)
            {
                outValue = outValue.PadLeft(precision);
            }
            else if (outValue.Length > precision)
            {
                outValue.FlipBytes(); //Flip the bytes (stored as little endian)
                Array.Resize(ref outValue, precision); //Truncate
                outValue.FlipBytes(); //Flip them back
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
            return res;
        }

        private ulong BytesToUlong(byte[] byteRef)
        {
            var bytes = (byte[])byteRef.Clone();
            //Fun switch of fun, but is faster for most paths than resizing to 8 bytes
            switch (bytes.Length)
            {
                case 1:
                    return (ulong)bytes[0];
                case 2:
                    return (ulong)BitConverter.ToUInt16(bytes, 0);
                case 3:
                    bytes.FlipBytes();
                    Array.Resize(ref bytes, 4);
                    bytes.FlipBytes();
                    return (ulong)BitConverter.ToUInt32(bytes, 0);
                case 4:
                    return (ulong)BitConverter.ToUInt32(bytes, 0);
                case 5:
                case 6:
                case 7:
                    bytes.FlipBytes();
                    Array.Resize(ref bytes, 8);
                    bytes.FlipBytes();
                    return BitConverter.ToUInt64(bytes, 0);
                case 8:
                    return BitConverter.ToUInt64(bytes, 0);
                default:
                    throw new Exception("Invalid byte count in BitLogicListFilter. Limiter must be less than 64 bits (8 bytes)");
            }
        }


        //Valid chars for lines (not including prefix)
        private static char[] validCharListHex = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', CHAR_WILD, CHAR_PASS };
        private static char[] validCharListBinary = new char[] { '0', '1', CHAR_WILD, CHAR_PASS };

        private BitlogicFilterEntry ParseLine(int lineNum, string filePath, string line, bool doFlipBytes)
        {
            bool flipBytes = !doFlipBytes; //to maintain sanity

            line = line.Trim(); //remove whitespace on both sides
            string originalLine = line;

            //Ignore empty lines and comments
            if (string.IsNullOrWhiteSpace(line) || (line.Length > 2 && (line.Substring(0, 2) == "//")))
            {
                return null; //do not throw error
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
            char[] validChars = isHex ? validCharListHex : validCharListBinary; //Get ref to correct valid char list
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
            if ((!isHex && line.Length > 64) || (isHex && line.Length > 16))
            {
                throw new Exception($"Error reading list {Path.GetFileName(filePath)} (Line {lineNum}), total number of bits must be 64 or less (8 bytes)"); //Warn user about line size too big
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

            //=========================== At this point it *should* be valid ===========================

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

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] //Inline hint (does it do anything here? idk)
        private ulong CharToUlongHex(char c)
        {
            //Ascii format
            int i = (int)c;
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

        private int GetPrecision(string s, int incr = 1)
        {
            return (s.Length * incr) / 8;
        }

        private BitlogicFilterEntry ParseHex(string line)
        {
            //Could be refactored and merged with ParseBin probably

            const ulong digitMask = 0b1111; //ulong mask for one hex digit

            ulong template = 0UL;
            ulong wildcard = 0UL;
            ulong passthrough = 0UL;
            ulong reserved = 0UL;

            //At this point we know only valid characters are in the line

            //Fill in the fields
            int curLeftShift = 0; //Additional variable to maintain my sanity
            for (int j = line.Length - 1; j >= 0; j--)
            {
                if (line[j] == CHAR_WILD) { wildcard |= digitMask << curLeftShift; } //Wildcard
                else if (line[j] == CHAR_PASS) { passthrough |= digitMask << curLeftShift; } //Passthrough
                else //is a Constant
                {
                    //line[j] is guaranteed to be Hex characters here
                    template |= CharToUlongHex(line[j]) << curLeftShift; //Convert char to ulong and shift
                    reserved |= digitMask << curLeftShift; //Also add to reserved mask
                }
                curLeftShift += 4; //add half byte shift
            }
            return new BitlogicFilterEntry(template, wildcard, passthrough, reserved, GetPrecision(line, 4));
        }

        private BitlogicFilterEntry ParseBin(string line)
        {
            ulong template = 0UL;
            ulong wildcard = 0UL;
            ulong passthrough = 0UL;
            ulong reserved = 0UL;

            //At this point we know only valid characters are in the line

            //Fill in the fields
            int curLeftShift = 0; //Additional variable to maintain my sanity
            for (int j = line.Length - 1; j >= 0; j--)
            {
                if (line[j] == CHAR_WILD) { wildcard |= 1UL << curLeftShift; } //Wildcard
                else if (line[j] == CHAR_PASS) { passthrough |= 1UL << curLeftShift; } //Passthrough
                else //Constant
                {
                    //line[j] is guaranteed to be '1' or '0' here
                    template |= ((ulong)line[j] - 48UL) << curLeftShift; //Convert char to ulong and shift
                    reserved |= 1UL << curLeftShift; //Also add to reserved mask
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
    [Ceras.MemberConfig(TargetMember.All)]
    public class BitlogicFilterEntry
    {
        ulong template;
        ulong wildcard;
        ulong passthrough;
        ulong reserved;
        ulong unreserved;

        public int Precision { get; private set; }
        public string OriginalLine { get; set; }

        //Current random, slow, replace eventually
        static byte[] byteBuffer = new byte[sizeof(ulong)];
        static ulong NextULong()
        {
            RtcCore.RND.NextBytes(byteBuffer);
            return BitConverter.ToUInt64(byteBuffer, 0);
        }

        public BitlogicFilterEntry(ulong template, ulong wildcard, ulong passthrough, ulong reserved, int precision)
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

        public bool Matches(ulong data)
        {
            //template == data and reserved mask
            return template == (data & reserved);
        }

        public ulong GetRandom(ulong data)
        {
            //When passthrough is implemented, uncomment this line and remove the other
            return (NextULong() & wildcard) | (data & passthrough) | template;
            //return (NextULong() & unreserved) | template;
        }

        public ulong GetRandomLegacy()
        {
            return (NextULong() & unreserved) | template;
        }

        /// <summary>
        /// Gets bytes for hashing
        /// </summary>
        public byte[] GetBytesForHash()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(template));
            bytes.AddRange(BitConverter.GetBytes(wildcard));
            bytes.AddRange(BitConverter.GetBytes(passthrough));
            bytes.AddRange(BitConverter.GetBytes(reserved));
            //Don't need unreserved, it's just reserved flipped
            return bytes.ToArray();
        }
    }
}
