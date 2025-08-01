using System.Threading.Tasks;

namespace RTCV.UI
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.NetCore.Enums;
    using RTCV.Common;
    using RTCV.UI.Extensions;
    using RTCV.UI.Modular;

    #pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    public partial class CoreForm : ColorizedForm
    {
        //This form traps events and forwards them.
        //It contains the single CanvasForm instance.

        internal static CoreForm thisForm;
        internal static CanvasForm cfForm;

        // Contains the previous grid in the main form and the previous external grid, in order of appearance
        public CanvasGrid[] PreviousGrids { get; } = new CanvasGrid[2];
        public int ExternalIndex = -1;

        //Values used for padding and scaling properly in high dpi
        internal static int xPadding { get; private set; }
        private static int corePadding; // height of the top bar
        internal static int yPadding { get; private set; }

        private Panel pnLockSidebar = null;

        public bool AutoCorrupt
        {
            get => RtcCore.AutoCorrupt;
            set
            {
                if (value)
                {
                    btnAutoCorrupt.Text = " Stop Auto-Corrupt";
                    S.GET<SimpleModeForm>().btnAutoCorrupt.Text = " Stop Auto-Corrupt";
                }
                else
                {
                    btnAutoCorrupt.Text = " Start Auto-Corrupt";
                    S.GET<SimpleModeForm>().btnAutoCorrupt.Text = " Start Auto-Corrupt";
                }

                RtcCore.AutoCorrupt = value;
            }
        }

        public CoreForm()
        {
            InitializeComponent();
            thisForm = this;
            this.FormClosing += OnFormClosing;

            cfForm = new CanvasForm
            {
                TopLevel = false,
                Dock = DockStyle.Fill,
            };

            this.Controls.Add(cfForm);
            cfForm.Location = new Point(0, pnSideBar.Size.Height);
            cfForm.Show();
            cfForm.BringToFront();

            //For Horizontal tab-style menu in coreform
            //xPadding = (Width - cfForm.Width);
            //coreYPadding = pnTopBar.Height;
            //yPadding = (Height - cfForm.Height) - coreYPadding;

            //For Vertical tab-style menu in coreform
            yPadding = (Height - cfForm.Height);
            corePadding = pnSideBar.Width;
            xPadding = (Width - cfForm.Width) - corePadding;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!UICore.isClosing)
            {
                if ((S.GET<StockpileManagerForm>().UnsavedEdits && DialogResult.No == MessageBox.Show("You have unsaved edits in the Glitch Harvester Stockpile. \n\n Are you sure you want to close RTC without saving?", "Unsaved edits in Stockpile", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly))
                 || (S.GET<SavestateManagerForm>().UnsavedEdits && DialogResult.No == MessageBox.Show("You have unsaved edits in the Glitch Harvester Savestate Manager. \n\n Are you sure you want to close RTC without saving?", "Unsaved edits in Savestate Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)))
                {
                    e.Cancel = true;
                    return;
                }
            }

            UICore.isClosing = true;

            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.EventShutdown, true);
            LocalNetCoreRouter.Route(NetCore.Endpoints.Vanguard, NetCore.Commands.Remote.EventCloseEmulator);

            //Sleep to make sure the message was sent since we don't handshake it
            System.Threading.Thread.Sleep(500);

            //Clean out the working folders
            if (!RtcCore.DontCleanSavestatesOnQuit)
            {
                Stockpile.EmptyFolder("WORKING");
            }

            //Shut down vanguard
            VanguardImplementation.Shutdown();

            //Indicate that RTC shut down cleanly
            Params.RemoveParam("RTC_AWAKE");
            
            //Signal the quit
            //Application.Exit();
            Environment.Exit(-1);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            btnLogo.Text = "RTCV " + RtcCore.RtcVersion;

            if (!Params.IsParamSet("DISCLAIMER_READ"))
            {
                string disclaimer = @"Welcome to the Real-Time Corruptor
Version [ver]

Disclaimer:
This program comes with absolutely ZERO warranty.
You may use it at your own risk.

RTC is distributed under an MIT License.
Detailed information about other licenses available in the
respective Vanguard implementation folders and the RTC folder.

Known facts(and warnings):
- Can generate incredible amounts of flashing and noise
- Can cause windows to BSOD (extremely rarely)
- Can write a significant amount of data to your hard drive depending on usage

This message only appears once.";

                string disclaimerPath = Path.Combine(RtcCore.RtcDir, "LICENSES", "DISCLAIMER.TXT");

                //Use the text file if it exists
                if (File.Exists(disclaimerPath))
                {
                    disclaimer = File.ReadAllText(disclaimerPath);
                }

                S.GET<IntroForm>().DisplayRtcvDisclaimer(disclaimer.Replace("[ver]", RtcCore.RtcVersion));

                Params.SetParam("DISCLAIMER_READ");

                if (S.GET<IntroForm>().selection == IntroAction.SIMPLEMODE)
                {
                    Params.SetParam("SIMPLE_MODE"); //Set RTC in Simple Mode

                    if (VanguardImplementation.connector.netConn.status == NetworkStatus.CONNECTED)
                    {
                        DefaultGrids.simpleMode.LoadToMain();
                        SimpleModeForm smForm = S.GET<SimpleModeForm>();
                        smForm.EnteringSimpleMode();
                    }
                }

                Params.SetParam("COMPRESS_STOCKPILE"); //Default param
                Params.SetParam("COMPRESS_SAVESTATES"); //Default param
                Params.SetParam("INCLUDE_REFERENCED_FILES"); //Default param
                Params.SetParam("LOAD_STASH_ON_ARROW_CLICK"); //Default param
                Params.SetParam("RASTERIZE_VMD_UPON_STOCKPILING"); //Default param
                Params.SetParam("AUTOSAVE_INTERVAL", "300"); //Default param (5 minutes in seconds)
                Params.SetParam("AUTOSAVE_MAX_SIZE", "2.5"); //Default param (2.5 GiB)
            }
            else if (Params.IsParamSet("RTC_AWAKE") && !Debugger.IsAttached)
            {
                if (Params.IsParamSet("AUTOSAVE"))
                {
                    DialogResult showAutoSaves = MessageBox.Show(
                        "It looks like RTC crashed or was closed improperly last time.\n" +
                        $"If you lost any unsaved work, auto-save backups may be available in {RtcCore.AutoSaveDir}.\n" +
                        "Would you like to go there now?",
                        "RTC did not shut down cleanly", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    if (showAutoSaves == DialogResult.Yes)
                    {
                        Process.Start(RtcCore.AutoSaveDir);
                    }
                }
                else
                {
                    MessageBox.Show(
                        "It looks like RTC crashed or was closed improperly last time.\n" +
                        $"If you lost any work, you can enable auto-save in the settings in case this happens again.",
                        "RTC did not shut down cleanly", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            else
            {
                Params.SetParam("RTC_AWAKE");
            }

            //RtcCore.DownloadProblematicProcesses();
            //DefaultGrids.engineConfig.LoadToMain();
        }

        public void SetSize(int width, int height, int minWidth, int minHeight)
        {
            //this.MinimumSize = new Size(x + xPadding, y + yPadding + coreYPadding); //For Horizontal tab-style menu in coreform
            this.MinimumSize = new Size(minWidth + xPadding + corePadding, minHeight + yPadding); //For Vertical tab-style menu in coreform
            //this.Size = new Size(x + xPadding, y + yPadding + coreYPadding); //For Horizontal tab-style menu in coreform
            this.Size = new Size(width + xPadding + corePadding, height + yPadding); //For Vertical tab-style menu in coreform
        }

        private void OnResizeBegin(object sender, EventArgs e)
        {
            //Sends event to SubForm
            if (cfForm.spForm != null)
            {
                cfForm.spForm.OnParentResizeBegin();
            }
        }

        private void OnResizeEnd(object sender, EventArgs e)
        {
            //Sends event to SubForm
            if (cfForm.spForm != null)
            {
                cfForm.spForm?.OnParentResizeEnd();
            }
        }

        private FormWindowState? LastWindowState = null;

        private void OnResize(object sender, EventArgs e)
        {
            // When window state changes
            if (WindowState != LastWindowState)
            {
                /*
                if (WindowState == FormWindowState.Maximized)
                {
                    // Maximized!
                }
                if (WindowState == FormWindowState.Normal)
                {
                    // Restored!
                }
                */

                if (cfForm.spForm != null)
                {
                    cfForm.spForm?.OnParentResizeEnd();
                }

                LastWindowState = WindowState;
            }
        }

        private void OpenTestSubform(object sender, EventArgs e)
        {
            //test button, loads a dummy form in SubForm mode

            var f = S.GET<ComponentFormSubForm>();
            cfForm.OpenSubForm(f, true);
        }

        private void OpenCustomLayout(object sender, MouseEventArgs e)
        {
            var layouts = CanvasGrid.GetEnabledCustomLayouts();
            if (layouts.Length == 0)
            {
                Point locate = e.GetMouseLocation(sender);
                new ContextMenuBuilder().AddText("No Custom Layouts Loaded", false).Build().Show(this, locate);
            }
            else if (layouts.Length == 1)
            {
                CanvasGrid.LoadCustomLayout(layouts.First().FullName);
            }
            else
            {
                Point locate = e.GetMouseLocation(sender);

                var builder = new ContextMenuBuilder();
                foreach (var layout in layouts)
                    builder.AddItem($"Load {layout.Name.Replace(layout.Extension, "")}", (ob, ev) => CanvasGrid.LoadCustomLayout(layout.FullName));

                builder.Build().Show(this, locate);
            }
        }

        private void PrepareLockSideBar()
        {
            if (pnLockSidebar == null || !pnSideBar.Controls.Contains(pnLockSidebar))
            {
                pnLockSidebar = new Panel
                {
                    Size = pnSideBar.Size,
                    Location = new Point(0, 0),
                    BackColor = Colors.Dark4Color,
                    Visible = false,
                };
                pnSideBar.Controls.Add(pnLockSidebar);
                pnLockSidebar.BringToFront();
            }
        }

        public void LockSideBar()
        {
            PrepareLockSideBar();

            Bitmap bmp = pnSideBar.getFormScreenShot();
            bmp.Tint(Color.FromArgb(0xF0, Colors.Dark4Color));
            pnLockSidebar.BackgroundImage = bmp;
            pnLockSidebar.Visible = true;
        }

        public void UnlockSideBar()
        {
            if (pnLockSidebar != null)
            {
                pnLockSidebar.Visible = false;
            }
        }

        public void OpenEngineConfig(object sender, EventArgs e)
        {
            if (Params.IsParamSet("SIMPLE_MODE"))
            {
                DefaultGrids.simpleMode.LoadToMain();
                SimpleModeForm smForm = S.GET<SimpleModeForm>();
                smForm.EnteringSimpleMode();
            }
            else
            {
                DefaultGrids.engineConfig.LoadToMain();
            }
        }

        private void OnAutoKillSwitchButtonMouseHover(object sender, EventArgs e)
        {
            //lbAks.ForeColor = Color.FromArgb(32, 32, 32);
            //pnAutoKillSwitch.BackColor = BackColor.ChangeColorBrightness(-0.10f);
            pnAutoKillSwitch.BackColor = Color.Red.ChangeColorBrightness(-0.80f);
        }

        private void OnAutoKillSwitchButtonMouseLeave(object sender, EventArgs e)
        {
            //lbAks.ForeColor = Color.White;
            pnAutoKillSwitch.BackColor = Color.Transparent;
        }

        public void OpenGlitchHarvester(object sender, EventArgs e)
        {
            if (Params.IsParamSet("GH_OPEN_MAIN"))
            {
                DefaultGrids.glitchHarvester.LoadToMain();
            }
            else
            {
                DefaultGrids.glitchHarvester.LoadToNewWindow("Glitch Harvester");
            }
            
            pnGlitchHarvesterOpen.Visible = true;
        }

        public void StartAutoCorrupt(object sender, EventArgs e)
        {
            if (btnAutoCorrupt.ForeColor == Color.Silver)
            {
                return;
            }

            AutoCorrupt = !AutoCorrupt;
            if (AutoCorrupt)
            {
                AllSpec.CorruptCoreSpec.Update(RTCSPEC.STEP_RUNBEFORE, true);
            }
        }

        public void ManualBlast(object sender, EventArgs e)
        {
            if (AllSpec.VanguardSpec[VSPEC.REPLACE_MANUALBLAST_WITH_GHCORRUPT] != null)
            {
                S.GET<GlitchHarvesterBlastForm>().Corrupt(sender, e);
            }
            else
            {
                LocalNetCoreRouter.Route(Endpoints.CorruptCore, NetCore.Commands.Basic.ManualBlast, true);
            }
        }

        public void SetDefaultGrid(CanvasGrid grid, bool isExternal = false)
        {
            if (this.PreviousGrids[1] == grid)
            {
                return;
            }

            if (isExternal)
            {   
                // we don't want to store two external grids in the history, that wouldn't make sense
                if (this.ExternalIndex < 1)
                {
                    this.PreviousGrids[0] = this.PreviousGrids[1];
                }
                else // if the last external form had any modules that should also be in the main form's grid, they need to be put back
                {
                    this.PreviousGrids[0].LoadToMain();
                }
            }

            this.PreviousGrids[1] = grid;
            if (isExternal)
            {
                this.ExternalIndex = 1;
            }
            else
            {
                this.ExternalIndex--; //Loading a custom layout 2.1 billion times crashes RTCV
            }
        }

        private void OnStartEasyModeClick(object sender, MouseEventArgs e)
        {
            bool simpleModeVisible = S.GET<SimpleModeForm>().Visible;

            Point locate = e.GetMouseLocation(sender);

            new ContextMenuBuilder()
                .AddItem("Switch to Simple Mode", (ob, ev) =>
                {
                    if ((AllSpec.VanguardSpec[VSPEC.NAME] as string)?.ToUpper().Contains("SPEC") ?? false)
                    {
                        MessageBox.Show("Simple Mode is currently only supported on Vanguard implementations.");
                        return;
                    }
                    
                    DefaultGrids.simpleMode.LoadToMain();
                    SimpleModeForm smForm = S.GET<SimpleModeForm>();
                    smForm.EnteringSimpleMode();
                }, !simpleModeVisible)
                .AddItem("Start Auto-Corrupt With Recommended Settings for Loaded Game", (ob, ev)
                    => StartEasyMode(true),
                    (bool)AllSpec.VanguardSpec[VSPEC.SUPPORTS_SAVESTATES] && !simpleModeVisible)
                .AddSeparator()
                .AddItem("Watch a Tutorial Video", (ob, ev) => Process.Start("http://rtctutorialvideo.r5x.cc/"))
                .AddItem("Open the Online Wiki", (ob, ev) => Process.Start("https://corrupt.wiki/"))
                .Build()
                .Show(this, locate);
        }

        private void OpenStockpilePlayer(object sender, EventArgs e)
        {
            DefaultGrids.stockpilePlayer.LoadToMain();
        }

        private int settingsRightClickTimer = 0;
        private Timer testErrorTimer = null;

        internal void OpenSettings(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (testErrorTimer == null && !Params.IsParamSet("DEBUG_FETCHMODE"))
                {
                    testErrorTimer = new Timer
                    {
                        Interval = 3000
                    };
                    testErrorTimer.Tick += TestErrorTimerTick;
                    testErrorTimer.Start();
                }

                settingsRightClickTimer++;

                Point locate = e.GetMouseLocation(sender);
                
                new ContextMenuBuilder()
                    .If(Params.IsParamSet("DEBUG_FETCHMODE") || settingsRightClickTimer > 2)
                    .AddItem("Open Debug Window", (ob, ev) => ForceCloudDebug())
                    .EndIf().Build().Show(this, locate);
            }
            else if (e.Button == MouseButtons.Left)
            {
                DefaultGrids.settings.LoadToMain();
            }
        }

        private void OnAutoKillSwitchClick(object sender, MouseEventArgs e)
        {
            //needed anymore?
            S.GET<CoreForm>().OnLogoClick(sender, e);

            S.GET<CoreForm>().pbAutoKillSwitchTimeout.Value = S.GET<CoreForm>().pbAutoKillSwitchTimeout.Maximum;
            AutoKillSwitch.ShouldKillswitchFire = true;

            //refactor this to not use string once old coreform is dead
            AutoKillSwitch.KillEmulator(true);
        }

        private void OnAutoKillSwitchCheckboxChanged(object sender, EventArgs e)
        {
            pbAutoKillSwitchTimeout.Visible = cbUseAutoKillSwitch.Checked;
            AutoKillSwitch.Enabled = cbUseAutoKillSwitch.Checked;
        }

        private void ToggleGameProtection(object sender, MouseEventArgs e)
        {
            cbUseGameProtection.Checked = !cbUseGameProtection.Checked;
        }

        private void OnUseGameProtectionCheckboxChanged(object sender, EventArgs e)
        {
            if (cbUseGameProtection.Checked)
            {
                GameProtection.Start();
            }
            else
            {
                GameProtection.Stop();
                btnGpJumpBack.Visible = false;
                btnGpJumpNow.Visible = false;
            }
        }

        public void OnGameProtectionBack(object sender, EventArgs e)
        {
            try
            {
                btnGpJumpBack.Visible = false;

                if (!GameProtection.HasBackedUpStates)
                {
                    return;
                }

                GameProtection.PopAndRunBackupState();
                GameProtection.Reset(false);
            }
            finally
            {
                if (GameProtection.HasBackedUpStates)
                {
                    btnGpJumpBack.Visible = true;
                }
            }
        }

        public void OnGameProtectionNow(object sender, EventArgs e)
        {
            try
            {
                btnGpJumpNow.Visible = false;

                //Do this to prevent any potential race
                var sk = StockpileManagerUISide.BackupedState;

                if (sk != null)
                {
                    GameProtection.AddBackupState(sk);
                    sk.Run();
                }

                GameProtection.Reset(false);
            }
            finally
            {
                btnGpJumpNow.Visible = true;
            }
        }

        private void OnLogoClick(object sender, EventArgs e)
        {
            DefaultGrids.connectionStatus.LoadToMain(setDefault: false);
        }

        private static void StartEasyMode(bool useTemplate)
        {
            //if (RTC_NetcoreImplementation.isStandaloneUI && !S.GET<RTC_Core_Form>().cbUseGameProtection.Checked)
            S.GET<CoreForm>().cbUseGameProtection.Checked = true;

            if (useTemplate)
            {
                //Put Console templates HERE
                string thisSystem = (string)AllSpec.VanguardSpec[VSPEC.SYSTEM];

                switch (thisSystem)
                {
                    case "NES":     //Nintendo Entertainment system
                        SetEngineByName("Nightmare Engine");
                        S.GET<GeneralParametersForm>().multiTB_Intensity.Value = 2;
                        S.GET<GeneralParametersForm>().multiTB_ErrorDelay.Value = 1;
                        break;

                    case "GB":      //Gameboy
                    case "GBC":     //Gameboy Color
                        SetEngineByName("Nightmare Engine");
                        S.GET<GeneralParametersForm>().multiTB_Intensity.Value = 1;
                        S.GET<GeneralParametersForm>().multiTB_ErrorDelay.Value = 4;
                        break;

                    case "SNES":    //Super Nintendo
                        SetEngineByName("Nightmare Engine");
                        S.GET<GeneralParametersForm>().multiTB_Intensity.Value = 1;
                        S.GET<GeneralParametersForm>().multiTB_ErrorDelay.Value = 2;
                        break;

                    case "GBA":     //Gameboy Advance
                        SetEngineByName("Nightmare Engine");
                        S.GET<GeneralParametersForm>().multiTB_Intensity.Value = 1;
                        S.GET<GeneralParametersForm>().multiTB_ErrorDelay.Value = 1;
                        break;

                    case "N64":     //Nintendo 64
                        SetEngineByName("Vector Engine");
                        S.GET<GeneralParametersForm>().multiTB_Intensity.Value = 15;
                        S.GET<GeneralParametersForm>().multiTB_ErrorDelay.Value = 1;
                        S.GET<CorruptionEngineForm>().SetVectorToExtendedExtended();
                        break;

                    case "Dolphin":     //GC/Wii
                        SetEngineByName("Vector Engine");
                        S.GET<GeneralParametersForm>().multiTB_Intensity.Value = 35;
                        S.GET<GeneralParametersForm>().multiTB_ErrorDelay.Value = 1;
                        S.GET<CorruptionEngineForm>().SetVectorToExtendedExtended();
                        break;

                    case "SG":      //Sega SG-1000
                    case "GG":      //Sega GameGear
                    case "SMS":     //Sega Master System
                    case "GEN":     //Sega Genesis and CD
                    case "PCE":     //PC-Engine / Turbo Grafx
                    case "PSX":     //Sony Playstation 1
                    case "A26":     //Atari 2600
                    case "A78":     //Atari 7800
                    case "LYNX":    //Atari Lynx
                    case "INTV":    //Intellivision
                    case "PCECD":   //related to PC-Engine / Turbo Grafx
                    case "SGX":     //related to PC-Engine / Turbo Grafx
                    case "TI83":    //Ti-83 Calculator
                    case "WSWAN":   //Wonderswan
                    case "C64":     //Commodore 64
                    case "Coleco":  //Colecovision
                    case "SGB":     //Super Gameboy
                    case "SAT":     //Sega Saturn
                    case "DGB":
                    default:
                        MessageBox.Show($"WARNING: No Easy-Mode template was made for this system ({thisSystem}). Please configure it manually and use the current settings.");
                        return;

                        //TODO: Add more domains for systems like gamegear, atari, turbo graphx
                }

                //Force control to commit new values
                S.GET<GeneralParametersForm>().multiTB_Intensity.OnValueChanged(null);
                S.GET<GeneralParametersForm>().multiTB_ErrorDelay.OnValueChanged(null);
            }

            S.GET<CoreForm>().AutoCorrupt = true;
        }

        public static void SetEngineByName(string name)
        {
            //Selects an engine from a given string name

            for (int i = 0; i < S.GET<CorruptionEngineForm>().cbSelectedEngine.Items.Count; i++)
            {
                if (S.GET<CorruptionEngineForm>().cbSelectedEngine.Items[i].ToString() == name)
                {
                    S.GET<CorruptionEngineForm>().cbSelectedEngine.SelectedIndex = i;
                    break;
                }
            }
        }

        private static void BlastRawStash()
        {
            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Basic.ManualBlast, true);
            S.GET<GlitchHarvesterBlastForm>().SendRawToStash(null, null);
        }

        public void btnManualBlast_MouseDown(object sender, MouseEventArgs e)
        {
            e ??= new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);
            if (e.Button != MouseButtons.Right) return;
            
            Point locate = e.GetMouseLocation(sender);
            
            new ContextMenuBuilder()
                .AddItem("Blast + Send RAW to Stash (Glitch Harvester)", (ob, ev) => BlastRawStash())
                .Build().Show(this, locate);
        }

        public static void ForceCloudDebug()
        {
            //SECRET CRASH DONT TELL ANYONE
            //Purpose: Testing debug window
            var image = @"
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⣿⣿⣿⣿⠟⠋⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⢿⣿⣿⣿⣿⣿⣿⣿⣿⠟⣿⣿⣿⣿⢻⣿⣿⣿⣯⣤⣶⣿⣿⡿⠿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣤⣄⣙⣿⣿⣿⣿⣿⣿⣿⣦⣿⣿⣿⠁⢸⣿⣿⡿⣿⣿⣿⣿⣿⣿⣦⡀⢀⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣍⡉⢻⣿⣿⣿⡿⠃⠀⡞⣹⡿⠁⠙⣿⣿⣿⣿⣿⣿⣿⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣌⠻⡛⠉⠙⠛⠚⠉⠉⣽⠁⠀⢠⣷⡏⠀⠀⠀⠹⣿⡿⠿⢛⣫⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠻⣿⣿⣿⡾⡄⠈⠳⠴⠒⠛⠛⠉⠁⠀⠀⠈⠙⠓⢤⡀⠀⠀⠘⣶⣿⡿⠿⠟⠛⢛⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣶⣿⣿⣿⣿⣿⡘⣄⠀⠀⣤⣄⠀⠀⠀⠀⠀⢀⡤⠀⠀⠈⠉⠉⠉⠁⠀⠀⠀⣠⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣭⣽⡷⠁⠀⣰⣿⣿⣏⡳⠀⢀⣴⣿⣧⠀⠀⠀⠀⠀⠀⠀⢀⣴⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣭⣍⣉⠉⠉⠉⠉⠉⠁⠀⠀⠉⠉⠉⢉⣁⣀⠉⠉⠉⠉⠀⠀⠀⠀⠀⡤⠚⢻⣿⠟⢻⣿⣟⠿⢿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣶⣶⠤⣄⠀⠀⠀⠀⠀⠙⠁⠀⠀⠀⠀⠀⠀⢀⡤⠋⠀⣠⠜⠁⠀⠈⣿⣿⣴⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⠿⠛⠉⠁⠀⣿⡏⠀⣾⠀⣠⡆⠀⠀⠀⣀⡀⠀⠀⢠⣶⣿⣧⣤⣤⣴⠁⢀⠀⠀⠀⠘⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⢟⣁⡤⠀⠀⠀⢀⣿⣷⣶⣧⡞⠁⢣⠀⣀⠜⠁⠱⡀⢀⢮⣾⣿⣿⠟⠉⢹⠀⣾⡄⢠⣧⠀⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣷⣿⣿⢁⣴⠇⢠⣿⣿⠿⣿⣿⣿⣶⣾⣜⣡⣴⠆⠀⢻⣿⡿⠟⠉⠀⠀⢀⣼⣼⣙⡇⣼⣿⣧⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣧⣿⣿⢠⣾⣿⣿⣾⣿⣿⣿⣿⡿⠻⡇⠀⣀⣀⠤⣤⢶⣄⡀⢀⣴⣿⣿⣿⣿⣿⣿⣷⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⣿⣛⣉⣤⣴⣶⣿⡀⠙⠦⡰⠃⠀⠀⠉⢻⡿⣿⣿⣿⣿⣿⣿⣟⠛⠿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⣿⣿⣿⣷⣤⣶⣄⣀⠀⠀⠀⠈⢿⣿⣿⣿⡘⠿⣿⣿⣷⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣏⠉⠀⣰⣿⣿⣿⣿⣋⣀⣉⣛⠻⠆⠀⠀⠀⠻⠿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣼⣿⣿⣿⡿⢿⣿⣿⣿⣿⣿⣷⣶⣶⣦⣤⣤⣤⣭⣿⣿⣿⣿⣿⢿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣤⣽⣿⣿⣿⣿⣦⣰⣿⣿⣿⣿⣿⡿⠻⣿⣿⣿⣿⣶⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿";

            var ex = new Exception("SECRET CRASH DONT TELL ANYONE\n" + image);

            Form error = new CloudDebug(ex, true);
            var result = error.ShowDialog();
        }

        private void TestErrorTimerTick(object sender, EventArgs e)
        {
            testErrorTimer?.Stop();
            testErrorTimer = null;
            settingsRightClickTimer = 0;
        }


        private void OnGlitchHarvesterMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            
            Point locate = e.GetMouseLocation(sender);
            
            new ContextMenuBuilder()
                .AddItem("Open Blast Editor", (ob, ev) => BlastEditorForm.OpenBlastEditor())
                .AddItem("Open the Glitch Harvester to Main Window", (ob, ev) => Params.ToggleParam("GH_OPEN_MAIN"),
                    isChecked: Params.IsParamSet("GH_OPEN_MAIN"))
                .Build().Show(this, locate);
        }

        private void pnCrashProtection_MouseEnter(object sender, EventArgs e) => pnCrashProtection_MouseHover(sender, e);
        private void pnCrashProtection_MouseHover(object sender, EventArgs e)
        {
            pnCrashProtection.BackColor = BackColor.ChangeColorBrightness(-0.10f);
        }

        private void pnCrashProtection_MouseLeave(object sender, EventArgs e)
        {
            pnCrashProtection.BackColor = Color.Transparent;
        }
    }
}
