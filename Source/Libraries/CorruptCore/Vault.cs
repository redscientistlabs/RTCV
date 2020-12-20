namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Newtonsoft.Json;


    public static class Vault
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        static bool initialized = false;

        static DirectoryInfo vaultDi;
        static string vaultDbPath => Path.Combine(RtcCore.VaultDir, "vault.json");
        public static string vaultBackupsPath => Path.Combine(RtcCore.VaultDir, "BACKUPS");
        public static string vaultWorkingPath => Path.Combine(RtcCore.VaultDir, "WORKING");

        public static event EventHandler VaultUpdated;

        static Dictionary<string, FileTarget> vaultDb = null;

        public static void Init()
        {
            if (!initialized)
            {
                vaultDi = new DirectoryInfo(RtcCore.VaultDir);
                var vaultBackupsDi = new DirectoryInfo(vaultBackupsPath);
                var vaultWorkingDi = new DirectoryInfo(vaultWorkingPath);

                if (!vaultDi.Exists)
                    vaultDi.Create();

                if (!vaultBackupsDi.Exists)
                    vaultBackupsDi.Create();

                if (!vaultWorkingDi.Exists)
                    vaultWorkingDi.Create();

                LoadVaultDb();

                initialized = true;
            }
        }

        public static FileTarget RequestFileTarget(string filePath, string baseDir = null)
        {
            Init();

            string targetId = FileTarget.getTargetId(filePath, baseDir);

            FileTarget target = null;
            vaultDb.TryGetValue(targetId, out target);

            if (target == null)
            {
                target = new FileTarget(filePath, baseDir);
                vaultDb[targetId] = target;
                SaveVaultDb();
            }

            if (!File.Exists(target.BackupFilePath))
                CopyRealToBackup(target);

            return target;
        }

        public static List<FileTarget> GetDirtyTargets() => vaultDb?.Values?.Where(it => it.isDirty).ToList();

        public static bool CopyRealToBackup(FileTarget target) => CopyTarget(target, FileTargetLocation.REAL, FileTargetLocation.BACKUP, false);

        public static bool CopyBackupToWorking(FileTarget target) => CopyTarget(target, FileTargetLocation.BACKUP, FileTargetLocation.WORKING, null);

        internal static void Migrate(FileTarget target, string prevFilePath, string prevBaseDir, FileTargetLocation prevLocation, string newFilePath, string newFileDir, FileTargetLocation newLocation)
        {
            string prevKey = FileTarget.getTargetId(prevFilePath, prevBaseDir);
            string newKey = FileTarget.getTargetId(newFilePath, newFileDir);

            string getPath(string key, FileTargetLocation location)
            {
                switch (location)
                {
                    case FileTargetLocation.REAL:
                        return target.RealFilePath;
                    case FileTargetLocation.WORKING:
                        return Path.Combine(Vault.vaultWorkingPath, key, new FileInfo(target.RealFilePath).Name);
                    case FileTargetLocation.BACKUP:
                        return Path.Combine(Vault.vaultBackupsPath, key, new FileInfo(target.RealFilePath).Name);
                    default:
                        throw null;
                }
            }

            var prevFile = new FileInfo(getPath(prevKey, prevLocation));
            var newFile = new FileInfo(getPath(newKey, newLocation));

            if (!newFile.Directory.Exists)
                newFile.Directory.Create();

            if (newFile.Exists)
                newFile.Delete();

            File.Copy(prevFile.FullName, newFile.FullName);
            Directory.Delete(prevFile.Directory.FullName, true);
        }

        public static bool CopyBackupToReal(FileTarget target) => CopyTarget(target, FileTargetLocation.BACKUP, FileTargetLocation.REAL, false);

        //working to real is dirty even when uncorrupted
        public static bool CopyWorkingToReal(FileTarget target) => CopyTarget(target, FileTargetLocation.WORKING, FileTargetLocation.REAL, true);


        static bool CopyTarget(FileTarget target, FileTargetLocation input, FileTargetLocation output, bool? enforceDirty)
        {
            Init();

            string inputFileLocation = target.GetPathFromLocation(input);
            string outputFileLocation = target.GetPathFromLocation(output);

            DirectoryInfo outputFolder = new DirectoryInfo(new FileInfo(outputFileLocation).DirectoryName);
            if (!outputFolder.Exists)
                outputFolder.Create();

            try
            {
                if (File.Exists(outputFileLocation))
                    File.Delete(outputFileLocation);

                File.Copy(inputFileLocation, outputFileLocation);

                if (enforceDirty != null)
                {
                    target.isDirty = enforceDirty.Value;
                }

                SaveVaultDb();

                return true;
            }
            catch (Exception ex)
            {
                logger.Trace($"Failed to copy file from {input} location '{inputFileLocation}' to {output} location '{outputFileLocation}'\n{ex}");
                return false;
            }

        }


        public static bool LoadVaultDb()
        {
            JsonSerializer serializer = new JsonSerializer();
            if (!File.Exists(vaultDbPath))
            {
                vaultDb = new Dictionary<string, FileTarget>();
                SaveVaultDb();
            }
            try
            {
                using (StreamReader sw = new StreamReader(vaultDbPath))
                using (JsonTextReader reader = new JsonTextReader(sw))
                {
                    vaultDb = serializer.Deserialize<Dictionary<string, FileTarget>>(reader);
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("Unable to read vault Database\nApplication will exit to prevent accidental damage\n" + e.ToString());
                Application.Exit();
                return false;
            }
            return true;
        }

        public static bool SaveVaultDb()
        {
            JsonSerializer serializer = new JsonSerializer();

            try
            {
                using (StreamWriter sw = new StreamWriter(File.Create(vaultDbPath)))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, vaultDb);
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("Unable to write to vault Database\n" + e.ToString());
                VaultUpdated?.Invoke(null,null);
                return false;
            }


            VaultUpdated?.Invoke(null, null);
            return true;
        }

        public static void ResetVault()
        {
            initialized = false;

            if (File.Exists(vaultDbPath))
                File.Delete(vaultDbPath);

            if (Directory.Exists(vaultWorkingPath))
                Directory.Delete(vaultWorkingPath, true);

            if (Directory.Exists(vaultBackupsPath))
                Directory.Delete(vaultBackupsPath, true);

            vaultDb = null;

            Init();
        }
    }
}
