namespace RTCV.Plugins.ScriptHost
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using NLog;
    using RTCV.Plugins.ScriptHost.Controls;

    public partial class ScriptHost : Form
    {
        public ScriptHost()
        {
            InitializeComponent();
            var defaultTab = new ScriptManagerTab();
            tc.Tabs.Add(defaultTab);
            tc.MouseDoubleClick += Tc_MouseDoubleClick;
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

        private void AddTab()
        {
            var newTab = new ScriptManagerTab();
            this.tc.Tabs.Add(newTab);
            this.tc.SelectedTab = newTab;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTab();
        }

        private void Tc_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (tc.TabArea.Contains(e.Location) && !tc.Tabs.All(x => x.ClientRectangle.Contains(e.Location)))
            {
                AddTab();
            }
        }
    }
}
