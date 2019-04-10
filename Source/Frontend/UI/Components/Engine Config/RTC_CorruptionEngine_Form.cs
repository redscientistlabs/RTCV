using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTCV.CorruptCore;
using RTCV.NetCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;

namespace RTCV.UI
{
	public partial class RTC_CorruptionEngine_Form : ComponentForm, IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

		private int defaultPrecision = -1;
		private bool updatingMinMax = false;


		public RTC_CorruptionEngine_Form()
		{
			InitializeComponent();

			this.undockedSizable = false;
		}

		private void RTC_CorruptionEngine_Form_Load(object sender, EventArgs e)
		{

			gbNightmareEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
			gbHellgenieEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
			gbDistortionEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
			gbFreezeEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
			gbPipeEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
			gbVectorEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
			gbBlastGeneratorEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
			gbCustomEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);


			cbSelectedEngine.SelectedIndex = 0;
			cbBlastType.SelectedIndex = 0;
			cbCustomPrecision.SelectedIndex = 0;


			cbVectorValueList.DataSource = null;
			cbVectorLimiterList.DataSource = null;
			cbVectorValueList.DisplayMember = "Name";
			cbVectorLimiterList.DisplayMember = "Name";

			cbVectorValueList.ValueMember = "Value";
			cbVectorLimiterList.ValueMember = "Value";

			//Do this here as if it's stuck into the designer, it keeps defaulting out
			cbVectorValueList.DataSource = CorruptCore.CorruptCore.ValueListBindingSource;
			cbVectorLimiterList.DataSource = CorruptCore.CorruptCore.LimiterListBindingSource;

			if (CorruptCore.CorruptCore.LimiterListBindingSource.Count > 0)
			{
				cbVectorLimiterList_SelectedIndexChanged(cbVectorLimiterList, null);
			}
			if (CorruptCore.CorruptCore.ValueListBindingSource.Count > 0)
			{
				cbVectorValueList_SelectedIndexChanged(cbVectorValueList, null);
			}

		}


		private void nmDistortionDelay_ValueChanged(object sender, EventArgs e)
		{
			RTC_DistortionEngine.Delay = Convert.ToInt32(nmDistortionDelay.Value);
		}

		private void btnResyncDistortionEngine_Click(object sender, EventArgs e)
		{
			LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_CLEARSTEPBLASTUNITS, null, true);
		}


		private void cbSelectedEngine_SelectedIndexChanged(object sender, EventArgs e)
		{
			gbNightmareEngine.Visible = false;
			gbHellgenieEngine.Visible = false;
			gbDistortionEngine.Visible = false;
			gbFreezeEngine.Visible = false;
			gbPipeEngine.Visible = false;
			gbVectorEngine.Visible = false;
			gbBlastGeneratorEngine.Visible = false;
			gbCustomEngine.Visible = false;

			pnCustomPrecision.Visible = false;
			

			S.GET<RTC_Core_Form>().btnAutoCorrupt.Visible = true;
			S.GET<RTC_GlitchHarvester_Form>().pnIntensity.Visible = true;
			S.GET<RTC_EngineConfig_Form>().pnGeneralParameters.Visible = true;
			S.GET<RTC_EngineConfig_Form>().pnMemoryDomains.Visible = true;

			switch (cbSelectedEngine.SelectedItem.ToString())
			{
				case "Nightmare Engine":
					CorruptCore.CorruptCore.SelectedEngine = CorruptionEngine.NIGHTMARE;
					gbNightmareEngine.Visible = true;
					pnCustomPrecision.Visible = true;
					break;

				case "Hellgenie Engine":
					CorruptCore.CorruptCore.SelectedEngine = CorruptionEngine.HELLGENIE;
					gbHellgenieEngine.Visible = true;
					pnCustomPrecision.Visible = true;
					break;

				case "Distortion Engine":
					CorruptCore.CorruptCore.SelectedEngine = CorruptionEngine.DISTORTION;
					gbDistortionEngine.Visible = true;
					pnCustomPrecision.Visible = true;
					break;

				case "Freeze Engine":
					CorruptCore.CorruptCore.SelectedEngine = CorruptionEngine.FREEZE;
					gbFreezeEngine.Visible = true;
					pnCustomPrecision.Visible = true;
					break;

				case "Pipe Engine":
					CorruptCore.CorruptCore.SelectedEngine = CorruptionEngine.PIPE;
					gbPipeEngine.Visible = true;
					pnCustomPrecision.Visible = true;
					break;

				case "Vector Engine":
					CorruptCore.CorruptCore.SelectedEngine = CorruptionEngine.VECTOR;
					gbVectorEngine.Visible = true;
					break;

				case "Custom Engine":
					CorruptCore.CorruptCore.SelectedEngine = CorruptionEngine.CUSTOM;
					gbCustomEngine.Visible = true;
					pnCustomPrecision.Visible = true;
					break;

				case "Blast Generator":
					CorruptCore.CorruptCore.SelectedEngine = CorruptionEngine.BLASTGENERATORENGINE;
					gbBlastGeneratorEngine.Visible = true;

					S.GET<RTC_Core_Form>().AutoCorrupt = false;
					S.GET<RTC_Core_Form>().btnAutoCorrupt.Visible = false;
					S.GET<RTC_EngineConfig_Form>().pnGeneralParameters.Visible = false;
					S.GET<RTC_EngineConfig_Form>().pnMemoryDomains.Visible = false;

					S.GET<RTC_GlitchHarvester_Form>().pnIntensity.Visible = false;
					break;

				default:
					break;
			}

			if (cbSelectedEngine.SelectedItem.ToString() == "Blast Generator")
			{
				S.GET<RTC_GeneralParameters_Form>().labelBlastRadius.Visible = false;
				S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Visible = false;
				S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Visible = false;
				S.GET<RTC_GeneralParameters_Form>().cbBlastRadius.Visible = false;
			}
			else
			{
				S.GET<RTC_GeneralParameters_Form>().labelBlastRadius.Visible = true;
				S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Visible = true;
				S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Visible = true;
				S.GET<RTC_GeneralParameters_Form>().cbBlastRadius.Visible = true;
			}

			cbSelectedEngine.BringToFront();
			pnCustomPrecision.BringToFront();

			LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_CLEARSTEPBLASTUNITS, null, true);
		}

		public void SetLockBoxes(bool enabled)
		{
			dontUpdate = true;
			cbLockPipes.Checked = enabled;
			dontUpdate = false;
		}
		public void SetRewindBoxes(bool enabled)
		{
			dontUpdate = true;
			S.GET<RTC_SettingsCorrupt_Form>().SetRewindBoxes(enabled);
			cbClearFreezesOnRewind.Checked = enabled;
			cbClearCheatsOnRewind.Checked = enabled;
			cbClearPipesOnRewind.Checked = enabled;
			dontUpdate = false;
		}
		public bool dontUpdate = false;
		private void cbClearRewind_CheckedChanged(object sender, EventArgs e)
		{
			if (dontUpdate)
				return;
			SetRewindBoxes(((CheckBox)sender).Checked);

			S.GET<RTC_CustomEngineConfig_Form>().SetRewindBoxes(((CheckBox)sender).Checked);

			StepActions.ClearStepActionsOnRewind = cbClearFreezesOnRewind.Checked;
		}

		private void btnClearPipes_Click(object sender, EventArgs e)
		{
			LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_CLEARSTEPBLASTUNITS, null, true);
		}

		private void cbLockPipes_CheckedChanged(object sender, EventArgs e)
		{
			S.GET<RTC_SettingsCorrupt_Form>().SetLockBoxes(cbLockPipes.Checked);
			StepActions.LockExecution = cbLockPipes.Checked;
		}


		private void cbVectorLimiterList_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)sender).SelectedItem;
			if(item != null)
				RTC_VectorEngine.LimiterListHash = item.Value;
		}

		private void cbVectorValueList_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)sender).SelectedItem;
			if (item != null)
				RTC_VectorEngine.ValueListHash = item.Value;
		}

		private void btnClearCheats_Click(object sender, EventArgs e)
		{
			LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_CLEARSTEPBLASTUNITS, null, true);
		}

		private void updateMinMaxBoxes(int precision)
		{
			updatingMinMax = true;
			switch (precision)
			{
				case 1:
					nmMinValueNightmare.Maximum = byte.MaxValue;
					nmMaxValueNightmare.Maximum = byte.MaxValue;

					nmMinValueHellgenie.Maximum = byte.MaxValue;
					nmMaxValueHellgenie.Maximum = byte.MaxValue;


					nmMinValueNightmare.Value = RTC_NightmareEngine.MinValue8Bit;
					nmMaxValueNightmare.Value = RTC_NightmareEngine.MaxValue8Bit;

					nmMinValueHellgenie.Value = RTC_HellgenieEngine.MinValue8Bit;
					nmMaxValueHellgenie.Value = RTC_HellgenieEngine.MaxValue8Bit;

					break;

				case 2:
					nmMinValueNightmare.Maximum = UInt16.MaxValue;
					nmMaxValueNightmare.Maximum = UInt16.MaxValue;

					nmMinValueHellgenie.Maximum = UInt16.MaxValue;
					nmMaxValueHellgenie.Maximum = UInt16.MaxValue;

					nmMinValueNightmare.Value = RTC_NightmareEngine.MinValue16Bit;
					nmMaxValueNightmare.Value = RTC_NightmareEngine.MaxValue16Bit;
																			
					nmMinValueHellgenie.Value = RTC_HellgenieEngine.MinValue16Bit;
					nmMaxValueHellgenie.Value = RTC_HellgenieEngine.MaxValue16Bit;

					break;
				case 4:
					nmMinValueNightmare.Maximum = UInt32.MaxValue;
					nmMaxValueNightmare.Maximum = UInt32.MaxValue;

					nmMinValueHellgenie.Maximum = UInt32.MaxValue;
					nmMaxValueHellgenie.Maximum = UInt32.MaxValue;

					nmMinValueNightmare.Value = RTC_NightmareEngine.MinValue32Bit;
					nmMaxValueNightmare.Value = RTC_NightmareEngine.MaxValue32Bit;

					nmMinValueHellgenie.Value = RTC_HellgenieEngine.MinValue32Bit;
					nmMaxValueHellgenie.Value = RTC_HellgenieEngine.MaxValue32Bit;

					break;
			}
			updatingMinMax = false;
		}

		private void cbCustomPrecision_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Deselect the updown boxes so they commit if they're selected.
			//As you can use the scroll wheel over the combobox while the textbox is focused, this is required
			cbCustomPrecision.Focus();

			if (cbCustomPrecision.SelectedIndex != -1)
			{

				switch (cbCustomPrecision.SelectedIndex)
				{
					case 0:
						CorruptCore.CorruptCore.CurrentPrecision = 1;
						break;
					case 1:
						CorruptCore.CorruptCore.CurrentPrecision = 2;
						break;
					case 2:
						CorruptCore.CorruptCore.CurrentPrecision = 4;
						break;
				}
				
				updateMinMaxBoxes(CorruptCore.CorruptCore.CurrentPrecision);
				S.GET<RTC_CustomEngineConfig_Form>().UpdateMinMaxBoxes(CorruptCore.CorruptCore.CurrentPrecision);
			}
		}

		private void btnOpenBlastGenerator_Click(object sender, EventArgs e)
		{
			if (S.GET<RTC_BlastGenerator_Form>() != null)
				S.GET<RTC_BlastGenerator_Form>().Close();
			S.SET(new RTC_BlastGenerator_Form());
			S.GET<RTC_BlastGenerator_Form>().LoadNoStashKey();
		}


		private void nmMinValueNightmare_ValueChanged(object sender, EventArgs e)
		{
			//We don't want to trigger this if it caps when stepping downwards
			if (updatingMinMax)
				return;

			long value = Convert.ToInt64(nmMinValueNightmare.Value);


			switch (CorruptCore.CorruptCore.CurrentPrecision)
			{
				case 1:
					RTC_NightmareEngine.MinValue8Bit = value;
					break;
				case 2:
					RTC_NightmareEngine.MinValue16Bit = value;
					break;
				case 4:
					RTC_NightmareEngine.MinValue32Bit = value;
					break;
			}

		}

		private void nmMaxValueNightmare_ValueChanged(object sender, EventArgs e)
		{
			//We don't want to trigger this if it caps when stepping downwards
			if (updatingMinMax)
				return;
			long value = Convert.ToInt64(nmMaxValueNightmare.Value);
			

			switch (CorruptCore.CorruptCore.CurrentPrecision)
			{
				case 1:
					RTC_NightmareEngine.MaxValue8Bit = value;
					break;
				case 2:
					RTC_NightmareEngine.MaxValue16Bit = value;
					break;
				case 4:
					RTC_NightmareEngine.MaxValue32Bit = value;
					break;
			}
		}

		private void nmMinValueHellgenie_ValueChanged(object sender, EventArgs e)
		{
			//We don't want to trigger this if it caps when stepping downwards
			if (updatingMinMax)
				return;
			long value = Convert.ToInt64(nmMinValueHellgenie.Value);

			switch (CorruptCore.CorruptCore.CurrentPrecision)
			{
				case 1:
					RTC_HellgenieEngine.MinValue8Bit = value;
					break;
				case 2:
					RTC_HellgenieEngine.MinValue16Bit = value;
					break;
				case 4:
					RTC_HellgenieEngine.MinValue32Bit = value;
					break;
			}
		}

		private void nmMaxValueHellgenie_ValueChanged(object sender, EventArgs e)
		{
			//We don't want to trigger this if it caps when stepping downwards
			if (updatingMinMax)
				return;

			long value = Convert.ToInt64(nmMaxValueHellgenie.Value);

			switch (CorruptCore.CorruptCore.CurrentPrecision)
			{
				case 1:
					RTC_HellgenieEngine.MaxValue8Bit = value;
					break;
				case 2:
					RTC_HellgenieEngine.MaxValue16Bit = value;
					break;
				case 4:
					RTC_HellgenieEngine.MaxValue32Bit = value;
					break;
			}
		}


		private void cbBlastType_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (cbBlastType.SelectedItem.ToString())
			{
				case "RANDOM":
					RTC_NightmareEngine.Algo = NightmareAlgo.RANDOM;
					nmMinValueNightmare.Enabled = true;
					nmMaxValueNightmare.Enabled = true;
					break;

				case "RANDOMTILT":
					RTC_NightmareEngine.Algo = NightmareAlgo.RANDOMTILT;
					nmMinValueNightmare.Enabled = true;
					nmMaxValueNightmare.Enabled = true;
					break;

				case "TILT":
					RTC_NightmareEngine.Algo = NightmareAlgo.TILT;
					nmMinValueNightmare.Enabled = false;
					nmMaxValueNightmare.Enabled = false;
					break;
			}
		}

		private void btnOpenCustomEngine_Click(object sender, EventArgs e)
		{
			S.GET<RTC_CustomEngineConfig_Form>().Show();
			S.GET<RTC_CustomEngineConfig_Form>().Focus();
		}

	}
}
