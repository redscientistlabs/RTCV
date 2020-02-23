using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;
using RTCV.PluginHost;

namespace RTCV.Plugins
{
    [Export(typeof(IPlugin))]
    public class TestPlugin : IPlugin
    {
        public string Name => "TestPlugin";
        public string Description => "Test plugin";
        public string Author => "Narry";
        public Version Version => new Version(0,0,1);
        public RTCSide SupportedSide => RTCSide.Both;

        public bool Start(RTCSide side)
        {
            Console.WriteLine("Test plugin loaded!");
            return true;
        }

        public bool Stop()
        {
            return true;
        }

        public void Dispose()
        {
        }
    }
}
