using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTCV.Launcher
{
    public partial class SidebarVersionsPanel : Form
    {
        public SidebarVersionsPanel()
        {
            InitializeComponent();
            //lbVersions.AutoSize = true;
        }


        private void lbVersions_SelectedIndexChanged(object sender, EventArgs e) => MainForm.mf.lbVersions_SelectedIndexChanged(sender, e);

        private void lbVersions_MouseDown(object sender, MouseEventArgs e) => MainForm.mf.lbVersions_MouseDown(sender, e);


        private void SidebarVersionsPanel_Load(object sender, EventArgs e)
        {
            VerticalScroll.Enabled = true;
        }
    }
}
