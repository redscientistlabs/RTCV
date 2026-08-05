namespace RTCV.UI.Modular
{
    using System.Windows.Forms;
    using RTCV.Common;

    public static class DefaultGrids
    {
        private static CanvasGrid _connectionStatus = null;
        public static CanvasGrid connectionStatus
        {
            get
            {
                if (_connectionStatus == null)
                {
                    var csGrid = new CanvasGrid(15, 13, "Connection Status");
                    Form csForm = S.GET<ConnectionStatusForm>();
                    csGrid.SetTileForm(csForm, 0, 0, 15, 13, false);
                    _connectionStatus = csGrid;
                }
                return _connectionStatus;
            }
        }

        public static SelectBoxForm DefaultTools = new SelectBoxForm(new ComponentForm[] {
                        S.GET<NoToolShortcuts>(),
                        //S.GET<MyListsForm>(),
                        //S.GET<MyVMDsForm>(),
                        //S.GET<MyPluginsForm>(),
                        S.GET<VmdPoolForm>(),
                        S.GET<VmdGenForm>(),
                        S.GET<VmdSimpleGenForm>(),
                        S.GET<VmdActForm>(),
                        S.GET<ListGenForm>(),
                        S.GET<VmdLimiterProfilerForm>(),
                        //S.GET<DomainAnalyticsForm>(),
                        S.GET<OpenToolsForm>(),
                        })
        {
            Text = "Advanced Tools and Plugins",
        };

        private static CanvasGrid _swapEmulator = null;
        public static CanvasGrid swapEmulator
        {
            get
            {
                if (_swapEmulator == null)
                {
                    var spGrid = new CanvasGrid(15, 13, "Swap Emulator");
                    Form spForm = S.GET<SwapEmulatorForm>();
                    spGrid.SetTileForm(spForm, 0, 0, 15, 13, false);
                    _swapEmulator = spGrid;
                }
                return _swapEmulator;
            }
        }

        private static CanvasGrid _engineConfig = null;

        public static CanvasGrid engineConfig
        {
            get
            {
                if (_engineConfig == null)
                {
                    var ecGrid = new CanvasGrid(15, 13, "Engine Config");

                    Form gpForm = S.GET<GeneralParametersForm>();
                    Form mdForm = S.GET<MemoryDomainsForm>();
                    Form ceForm = S.GET<CorruptionEngineForm>();

                    //UICore.mtForm = DefaultTools;

                    ecGrid.SetTileForm(gpForm, 0, 0, 5, 6, true, AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
                    ecGrid.SetTileForm(ceForm, 5, 0, 10, 6, true, AnchorStyles.Top | AnchorStyles.Right);
                    ecGrid.SetTileForm(mdForm, 0, 6, 5, 7, true, AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
                    ecGrid.SetTileForm(UICore.mtForm, 5, 6, 10, 7, true, AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right);

                    _engineConfig = ecGrid;
                }
                return _engineConfig;
            }
        }

        private static CanvasGrid _stockpilePlayer = null;
        public static CanvasGrid stockpilePlayer
        {
            get
            {
                if (_stockpilePlayer == null)
                {
                    var spGrid = new CanvasGrid(15, 13, "Stockpile Player");
                    Form spForm = S.GET<StockpilePlayerForm>();
                    spGrid.SetTileForm(spForm, 0, 0, 15, 13, false);
                    _stockpilePlayer = spGrid;
                }
                return _stockpilePlayer;
            }
        }

        private static CanvasGrid _settings = null;
        public static CanvasGrid settings
        {
            get
            {
                if (_settings == null)
                {
                    var stGrid = new CanvasGrid(15, 13, "Settings and Tools");

                    Form stForm = S.GET<SettingsForm>();
                    stGrid.SetTileForm(stForm, 0, 0, 15, 13, false);
                    _settings = stGrid;
                }
                return _settings;
            }
        }

        private static CanvasGrid _glitchHarvester = null;
        public static CanvasGrid glitchHarvester
        {
            get
            {
                if (_glitchHarvester == null)
                {
                    var ghGrid = new CanvasGrid(20, 13, 20, 10, "Glitch Harvester")
                    {
                        isResizable = true
                    };

                    Form ghbForm = S.GET<GlitchHarvesterBlastForm>();
                    Form ghiForm = S.GET<GlitchHarvesterIntensityForm>();
                    Form ssmForm = S.GET<SavestateManagerForm>();
                    Form shForm = S.GET<StashHistoryForm>();
                    Form spmForm = S.GET<StockpileManagerForm>();

                    ghGrid.SetTileForm(ghbForm, 0, 0, 4, 4, true);
                    ghGrid.SetTileForm(ssmForm, 0, 4, 4, 9, true, AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
                    ghGrid.SetTileForm(ghiForm, 4, 0, 5, 3, true);
                    ghGrid.SetTileForm(shForm, 4, 3, 5, 10, true, AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom);
                    ghGrid.SetTileForm(spmForm, 9, 0, 11, 13, true, AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom);

                    _glitchHarvester = ghGrid;
                }
                return _glitchHarvester;
            }
        }
    }
}
