namespace RTCV.UI
{
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.Diagnostics;
    using RTCV.CorruptCore;
    using RTCV.UI.Modular;
    using RTCV.PluginHost;
    using System.Dynamic;

    public partial class MyPluginsForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public MyPluginsForm()
        {
            InitializeComponent();
            AllowDrop = true;
        }





        public void RefreshPlugins()
        {
            lbKnownPlugins.Items.Clear();

            if (!Directory.Exists(RtcCore.PluginDir))
            {
                Directory.CreateDirectory(RtcCore.PluginDir);
            }

            var orderedPlugins = PluginHost.Manager.GetPlugins().OrderBy(it => it.ToString());
            foreach (var plugin in orderedPlugins)
            {
                //dynamic item = new ExpandoObject();
                //item.value = plugin;
                //item.text = plugin.ToString();
                lbKnownPlugins.Items.Add(new { value = plugin, text = plugin.ToString()});
            }

            //var cmbDatasource = (from plugin in 
            //                     select new { value = plugin, text = plugin.ToString() }).ToList();

            //lbKnownPlugins.DataSource = cmbDatasource;
            lbKnownPlugins.ValueMember = "value";
            lbKnownPlugins.DisplayMember = "text";

        }


        private void OnFormLoad(object sender, EventArgs e)
        {
            RefreshPlugins();
        }

        private void OnKnownListSelectedIndexChanged(object sender, EventArgs e)
        {

            if (lbKnownPlugins.SelectedIndex == -1)
            {
                btnRemoveList.Visible = false;
                btnEnableDisableList.Visible = false;
                return;
            }

            btnRemoveList.Visible = true;
            btnEnableDisableList.Visible = true;

            PluginInfo pluginInfo;
            if (lbKnownPlugins.SelectedItem is PluginInfo pi)
                pluginInfo = pi;
            else
                pluginInfo = ((lbKnownPlugins.SelectedItem as dynamic).value as PluginInfo);


            switch (pluginInfo.Status)
            {
                case "ENABLED":
                    btnEnableDisableList.Text = "  Disable Plugin";
                    btnRemoveList.Text = "  Delete Plugin";
                    break;
                case "DISABLED":
                    btnEnableDisableList.Text = "  Enable Plugin";
                    btnRemoveList.Text = "  Delete Plugin";
                    break;
                case "DELETE":
                    btnEnableDisableList.Visible = false;
                    btnRemoveList.Text = "  Restore Plugin";
                    break;
            }
        }


        private void RefreshVMDFiles(object sender, EventArgs e)
        {

            RefreshPlugins();
        }

        private void btnRestartRTC_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("WARNING\nRestarting RTC this way does not save your stockpile. Continue restart?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                RefreshPlugins();
                return;
            }

            string standaloneRtcPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            var oldEmuDir = CorruptCore.RtcCore.EmuDir;
            string emuRestartPath = Path.Combine(oldEmuDir, "RESTARTDETACHEDRTC.bat");
            string batchPath = Path.Combine(RtcCore.workingDir, "SESSION", $"{RtcCore.GetRandomKey()}_DeletePlugin.bat");

            string batchScript = @$"
@ECHO OFF
taskkill /F /IM {"\"" + "StandaloneRTC.exe" + "\""}

start {"\"" + "\""} {"\"" + standaloneRtcPath + "\""}
ping 127.0.0.1 -n 2 -w 2000 > NUL
cd {"\"" + new FileInfo(emuRestartPath).DirectoryName + "\""}
start {"\"" + "\""} {"\"" + emuRestartPath + "\""}
exit
";

            File.WriteAllText(batchPath, batchScript);
            AutoKillSwitch.KillEmulator(true,true);
            Process.Start(batchPath);
        }


        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            Process.Start(RtcCore.PluginDir);
        }

        bool restartAsked = false;
        private void btnEnableDisableList_Click(object sender, EventArgs e)
        {
            if (lbKnownPlugins.SelectedIndex == -1)
            {
                return;
            }

            PluginInfo pluginInfo;
            if (lbKnownPlugins.SelectedItem is PluginInfo pi)
                pluginInfo = pi;
            else
                pluginInfo = ((lbKnownPlugins.SelectedItem as dynamic).value as PluginInfo);

            if (pluginInfo.Status == "DISABLED")
            {
                pluginInfo.MarkForEnabled();
            }
            else
            {
                pluginInfo.MarkForDisabled();
            }

            if (restartAsked || MessageBox.Show("You need to Restart RTC to apply changes. Do it now?", "Restart needed", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                restartAsked = true;
                btnRestartRTC.Visible = true;
                RefreshPlugins();
                return;
            }

            btnRestartRTC_Click(null, null);
        }

        private void DeletePlugin(object sender, EventArgs e)
        {
            if (lbKnownPlugins.SelectedIndex == -1)
            {
                return;
            }

            PluginInfo pluginInfo;
            if (lbKnownPlugins.SelectedItem is PluginInfo pi)
                pluginInfo = pi;
            else
                pluginInfo = ((lbKnownPlugins.SelectedItem as dynamic).value as PluginInfo);

            if (pluginInfo.Status == "DELETE")
            {
                pluginInfo.MarkForEnabled();
            }
            else
            {
                pluginInfo.MarkForDeletion();
            }


            if (restartAsked || MessageBox.Show("You need to Restart RTC to apply changes. Do it now?", "Restart needed", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                restartAsked = true;
                RefreshPlugins();
                btnRestartRTC.Visible = true;
                return;
            }

            btnRestartRTC_Click(null, null);
        }
    }
}
