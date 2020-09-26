namespace RTCV.UI
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public partial class ConnectionStatusForm : ComponentForm, IBlockable
    {
        public ConnectionStatusForm()
        {
            InitializeComponent();
            this.Shown += OnFormShown;
            this.btnTriggerKillswitch.MouseClick += OnTriggerKillswitchMouseClick;
        }

        private readonly string[] _flavorText = {
            "Imagine if we had actual flavor text",
            "Fun flavor text goes here",
            "The V probably stands for Vanguard",
            "Now with naming conventions!",
            "Over 45,000 lines of code to make Mario do the funny",
            "We love circular references",
            "Don't show this to your epileptic friends",
            "Clown vomit, spikes and needles",
            "My mom says to blow in the cartridge so it works best",
            "The Vanguard of unreliable sloshy behavior r&&d innovation for the thrill of gamers",
            "Where are all these plates coming from anyway",
            "Preparing more plates, please standby",
            "Unpredictable and profoundly cursed",
            "Violates every design paradigm in the book",
        };

        private void OnFormLoad(object sender, EventArgs e)
        {
            int crashSound = 0;

            if (NetCore.Params.IsParamSet("CRASHSOUND"))
            {
                crashSound = Convert.ToInt32(NetCore.Params.ReadParam("CRASHSOUND"));
            }

            S.GET<SettingsNetCoreForm>().cbCrashSoundEffect.SelectedIndex = crashSound;
        }

        private void OnFormShown(object sender, EventArgs e)
        {
            lbFlavorText.Text = _flavorText[CorruptCore.RtcCore.RND.Next(0, _flavorText.Length)];
        }

        private void EmergencySaveAsStockpile(object sender, EventArgs e)
        {
            S.GET<StockpileManagerForm>().SaveStockpileAs(null, null);
        }

        private void OnTriggerKillswitchMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point locate = e.GetMouseLocation(sender);
                ContextMenuStrip columnsMenu = new ContextMenuStrip();
                columnsMenu.Items.Add("Open Debug window", null, new EventHandler((ob, ev) => { CoreForm.ForceCloudDebug(); }));
                columnsMenu.Show(this, locate);
                return;
            }

            S.GET<CoreForm>().pbAutoKillSwitchTimeout.Value = S.GET<CoreForm>().pbAutoKillSwitchTimeout.Maximum;
            AutoKillSwitch.KillEmulator(true);
        }

        private void BreakCrashLoop(object sender, EventArgs e)
        {
            S.GET<CoreForm>().cbUseAutoKillSwitch.Checked = false;
            AutoKillSwitch.KillEmulator(true);
        }
    }
}
