using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace VanguardHook
{
    public class Root
    {
        public VSpecConfig VSpecConfig { get; set; }
        public List<MemoryDomainConfig> MemoryDomainConfig { get; set; }
    }

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
    }

    public class MemoryDomainConfig
    {
        public string Name { get; set; }
        public List<string> Profiles { get; set; }
        public bool BigEndian { get; set; }
        public string Size { get; set; }
        public int WordSize { get; set; }
        public string Offset { get; set; }
    }

    public class VanguardConfigReader
    {
        static readonly string config = File.ReadAllText(VanguardCore.emuDir + "VanguardSpec.Json");
        public static Root configFile = JsonConvert.DeserializeObject<Root>(config);
    }
    
}
