using RTCV.NetCore.StaticTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI.Modular
{
    public static class UI_DefaultGrids
    {
        public static CanvasGrid _connectionStatus = null;
        public static CanvasGrid connectionStatus
        {
            get
            {
                if (_connectionStatus == null)
                {
                    var csGrid = new CanvasGrid(15, 12, "Connection Status");
                    Form csForm = S.GET<RTC_ConnectionStatus_Form>();
                    csGrid.SetTileForm(csForm, 0, 0, 15, 12);
                    _connectionStatus = csGrid;
                }
                return _connectionStatus;
            }
        }

        public static CanvasGrid _engineConfig = null;
        public static CanvasGrid engineConfig
        {
            get
            {
                if (_engineConfig == null)
                {

                    var ecGrid = new CanvasGrid(15, 12, "Engine Config");

                    Form mtForm = new RTC_SelectBox_Form(new ComponentForm[] {
                        S.GET<RTC_VmdNoTool_Form>(),
                        S.GET<RTC_VmdPool_Form>(),
                        S.GET<RTC_VmdGen_Form>(),
                        S.GET<RTC_VmdAct_Form>(),
                        S.GET<RTC_ListGen_Form>(),
                        })
                    {
                        popoutAllowed = false,
                        Text = "Advanced Memory Tools",
                    };

                    Form gpForm = S.GET<RTC_GeneralParameters_Form>();
                    Form mdForm = S.GET<RTC_MemoryDomains_Form>();
                    Form ceForm = S.GET<RTC_CorruptionEngine_Form>();

                    ecGrid.SetTileForm(gpForm, 0, 0, 5, 5);
                    ecGrid.SetTileForm(ceForm, 5, 0, 10, 5);
                    ecGrid.SetTileForm(mdForm, 0, 5, 5, 7);
                    ecGrid.SetTileForm(mtForm, 5, 5, 10, 7);

                    _engineConfig = ecGrid;
                }
                return _engineConfig;
            }
        }

        public static CanvasGrid _stockpilePlayer = null;
        public static CanvasGrid stockpilePlayer
        {
            get
            {
                if (_connectionStatus == null)
                {
                    var spGrid = new CanvasGrid(15, 12, "Stockpile Player");
                    Form spForm = S.GET<RTC_StockpilePlayer_Form>();
                    spGrid.SetTileForm(spForm, 0, 0, 15, 12, false);
                    _stockpilePlayer = spGrid;
                }
                return _stockpilePlayer;
            }
        }

        public static CanvasGrid _settings = null;
        public static CanvasGrid settings
        {
            get
            {
                if (_connectionStatus == null)
                {
                    var stGrid = new CanvasGrid(15, 12, "Settings and Tools");

                    Form stForm = S.GET<RTC_Settings_Form>();
                    stGrid.SetTileForm(stForm, 0, 0, 15, 12, false);
                    _settings = stGrid;
                }
                return _settings;
            }
        }

    }
}
