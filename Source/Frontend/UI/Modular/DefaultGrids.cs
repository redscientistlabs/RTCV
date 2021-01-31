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
                    var csGrid = new CanvasGrid(15, 12, "Connection Status");
                    Form csForm = S.GET<ConnectionStatusForm>();
                    csGrid.SetTileForm(csForm, 0, 0, 15, 12, false);
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
            popoutAllowed = false,
            Text = "Advanced Tools and Plugins",
        };

        private static CanvasGrid _engineConfig = null;
        public static CanvasGrid engineConfig
        {
            get
            {
                if (_engineConfig == null)
                {
                    var ecGrid = new CanvasGrid(15, 12, "Engine Config");

                    Form gpForm = S.GET<GeneralParametersForm>();
                    Form mdForm = S.GET<MemoryDomainsForm>();
                    Form ceForm = S.GET<CorruptionEngineForm>();

                    //UICore.mtForm = DefaultTools;

                    ecGrid.SetTileForm(gpForm, 0, 0, 5, 5, true);
                    ecGrid.SetTileForm(ceForm, 5, 0, 10, 5, true);
                    ecGrid.SetTileForm(mdForm, 0, 5, 5, 7, true);
                    ecGrid.SetTileForm(UICore.mtForm, 5, 5, 10, 7, true);

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
                    var spGrid = new CanvasGrid(15, 12, "Stockpile Player");
                    Form spForm = S.GET<StockpilePlayerForm>();
                    spGrid.SetTileForm(spForm, 0, 0, 15, 12, false);
                    _stockpilePlayer = spGrid;
                }
                return _stockpilePlayer;
            }
        }

        private static CanvasGrid _simpleMode = null;

        public static CanvasGrid simpleMode
        {
            get
            {
                if (_simpleMode == null)
                {
                    var smGrid = new CanvasGrid(15, 12, "Simple Mode");
                    Form smForm = S.GET<SimpleModeForm>();
                    smGrid.SetTileForm(smForm, 0, 0, 15, 12, false);
                    _simpleMode = smGrid;
                }
                return _simpleMode;
            }
        }

        private static CanvasGrid _settings = null;
        public static CanvasGrid settings
        {
            get
            {
                if (_settings == null)
                {
                    var stGrid = new CanvasGrid(15, 12, "Settings and Tools");

                    Form stForm = S.GET<SettingsForm>();
                    stGrid.SetTileForm(stForm, 0, 0, 15, 12, false);
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
                    var ghGrid = new CanvasGrid(20, 12, "Glitch Harvester")
                    {
                        isResizable = true
                    };

                    Form ghbForm = S.GET<GlitchHarvesterBlastForm>();
                    Form ghiForm = S.GET<GlitchHarvesterIntensityForm>();
                    Form ssmForm = S.GET<SavestateManagerForm>();
                    Form shForm = S.GET<StashHistoryForm>();
                    Form spmForm = S.GET<StockpileManagerForm>();

                    ghGrid.SetTileForm(ghbForm, 0, 0, 4, 4, true);
                    ghGrid.SetTileForm(ssmForm, 0, 4, 4, 8, true);
                    ghGrid.SetTileForm(ghiForm, 4, 0, 5, 3, true);
                    ghGrid.SetTileForm(shForm, 4, 3, 5, 9, true, (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom));
                    ghGrid.SetTileForm(spmForm, 9, 0, 11, 12, true, (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom));

                    _glitchHarvester = ghGrid;
                }
                return _glitchHarvester;
            }
        }
    }
}
