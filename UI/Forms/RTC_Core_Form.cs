using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using RTCV.CorruptCore;
using RTCV.NetCore;
using RTCV.NetCore.StaticTools;
using RTCV.UI;
using static RTCV.UI.UI_Extensions;
using Timer = System.Windows.Forms.Timer;

namespace RTCV.UI
{
	public partial class RTC_Core_Form : Form, IAutoColorize // replace by : UserControl for panel
	{
		public Form previousForm = null;
		public Form activeForm = null;
		private const int CP_NOCLOSE_BUTTON = 0x200;
		

		protected override CreateParams CreateParams
		{
			get
			{
				return base.CreateParams;

				CreateParams myCp = base.CreateParams;
				myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
				return myCp;
			}
		}

		public bool AutoCorrupt
		{
			get
			{
				return CorruptCore.CorruptCore.AutoCorrupt;
			}
			set
			{
				if (value)
					btnAutoCorrupt.Text = "Stop Auto-Corrupt";
				else
					btnAutoCorrupt.Text = "Start Auto-Corrupt";

				CorruptCore.CorruptCore.AutoCorrupt = value;
			}
		}

		public RTC_Core_Form()
		{
			InitializeComponent();
			pnAutoKillSwitch.Visible = true;
		}

		public void btnManualBlast_Click(object sender, EventArgs e)
		{
			LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.ASYNCBLAST, true);

		}

		public void btnAutoCorrupt_Click(object sender, EventArgs e)
		{
			if (btnAutoCorrupt.ForeColor == Color.Silver)
				return;

			AutoCorrupt = !AutoCorrupt;
			if(AutoCorrupt)
				RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.STEP_RUNBEFORE.ToString(), true);
		}
		private void RTC_Form_Load(object sender, EventArgs e)
		{
			btnLogo.Text = "   Version " + CorruptCore.CorruptCore.RtcVersion;
			/*
			GhostBoxInvisible(btnEasyMode);
			GhostBoxInvisible(btnEngineConfig);
			GhostBoxInvisible(btnGlitchHarvester);
			GhostBoxInvisible(btnStockpilePlayer);
			GhostBoxInvisible(btnManualBlast);
			GhostBoxInvisible(btnAutoCorrupt);
			GhostBoxInvisible(pnCrashProtection);*/

			if (!NetCore.Params.IsParamSet("DISCLAIMER_READ"))
			{
				MessageBox.Show(File.ReadAllText(CorruptCore.CorruptCore.rtcDir + Path.DirectorySeparatorChar + "LICENSES\\DISCLAIMER.TXT").Replace("[ver]", CorruptCore.CorruptCore.RtcVersion), "RTC", MessageBoxButtons.OK, MessageBoxIcon.Information);
				NetCore.Params.SetParam("DISCLAIMER_READ");
			}

			CorruptCore.CorruptCore.DownloadProblematicProcesses();

		}

		private void btnGlitchHarvester_Click(object sender, EventArgs e)
		{
			if (!btnGlitchHarvester.Text.Contains("○"))
				btnGlitchHarvester.Text = "○ " + btnGlitchHarvester.Text;

			S.GET<RTC_GlitchHarvester_Form>().Show();
			S.GET<RTC_GlitchHarvester_Form>().Focus();
		}

		private void RTC_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (S.GET<RTC_GlitchHarvester_Form>().UnsavedEdits && !UICore.isClosing && MessageBox.Show("You have unsaved edits in the Glitch Harvester Stockpile. \n\n Are you sure you want to close RTC without saving?", "Unsaved edits in Stockpile", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				e.Cancel = true;
				return;
			}

			
			LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_EVENT_CLOSEEMULATOR);

			//Sleep to make sure the message is sent
			Thread.Sleep(500);

			UICore.CloseAllRtcForms();
		}
		
		public void btnEasyModeCurrent_Click(object sender, EventArgs e)
		{
			StartEasyMode(false);
		}

		public void btnEasyModeTemplate_Click(object sender, EventArgs e)
		{
			StartEasyMode(true);
		}

		public void SetEngineByName(string name)
		{
			//Selects an engine from a given string name

			for (int i = 0; i < S.GET<RTC_CorruptionEngine_Form>().cbSelectedEngine.Items.Count; i++)
				if (S.GET<RTC_CorruptionEngine_Form>().cbSelectedEngine.Items[i].ToString() == name)
				{
					S.GET<RTC_CorruptionEngine_Form>().cbSelectedEngine.SelectedIndex = i;
					break;
				}
		}
		public void StartEasyMode(bool useTemplate)
		{
		//	if (RTC_NetcoreImplementation.isStandaloneUI && !S.GET<RTC_Core_Form>().cbUseGameProtection.Checked)
				S.GET<RTC_Core_Form>().cbUseGameProtection.Checked = true;


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

			this.AutoCorrupt = true;
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
				StockpileManager_UISide.BackupedState = null;
				StockpileManager_UISide.AllBackupStates.Clear();
				btnGpJumpBack.Visible = false;
				btnGpJumpNow.Visible = false;
			}
		}

		private void btnLogo_MouseClick(object sender, MouseEventArgs e)
		{
			//if (RTC_NetcoreImplementation.isStandaloneUI)
			ShowPanelForm(S.GET<RTC_ConnectionStatus_Form>(), false);
		}

		private void btnEasyMode_MouseDown(object sender, MouseEventArgs e)
		{
			Point locate = new Point(((Button)sender).Location.X + e.Location.X, ((Button)sender).Location.Y + e.Location.Y);

			ContextMenuStrip easyButtonMenu = new ContextMenuStrip();
			easyButtonMenu.Items.Add("Start with Recommended Settings", null, new EventHandler(btnEasyModeTemplate_Click));
			easyButtonMenu.Items.Add(new ToolStripSeparator());
			//EasyButtonMenu.Items.Add("Watch a tutorial video", null, new EventHandler((ob,ev) => Process.Start("https://www.youtube.com/watch?v=sIELpn4-Umw"))).Enabled = false;
			easyButtonMenu.Items.Add("Open the online wiki", null, new EventHandler((ob, ev) => Process.Start("https://corrupt.wiki/")));
			easyButtonMenu.Show(this, locate);
		}

		public void GhostBoxInvisible(Control ctrl)
		{
			Panel pn = new Panel();
			Color col = ctrl.Parent.BackColor;
			pn.BorderStyle = BorderStyle.None;
			pn.BackColor = col.ChangeColorBrightness(-0.10f);
			pn.Tag = "GHOST";
			pn.Location = ctrl.Location;
			pn.Size = ctrl.Size;
			ctrl.Parent.Controls.Add(pn);
			ctrl.Visible = false;
		}

		public void RemoveGhostBoxes()
		{
			List<Control> controlsToBeRemoved = new List<Control>();
			foreach (Control ctrl in Controls)
				if (ctrl.Tag != null && ctrl.Tag.ToString() == "GHOST")
					controlsToBeRemoved.Add(ctrl);

			foreach (Control ctrl in controlsToBeRemoved)
				Controls.Remove(ctrl);
		}

		public void ShowPanelForm(Form frm, bool hideButtons = true)
		{
			if (frm == null)
				return;

			if (hideButtons && frm is RTC_ConnectionStatus_Form)
			{
				GhostBoxInvisible(btnEasyMode);
				GhostBoxInvisible(btnEngineConfig);
				GhostBoxInvisible(btnGlitchHarvester);
				GhostBoxInvisible(btnStockpilePlayer);
				GhostBoxInvisible(btnAutoCorrupt);
				GhostBoxInvisible(btnManualBlast);
			}

			btnEngineConfig.Text = btnEngineConfig.Text.Replace("● ", "");
			btnStockpilePlayer.Text = btnStockpilePlayer.Text.Replace("● ", "");

			Button btn = null;

			if (frm is RTC_EngineConfig_Form)
				btn = btnEngineConfig;
			else if (frm is RTC_StockpilePlayer_Form)
				btn = btnStockpilePlayer;

			if (btn != null)
				btn.Text = "● " + btn.Text;

			if (!(frm is RTC_ConnectionStatus_Form) || !hideButtons)
			{
				if (!(frm is RTC_Settings_Form) || !hideButtons)
				{
					btnEasyMode.Visible = true;
					btnEngineConfig.Visible = true;
					btnGlitchHarvester.Visible = true;
					btnStockpilePlayer.Visible = true;
					btnAutoCorrupt.Visible = true;
					btnManualBlast.Visible = true;

					RemoveGhostBoxes();

					pnCrashProtection.Visible = true;
				}
			}

			if (!this.Controls.Contains(frm))
			{
				if (activeForm != null)
					activeForm.Hide();

				Controls.Remove(activeForm);
				frm.TopLevel = false;
				this.Controls.Add(frm);
				frm.Dock = DockStyle.Left;
				frm.SendToBack();
				frm.BringToFront();
				if(activeForm?.GetType() != typeof(RTC_ConnectionStatus_Form))
					previousForm = activeForm;
				activeForm = frm;
				frm.Show();
			}

			frm.Refresh();
		}

		public void btnEngineConfig_Click(object sender, EventArgs e) => ShowPanelForm(S.GET<RTC_EngineConfig_Form>());

		private void btnSettings_Click(object sender, EventArgs e) => ShowPanelForm(S.GET<RTC_Settings_Form>());

		private void btnStockPilePlayer_Click(object sender, EventArgs e) => ShowPanelForm(S.GET<RTC_StockpilePlayer_Form>());

		public void btnGpJumpBack_Click(object sender, EventArgs e)
		{
			try
			{
				btnGpJumpBack.Visible = false;

				if (StockpileManager_UISide.AllBackupStates.Count == 0)
					return;

				StashKey sk = StockpileManager_UISide.AllBackupStates.Pop();

				sk?.Run();

				GameProtection.Reset();
			}
			finally
			{
				if (StockpileManager_UISide.AllBackupStates.Count != 0)
					btnGpJumpBack.Visible = true;
			}
		}

		public void btnGpJumpNow_Click(object sender, EventArgs e)
		{
			try
			{
				btnGpJumpNow.Visible = false;

				if (StockpileManager_UISide.BackupedState != null)
					StockpileManager_UISide.BackupedState.Run();

				GameProtection.Reset();
			}
			finally
			{
				btnGpJumpNow.Visible = true;
			}
		}

		private void BlastRawStash()
		{
			LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.ASYNCBLAST, true);
			S.GET<RTC_GlitchHarvester_Form>().btnSendRaw_Click(null, null);
		}

		int manualBlastRightClickCount = 0;
		System.Windows.Forms.Timer testErrorTimer = null;
		private void btnManualBlast_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				if (testErrorTimer == null && !RTCV.NetCore.Params.IsParamSet("DEBUG_FETCHMODE"))
				{
					testErrorTimer = new System.Windows.Forms.Timer();
					testErrorTimer.Interval = 3000;
					testErrorTimer.Tick += TestErrorTimer_Tick;
					testErrorTimer.Start();
				}

				manualBlastRightClickCount++;


				Point locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);

				ContextMenuStrip columnsMenu = new ContextMenuStrip();
				columnsMenu.Items.Add("Blast + Send RAW To Stash (Glitch Harvester)", null, new EventHandler((ob, ev) =>
				{
					BlastRawStash();
				}));

				if (RTCV.NetCore.Params.IsParamSet("DEBUG_FETCHMODE") || manualBlastRightClickCount > 2)
				{
					columnsMenu.Items.Add("Open Debug window", null, new EventHandler((ob, ev) =>
					{
						error();
					}));
				}

				columnsMenu.Show(this, locate);
			}

		}

		private void error()
		{
			//SECRET CRASH DONT TELL ANYONE
			//Trigger: Hold Manual Blast for 7 seconds
			//Purpose: Testing debug window
			var ex = new CustomException("SECRET CRASH DONT TELL ANYONE",
"───────▄▀▀▀▀▀▀▀▀▀▀▄▄" + Environment.NewLine + "────▄▀▀─────────────▀▄" + Environment.NewLine + "──▄▀──────────────────▀▄" + Environment.NewLine +
"──█─────────────────────▀▄" + Environment.NewLine + "─▐▌────────▄▄▄▄▄▄▄───────▐▌" + Environment.NewLine + "─█───────────▄▄▄▄──▀▀▀▀▀──█" + Environment.NewLine +
"▐▌───────▀▀▀▀─────▀▀▀▀▀───▐▌" + Environment.NewLine + "█─────────▄▄▀▀▀▀▀────▀▀▀▀▄─█" + Environment.NewLine + "█────────────────▀───▐─────▐▌" +
Environment.NewLine + "▐▌─────────▐▀▀██▄──────▄▄▄─▐▌" + Environment.NewLine + "─█───────────▀▀▀──────▀▀██──█" + Environment.NewLine + "─▐▌────▄─────────────▌──────█" + Environment.NewLine + "──▐▌──▐──────────────▀▄─────█" +
Environment.NewLine + "───█───▌────────▐▀────▄▀───▐▌" + Environment.NewLine + "───▐▌──▀▄────────▀─▀─▀▀───▄▀" + Environment.NewLine + "───▐▌──▐▀▄────────────────█" + Environment.NewLine + "───▐▌───▌─▀▄────▀▀▀▀▀▀───█" + Environment.NewLine +
"───█───▀────▀▄──────────▄▀" + Environment.NewLine + "──▐▌──────────▀▄──────▄▀" + Environment.NewLine +
"─▄▀───▄▀────────▀▀▀▀█▀" + Environment.NewLine + "▀───▄▀──────────▀───▀▀▀▀▄▄▄▄▄"
			);

			Form error = new RTCV.NetCore.CloudDebug(ex, true);
			var result = error.ShowDialog();
		}

		private void TestErrorTimer_Tick(object sender, EventArgs e)
		{
			testErrorTimer?.Stop();
			testErrorTimer = null;
			manualBlastRightClickCount = 0;
		}

		private void cbUseAutoKillSwitch_CheckedChanged(object sender, EventArgs e)
		{
			pbAutoKillSwitchTimeout.Visible = cbUseAutoKillSwitch.Checked;
			AutoKillSwitch.Enabled = cbUseAutoKillSwitch.Checked;
		}

		private void cbAutoKillSwitchExecuteAction_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnAutoKillSwitchExecute.Text = cbAutoKillSwitchExecuteAction.SelectedItem?.ToString() ?? "Kill + Restart";
		}

		private void btnAutoKillSwitchExecute_Click(object sender, EventArgs e)
		{
			ShowPanelForm(S.GET<RTC_ConnectionStatus_Form>());

			S.GET<RTC_Core_Form>().pbAutoKillSwitchTimeout.Value = S.GET<RTC_Core_Form>().pbAutoKillSwitchTimeout.Maximum;
			AutoKillSwitch.ShouldKillswitchFire = true;
			AutoKillSwitch.KillEmulator(btnAutoKillSwitchExecute.Text.ToUpper(), true);
		}

	}
}
