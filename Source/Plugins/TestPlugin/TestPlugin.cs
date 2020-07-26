namespace RTCV.Plugins
{
    using System;
    using System.ComponentModel.Composition;
    using RTCV.PluginHost;

    [Export(typeof(IPlugin))]
    public class TestPlugin : IPlugin
    {
        public string Name => "TestPlugin";
        public string Description => "Test plugin";
        public string Author => "Narry";

        #pragma warning disable SA1001 //Avoiding spaces between commas makes version string more clear
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
