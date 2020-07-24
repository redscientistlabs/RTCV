namespace RTCV.UI
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    #pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    public partial class UI_CoreForm : Form, IAutoColorize
    {
        //This form traps events and forwards them.
        //It contains the single UI_CanvasForm instance.

        public static UI_CoreForm thisForm;
        public static UI_CanvasForm cfForm;

        public CanvasGrid previousGrid = null;

        //Vallues used for padding and scaling properly in high dpi
        public static int xPadding;
        public static int corePadding; // height of the top bar
        public static int yPadding;

        public Panel pnLockSidebar = null;

        public bool AutoCorrupt
        {
            get => CorruptCore.RtcCore.AutoCorrupt;
            set
            {
                if (value)
                {
                    btnAutoCorrupt.Text = " Stop Auto-Corrupt";
                    S.GET<RTC_SimpleMode_Form>().btnAutoCorrupt.Text = " Stop Auto-Corrupt";
                }
                else
                {
                    btnAutoCorrupt.Text = " Start Auto-Corrupt";
                    S.GET<RTC_SimpleMode_Form>().btnAutoCorrupt.Text = " Start Auto-Corrupt";
                }

                CorruptCore.RtcCore.AutoCorrupt = value;
            }
        }

        public UI_CoreForm()
        {
            InitializeComponent();
            thisForm = this;
            this.FormClosing += UI_CoreForm_FormClosing;

            cfForm = new UI_CanvasForm
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

            //UICore.SetRTCColor(UICore.GeneralColor);
        }

        private void UI_CoreForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (S.GET<RTC_StockpileManager_Form>().UnsavedEdits && !UICore.isClosing && MessageBox.Show("You have unsaved edits in the Glitch Harvester Stockpile. \n\n Are you sure you want to close RTC without saving?", "Unsaved edits in Stockpile", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_EVENT_SHUTDOWN, true);
            LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_EVENT_CLOSEEMULATOR);

            //Sleep to make sure the message is sent
            System.Threading.Thread.Sleep(500);

            UICore.CloseAllRtcForms();
        }

        private void UI_CoreForm_Load(object sender, EventArgs e)
        {
            btnLogo.Text = "RTCV " + CorruptCore.RtcCore.RtcVersion;

            if (!NetCore.Params.IsParamSet("DISCLAIMER_READ"))
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

                string disclaimerPath = Path.Combine(CorruptCore.RtcCore.RtcDir, "LICENSES", "DISCLAIMER.TXT");

                //Use the text file if it exists
                if (File.Exists(disclaimerPath))
                {
                    disclaimer = File.ReadAllText(disclaimerPath);
                }

                S.GET<RTC_Intro_Form>().DisplayRtcvDisclaimer(disclaimer.Replace("[ver]", CorruptCore.RtcCore.RtcVersion));
                //MessageBox.Show(disclaimer.Replace("[ver]", CorruptCore.RtcCore.RtcVersion), "RTC", MessageBoxButtons.OK, MessageBoxIcon.Information);

                NetCore.Params.SetParam("DISCLAIMER_READ");

                if (S.GET<RTC_Intro_Form>().selection == IntroAction.SIMPLEMODE)
                {
                    NetCore.Params.SetParam("SIMPLE_MODE"); //Set RTC in Simple Mode

                    if (UI_VanguardImplementation.connector.netConn.status == NetworkStatus.CONNECTED)
                    {
                        UI_DefaultGrids.simpleMode.LoadToMain();
                        RTC_SimpleMode_Form smForm = S.GET<RTC_SimpleMode_Form>();
                        smForm.EnteringSimpleMode();
                    }
                }

                NetCore.Params.SetParam("COMPRESS_STOCKPILE"); //Default param
                NetCore.Params.SetParam("INCLUDE_REFERENCED_FILES"); //Default param
            }

            CorruptCore.RtcCore.DownloadProblematicProcesses();

            //UI_DefaultGrids.engineConfig.LoadToMain();
        }

        internal void SetCustomLayoutName(string customLayoutPath)
        {
            string[] layoutFileData = File.ReadAllLines(customLayoutPath);

            string gridNameLine = layoutFileData.FirstOrDefault(it => it.StartsWith("GridName:"));

            if (gridNameLine == null)
                return;

            string[] parts = gridNameLine.Trim().Split(':');

            if (parts.Length == 1 || string.IsNullOrWhiteSpace(parts[1]))
                return;

            btnOpenCustomLayout.Text = $"Load {parts[1]}";
        }

        public void SetSize(int x, int y)
        {
            //this.Size = new Size(x + xPadding, y + yPadding + coreYPadding); //For Horizontal tab-style menu in coreform
            this.Size = new Size(x + xPadding + corePadding, y + yPadding); //For Vertical tab-style menu in coreform
        }

        private void UI_CoreForm_ResizeBegin(object sender, EventArgs e)
        {
            //Sends event to SubForm
            if (cfForm.spForm != null)
            {
                cfForm.spForm.Parent_ResizeBegin();
            }
        }

        private void UI_CoreForm_ResizeEnd(object sender, EventArgs e)
        {
            //Sends event to SubForm
            if (cfForm.spForm != null)
            {
                cfForm.spForm?.Parent_ResizeEnd();
            }
        }

        private FormWindowState? LastWindowState = null;

        private void UI_CoreForm_Resize(object sender, EventArgs e)
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
                    cfForm.spForm?.Parent_ResizeEnd();
                }

                LastWindowState = WindowState;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //test button, loads a dummy form in SubForm mode

            var f = S.GET<UI_ComponentFormSubForm>();
            cfForm.OpenSubForm(f, true);
        }

        private void BtnOpenCustomLayout_Click(object sender, EventArgs e)
        {
            CanvasGrid.LoadCustomLayout();
        }

        private void PrepareLockSideBar()
        {
            if (pnLockSidebar == null || !pnSideBar.Controls.Contains(pnLockSidebar))
            {
                pnLockSidebar = new Panel()
                {
                    Size = pnSideBar.Size,
                    Location = new Point(0, 0),
                    BackColor = UICore.Dark4Color,
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
            bmp.Tint(Color.FromArgb(0xF0, UICore.Dark4Color));
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

        public void btnEngineConfig_Click(object sender, EventArgs e)
        {
            if (NetCore.Params.IsParamSet("SIMPLE_MODE"))
            {
                UI_DefaultGrids.simpleMode.LoadToMain();
                RTC_SimpleMode_Form smForm = S.GET<RTC_SimpleMode_Form>();
                smForm.EnteringSimpleMode();
            }
            else
            {
                UI_DefaultGrids.engineConfig.LoadToMain();
            }
        }

        private void pnAutoKillSwitch_MouseHover(object sender, EventArgs e)
        {
            lbAks.ForeColor = Color.FromArgb(32, 32, 32);
            pnAutoKillSwitch.BackColor = Color.Salmon;
        }

        private void pnAutoKillSwitch_MouseLeave(object sender, EventArgs e)
        {
            lbAks.ForeColor = Color.White;
            pnAutoKillSwitch.BackColor = Color.Transparent;
        }

        private void btnGlitchHarvester_Click(object sender, EventArgs e)
        {
            pnGlitchHarvesterOpen.Visible = true;

            UI_DefaultGrids.glitchHarvester.LoadToNewWindow("Glitch Harvester");
        }

        public void btnAutoCorrupt_Click(object sender, EventArgs e)
        {
            if (btnAutoCorrupt.ForeColor == Color.Silver)
            {
                return;
            }

            AutoCorrupt = !AutoCorrupt;
            if (AutoCorrupt)
            {
                RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.STEP_RUNBEFORE, true);
            }
        }

        public void btnManualBlast_Click(object sender, EventArgs e)
        {
            if (AllSpec.VanguardSpec[VSPEC.REPLACE_MANUALBLAST_WITH_GHCORRUPT] != null)
            {
                S.GET<RTC_GlitchHarvesterBlast_Form>().btnCorrupt_Click(sender, e);
            }
            else
            {
                LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.MANUALBLAST, true);
            }
        }

        private void btnEasyMode_MouseDown(object sender, MouseEventArgs e)
        {
            bool simpleModeVisible = S.GET<RTC_SimpleMode_Form>().Visible;

            Point locate = e.GetMouseLocation(sender);

            ContextMenuStrip easyButtonMenu = new ContextMenuStrip();
            (easyButtonMenu.Items.Add("Switch to Simple Mode", null, new EventHandler((ob, ev) =>
            {
                if ((AllSpec.VanguardSpec[VSPEC.NAME] as string)?.ToUpper().Contains("SPEC") ?? false)
                {
                    MessageBox.Show("Simple Mode is currently only supported on Vanguard implementations.");
                    return;
                }

                UI_DefaultGrids.simpleMode.LoadToMain();
                RTC_SimpleMode_Form smForm = S.GET<RTC_SimpleMode_Form>();

                smForm.EnteringSimpleMode();
            }))).Enabled = !simpleModeVisible;
            (easyButtonMenu.Items.Add("Start Auto-Corrupt with Recommended Settings for loaded game", null, new EventHandler(((ob, ev) => { S.GET<UI_CoreForm>().StartEasyMode(true); })))).Enabled = ((bool)AllSpec.VanguardSpec[VSPEC.SUPPORTS_SAVESTATES] == true) && !simpleModeVisible;
            easyButtonMenu.Items.Add(new ToolStripSeparator());
            //EasyButtonMenu.Items.Add("Watch a tutorial video", null, new EventHandler((ob,ev) => Process.Start("https://www.youtube.com/watch?v=sIELpn4-Umw"))).Enabled = false;
            easyButtonMenu.Items.Add("Open the online wiki", null, new EventHandler((ob, ev) => Process.Start("https://corrupt.wiki/")));
            easyButtonMenu.Show(this, locate);
        }

        private void btnStockpilePlayer_Click(object sender, EventArgs e)
        {
            UI_DefaultGrids.stockpilePlayer.LoadToMain();
        }

        private int settingsRightClickTimer = 0;
        private System.Windows.Forms.Timer testErrorTimer = null;

        public void btnSettings_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (testErrorTimer == null && !RTCV.NetCore.Params.IsParamSet("DEBUG_FETCHMODE"))
                {
                    testErrorTimer = new System.Windows.Forms.Timer
                    {
                        Interval = 3000
                    };
                    testErrorTimer.Tick += TestErrorTimer_Tick;
                    testErrorTimer.Start();
                }

                settingsRightClickTimer++;

                Point locate = e.GetMouseLocation(sender);
                ContextMenuStrip columnsMenu = new ContextMenuStrip();

                if (RTCV.NetCore.Params.IsParamSet("DEBUG_FETCHMODE") || settingsRightClickTimer > 2)
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
                UI_DefaultGrids.settings.LoadToMain();
            }
        }

        private void pnAutoKillSwitch_MouseClick(object sender, MouseEventArgs e)
        {
            //needed anymore?
            S.GET<UI_CoreForm>().btnLogo_Click(sender, e);

            S.GET<UI_CoreForm>().pbAutoKillSwitchTimeout.Value = S.GET<UI_CoreForm>().pbAutoKillSwitchTimeout.Maximum;
            AutoKillSwitch.ShouldKillswitchFire = true;

            //refactor this to not use string once old coreform is dead
            AutoKillSwitch.KillEmulator(true);
        }

        private void cbUseAutoKillSwitch_CheckedChanged(object sender, EventArgs e)
        {
            pbAutoKillSwitchTimeout.Visible = cbUseAutoKillSwitch.Checked;
            AutoKillSwitch.Enabled = cbUseAutoKillSwitch.Checked;
        }

        private void LbGameProtection_MouseClick(object sender, MouseEventArgs e)
        {
            cbUseGameProtection.Checked = !cbUseGameProtection.Checked;
        }

        private void cbUseGameProtection_CheckedChanged(object sender, EventArgs e)
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

        public void btnGpJumpBack_Click(object sender, EventArgs e)
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

        public void btnGpJumpNow_Click(object sender, EventArgs e)
        {
            try
            {
                btnGpJumpNow.Visible = false;

                //Do this to prevent any potential race
                var sk = StockpileManager_UISide.BackupedState;

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

        private void btnLogo_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void btnLogo_Click(object sender, EventArgs e)
        {
            UI_DefaultGrids.connectionStatus.LoadToMain();
        }

        public void StartEasyMode(bool useTemplate)
        {
            //if (RTC_NetcoreImplementation.isStandaloneUI && !S.GET<RTC_Core_Form>().cbUseGameProtection.Checked)
            S.GET<UI_CoreForm>().cbUseGameProtection.Checked = true;

            if (useTemplate)
            {
                //Put Console templates HERE
                string thisSystem = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.SYSTEM];

                switch (thisSystem)
                {
                    case "NES":     //Nintendo Entertainment system
                        SetEngineByName("Nightmare Engine");
                        S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Value = 2;
                        S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Value = 1;
                        break;

                    case "GB":      //Gameboy
                    case "GBC":     //Gameboy Color
                        SetEngineByName("Nightmare Engine");
                        S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Value = 1;
                        S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Value = 4;
                        break;

                    case "SNES":    //Super Nintendo
                        SetEngineByName("Nightmare Engine");
                        S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Value = 1;
                        S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Value = 2;
                        break;

                    case "GBA":     //Gameboy Advance
                        SetEngineByName("Nightmare Engine");
                        S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Value = 1;
                        S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Value = 1;
                        break;

                    case "N64":     //Nintendo 64
                        SetEngineByName("Vector Engine");
                        S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Value = 75;
                        S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Value = 1;
                        break;

                    case "Dolphin":     //GC/Wii
                        SetEngineByName("Vector Engine");
                        S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Value = 75;
                        S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Value = 1;
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
            }

            S.GET<UI_CoreForm>().AutoCorrupt = true;
        }

        public void SetEngineByName(string name)
        {
            //Selects an engine from a given string name

            for (int i = 0; i < S.GET<RTC_CorruptionEngine_Form>().cbSelectedEngine.Items.Count; i++)
            {
                if (S.GET<RTC_CorruptionEngine_Form>().cbSelectedEngine.Items[i].ToString() == name)
                {
                    S.GET<RTC_CorruptionEngine_Form>().cbSelectedEngine.SelectedIndex = i;
                    break;
                }
            }
        }

        private void BlastRawStash()
        {
            LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.MANUALBLAST, true);
            S.GET<RTC_GlitchHarvesterBlast_Form>().btnSendRaw_Click(null, null);
        }

        private void BtnManualBlast_MouseDown(object sender, MouseEventArgs e)
        {
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

            Form error = new RTCV.NetCore.CloudDebug(ex, true);
            var result = error.ShowDialog();
        }

        private void TestErrorTimer_Tick(object sender, EventArgs e)
        {
            testErrorTimer?.Stop();
            testErrorTimer = null;
            settingsRightClickTimer = 0;
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            UICore.LockInterface();
            UI_DefaultGrids.connectionStatus.LoadToMain();
        }

        private void BtnGlitchHarvester_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip columnsMenu = new ContextMenuStrip();

                Point locate = e.GetMouseLocation(sender);
                columnsMenu.Items.Add("Open Blast Editor", null, new EventHandler((ob, ev) =>
                {
                    RTC_NewBlastEditor_Form.OpenBlastEditor();
                }));

                columnsMenu.Show(this, locate);
            }
        }

        private void BtnNetcoreTest_Click(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, "TEST");
        }
    }
}
