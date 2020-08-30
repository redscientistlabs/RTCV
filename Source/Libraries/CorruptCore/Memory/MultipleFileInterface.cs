namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
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
}
