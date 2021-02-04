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
    public partial class CoreForm : Modular.ColorizedForm
    {
        //This form traps events and forwards them.
        //It contains the single CanvasForm instance.

        internal static CoreForm thisForm;
        internal static CanvasForm cfForm;

        public CanvasGrid previousGrid { get; set; } = null;

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
            if (S.GET<StockpileManagerForm>().UnsavedEdits && !UICore.isClosing && MessageBox.Show("You have unsaved edits in the Glitch Harvester Stockpile. \n\n Are you sure you want to close RTC without saving?", "Unsaved edits in Stockpile", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
                return;
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
                //MessageBox.Show(disclaimer.Replace("[ver]", CorruptCore.RtcCore.RtcVersion), "RTC", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                Params.SetParam("INCLUDE_REFERENCED_FILES"); //Default param
            }

            RtcCore.DownloadProblematicProcesses();

            //DefaultGrids.engineConfig.LoadToMain();
        }

        public void SetSize(int x, int y)
        {
            //this.Size = new Size(x + xPadding, y + yPadding + coreYPadding); //For Horizontal tab-style menu in coreform
            this.Size = new Size(x + xPadding + corePadding, y + yPadding); //For Vertical tab-style menu in coreform
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

                ContextMenuStrip openCustomLayoutMenu = new ContextMenuStrip();
                var item = openCustomLayoutMenu.Items.Add($"No Custom Layouts loaded", null, new EventHandler((ob, ev) => CanvasGrid.LoadCustomLayout("")));
                item.Enabled = false;
                openCustomLayoutMenu.Show(this, locate);
            }
            else if (layouts.Length == 1)
            {
                CanvasGrid.LoadCustomLayout(layouts.First().FullName);
            }
            else
            {
                Point locate = e.GetMouseLocation(sender);

                ContextMenuStrip openCustomLayoutMenu = new ContextMenuStrip();
                foreach (var layout in layouts)
                    openCustomLayoutMenu.Items.Add($"Load {layout.Name.Replace(layout.Extension, "")}", null, new EventHandler((ob, ev) => CanvasGrid.LoadCustomLayout(layout.FullName)));

                openCustomLayoutMenu.Show(this, locate);

            }
        }

        private void PrepareLockSideBar()
        {
            if (pnLockSidebar == null || !pnSideBar.Controls.Contains(pnLockSidebar))
            {
                pnLockSidebar = new Panel()
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
            pnGlitchHarvesterOpen.Visible = true;

            if (Params.IsParamSet("GH_OPEN_MAIN"))
            {
                DefaultGrids.glitchHarvester.LoadToMain();
            }
            else
            {
                DefaultGrids.glitchHarvester.LoadToNewWindow("Glitch Harvester");
            }
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
                LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Basic.ManualBlast, true);
            }
        }

        private void OnStartEasyModeClick(object sender, MouseEventArgs e)
        {
            bool simpleModeVisible = S.GET<SimpleModeForm>().Visible;

            Point locate = e.GetMouseLocation(sender);

            ContextMenuStrip easyButtonMenu = new ContextMenuStrip();
            (easyButtonMenu.Items.Add("Switch to Simple Mode", null, new EventHandler((ob, ev) =>
            {
                if ((AllSpec.VanguardSpec[VSPEC.NAME] as string)?.ToUpper().Contains("SPEC") ?? false)
                {
                    MessageBox.Show("Simple Mode is currently only supported on Vanguard implementations.");
                    return;
                }

                DefaultGrids.simpleMode.LoadToMain();
                SimpleModeForm smForm = S.GET<SimpleModeForm>();

                smForm.EnteringSimpleMode();
            }))).Enabled = !simpleModeVisible;
            (easyButtonMenu.Items.Add("Start Auto-Corrupt with Recommended Settings for loaded game", null, new EventHandler(((ob, ev) => { StartEasyMode(true); })))).Enabled = ((bool)AllSpec.VanguardSpec[VSPEC.SUPPORTS_SAVESTATES] == true) && !simpleModeVisible;
            easyButtonMenu.Items.Add(new ToolStripSeparator());
            //EasyButtonMenu.Items.Add("Watch a tutorial video", null, new EventHandler((ob,ev) => Process.Start("https://www.youtube.com/watch?v=sIELpn4-Umw"))).Enabled = false;
            easyButtonMenu.Items.Add("Open the online wiki", null, new EventHandler((ob, ev) => Process.Start("https://corrupt.wiki/")));
            easyButtonMenu.Show(this, locate);
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
                ContextMenuStrip columnsMenu = new ContextMenuStrip();

                if (Params.IsParamSet("DEBUG_FETCHMODE") || settingsRightClickTimer > 2)
                {
                    columnsMenu.Items.Add("Open Debug window", null, new EventHandler((ob, ev) =>
                    {
                        ForceCloudDebug();
                    }));
                }

                columnsMenu.Show(this, locate);
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
            DefaultGrids.connectionStatus.LoadToMain();
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
                        S.GET<GeneralParametersForm>().multiTB_Intensity.Value = 25;
                        S.GET<GeneralParametersForm>().multiTB_ErrorDelay.Value = 1;
                        break;

                    case "Dolphin":     //GC/Wii
                        SetEngineByName("Vector Engine");
                        S.GET<GeneralParametersForm>().multiTB_Intensity.Value = 75;
                        S.GET<GeneralParametersForm>().multiTB_ErrorDelay.Value = 1;
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
                        MessageBox.Show("WARNING: No Easy-Mode template was made for this system. Please configure it manually and use the current settings.");
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
            if (e == null)
                e = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);

            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip columnsMenu = new ContextMenuStrip();

                Point locate = e.GetMouseLocation(sender);
                columnsMenu.Items.Add("Blast + Send RAW To Stash (Glitch Harvester)", null, new EventHandler((ob, ev) =>
                {
                    BlastRawStash();
                }));

                columnsMenu.Show(this, locate);
            }
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
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip columnsMenu = new ContextMenuStrip();

                Point locate = e.GetMouseLocation(sender);
                columnsMenu.Items.Add("Open Blast Editor", null, new EventHandler((ob, ev) =>
                {
                    BlastEditorForm.OpenBlastEditor();
                }));

                var ghmain = (columnsMenu.Items.Add("Open the Glitch Harvester to Main Window", null, new EventHandler((ob, ev) =>
                {
                    if (Params.IsParamSet("GH_OPEN_MAIN"))
                    {
                        Params.RemoveParam("GH_OPEN_MAIN");
                    }
                    else
                    {
                        Params.SetParam("GH_OPEN_MAIN");
                    }
                })) as ToolStripMenuItem);

                ghmain.Checked = Params.IsParamSet("GH_OPEN_MAIN");

                columnsMenu.Show(this, locate);
            }
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
