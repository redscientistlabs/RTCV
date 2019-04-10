using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Diagnostics;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;

namespace RTCV.UI
{
	public partial class RTC_SettingsCorrupt_Form : ComponentForm, IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public RTC_SettingsCorrupt_Form()
		{
			InitializeComponent();


			UICore.SetRTCColor(UICore.GeneralColor, this);

			Load += RTC_SettingRerollForm_Load;

			nmMaxInfiniteStepUnits.registerSlave(S.GET<RTC_CorruptionEngine_Form>().updownMaxCheats);
			nmMaxInfiniteStepUnits.registerSlave(S.GET<RTC_CorruptionEngine_Form>().updownMaxFreeze);
			nmMaxInfiniteStepUnits.registerSlave(S.GET<RTC_CorruptionEngine_Form>().updownMaxPipes);
			nmMaxInfiniteStepUnits.registerSlave(S.GET<RTC_CustomEngineConfig_Form>().updownMaxInfiniteUnits);
			nmMaxInfiniteStepUnits.ValueChanged += nmMaxInfiniteStepUnits_ValueChanged;


			cbRerollAddress.Checked = CorruptCore.CorruptCore.RerollAddress;
			cbRerollSourceAddress.Checked = CorruptCore.CorruptCore.RerollSourceAddress;

			cbRerollDomain.Checked = CorruptCore.CorruptCore.RerollDomain;
			cbRerollSourceDomain.Checked = CorruptCore.CorruptCore.RerollSourceDomain;

			cbRerollFollowsCustom.Checked = CorruptCore.CorruptCore.RerollFollowsCustomEngine;
			cbIgnoreUnitOrigin.Checked = CorruptCore.CorruptCore.RerollIgnoresOriginalSource;
		}

		private void nmMaxInfiniteStepUnits_ValueChanged(object sender, EventArgs e)
		{
			CorruptCore.StepActions.MaxInfiniteBlastUnits = Convert.ToInt32(nmMaxInfiniteStepUnits.Value);
		}

		private void RTC_SettingRerollForm_Load(object sender, EventArgs e)
		{
		}

		private void cbRerollSourceAddress_CheckedChanged(object sender, EventArgs e)
		{
			CorruptCore.CorruptCore.RerollSourceAddress = cbRerollSourceAddress.Checked;
			if (!cbRerollSourceAddress.Checked)
			{
				cbRerollSourceDomain.Checked = false;
				cbRerollSourceDomain.Enabled = false;
			}
			else
			{
				cbRerollSourceDomain.Enabled = true;
			}
		}

		private void cbRerollDomain_CheckedChanged(object sender, EventArgs e)
		{
			CorruptCore.CorruptCore.RerollDomain = cbRerollDomain.Checked;
		}

		private void cbRerollSourceDomain_CheckedChanged(object sender, EventArgs e)
		{
			CorruptCore.CorruptCore.RerollSourceDomain = cbRerollSourceDomain.Checked;
		}

		private void cbRerollAddress_CheckedChanged(object sender, EventArgs e)
		{
			CorruptCore.CorruptCore.RerollAddress = cbRerollAddress.Checked;
			if (!cbRerollAddress.Checked)
			{
				cbRerollDomain.Checked = false;
				cbRerollDomain.Enabled = false;
			}
			else
			{
				cbRerollDomain.Enabled = true;
			}
		}

		private void CbRerollFollowsCustom_CheckedChanged(object sender, EventArgs e)
		{
			CorruptCore.CorruptCore.RerollFollowsCustomEngine = cbRerollFollowsCustom.Checked;
		}

		private void CBRerollIgnoresOriginalSource(object sender, EventArgs e)
		{
			CorruptCore.CorruptCore.RerollIgnoresOriginalSource = cbIgnoreUnitOrigin.Checked;
		}

		public void SetRewindBoxes(bool enabled)
		{
			DontUpdateSpec = true;
			cbClearStepUnitsOnRewind.Checked = true;
			DontUpdateSpec = false;
		}
		public void SetLockBoxes(bool enabled)
		{
			DontUpdateSpec = true;
			cbLockUnits.Checked = true;
			DontUpdateSpec = false;
		}

		public bool DontUpdateSpec;
		private void CbClearStepUnitsOnRewind_CheckedChanged(object sender, EventArgs e)
		{
			if (DontUpdateSpec)
				return;

			S.GET<RTC_CorruptionEngine_Form>().SetRewindBoxes(cbClearStepUnitsOnRewind.Checked);
			S.GET<RTC_CustomEngineConfig_Form>().SetRewindBoxes(cbClearStepUnitsOnRewind.Checked);

			StepActions.ClearStepActionsOnRewind = cbClearStepUnitsOnRewind.Checked;
		}

		private void CbLockUnits_CheckedChanged(object sender, EventArgs e)
		{
			if (DontUpdateSpec)
				return;

			S.GET<RTC_CorruptionEngine_Form>().SetLockBoxes(cbLockUnits.Checked);

			StepActions.LockExecution = cbLockUnits.Checked;
		}
	}
}
