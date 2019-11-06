using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RTCV.Launcher
{
    public class LauncherConf
    {
        public string launcherAssetLocation;
        public string launcherConfLocation;
        public string batchFilesLocation;
        public string version;

        public LauncherConfItem[] items = { };

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


    public class ExecutableCommand
    {
        public string DisplayName;
        public string FileName;
        public string Arguments;
        public bool WaitForExit;
        public int WaitForExitTimeout = Int32.MaxValue;
        [JsonConverter(typeof(StringEnumConverter))]
        public ProcessWindowStyle WindowStyle = ProcessWindowStyle.Normal;
        public List<ExecutableCommand> PreExecuteCommands = new List<ExecutableCommand>();
        public List<ExecutableCommand> PostExecuteCommands = new List<ExecutableCommand>();

        public ExecutableCommand(string displayName, string fileName, string arguments, bool waitForExit)
        {
            DisplayName = displayName;
            FileName = fileName;
            Arguments = arguments;
            WaitForExit = waitForExit;
        }

        public bool Execute(bool runPreExecute = true, bool runPostExecute = true)
        {
            bool success = true;

            if (runPreExecute)
            {
                foreach (var exe in PreExecuteCommands)
                {
                    if (!exe.Execute())
                        Console.WriteLine($"Executing PreExecuteCommand {exe.DisplayName} failed!");
                }
            }

            var psi = new ProcessStartInfo
            {
                WindowStyle = WindowStyle,
                Arguments = Arguments,
                UseShellExecute = true
            };

            if (File.Exists(Path.GetFullPath(FileName)))
            {
                psi.FileName = Path.GetFullPath(FileName);
                psi.WorkingDirectory = Path.GetDirectoryName(Path.GetFullPath(FileName)) ?? "";
            }
            else
                psi.FileName = FileName;

            try
            {
                var p = Process.Start(psi);

                if (WaitForExit)
                    success = p?.WaitForExit(WaitForExitTimeout) ?? false;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Failed to start process {psi.FileName}.\nException: {e.Message}");
            }

            if (runPostExecute)
            {
                foreach (var exe in PostExecuteCommands)
                {
                    if (!exe.Execute())
                        Console.WriteLine($"Executing PostExecuteCommand {exe.DisplayName} failed!");
                }
            }

            return success;
        }
    }

    public class LauncherConfJson
    {
        public string LauncherAssetLocation;
        public string LauncherConfLocation;
        public string VersionLocation;
        public string Version;

        public LauncherConfJsonItem[] Items = { };

        public LauncherConfJson(string _version)
        {
            Version = _version;
            LauncherAssetLocation = Path.Combine(MainForm.launcherDir, "VERSIONS", Version, "Launcher");
            LauncherConfLocation = Path.Combine(LauncherAssetLocation, "launcher.json");
            VersionLocation = Path.Combine(MainForm.launcherDir, "VERSIONS", Version);

            if (!File.Exists(LauncherConfLocation))
                return;

            Directory.SetCurrentDirectory(VersionLocation); //Move ourselves to this working directory


            var json = File.ReadAllText(LauncherConfLocation);
            Items = JsonConvert.DeserializeObject<LauncherConfJsonItem[]>(json);
        }
    }

    public class LauncherConfJsonItem
    {
        [JsonProperty]
        public readonly string FolderName;
        [JsonProperty]
        public readonly string ImageName;
        [JsonProperty]
        public readonly string DownloadVersion;
        [JsonProperty] 
        public readonly ReadOnlyDictionary<string, ExecutableCommand> ExecutableCommands;

        public LauncherConfJsonItem(string imageName, string downloadVersion, string folderName, ReadOnlyDictionary<string, ExecutableCommand> executableCommands)
        {
            ImageName = imageName;
            DownloadVersion = downloadVersion;
            FolderName = folderName;
            ExecutableCommands = executableCommands;
        }

        public bool Execute(bool runPreExecute = true, bool runPostExecute = true)
        {
            bool success = true;
            foreach (var e in ExecutableCommands.Values)
                success = (e.Execute(runPreExecute, runPostExecute) & success);
            return success;
        }

        public bool Execute(string key, bool runPreExecute = true, bool runPostExecute = true)
        {
            ExecutableCommands.TryGetValue(key, out ExecutableCommand e);
            return e?.Execute(runPreExecute, runPostExecute) ?? false;
        }
    }
}
