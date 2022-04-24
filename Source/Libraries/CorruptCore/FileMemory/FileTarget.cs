namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public enum FileTargetLocation
    {
        REAL,
        WORKING,
        BACKUP,
        WORKINGFOLDER,
        BACKUPFOLDER
    }

    [Serializable()]
    public class FileTarget
    {
        public string FilePath { get; set; } = "";
        public string BaseDir { get; set; } = "";
        public long PaddingHeader { get; set; } = 0;
        public long PaddingFooter { get; set; } = 0;
        public bool IsVaulted { get; set; } = true;
        public bool BigEndian { get; set; } = true;
        public string OriginalChecksum { get; set; } = null;
        public long OriginalSize { get; set; } = -1;
        public bool isDirty { get; set; } = false;

        public FileTarget(string filePath, string baseDir)
        {
            FilePath = filePath;

            if (!string.IsNullOrWhiteSpace(baseDir))
                BaseDir = baseDir;
        }

        public static string getTargetId(string filePart, string baseDir = null)
        {
            string CreateMd5HashString(byte[] input)
            {
                var hashBytes = System.Security.Cryptography.MD5.Create().ComputeHash(input);
                return string.Join("", hashBytes.Select(b => b.ToString("X")));
            }

            string basepart = "";
            if (!string.IsNullOrWhiteSpace(baseDir))
                basepart = CreateMd5HashString(System.Text.Encoding.UTF8.GetBytes(baseDir));

            string filepart = CreateMd5HashString(System.Text.Encoding.UTF8.GetBytes(filePart));

            return $"{basepart}$${filepart}";
        }

        public string getTargetId() => getTargetId(FilePath, BaseDir);

        public string RealFilePath => BaseDir + FilePath;
        public string WorkingFilePath => Path.Combine(Vault.vaultWorkingPath, getTargetId(), new FileInfo(RealFilePath).Name);
        public string BackupFilePath => Path.Combine(Vault.vaultBackupsPath, getTargetId(), new FileInfo(RealFilePath).Name);

        public bool SetBaseDir(string baseDir)
        {
            if (string.IsNullOrWhiteSpace(baseDir))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(BaseDir))
            {
                if (!FilePath.Contains(baseDir))
                    return false;

                string newFilePath = FilePath.Replace(baseDir, "");
                Vault.Migrate(this, FilePath, BaseDir, FileTargetLocation.BACKUP, newFilePath, baseDir, FileTargetLocation.BACKUP);
                BaseDir = baseDir;
                FilePath = newFilePath;
                return true;
            }
            else
            {
                Vault.Migrate(this, FilePath, BaseDir, FileTargetLocation.BACKUP, FilePath, baseDir, FileTargetLocation.BACKUP);
                BaseDir = baseDir;
                return true;
            }
        }

        public string GetPathFromLocation(FileTargetLocation location)
        {
            switch (location)
            {
                case FileTargetLocation.BACKUP:
                    return BackupFilePath;
                case FileTargetLocation.BACKUPFOLDER:
                    return new FileInfo(BackupFilePath).DirectoryName;
                case FileTargetLocation.WORKING:
                    return WorkingFilePath;
                case FileTargetLocation.WORKINGFOLDER:
                    return new FileInfo(WorkingFilePath).DirectoryName;
                default:
                case FileTargetLocation.REAL:
                    return RealFilePath;
            }
        }

        public override string ToString()
        {
            return FilePath;
        }
    }
}
