namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Newtonsoft.Json;


    public static class Drive
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static string PackageDrive(string folderPath)
        {

            string sessionpath = Path.Combine(RtcCore.workingDir, "SESSION", $"DATA_{RtcCore.GetRandomKey()}.drive");

            if (File.Exists(sessionpath))
                File.Delete(sessionpath);

            ZipFile.CreateFromDirectory(folderPath, sessionpath);

            //generate a path for a file in session with a random key ending in .rom
            //zip contents of folderPath to the path determined for the rom file
            //outputs the path of the created file in the session folder

            return sessionpath;

        }

        public static string PackageCurrentDrive()
        {
            string drivepath = Path.Combine(RtcCore.workingDir, "DRIVE");
            string sessionpath = Path.Combine(RtcCore.workingDir, "SESSION", $"DATA_{RtcCore.GetRandomKey()}.drive");

            if (File.Exists(sessionpath))
                File.Delete(sessionpath);

            ZipFile.CreateFromDirectory(drivepath, sessionpath);

            //generate a path for a file in session with a random key ending in .rom
            //zip contents of folderPath to the path determined for the rom file
            //outputs the path of the created file in the session folder

            return sessionpath;

        }

        public static void SaveCurrentDriveAs()
        {
            var drivefile = PackageCurrentDrive();
            var fi = new FileInfo(drivefile);

            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                DefaultExt = "drive",
                Title = "Save Drive to File",
                Filter = "RTC Drive file|*.drive",
                FileName = fi.Name,
                RestoreDirectory = true
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var filename = saveFileDialog1.FileName;
                File.Move(drivefile, filename);
            }
        }

        public static string UnpackageDrive(string packagePath)
        {
            string sessionpath = Path.Combine(RtcCore.workingDir, "SESSION", $"DATA_{RtcCore.GetRandomKey()}.drive");
            string drivepath = Path.Combine(RtcCore.workingDir, "DRIVE");
            string autoexecpath = Path.Combine(drivepath, "autoexec.rom");

            var di = new DirectoryInfo(drivepath);

            if (di.Exists)
                di.Delete(true);

            di.Create();

            ZipFile.ExtractToDirectory(packagePath, drivepath);

            if(File.Exists(autoexecpath))
            {
                return File.ReadAllText(autoexecpath);
            }
            else
            {
                return null;
            }
        }


    }
}
