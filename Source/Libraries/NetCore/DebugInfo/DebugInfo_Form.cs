namespace RTCV.NetCore
{
    using System;
    using System.Windows.Forms;

    public partial class DebugInfo_Form : Form
    {
        public DebugInfo_Form()
        {
            InitializeComponent();
            this.FormClosing += RTC_Debug_Form_FormClosing;
            this.Focus();
            this.BringToFront();
        }

        private void RTC_Debug_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void btnGetDebugRTC_Click(object sender, EventArgs e) => tbRTC.Text = CloudDebug.getRTCInfo();

        private void btnGetDebugEmu_Click(object sender, EventArgs e) => richTextBox2.Text = CloudDebug.getEmuInfo();
    }
}
