namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;
    using Newtonsoft.Json;

    public enum FileInterfaceIdentity
    {
        SELF_DESCRIBE,
        HASHED_PREFIX,
        FULL_PATH,
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

        public long StartPadding { get; }
        public long EndPadding { get; }
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

        public FileInterface(string _targetId, bool _bigEndian, bool _useAutomaticFileBackups = false, long _startPadding = 0, long _endPadding = 0)
        {
            try
            {
                string[] targetId = _targetId.Split('|');
                Filename = targetId[1];
                var fi = new FileInfo(Filename);
                ShortFilename = fi.Name;
                BigEndian = _bigEndian;
                StartPadding = 0;
                EndPadding = 0;

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

            lastRealMemorySize = new FileInfo(Filename).Length - (StartPadding + EndPadding);

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
            long offsetAddress = address + StartPadding;

            if (offsetAddress + data.Length >= lastRealMemorySize)
            {
                return;
            }

            if (stream == null)
            {
                stream = File.Open(SetWorkingFile(), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }

            stream.Position = offsetAddress;
            stream.Write(data, 0, data.Length);

            if (cacheEnabled)
            {
                MemoryBanks.PokeBytes(lastMemoryDump, offsetAddress, data);
            }

            /*
            if (lastMemoryDump != null)
                for (int i = 0; i < data.Length; i++)
                    lastMemoryDump[address + i] = data[i];
            */
        }

        public override void PokeByte(long address, byte data)
        {
            long offsetAddress = address + StartPadding;

            if (offsetAddress >= lastRealMemorySize)
            {
                return;
            }

            try
            {
                if (stream == null)
                {
                    stream = File.Open(SetWorkingFile(), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                }

                stream.Position = offsetAddress;
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
                MemoryBanks.PokeByte(lastMemoryDump, offsetAddress, data);
            }
            //lastMemoryDump[address] = data;
        }

        public override byte PeekByte(long address)
        {
            long offsetAddress = address + StartPadding;

            if (offsetAddress >= lastRealMemorySize)
            {
                return 0;
            }

            if (cacheEnabled)
            {
                return MemoryBanks.PeekByte(lastMemoryDump, offsetAddress);
            }
            //return lastMemoryDump[address];
            try
            {
                byte[] readBytes = new byte[1];

                if (stream == null)
                {
                    stream = File.Open(SetWorkingFile(), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                }

                stream.Position = offsetAddress;
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
            long offsetAddress = address + StartPadding;

            if (offsetAddress + length > lastRealMemorySize)
            {
                return new byte[length];
            }

            if (cacheEnabled)
            {
                return MemoryBanks.PeekBytes(lastMemoryDump, offsetAddress, length);
            }
            //return lastMemoryDump.SubArray(address, range);

            byte[] readBytes = new byte[length];

            if (stream == null)
            {
                stream = File.Open(SetWorkingFile(), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }

            stream.Position = offsetAddress;
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
}
