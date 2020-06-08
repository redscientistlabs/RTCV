namespace RTCV.Plugins.ScriptHost
{
    using System;
    using System.ComponentModel.Composition;
    using NLog;
    using RTCV.Common;
    using RTCV.PluginHost;
    using RTCV.Plugins.ScriptHost.Controls;
    using RTCV.UI;

    [Export(typeof(IPlugin))]
    public class Loader : IPlugin
    {
        public string Name => "ScriptHost";
        public string Description => "A script host for the RTC allowing the running of C# code within the RTC";
        public string Author => "Narry";
        public Version Version => new Version(0, 0, 1);
        public RTCSide SupportedSide => RTCSide.Server;

        public bool Start(RTCSide side)
        {
            RTCV.Common.Logging.GlobalLogger.Info($"ScriptHost v{Version} initializing.");

            S.GET<RTC_OpenTools_Form>().RegisterTool("ScriptHost", "Open Script Host", () =>
            {
                S.SET(new ScriptHost());
                S.GET<ScriptHost>().Show();
            });

            RTCV.Common.Logging.GlobalLogger.Info($"ScriptHost v{Version} initialized.");

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
