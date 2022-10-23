namespace RTCV.PluginHost
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using RTCV.NetCore;

    public static class Manager
    {
        static List<PluginInfo> plugins = new List<PluginInfo>();
        static Manager()
        {
            LoadFromParam();
        }

        private static void SaveToParam()
        {
            Params.SetParam("KNOWN_PLUGINS", string.Join("\n", plugins.Select(it => it.ToLine()).ToArray()));
        }

        private static void LoadFromParam()
        {
            string param = Params.ReadParam("KNOWN_PLUGINS");

            if (!string.IsNullOrWhiteSpace(param))
            {
                plugins.Clear();
                var lines = param.Split('\n').Distinct().ToList();

                plugins.AddRange(lines.Where(line => !string.IsNullOrWhiteSpace(line)).Select(it => PluginInfo.FromLine(it)));
            }
        }

        public static void Cleanup()
        {
            //as you can probably tell, this whole thing should be rewritten from the ground.
            //we will live with it for now

            //remove duplicates and broken
            var allplugins = plugins.ToList();
            Dictionary<string, PluginInfo> uniques = new Dictionary<string, PluginInfo>();
            foreach (var plugin in allplugins)
                uniques[plugin.Filename.ToUpper()] = plugin;

            plugins = uniques.Values.ToList();

            allplugins = plugins.ToList();
            foreach (var plugin in allplugins)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(plugin.Filename))
                    {
                        plugins.Remove(plugin);
                    }
                }
                catch { }
            }

            //delete deleted
            var deleted = plugins.Where(it => it.Status == "DELETE").ToList();
            foreach (var plugin in deleted)
            {
                var disabledFilename = plugin.GetDisabledFilename();


                try
                {
                    if (File.Exists(plugin.Filename))
                        File.Delete(plugin.Filename);

                    if (File.Exists(disabledFilename))
                        File.Delete(disabledFilename);
                }
                catch { }

                if (!File.Exists(plugin.Filename))
                {
                    plugins.Remove(plugin);
                }
            }

            //disable pending disabled
            var disabled = plugins.Where(it => it.Status == "DISABLED").ToList();
            foreach (var plugin in disabled)
            {
                var disabledFilename = plugin.GetDisabledFilename();

                try
                {
                    if (File.Exists(plugin.Filename))
                    {
                        if (File.Exists(disabledFilename))
                            File.Delete(disabledFilename);

                        File.Move(plugin.Filename, disabledFilename);
                    }

                    if (!File.Exists(disabledFilename))
                    {
                        plugins.Remove(plugin);
                    }
                }
                catch { }
            }

            //enabled previously disabled
            var enabled = plugins.Where(it => it.Status == "ENABLED").ToList();
            foreach (var plugin in enabled)
            {
                var disabledFilename = plugin.GetDisabledFilename();

                try
                {
                    if (!File.Exists(plugin.Filename))
                    {
                        if (File.Exists(disabledFilename))
                            File.Move(disabledFilename, plugin.Filename);
                        else
                            plugins.Remove(plugin);
                    }
                }
                catch { }
            }

            SaveToParam();
        }

        internal static void ReportExistingDisabled(IEnumerable<string> dirFiles)
        {
            if (dirFiles == null || dirFiles.Count() == 0)
                return;

            foreach (var location in dirFiles)
            {
                var expectedLocation = location.Replace(".disabled", "");

                var found = plugins.FirstOrDefault(it => it.Filename == expectedLocation);
                if (found == null)
                {
                    var p = new PluginInfo(expectedLocation);
                    plugins.Add(p);
                    p.MarkForDisabled(false);
                    SaveToParam();
                }
            }
        }

        public static PluginInfo[] GetPlugins()
        {
            return plugins.ToArray();
        }

        public static void ReportExisting(string location)
        {
            if (location == null)
                return;

            var found = plugins.FirstOrDefault(it => it.Filename == location);
            if (found == null)
            {
                plugins.Add(new PluginInfo(location));
                SaveToParam();
            }
        }

        public static void Save() => SaveToParam();

        //internal static void SetPluginDir(string v)
        //{
        //    pluginDir = v;
        //}
    }

    public class PluginInfo
    {
        public string Filename { get; set; } = "";
        public string Status { get; set; } = "ENABLED";

        public static PluginInfo FromLine(string line)
        {
            string[] parts = line.Split('|');
            var p = new PluginInfo(parts[1])
            {
                Status = parts[0]
            };
            return p;
        }

        public PluginInfo(string filename)
        {
            Filename = filename;
        }

        public string GetDisabledFilename()
        {
            return $"{Filename}.disabled";
        }

        public void MarkForDeletion(bool save = true)
        {
            Status = "DELETE";

            if (save)
                Manager.Save();
        }

        public void MarkForDisabled(bool save = true)
        {
            Status = "DISABLED";

            if (save)
            Manager.Save();
        }

        public void MarkForEnabled(bool save = true)
        {
            Status = "ENABLED";

            if (save)
                Manager.Save();
        }

        public string ToLine()
        {
            return $"{Status}|{Filename}";
        }

        public override string ToString()
        {
            var statusPart = "";
            if (Status == "DISABLED")
                statusPart = "[DISABLED] ";
            else if (Status == "DELETE")
                statusPart = "[DELETED] ";

            

            return $"{statusPart}{Path.GetFileName(Filename)}";
        }
    }
}
