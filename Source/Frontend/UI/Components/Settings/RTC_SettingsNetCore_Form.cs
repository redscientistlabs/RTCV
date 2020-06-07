namespace RTCV.UI
{
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Media;
    using System.Windows.Forms;
    using RTCV.Common;
    using static RTCV.UI.UI_Extensions;

    public partial class RTC_SettingsNetCore_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public RTC_SettingsNetCore_Form()
        {
            InitializeComponent();
        }

        private void cbCrashSoundEffect_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbCrashSoundEffect.SelectedIndex)
            {
                case 0:
                    var PlatesHdFiles = Directory.GetFiles(Path.Combine(CorruptCore.RtcCore.assetsDir, "PLATESHD"));
                    AutoKillSwitch.LoadedSounds = PlatesHdFiles.Select(it => new SoundPlayer(it)).ToArray();
                    break;
                case 1:
                    AutoKillSwitch.LoadedSounds = new SoundPlayer[] { new SoundPlayer(Path.Combine(CorruptCore.RtcCore.assetsDir, "crash.wav")) };
                    break;

                case 2:
                    AutoKillSwitch.LoadedSounds = null;
                    break;
                case 3:
                    var CrashSoundsFiles = Directory.GetFiles(Path.Combine(CorruptCore.RtcCore.assetsDir, "CRASHSOUNDS"));
                    AutoKillSwitch.LoadedSounds = CrashSoundsFiles.Select(it => new SoundPlayer(it)).ToArray();
                    break;
            }

            RTCV.NetCore.Params.SetParam("CRASHSOUND", cbCrashSoundEffect.SelectedIndex.ToString());
        }

        private void nmGameProtectionDelay_ValueChanged(object sender, KeyPressEventArgs e)
        {
            UpdateGameProtectionDelay();
        }

        private void nmGameProtectionDelay_ValueChanged(object sender, KeyEventArgs e)
        {
            UpdateGameProtectionDelay();
        }

        private void nmGameProtectionDelay_ValueChanged(object sender, EventArgs e)
        {
            UpdateGameProtectionDelay();
        }

        public void UpdateGameProtectionDelay()
        {
            GameProtection.BackupInterval = Convert.ToInt32(S.GET<RTC_SettingsNetCore_Form>().nmGameProtectionDelay.Value);
            if (GameProtection.isRunning)
            {
                GameProtection.Reset(false);
            }
        }

        private void RTC_SettingsNetCore_Form_Load(object sender, EventArgs e)
        {
        }
    }
}
