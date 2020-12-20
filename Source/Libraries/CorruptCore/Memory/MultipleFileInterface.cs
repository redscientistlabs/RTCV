namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    [Serializable()]
    public class MultipleFileInterface : FileMemoryInterface, IMemoryDomain
    {
        public static Dictionary<string, string> CompositeFilenameDico { get; set; }

        public override string Name => ShortFilename;
        public override long Size => lastMemorySize.GetValueOrDefault(0);

        public override bool BigEndian { get; }
        public override int WordSize => 4;

        public string ShortFilename { get; set; }

        public List<FileInterface> FileInterfaces { get; private set; } = new List<FileInterface>();

        public MultipleFileInterface(FileTarget[] targets, bool bigEndian, bool useAutomaticFileBackups = false)
        {
            if (targets == null)
            {
                throw new ArgumentNullException(nameof(targets));
            }

            try
            {
                BigEndian = bigEndian;
                foreach (var target in targets)
                {
                    try
                    {
                        var fi = new FileInterface(target);
                        FileInterfaces.Add(fi);

                        if (useAutomaticFileBackups)
                        {
                            if (!File.Exists(target.BackupFilePath))
                                fi.SendRealToBackup(false);
                        }
                    }
                    catch
                    {
                        if (LoadAnything)
                        {
                            break;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                ShortFilename = "MultipleFiles";

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

        public override bool CommitChangesToReal()
        {
            bool allSucceeded = true;
            foreach (var fi in FileInterfaces)
                if (!fi.CommitChangesToReal())
                    allSucceeded = false;

            return allSucceeded;
        }

        public override bool SendRealToBackup(bool askConfirmation = true)
        {
            if (askConfirmation && MessageBox.Show("Are you sure you want to reset the backup using the target files?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return false;
            }

            bool allSucceeded = true;
            foreach (var fi in FileInterfaces)
                if (!fi.SendRealToBackup(false))
                    allSucceeded = false;

            return allSucceeded;
        }

        public override bool SendBackupToReal(bool announce = true)
        {
            bool allSucceeded = true;
            foreach (var fi in FileInterfaces)
                if (!fi.SendBackupToReal(false))
                    allSucceeded = false;

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
                    break;
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

            var targets = GetFileTargets();
            if (targets != null)
                foreach (var target in targets)
                    target.isDirty = true;
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

        public override FileTarget[] GetFileTargets()
        {
            List<FileTarget> fileTargets = new List<FileTarget>();

            foreach (var fi in FileInterfaces)
            {
                var targets = fi.GetFileTargets();
                if (targets != null)
                    fileTargets.AddRange(targets);
            }

            if (fileTargets.Count == 0)
                return null;

            return fileTargets.ToArray();
        }
    }
}
