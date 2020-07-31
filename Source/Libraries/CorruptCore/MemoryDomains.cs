namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using Ceras;
    using Newtonsoft.Json;

    public class RomParts
    {
        public string Error { get; set; }
        public string PrimaryDomain { get; set; }
        public string SecondDomain { get; set; }
        public int SkipBytes { get; set; } = 0;
    }

    [XmlInclude(typeof(BlastLayer))]
    [XmlInclude(typeof(BlastUnit))]
    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class VmdPrototype
    {
        public string VmdName { get; set; }
        public string GenDomain { get; set; }
        public bool BigEndian { get; set; }
        public int WordSize { get; set; }
        public long PointerSpacer { get; set; } = 1;

        public long Padding { get; set; }

        public List<long> AddSingles { get; set; } = new List<long>();
        public List<long> RemoveSingles { get; set; } = new List<long>();

        public List<long[]> AddRanges { get; set; } = new List<long[]>();
        public List<long[]> RemoveRanges { get; set; } = new List<long[]>();

        public BlastLayer SuppliedBlastLayer = null;

        public VmdPrototype()
        {
        }

        public VirtualMemoryDomain Generate()
        {
            VirtualMemoryDomain VMD = new VirtualMemoryDomain
            {
                Proto = this,
                Name = VmdName,
                BigEndian = BigEndian,
                WordSize = WordSize
            };

            if (SuppliedBlastLayer != null)
            {
                VMD.AddFromBlastLayer(SuppliedBlastLayer);
                VMD.Compact();
                return VMD;
            }

            int addressCount = 0;
            for (int i = 0; i < Padding; i++)
            {
                //VMD.PointerDomains.Add(GenDomain);
                VMD.PointerAddresses.Add(i);
            }

            foreach (long[] range in AddRanges)
            {
                long start = range[0];
                long end = range[1];
                if (end < start)
                {
                    continue;
                }

                for (long i = start; i < end; i++)
                {
                    if (!IsAddressInRanges(i, RemoveSingles, RemoveRanges))
                    {
                        if (PointerSpacer == 1 || addressCount % PointerSpacer == 0)
                        {
                            //VMD.PointerDomains.Add(GenDomain);
                            VMD.PointerAddresses.Add(i);
                        }
                    }

                    addressCount++;
                }
            }

            foreach (long single in AddSingles)
            {
                //VMD.PointerDomains.Add(GenDomain);
                VMD.PointerAddresses.Add(single);
                addressCount++;
            }

            VMD.CompactPointerDomains = new string[] { GenDomain };
            VMD.CompactPointerAddresses = new long[][] { VMD.PointerAddresses.ToArray() };

            VMD.Compact(true);

            return VMD;
        }

        public bool IsAddressInRanges(long address, List<long> singles, List<long[]> ranges)
        {
            if (singles.Contains(address))
            {
                return true;
            }

            foreach (long[] range in ranges)
            {
                long start = range[0];
                long end = range[1];

                if (address >= start && address < end)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddFromTrimmedLine(string trimmedLine, long currentDomainSize, bool remove)
        {
            var lineParts = trimmedLine.Split('-');

            if (lineParts.Length > 1)
            {
                var start = SafeStringToLong(lineParts[0]);
                var end = SafeStringToLong(lineParts[1]);

                if (end < start)
                {
                    return;
                }

                if (end >= currentDomainSize)
                {
                    end = Convert.ToInt64(currentDomainSize - 1);
                }

                if (remove)
                {
                    RemoveRanges.Add(new long[] { start, end });
                }
                else
                {
                    AddRanges.Add(new long[] { start, end });
                }
            }
            else
            {
                var address = SafeStringToLong(lineParts[0]);

                if (address > 0 && address < currentDomainSize)
                {
                    if (remove)
                    {
                        RemoveSingles.Add(address);
                    }
                    else
                    {
                        AddSingles.Add(address);
                    }
                }
            }
        }

        private static long SafeStringToLong(string input)
        {
            try
            {
                if (input.IndexOf("0X", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return long.Parse(input.Substring(2), NumberStyles.HexNumber);
                }
                else
                {
                    return long.Parse(input, NumberStyles.HexNumber);
                }
            }
            catch (FormatException e)
            {
                Console.Write(e);
                return -1;
            }
        }
    }

    [Serializable()]
    public abstract class FileMemoryInterface : IMemoryDomain
    {
        [NonSerialized]
        [Ceras.Exclude]
        internal static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public abstract string Name { get; }

        public abstract long Size { get; }

        public abstract int WordSize { get; }
        public abstract bool BigEndian { get; }

        public abstract void CloseStream();
        public abstract void getMemoryDump();
        public abstract void wipeMemoryDump();
        public abstract byte[][] lastMemoryDump { get; set; }
        public abstract bool cacheEnabled { get; }

        public abstract long getMemorySize();
        public abstract long? lastMemorySize { get; set; }

        //public abstract Dictionary<String, String> CompositeFilenameDico { get; set; }

        public abstract void PokeByte(long address, byte data);
        public abstract void PokeBytes(long address, byte[] data);
        public abstract byte PeekByte(long address);
        public abstract byte[] PeekBytes(long address, int length);

        public abstract bool SetBackup();
        public abstract bool ResetBackup(bool askConfirmation = true);
        public abstract bool RestoreBackup(bool announce = true);
        public abstract bool ResetWorkingFile();
        public abstract bool ApplyWorkingFile();

        public volatile System.IO.Stream stream = null;
    }

    [Serializable()]
    public class FileInterface : FileMemoryInterface
    {
        //File management
        public static Dictionary<string, string> CompositeFilenameDico { get; set; }

        public static FileInterfaceIdentity identity = FileInterfaceIdentity.SELF_DESCRIBE;
        public override string Name => ShortFilename;
        public override long Size => lastMemorySize.GetValueOrDefault(0);

        public override bool BigEndian { get; }
        public override int WordSize => 4;

        public string Filename;
        public string ShortFilename = null;

        public MultipleFileInterface parent = null;
        public override byte[][] lastMemoryDump { get; set; } = null;
        public override bool cacheEnabled => lastMemoryDump != null;

        //lastMemorySize gets rounded up to a multiplier of 4 to make the vector engine work on multiple files
        //lastRealMemorySize is used in peek/poke to cancel out non-existing adresses
        public override long? lastMemorySize { get; set; }
        public long? lastRealMemorySize { get; set; }
        public bool useAutomaticFileBackups { get; set; } = false;

        public long MultiFilePosition = 0;
        public long MultiFilePositionCeiling = 0;

        public string InterfaceUniquePrefix = "";

        public override string ToString()
        {
            switch (identity)
            {
                case FileInterfaceIdentity.HASHED_PREFIX:
                    return InterfaceUniquePrefix + ":" + ShortFilename;
                case FileInterfaceIdentity.FULL_PATH:
                    return Filename;
                case FileInterfaceIdentity.SELF_DESCRIBE:
                default:
                    return ShortFilename;
            }
        }

        public FileInterface(string _targetId, bool _bigEndian, bool _useAutomaticFileBackups = false)
        {
            try
            {
                string[] targetId = _targetId.Split('|');
                Filename = targetId[1];
                var fi = new FileInfo(Filename);
                ShortFilename = fi.Name;
                BigEndian = _bigEndian;
                InterfaceUniquePrefix = Filename.CreateMD5().Substring(0, 4).ToUpper();
                useAutomaticFileBackups = _useAutomaticFileBackups;

                if (!File.Exists(Filename))
                {
                    throw new FileNotFoundException("The file " + Filename + " doesn't exist! Cancelling load");
                }

                FileInfo info = new System.IO.FileInfo(Filename);

                if (info.IsReadOnly)
                {
                    throw new Exception("The file " + Filename + " is read - only! Cancelling load");
                }
                try
                {
                    using (Stream stream = new FileStream(Filename, FileMode.Open, FileAccess.ReadWrite))
                    {
                        Console.Write(stream.Length);
                    }
                }
                catch (IOException ex)
                {
                    if (ex is PathTooLongException)
                    {
                        throw new Exception($"FileInterface failed to load something because the path is too long. Try moving it closer to root \n" + "Culprit file: " + Filename + "\n" + ex.Message);
                    }
                    throw new Exception($"FileInterface failed to load something because the file is (probably) in use \n" + "Culprit file: " + Filename + "\n", ex);
                }

                if (useAutomaticFileBackups)
                {
                    SetBackup();
                }

                //getMemoryDump();
                getMemorySize();
            }
            catch (Exception ex)
            {
                if (parent != null && !MultipleFileInterface.LoadAnything)
                {
                    MessageBox.Show($"FileInterface failed to load something \n\n" + "Culprit file: " + Filename + "\n\n" + ex.ToString());
                }

                throw;
            }
            finally
            {
                CloseStream();
            }
        }

        public string getCompositeFilename()
        {
            if (CompositeFilenameDico.ContainsKey(Filename))
            {
                return CompositeFilenameDico[Filename];
            }
            //Add it to the dico
            string name = (CompositeFilenameDico.Keys.Count + 1).ToString();
            CompositeFilenameDico[Filename] = name;
            //Flush to disk
            SaveCompositeFilenameDico();
            return name;
        }

        public static bool LoadCompositeFilenameDico(string jsonBaseDir = null)
        {
            if (jsonBaseDir == null)
            {
                jsonBaseDir = RtcCore.EmuDir;
            }

            JsonSerializer serializer = new JsonSerializer();
            var path = Path.Combine(jsonBaseDir, "FILEBACKUPS", "filemap.json");
            if (!File.Exists(path))
            {
                CompositeFilenameDico = new Dictionary<string, string>();
                return true;
            }
            try
            {
                using (StreamReader sw = new StreamReader(path))
                using (JsonTextReader reader = new JsonTextReader(sw))
                {
                    CompositeFilenameDico = serializer.Deserialize<Dictionary<string, string>>(reader);
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("Unable to access the filemap! Figure out what's locking it and then restart the WGH.\n" + e.ToString());
                return false;
            }
            return true;
        }

        public static bool SaveCompositeFilenameDico(string jsonFilePath = null)
        {
            if (jsonFilePath == null)
            {
                jsonFilePath = RtcCore.EmuDir;
            }

            JsonSerializer serializer = new JsonSerializer();
            var folder = Path.Combine(jsonFilePath, "FILEBACKUPS");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var path = Path.Combine(folder, "filemap.json");
            try
            {
                using (StreamWriter sw = new StreamWriter(File.Create(path)))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, CompositeFilenameDico);
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("Unable to access the filemap!\n" + e.ToString());
                return false;
            }
            return true;
        }

        public string getCorruptFilename(bool overrideWriteCopyMode = false)
        {
            if (overrideWriteCopyMode)
            {
                return Path.Combine(RtcCore.EmuDir, "FILEBACKUPS", getCompositeFilename());
            }
            else
            {
                return Filename;
            }
        }

        public string getBackupFilename()
        {
            return Path.Combine(RtcCore.EmuDir, "FILEBACKUPS", getCompositeFilename());
        }

        public override bool ResetWorkingFile()
        {
            try
            {
                if (File.Exists(getCorruptFilename()))
                {
                    File.Delete(getCorruptFilename());
                }
            }
            catch
            {
                MessageBox.Show($"Could not get access to {getCorruptFilename()}\n\nClose the file then try whatever you were doing again", "WARNING");
                return false;
            }

            SetWorkingFile();
            return true;
        }

        public string SetWorkingFile()
        {
            string corruptFilename = getCorruptFilename();

            if (!File.Exists(corruptFilename))
            {
                File.Copy(getBackupFilename(), corruptFilename, true);
            }

            return corruptFilename;
        }

        public override bool ApplyWorkingFile()
        {
            CloseStream();
            return true;
        }

        public override bool SetBackup()
        {
            try
            {
                if (!File.Exists(getBackupFilename()))
                {
                    File.Copy(Filename, getBackupFilename(), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Couldn't set backup of {Filename}!");
                logger.Debug(ex, "SetBackup failed");
                return false;
            }
            return true;
        }

        public override bool ResetBackup(bool askConfirmation = true)
        {
            if (askConfirmation && MessageBox.Show("Are you sure you want to reset the backup using the target file?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return false;
            }

            try
            {
                if (File.Exists(getBackupFilename()))
                {
                    File.Delete(getBackupFilename());
                }

                SetBackup();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Couldn't reset backup of {Filename}!");
                logger.Debug(ex, "ResetBackup failed");
                return false;
            }
            return true;
        }

        public override bool RestoreBackup(bool announce = true)
        {
            if (File.Exists(getBackupFilename()))
            {
                try
                {
                    File.Copy(getBackupFilename(), Filename, true);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Unable to restore backup of {Filename}!");
                    logger.Debug(e, "RestoreBackup failed");
                    return false;
                }

                if (announce)
                {
                    MessageBox.Show("Backup of " + ShortFilename + " was restored");
                }
            }
            else
            {
                MessageBox.Show("Couldn't find backup of " + ShortFilename);
            }

            return true;
        }

        public override void wipeMemoryDump()
        {
            lastMemoryDump = null;
            //GC.Collect();
            //GC.WaitForFullGCComplete();
        }

        public override void getMemoryDump()
        {
            if (useAutomaticFileBackups)
            {
                lastMemoryDump = MemoryBanks.ReadFile(getBackupFilename());
            }
            else
            {
                lastMemoryDump = MemoryBanks.ReadFile(Filename);
            }
        }

        public override long getMemorySize()
        {
            if (lastMemorySize != null)
            {
                return (long)lastMemorySize;
            }

            lastRealMemorySize = new FileInfo(Filename).Length;

            long Alignment32bitReminder = lastRealMemorySize.Value % 4;

            if (Alignment32bitReminder != 0)
            {
                lastMemorySize = lastRealMemorySize.Value + (4 - Alignment32bitReminder);
            }
            else
            {
                lastMemorySize = lastRealMemorySize;
            }

            return (long)lastMemorySize;
        }

        public override void PokeBytes(long address, byte[] data)
        {
            if (address + data.Length >= lastRealMemorySize)
            {
                return;
            }

            if (stream == null)
            {
                stream = File.Open(SetWorkingFile(), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }

            stream.Position = address;
            stream.Write(data, 0, data.Length);

            if (cacheEnabled)
            {
                MemoryBanks.PokeBytes(lastMemoryDump, address, data);
            }

            /*
            if (lastMemoryDump != null)
                for (int i = 0; i < data.Length; i++)
                    lastMemoryDump[address + i] = data[i];
            */
        }

        public override void PokeByte(long address, byte data)
        {
            if (address >= lastRealMemorySize)
            {
                return;
            }

            try
            {
                if (stream == null)
                {
                    stream = File.Open(SetWorkingFile(), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                }

                stream.Position = address;
                stream.WriteByte(data);
            }
            catch (IOException e)
            {
                logger.Error(e, "IOException in FileInterface.PeekByte!");
                Exception _e = e.InnerException;
                while (_e != null)
                {
                    logger.Error(e, "InnerException in FileInterface.PeekByte!");
                    _e = _e.InnerException;
                }
                throw;
            }

            if (cacheEnabled)
            {
                MemoryBanks.PokeByte(lastMemoryDump, address, data);
            }
            //lastMemoryDump[address] = data;
        }

        public override byte PeekByte(long address)
        {
            if (address >= lastRealMemorySize)
            {
                return 0;
            }

            if (cacheEnabled)
            {
                return MemoryBanks.PeekByte(lastMemoryDump, address);
            }
            //return lastMemoryDump[address];
            try
            {
                byte[] readBytes = new byte[1];

                if (stream == null)
                {
                    stream = File.Open(SetWorkingFile(), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                }

                stream.Position = address;
                stream.Read(readBytes, 0, 1);

                //fs.Close();

                return readBytes[0];
            }
            catch (IOException e)
            {
                logger.Error(e, "IOException in FileInterface.PeekByte!");
                Exception _e = e.InnerException;
                while (_e != null)
                {
                    logger.Error(e, "InnerException in FileInterface.PeekByte!");
                    _e = _e.InnerException;
                }
                return 0;
            }
        }

        public override byte[] PeekBytes(long address, int length)
        {
            if (address + length > lastRealMemorySize)
            {
                return new byte[length];
            }

            if (cacheEnabled)
            {
                return MemoryBanks.PeekBytes(lastMemoryDump, address, length);
            }
            //return lastMemoryDump.SubArray(address, range);

            byte[] readBytes = new byte[length];

            if (stream == null)
            {
                stream = File.Open(SetWorkingFile(), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }

            stream.Position = address;
            stream.Read(readBytes, 0, length);

            //fs.Close();

            return readBytes;
        }

        public override void CloseStream()
        {
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }
        }
    }

    [Serializable()]
    public class MultipleFileInterface : FileMemoryInterface, IMemoryDomain
    {
        public static Dictionary<string, string> CompositeFilenameDico { get; set; }

        public override string Name => ShortFilename;
        public override long Size => lastMemorySize.GetValueOrDefault(0);

        public override bool BigEndian { get; }
        public override int WordSize => 4;

        public string Filename;
        public string ShortFilename;

        public List<FileInterface> FileInterfaces = new List<FileInterface>();

        public MultipleFileInterface(string _targetId, bool _bigEndian, bool _useAutomaticFileBackups = false)
        {
            try
            {
                BigEndian = _bigEndian;
                string[] targetId = _targetId.Split('|');
                foreach (string t in targetId)
                {
                    try
                    {
                        var fi = new FileInterface("File|" + t, _bigEndian, _useAutomaticFileBackups)
                        {
                            parent = this
                        };
                        FileInterfaces.Add(fi);
                    }
                    catch
                    {
                        if (MultipleFileInterface.LoadAnything)
                        {
                            break;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                Filename = "MultipleFiles";
                ShortFilename = "MultipleFiles";

                if (_useAutomaticFileBackups)
                {
                    SetBackup();
                }

                //getMemoryDump();
                getMemorySize();

                setFilePositions();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"MultipleFileInterface failed to load something \n\n" + ex.ToString());
            }
        }

        public override void CloseStream()
        {
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }

            foreach (var fi in FileInterfaces)
            {
                if (fi.stream != null)
                {
                    fi.stream.Close();
                    fi.stream = null;
                }
            }
        }

        public override string ToString()
        {
            return "Multiple Files";
        }

        public string getCompositeFilename()
        {
            return string.Join("|", FileInterfaces.Select(it => it.getCompositeFilename()));
        }

        public string getCorruptFilename(bool overrideWriteCopyMode = false)
        {
            return string.Join("|", FileInterfaces.Select(it => it.getCorruptFilename(overrideWriteCopyMode)));
        }

        public string getBackupFilename()
        {
            return string.Join("|", FileInterfaces.Select(it => it.getBackupFilename()));
        }

        public override bool ResetWorkingFile()
        {
            bool allSucceeded = true;
            foreach (var fi in FileInterfaces)
            {
                if (allSucceeded)
                {
                    allSucceeded = fi.ResetWorkingFile();
                }
                else
                {
                    fi.ResetWorkingFile();
                }
            }

            return allSucceeded;
        }

        public string SetWorkingFile()
        {
            return string.Join("|", FileInterfaces.Select(it => it.SetWorkingFile()));
        }

        public override bool ApplyWorkingFile()
        {
            bool allSucceeded = true;
            foreach (var fi in FileInterfaces)
            {
                if (allSucceeded)
                {
                    allSucceeded = fi.ApplyWorkingFile();
                }
                else
                {
                    fi.ApplyWorkingFile();
                }
            }

            return allSucceeded;
        }

        public override bool SetBackup()
        {
            bool allSucceeded = true;
            foreach (var fi in FileInterfaces)
            {
                if (allSucceeded)
                {
                    allSucceeded = fi.SetBackup();
                }
                else
                {
                    fi.SetBackup();
                }
            }

            return allSucceeded;
        }

        public override bool ResetBackup(bool askConfirmation = true)
        {
            bool allSucceeded = true;
            if (askConfirmation && MessageBox.Show("Are you sure you want to reset the backup using the target files?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return false;
            }

            foreach (var fi in FileInterfaces)
            {
                if (allSucceeded)
                {
                    allSucceeded = fi.ResetBackup(false);
                }
                else
                {
                    fi.ResetBackup(false);
                }
            }
            return allSucceeded;
        }

        public override bool RestoreBackup(bool announce = true)
        {
            bool allSucceeded = true;
            foreach (var fi in FileInterfaces)
            {
                if (allSucceeded)
                {
                    allSucceeded = fi.RestoreBackup(false);
                }
                else
                {
                    fi.RestoreBackup(false);
                }
            }
            if (announce)
            {
                MessageBox.Show("Backups of " + string.Join(",", FileInterfaces.Select(it => (it as FileInterface).ShortFilename)) + " were restored");
            }

            return allSucceeded;
        }

        public void setFilePositions()
        {
            long addressPad = 0;

            //find which fileInterface contains the file we want
            foreach (var fi in FileInterfaces)
            {
                fi.MultiFilePosition = addressPad;
                addressPad += fi.getMemorySize();
                fi.MultiFilePositionCeiling = addressPad;
            }
        }

        public override void wipeMemoryDump()
        {
            for (int i = 0; i < FileInterfaces.Count; i++)
            {
                FileInterfaces[i].wipeMemoryDump();
                //GC.Collect();
                //GC.WaitForFullGCComplete();
            }
        }

        public override void getMemoryDump()
        {
            long totalDumpSize = 0;

            for (int i = 0; i < FileInterfaces.Count; i++)
            {
                totalDumpSize += FileInterfaces[i].lastMemorySize.Value;
                FileInterfaces[i].getMemoryDump();
            }

            //lastMemoryDump = new byte[totalDumpSize];

            //long targetAddress = 0;

            //for (int i = 0; i < FileInterfaces.Count; i++)
            //{

            //Removed copying of the memory in a local big file because
            //it's smarter to actually use the FileInterfaces themselves
            /*
            long targetLength = FileInterfaces[i].lastMemorySize.Value;
            Array.Copy(FileInterfaces[i].lastMemoryDump, 0, lastMemoryDump, targetAddress, targetLength);
            targetAddress += targetLength;
            FileInterfaces[i].lastMemoryDump = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            */
            //}
            /*
            List<byte> allBytes = new List<byte>();

            foreach (var fi in FileInterfaces)
            {
                allBytes.AddRange(fi.getMemoryDump());
                fi.lastMemoryDump = null;
            }

        lastMemoryDump = allBytes.ToArray();
        */

            //return lastMemoryDump;
        }

        #pragma warning disable CA1065
        public override byte[][] lastMemoryDump
        {
            get => throw new Exception("FORBIDDEN USE OF LASTMEMORYDUMP ON MULTIPLEFILEINTERFACE");
            set => throw new Exception("FORBIDDEN USE OF LASTMEMORYDUMP ON MULTIPLEFILEINTERFACE");
        }

        public override bool cacheEnabled => FileInterfaces.Count > 0 && FileInterfaces[0].lastMemoryDump != null;

        public override long getMemorySize()
        {
            long size = 0;

            foreach (var fi in FileInterfaces)
            {
                size += fi.getMemorySize();
            }

            lastMemorySize = size;
            return (long)lastMemorySize;
        }

        public override long? lastMemorySize { get; set; }
        public static bool LoadAnything { get; set; } = false;

        public override void PokeBytes(long address, byte[] data)
        {
            //find which fileInterface contains the file we want
            for (int i = 0; i < FileInterfaces.Count; i++)
            {
                var fi = FileInterfaces[i];

                if (fi.MultiFilePositionCeiling > address)
                {
                    fi.PokeBytes(address - fi.MultiFilePosition, data);
                    return;
                }
            }
        }

        public override void PokeByte(long address, byte data)
        {
            //find which fileInterface contains the file we want
            for (int i = 0; i < FileInterfaces.Count; i++)
            {
                var fi = FileInterfaces[i];

                if (fi.MultiFilePositionCeiling > address)
                {
                    fi.PokeByte(address - fi.MultiFilePosition, data);
                    return;
                }
            }
        }

        public override byte PeekByte(long address)
        {
            //find which fileInterface contains the file we want
            for (int i = 0; i < FileInterfaces.Count; i++)
            {
                var fi = FileInterfaces[i];

                if (fi.MultiFilePositionCeiling > address)
                {
                    return fi.PeekByte(address - fi.MultiFilePosition);
                }
            }

            //if wasn't found
            return 0;
        }

        public override byte[] PeekBytes(long address, int range)
        {
            //find which fileInterface contains the file we want
            for (int i = 0; i < FileInterfaces.Count; i++)
            {
                var fi = FileInterfaces[i];

                if (fi.MultiFilePositionCeiling > address)
                {
                    return fi.PeekBytes(address - fi.MultiFilePosition, range);
                }
            }

            //if wasn't found
            return null;
        }
    }

    public enum FileInterfaceIdentity
    {
        SELF_DESCRIBE,
        HASHED_PREFIX,
        FULL_PATH,
    }

    public interface IMemoryDomain
    {
        string Name { get; }
        long Size { get; }
        int WordSize { get; }
        bool BigEndian { get; }

        byte PeekByte(long addr);
        byte[] PeekBytes(long address, int length);
        void PokeByte(long addr, byte val);
    }
}
