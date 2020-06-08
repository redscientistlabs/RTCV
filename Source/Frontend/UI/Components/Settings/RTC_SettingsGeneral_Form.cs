namespace RTCV.UI
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.Common;
    using static RTCV.UI.UI_Extensions;

    public partial class RTC_SettingsGeneral_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public RTC_SettingsGeneral_Form()
        {
            InitializeComponent();

            popoutAllowed = false;
        }

        //todo - rewrite this?
        /*
        private void btnImportKeyBindings_Click(object sender, EventArgs e)
        {

            if (UI_VanguardImplementation.connector.netConn.status != NetworkStatus.CONNECTED)
            {
                MessageBox.Show("Can't import keybindings when not connected to Bizhawk!");
                return;
            }

            try
            {
                if (CorruptCore.CorruptCore.EmuDir.Contains(Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar))
                {
                    var bizhawkFolder = new DirectoryInfo(CorruptCore.CorruptCore.EmuDir);
                    var LauncherVersFolder = bizhawkFolder.Parent.Parent;

                    var versions = LauncherVersFolder.GetDirectories().Reverse().ToArray();

                    var prevVersion = versions[1].Name;

                    var dr = MessageBox.Show(
                        "RTC Launcher detected,\n" +
                        $"Do you want to import Controller/Hotkey bindings from version {prevVersion}"
                        , $"Import config from previous version ?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (dr == DialogResult.Yes)
                        Stockpile.LoadBizhawkKeyBindsFromIni(versions[1].FullName + Path.DirectorySeparatorChar + "BizHawk\\config.ini");
                    else
                        Stockpile.LoadBizhawkKeyBindsFromIni();
                }
                else
                    Stockpile.LoadBizhawkKeyBindsFromIni();
            }
            finally
            {
            }
        }*/

        private void btnOpenOnlineWiki_Click(object sender, EventArgs e)
        {
            Process.Start("https://corrupt.wiki/");
        }

        private void btnChangeRTCColor_Click(object sender, EventArgs e)
        {
            UICore.SelectRTCColor();
        }

        private void cbDisableBizhawkOSD_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDisableEmulatorOSD.Checked)
            {
                RTCV.NetCore.Params.SetParam(RTCSPEC.CORE_EMULATOROSDDISABLED);
            }
            else
            {
                RTCV.NetCore.Params.RemoveParam(RTCSPEC.CORE_EMULATOROSDDISABLED);
            }

            CorruptCore.RtcCore.EmulatorOsdDisabled = cbDisableEmulatorOSD.Checked;
        }

        private void cbAllowCrossCoreCorruption_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllowCrossCoreCorruption.Checked)
            {
                RTCV.NetCore.Params.SetParam("ALLOW_CROSS_CORE_CORRUPTION");
            }
            else
            {
                RTCV.NetCore.Params.RemoveParam("ALLOW_CROSS_CORE_CORRUPTION");
            }

            CorruptCore.RtcCore.AllowCrossCoreCorruption = cbAllowCrossCoreCorruption.Checked;
        }

        private void cbDontCleanAtQuit_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDontCleanAtQuit.Checked)
            {
                RTCV.NetCore.Params.SetParam("DONT_CLEAN_SAVESTATES_AT_QUIT");
            }
            else
            {
                RTCV.NetCore.Params.RemoveParam("DONT_CLEAN_SAVESTATES_AT_QUIT");
            }

            CorruptCore.RtcCore.DontCleanSavestatesOnQuit = cbDontCleanAtQuit.Checked;
        }

        private void CbUncapIntensity_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUncapIntensity.Checked)
            {
                RTCV.NetCore.Params.SetParam("UNCAP_INTENSITY");
            }
            else
            {
                RTCV.NetCore.Params.RemoveParam("UNCAP_INTENSITY");
            }

            S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.UncapNumericBox = cbUncapIntensity.Checked;
            S.GET<RTC_GlitchHarvesterIntensity_Form>().multiTB_Intensity.UncapNumericBox = cbUncapIntensity.Checked;
        }

        private void btnRefreshInputDevices_Click(object sender, EventArgs e)
        {
            Input.Input.Initialize();
        }
    }
}
