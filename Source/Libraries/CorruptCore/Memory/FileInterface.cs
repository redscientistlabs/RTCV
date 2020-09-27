namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Newtonsoft.Json;
    using RTCV.CorruptCore.Extensions;

    [SuppressMessage("Microsoft.Design", "CA1707", Justification = "FileInterfaceIdentity enum values may have underscores since changing this may break serialization.")]
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

        [SuppressMessage("Microsoft.Design", "CA2211", Justification = "Unknown serialization impact of making this field a property")]
        public static FileInterfaceIdentity identity = FileInterfaceIdentity.SELF_DESCRIBE;
        public override string Name => ShortFilename;
        public override long Size => lastMemorySize.GetValueOrDefault(0);

        public override bool BigEndian { get; }

        public long StartPadding { get; }
        public long EndPadding { get; }
        public override int WordSize => 4;

        public string Filename { get; set; }
        public string ShortFilename { get; private set; } = null;

        public MultipleFileInterface parent { get; set; } = null;
        public override byte[][] lastMemoryDump { get; set; } = null;
        public override bool cacheEnabled => lastMemoryDump != null;

        //lastMemorySize gets rounded up to a multiplier of 4 to make the vector engine work on multiple files
        //lastRealMemorySize is used in peek/poke to cancel out non-existing adresses
        public override long? lastMemorySize { get; set; }
        public long? lastRealMemorySize { get; set; }
        public bool UseAutomaticFileBackups { get; set; } = false;

        public long MultiFilePosition { get; set; } = 0;
        public long MultiFilePositionCeiling { get; set; } = 0;

        private string InterfaceUniquePrefix = "";

        private FileTarget target;

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

        [SuppressMessage("Microsoft.Design", "CA1801", Justification = "_startPadding and endPadding will be used eventually")]
        //public FileInterface(string targetId, bool bigEndian, bool useAutomaticFileBackups = false, long startPadding = 0, long endPadding = 0)
        public FileInterface(FileTarget fileTarget)
        {
            if (fileTarget == null)
            {
                throw new ArgumentNullException(nameof(fileTarget));
            }

            try
            {
                target = fileTarget;

                Filename = target.RealFilePath;
                var fi = new FileInfo(Filename);
                ShortFilename = fi.Name;
                BigEndian = target.BigEndian;
                StartPadding = target.PaddingHeader;
                EndPadding = target.PaddingFooter;

                InterfaceUniquePrefix = Filename.CreateMD5().Substring(0, 4).ToUpper();
                this.UseAutomaticFileBackups = target.IsVaulted;

                if (!File.Exists(Filename))
                {
                    throw new FileNotFoundException("The file " + Filename + " doesn't exist! Cancelling load");
                }

                FileInfo info = new FileInfo(Filename);

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

                if (this.UseAutomaticFileBackups)
                {
                    if (!File.Exists(target.BackupFilePath))
                        SendRealToBackup(false);
                }

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

        public override bool CommitChangesToReal()
        {
            CloseStream();
            return true;
        }

        public override bool SendRealToBackup(bool askConfirmation = true)
        {
            if (askConfirmation && MessageBox.Show("Are you sure you want to reset the backup using the target file?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return false;
            }

            try
            {
                Vault.CopyRealToBackup(target);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Couldn't set backup of {target.RealFilePath}!");
                logger.Debug(ex, "SetBackup failed");
                return false;
            }

            return true;
        }

        public override bool SendBackupToReal(bool announce = true)
        {
            if (File.Exists(target.BackupFilePath))
            {
                try
                {
                    Vault.CopyBackupToReal(target);
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
            if (UseAutomaticFileBackups)
            {
                lastMemoryDump = MemoryBanks.ReadFile(target.BackupFilePath);
            }
            else
            {
                lastMemoryDump = MemoryBanks.ReadFile(target.RealFilePath);
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
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            long offsetAddress = address + StartPadding;
            if (offsetAddress + data.Length >= lastRealMemorySize)
            {
                return;
            }

            if (stream == null)
            {
                stream = File.Open(target.RealFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }

            stream.Position = offsetAddress;
            stream.Write(data, 0, data.Length);

            if (cacheEnabled)
            {
                MemoryBanks.PokeBytes(lastMemoryDump, offsetAddress, data);
            }

            if (target != null)
                target.isDirty = true;
        }

        public override void PokeByte(long address, byte data)
        {
            long offsetAddress = address + StartPadding;

            if (offsetAddress >= lastRealMemorySize)
            {
                return;
            }

            if (cacheEnabled)
            {
                MemoryBanks.PokeByte(lastMemoryDump, offsetAddress, data);
            }

            try
            {
                if (stream == null)
                {
                    stream = File.Open(target.RealFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
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

            if (target != null)
                target.isDirty = true;
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

            try
            {
                byte[] readBytes = new byte[1];

                if (stream == null)
                {
                    stream = File.Open(target.RealFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                }

                stream.Position = offsetAddress;
                stream.Read(readBytes, 0, 1);

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

            byte[] readBytes = new byte[length];

            if (stream == null)
            {
                stream = File.Open(target.RealFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }

            stream.Position = offsetAddress;
            stream.Read(readBytes, 0, length);

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

        public override FileTarget[] GetFileTargets()
        {
            if (target == null)
                return null;

            return new FileTarget[] { target };
        }
    }
}
