namespace RTCV.UI
{
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Media;
    using System.Windows.Forms;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public partial class SettingsNetCoreForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public SettingsNetCoreForm()
        {
            InitializeComponent();
        }

        private void OnCrashSoundeffectChange(object sender, EventArgs e)
        {
            switch (cbCrashSoundEffect.SelectedIndex)
            {
                case 0:
                    var PlatesHdFiles = Directory.GetFiles(Path.Combine(CorruptCore.RtcCore.AssetsDir, "PLATESHD"));
                    AutoKillSwitch.LoadedSounds = PlatesHdFiles.Select(it => new SoundPlayer(it)).ToArray();
                    break;
                case 1:
                    AutoKillSwitch.LoadedSounds = new SoundPlayer[] { new SoundPlayer(Path.Combine(CorruptCore.RtcCore.AssetsDir, "crash.wav")) };
                    break;

                case 2:
                    AutoKillSwitch.LoadedSounds = null;
                    break;
                case 3:
                    var CrashSoundsFiles = Directory.GetFiles(Path.Combine(CorruptCore.RtcCore.AssetsDir, "CRASHSOUNDS"));
                    AutoKillSwitch.LoadedSounds = CrashSoundsFiles.Select(it => new SoundPlayer(it)).ToArray();
                    break;
            }

            NetCore.Params.SetParam("CRASHSOUND", cbCrashSoundEffect.SelectedIndex.ToString());
        }

        private void OnGameProtectionDelayChange(object sender, EventArgs e) => UpdateGameProtectionDelay();

        public static void UpdateGameProtectionDelay()
        {
            GameProtection.BackupInterval = Convert.ToInt32(S.GET<SettingsNetCoreForm>().nmGameProtectionDelay.Value);
            if (GameProtection.isRunning)
            {
                GameProtection.Reset(false);
            }
        }

        private void OnGameProtectionDelayChange(object sender, KeyPressEventArgs e)
        {

        }

        private void OnGameProtectionDelayChange(object sender, KeyEventArgs e)
        {

        }
    }
}
