using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RTCV.Plugins.ScriptHost
{
    public partial class ScriptHost : Form
    {
        Color DarkerGray = Color.FromArgb(64, 64, 64);
        public ScriptHost()
        {
            InitializeComponent();
            tabControl1.PageAdded += TabControl1_PageAdded;
            tabControl1.TabClick += TabControl1_TabClick;
            var defaultTab = new Manina.Windows.Forms.Tab()
            {
                Name = "Script 1",
                Text = "Script 1",
                BackColor = DarkerGray,
                ForeColor = Color.White
            };
            var addTab = new Manina.Windows.Forms.Tab()
            {
                Name = " + ",
                Text = " + ",
                BackColor = DarkerGray,
                ForeColor = Color.White
            };
            tabControl1.Tabs.Add(defaultTab);
            tabControl1.Tabs.Add(addTab);
        }

        private void TabControl1_TabClick(object sender, Manina.Windows.Forms.TabMouseEventArgs e)
        {
            if (this.tabControl1.GetTabBounds(this.tabControl1.Tabs.Last()).Contains(e.Location))
            {
                var newTab = new Manina.Windows.Forms.Tab()
                {
                    Name = $"Script {tabControl1.Tabs.Count}",
                    Text = $"Script {tabControl1.Tabs.Count}",
                    BackColor = DarkerGray,
                    ForeColor = Color.White
                };
                this.tabControl1.Tabs.Insert(this.tabControl1.Tabs.Count - 1, newTab);
                this.tabControl1.SelectedTab = newTab;
            }
        }

        private void TabControl1_PageAdded(object sender, Manina.Windows.Forms.PageEventArgs e)
        {
            var sm = new RTCV.Plugins.ScriptHost.Controls.ScriptManager();
            sm.Dock = DockStyle.Fill;
            e.Page.Controls.Add(sm);
        }
    }
}
