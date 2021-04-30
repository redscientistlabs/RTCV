namespace RTCV.UI
{
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.Diagnostics;
    using RTCV.CorruptCore;
    using RTCV.Common;
    using RTCV.UI.Modular;


    public partial class MyPluginsForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public MyPluginsForm()
        {
            InitializeComponent();
            AllowDrop = true;
        }



        private void DeletePlugin(object sender, EventArgs e)
        {
            if (lbKnownPlugins.SelectedIndex == -1)
            {
                return;
            }


            if (MessageBox.Show("WARNING\nDeleting a plugin requires RTC to restart.\nThis type of restart doesn't let you save your unchanged stockpile changes so make sure your stuff is already saved.\n\nDo you want to continue?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            foreach (var item in lbKnownPlugins.SelectedItems)
            {
                string listPathDll = Path.Combine(RtcCore.PluginDir, item.ToString().Replace("[DISABLED] ", "$"));
                string listPathPdb = listPathDll.Replace(".dll", ".pdb");
                string standaloneRtcPath = System.Reflection.Assembly.GetEntryAssembly().Location;
                string batchPath = Path.Combine(RtcCore.workingDir, "SESSION", $"{RtcCore.GetRandomKey()}_DeletePlugin.bat");

                string batchScript = @$"
@ECHO OFF
taskkill /F /IM {"\"" + "StandaloneRTC.exe" + "\""}
echo.
echo RTC Will close itself to free the DLL files.
echo.
pause
del {"\"" + listPathDll + "\""}
del {"\"" + listPathPdb + "\""}
start {"\"" + "\""} {"\"" + standaloneRtcPath + "\""}
exit
";

                File.WriteAllText(batchPath, batchScript);

                Process.Start(batchPath);


            }

            RefreshPlugins();
        }

        public void RefreshPlugins()
        {
            lbKnownPlugins.Items.Clear();

            if (!Directory.Exists(RtcCore.PluginDir))
            {
                Directory.CreateDirectory(RtcCore.PluginDir);
            }

            var files = Directory.GetFiles(RtcCore.PluginDir).Where(it => it.EndsWith(".dll")).OrderBy(it => it.Replace("$", ""));
            foreach (var file in files)
            {
                string shortfile = file.Substring(file.LastIndexOf('\\') + 1);
                lbKnownPlugins.Items.Add(shortfile.Replace("$", "[DISABLED] "));
            }

            btnRemoveList.Enabled = false;
        }


        private void OnFormLoad(object sender, EventArgs e)
        {
            RefreshPlugins();
        }

        private void OnKnownListSelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemoveList.Enabled = false;

            if (lbKnownPlugins.SelectedItem == null)
            {
                return;
            }

            btnRemoveList.Enabled = true;

            //bool allDisabled = true;

            foreach (var item in lbKnownPlugins.SelectedItems)
            {
                if (!item.ToString().Contains("[DISABLED] "))
                {
                    //allDisabled = false;
                    break;
                }
            }

        }


        private void RefreshVMDFiles(object sender, EventArgs e)
        {

            RefreshPlugins();
        }

        private void btnRestartRTC_Click(object sender, EventArgs e)
        {
            string standaloneRtcPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            string batchPath = Path.Combine(RtcCore.workingDir, "SESSION", $"{RtcCore.GetRandomKey()}_DeletePlugin.bat");

            string batchScript = @$"
@ECHO OFF
taskkill /F /IM {"\"" + "StandaloneRTC.exe" + "\""}
start {"\"" + "\""} {"\"" + standaloneRtcPath + "\""}
exit
";

            File.WriteAllText(batchPath, batchScript);

            Process.Start(batchPath);

        }

        private void btnImportList_Click(object sender, EventArgs e)
        {
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            Process.Start(RtcCore.PluginDir);
        }
    }
}
