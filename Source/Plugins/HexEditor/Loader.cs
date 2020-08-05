namespace RTCV.Plugins.HexEditor
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Forms;
    using NLog;
    using RTCV.Common;
    using RTCV.PluginHost;
    using RTCV.NetCore;

    [Export(typeof(IPlugin))]
    public class Loader : IPlugin
    {
        public string Name => "Hex Editor";
        public string Description => "A hex editor for RTC implementations";
        public string Author => "Narry";
        public Version Version => new Version(0, 0, 1);
        public RTCSide SupportedSide => RTCSide.Both;

        public bool Start(RTCSide side)
        {
            RTCV.Common.Logging.GlobalLogger.Info($"{Name} v{Version} initializing.");


            if (side == RTCSide.Client)
            {
                var conn = new HexEditorConnector();
                LocalNetCoreRouter.registerEndpoint(conn, "HEXEDITOR");
            }

            RTCV.Common.Logging.GlobalLogger.Info($"{Name} v{Version} initialized.");

            return true;
        }

        public bool Stop()
        {
            if (!S.ISNULL<HexEditor>())
            {
                S.GET<HexEditor>()._hideOnClose = false;
                S.GET<HexEditor>().Close();
            }
            return true;
        }

        public void Dispose()
        {
        }
    }
}
