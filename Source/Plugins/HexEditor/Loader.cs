using System;
using System.ComponentModel.Composition;
using NLog;
using RTCV.Common;
using RTCV.PluginHost;
using RTCV.NetCore;
using System.Windows.Forms;

namespace RTCV.Plugins.HexEditor
{
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

            if (!Params.IsParamSet("HEXEDITOR_WARNING"))
            {
                MessageBox.Show("While the hex editor works fine for most people, on some systems it'll cause random crashes.\n" +
                    "If you find yourself experiencing weird emulator issues, try disabling this plugin.\n\n" +
                    "This message will only appear once.");
                Params.SetParam("HEXEDITOR_WARNING");
            }

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
                S.GET<HexEditor>().HideOnClose = false;
                S.GET<HexEditor>().Close();
            }
            return true;
        }

        public void Dispose()
        {
        }
    }
}
