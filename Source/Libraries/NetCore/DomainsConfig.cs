namespace RTCV.NetCore
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;

    public static class DomainsConfig
    {
        public static string currentConfigSystemName = string.Empty;

        //Todo - Isolate this out
        public static string ConfigDir
        {
            get
            {
                if (AllSpec.CorruptCoreSpec?["RTCDIR"] is string rtcDir)
                {
                    return Path.Combine(rtcDir, "DOMAINSCONFIG", currentConfigSystemName);
                }

                //Check for the normal rtc dir
                if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "..", "RTC", "DOMAINSCONFIG", currentConfigSystemName)))
                {
                    return Path.Combine(Directory.GetCurrentDirectory(), "..", "RTC", "DOMAINSCONFIG", currentConfigSystemName);
                }

                //Fall back to our current dir
                var path = Path.Combine(Directory.GetCurrentDirectory(), "RTC", "DOMAINSCONFIG", currentConfigSystemName);
                Directory.CreateDirectory(path);
                return path;
            }
        }
        public static DomainConfigRoot GetConfig(string configFileName, bool domainsChanged, bool defaultConfig)
        {
            var config = new DomainConfigRoot();
            if (DomainsConfig.DoesConfigExist(configFileName, defaultConfig))
            {
                var fileName = $"{(defaultConfig ? "DEFAULT_" : "")}{configFileName}DOMAINS.cfg";

                var configFile = File.ReadAllText(Path.Combine(ConfigDir, fileName));
                var jsonString = JsonConvert.DeserializeObject<DomainConfigRoot>(configFile);

                foreach (string system in jsonString.DomainConfigSystem.Keys)
                {
                    config.DomainConfigSystem[system] = new DomainConfigSystem();
                    config.DomainConfigSystem[system].DomainConfig = jsonString.DomainConfigSystem[system].DomainConfig;
                }
            }
            return config;
        }

        public static void SetConfig(string configFileName, string data, bool defaultConfig)
        {
            if (data == null)
            {
                if (!DoesConfigExist(configFileName, defaultConfig))
                {
                    SetConfig(configFileName, "", defaultConfig);
                }
            }
            else
            {
                var fileName = $"{(defaultConfig ? "DEFAULT_" : "")}{configFileName}DOMAINS.cfg";
                File.WriteAllText(Path.Combine(ConfigDir, fileName), data);
            }
        }

        public static void RemoveConfig(string configFileName, bool defaultConfig)
        {
            var fileName = $"{(defaultConfig ? "DEFAULT_" : "")}{configFileName}DOMAINS.cfg";
            File.Delete(Path.Combine(ConfigDir, fileName));
        }

        private static bool GetOrCreateDirectory()
        {
            if (Directory.Exists(ConfigDir))
                return true;
            else
            {
                try
                {
                    Directory.CreateDirectory(ConfigDir);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool DoesConfigExist(string configFileName, bool defaultConfig)
        {
            if (GetOrCreateDirectory())
            {
                var fileName = $"{(defaultConfig ? "DEFAULT_" : "")}{configFileName}DOMAINS.cfg";
                return File.Exists(Path.Combine(ConfigDir, fileName));
            }

            return false;
        }
    }

    public class DomainConfigRoot
    {
        [JsonProperty("System")]
        public Dictionary<string, DomainConfigSystem> DomainConfigSystem { get; set; }

        public DomainConfigRoot()
        {
            DomainConfigSystem = new Dictionary<string, DomainConfigSystem>();
        }
    }

    public class DomainConfigSystem
    {
        [JsonProperty("Domain")]
        public Dictionary<string, DomainConfig> DomainConfig { get; set; }

        public DomainConfigSystem()
        {
            DomainConfig = new Dictionary<string, DomainConfig>();
        }
    }

    public class DomainConfig
    {
        public bool VISIBLE { get; set; }
        public bool AUTOSELECT { get; set; }

        public DomainConfig(bool visible, bool autoSelect)
        {
            VISIBLE = visible;
            AUTOSELECT = autoSelect;
        }
    }
}
