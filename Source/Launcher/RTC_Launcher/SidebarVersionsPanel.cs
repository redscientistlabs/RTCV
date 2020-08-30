namespace RTCV.Launcher
{
    using System;
    using System.Windows.Forms;

    public partial class SidebarVersionsPanel : Form
    {
        public SidebarVersionsPanel()
        {
            InitializeComponent();
        }

        private void lbVersions_SelectedIndexChanged(object sender, EventArgs e) => MainForm.mf.lbVersions_SelectedIndexChanged(sender, e);

        private void lbVersions_MouseDown(object sender, MouseEventArgs e) => MainForm.mf.lbVersions_MouseDown(sender, e);

        private void SidebarVersionsPanel_Load(object sender, EventArgs e)
        {
            VerticalScroll.Enabled = true;
        }
    }
}
