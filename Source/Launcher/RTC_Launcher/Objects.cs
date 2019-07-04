using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTCV.Launcher
{
    public class LauncherConf
    {
        public string launcherAssetLocation;
        public string launcherConfLocation;
        public string batchFilesLocation;
        public string version;

        public LauncherConfItem[] items = new LauncherConfItem[] { };

        public LauncherConf(string _version)
        {
            version = _version;
            launcherAssetLocation = Path.Combine(MainForm.launcherDir, "VERSIONS" + Path.DirectorySeparatorChar + version + Path.DirectorySeparatorChar + "Launcher");
            launcherConfLocation = Path.Combine(launcherAssetLocation, "launcher.ini");
            batchFilesLocation = Path.Combine(MainForm.launcherDir, "VERSIONS", version);

            if (!File.Exists(launcherConfLocation))
                return;

            string[] confLines = File.ReadAllLines(launcherConfLocation);

            items = confLines.Select(it => new LauncherConfItem(this, it)).ToArray();
        }
    }
    public class SystemRequirements
    {
        public bool supports32Bit;
        public int minDotNetVersion;
        public string minDotNetVersionDownload;

        public SystemRequirements(string version)
        {
            string reqPath = Path.Combine(MainForm.launcherDir, "VERSIONS",version,"Launcher","requirements.ini");

            if (!File.Exists(reqPath))
                return;

            string[] confLines = File.ReadAllLines(reqPath);

            if (bool.TryParse(confLines[0], out bool b))
            {
                supports32Bit = b;
            }
            if (int.TryParse(confLines[1], out int v))
            {
                minDotNetVersion = v;
            }

            minDotNetVersionDownload = confLines[2];
        }
    }

    public class LauncherConfItem
    {
        public string[] lineItems;
        public string imageLocation;
        public string batchName;
        public string batchLocation;
        public string folderName;
        public string folderLocation;
        public string downloadVersion;
        public string line;


        public LauncherConfItem(LauncherConf lc, string _line)
        {
            line = _line;
            lineItems = _line.Split('|');
            imageLocation = Path.Combine(lc.launcherAssetLocation, lineItems[0]);
            batchName = lineItems[1];
            batchLocation = Path.Combine(lc.batchFilesLocation, batchName);
            folderName = lineItems[2];
            folderLocation = Path.Combine(lc.batchFilesLocation, folderName);
            downloadVersion = lineItems[3];

        }
    }
}
