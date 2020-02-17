using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NLog;
using RTCV.Plugins.ScriptHost.Controls;

namespace RTCV.Plugins.ScriptHost
{
    public partial class ScriptHost : Form
    {
        Color DarkerGray = Color.FromArgb(64, 64, 64);
        public ScriptHost()
        {
            InitializeComponent();
            tc.TabClick += TcTabClick;
            var defaultTab = new ScriptManagerTab();
            var addTab = new Manina.Windows.Forms.Tab()
            {
                Name = " + ",
                Text = " + ",
                BackColor = DarkerGray,
                ForeColor = Color.White
            };
            tc.Tabs.Add(defaultTab);
            tc.Tabs.Add(addTab);
        }

        private void TcTabClick(object sender, Manina.Windows.Forms.TabMouseEventArgs e)
        {
            if (this.tc.GetTabBounds(this.tc.Tabs.Last()).Contains(e.Location))
            {
                var newTab = new ScriptManagerTab();
                this.tc.Tabs.Insert(tc.Tabs.Count - 1, newTab);
                this.tc.SelectedTab = newTab;
            }
        }

        private ScriptManagerTab GetCurrentTab()
        {
            var shTab = tc.SelectedTab as ScriptManagerTab;
            return shTab;
        }
        private ScriptManager GetCurrentManager()
        {
            return GetCurrentTab()?.ScriptManager;
        }

        private void SaveScript(string filename = null)
        {
            var manager = GetCurrentManager();
            if (manager == null)
                return;
            var script =  manager.GetScript();

            if (string.IsNullOrWhiteSpace(script))
            {
                MessageBox.Show("Script is empty");
                return;
            }

            if (filename == null)
            {
                SaveFileDialog sfd = new SaveFileDialog();

                sfd.Filter = "C# script files (*.cs)|*.cs|All files (*.*)|*.*";
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    filename = sfd.FileName;
                }
                else
                {
                    return;
                }
            }

            try
            {
                File.WriteAllText(filename, script);
            }
            catch (Exception e)
            {
                RTCV.Common.Logging.GlobalLogger.Error(e, "Unable to save file.");
                MessageBox.Show($"Unable to save file. Error message: {e.Message}");
                return;
            }
            var shortName = Path.GetFileNameWithoutExtension(filename);
            var t = GetCurrentTab();
            t.Name = shortName;
            t.Text = shortName;
        }
        private void LoadScript(string filename = null)
        {
            var tab = new ScriptManagerTab();
            if (filename == null)
            {
                OpenFileDialog sfd = new OpenFileDialog();

                sfd.Filter = "C# script files (*.cs)|*.cs|All files (*.*)|*.*";
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        filename = sfd.FileName;
                    }
                    catch (Exception e)
                    {
                        RTCV.Common.Logging.GlobalLogger.Error(e, "Unable to open file.");
                        MessageBox.Show($"Unable to open file. Error message: {e.Message}");
                        return;
                    }
                }
            }
            tab.ScriptManager.LoadScript(filename);
            var shortName = Path.GetFileNameWithoutExtension(filename);
            tab.Name = shortName;
            tab.Text = shortName;

            tc.Tabs.Insert(tc.Tabs.Count - 1, tab);
            tc.SelectedTab = tab;
        }

        private void loadToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            LoadScript();
        }

        private void saveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SaveScript(GetCurrentManager()?.FilePath);
        }

        private void saveAsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SaveScript();
        }
    }
}
