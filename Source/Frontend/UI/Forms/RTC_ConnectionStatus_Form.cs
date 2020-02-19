namespace RTCV.UI
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using RTCV.Common;
    using static RTCV.UI.UI_Extensions;

    public partial class RTC_ConnectionStatus_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public RTC_ConnectionStatus_Form()
        {
            InitializeComponent();
            this.Shown += RTC_ConnectionStatus_Form_Shown;
            this.btnTriggerKillswitch.MouseClick += BtnTriggerKillswitch_MouseClick;
        }

        private readonly string[] _flavorText = {
            "Imagine if we had actual flavor text",
            "Fun flavor text goes here",
            "The V probably stands for Vanguard",
            "What are naming conventions",
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

        private void RTC_ConnectionStatus_Form_Load(object sender, EventArgs e)
        {
            int crashSound = 0;

            if (NetCore.Params.IsParamSet("CRASHSOUND"))
            {
                crashSound = Convert.ToInt32(NetCore.Params.ReadParam("CRASHSOUND"));
            }

            S.GET<RTC_SettingsNetCore_Form>().cbCrashSoundEffect.SelectedIndex = crashSound;
        }

        private void RTC_ConnectionStatus_Form_Shown(object sender, EventArgs e)
        {
            lbFlavorText.Text = _flavorText[CorruptCore.RtcCore.RND.Next(0, _flavorText.Length)];
        }

        private void BtnEmergencySaveAs_Click(object sender, EventArgs e)
        {
            S.GET<RTC_StockpileManager_Form>().btnSaveStockpileAs_Click(null, null);
        }

        private void BtnTriggerKillswitch_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point locate = e.GetMouseLocation(sender);
                ContextMenuStrip columnsMenu = new ContextMenuStrip();
                columnsMenu.Items.Add("Open Debug window", null, new EventHandler((ob, ev) => { UI_CoreForm.ForceCloudDebug(); }));
                columnsMenu.Show(this, locate);
                return;
            }

            S.GET<UI_CoreForm>().pbAutoKillSwitchTimeout.Value = S.GET<UI_CoreForm>().pbAutoKillSwitchTimeout.Maximum;
            AutoKillSwitch.KillEmulator(true);
        }

        private void btnBreakCrashLoop_Click(object sender, EventArgs e)
        {
            S.GET<UI_CoreForm>().cbUseAutoKillSwitch.Checked = false;
            AutoKillSwitch.KillEmulator(true);
        }
    }
}
