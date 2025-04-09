using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace VanguardHook
{
    // Root object of config file
    public class ConfigRoot
    {
        public VSpecConfig VSpecConfig { get; set; }
        public List<MemoryDomainConfig> MemoryDomainConfig { get; set; }
    }

    // Vanguard configuration settings from config file

    public class VSpecConfig
    {
        public string EmuEXE { get; set; }
        public string NAME { get; set; }
        public string PROFILE { get; set; }
        public int OVERRIDE_DEFAULTMAXINTENSITY { get; set; }
        public bool SUPPORTS_RENDERING { get; set; }
        public bool SUPPORTS_CONFIG_MANAGEMENT { get; set; }
        public bool SUPPORTS_CONFIG_HANDOFF { get; set; }
        public bool SUPPORTS_KILLSWITCH { get; set; }
        public bool SUPPORTS_REALTIME { get; set; }
        public bool SUPPORTS_SAVESTATES { get; set; }
        public bool SUPPORTS_REFERENCES { get; set; }
        public bool SUPPORTS_MIXED_STOCKPILE { get; set; }
        public bool CORE_DISKBASED { get; set; }
    }

    // Memory domains used by emulator from config file
    public class MemoryDomainConfig
    {
        public string Name { get; set; }
        public string[] Profiles { get; set; }
        public bool BigEndian { get; set; }
        public string Size { get; set; }
        public int WordSize { get; set; }
        public string Offset { get; set; }
        public int PeekPokeSel { get; set; }
    }

    public class VanguardConfigReader
    {
        public static ConfigRoot configFile = GetConfigFile();

        // Read the config file and parse the data into the class
        public static ConfigRoot GetConfigFile()
        {
            string configErrMessage = "Vanguard could not find the target Emulator's config file at " + EmuDirectory.emuDir + ". Try reinstalling " +
                                      "and launching Vanguard.\n\nIf you keep getting this message, poke " +
                                      "the RTC devs for help (Discord is in the launcher).";

            if (!File.Exists(EmuDirectory.emuDir + "VanguardConfig.Json"))
            {
                MessageBox.Show(configErrMessage,
                "RTC Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Error,
                                     MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                
                Environment.Exit(-1);
                return null;
            }

            string config = File.ReadAllText(EmuDirectory.emuDir + "VanguardConfig.Json");
            ConfigRoot configFile = JsonConvert.DeserializeObject<ConfigRoot>(config);
            return configFile;
        }
    }

    public class BlacklistedDomainsConfig
    {
        public string[] MEMORYDOMAINS_BLACKLISTEDDOMAINS { get; set; }
    }


    public class VanguardBlacklistedDomains
    {
        public static BlacklistedDomainsConfig domains = GetConfigFile();

        // Read the config file and parse the blacklisted domains into the class
        public static BlacklistedDomainsConfig GetConfigFile()
        {
            if (!File.Exists(EmuDirectory.emuDir + "VanguardBlacklistedDomains.Json"))
            {
                MessageBox.Show(
                "Vanguard could not find the target Emulator's blacklisted domains file at " + EmuDirectory.emuDir + ". Try reinstalling " +
                "and launching Vanguard.\n\nIf you keep getting this message, poke " +
                "the RTC devs for help (Discord is in the launcher).",
                "RTC Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Error,
                                     MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                Environment.Exit(-1);
                return null;
            }

            string config = File.ReadAllText(EmuDirectory.emuDir + "VanguardBlacklistedDomains.Json");
            BlacklistedDomainsConfig domains = JsonConvert.DeserializeObject<BlacklistedDomainsConfig>(config);
            return domains;
        }
    }
}
