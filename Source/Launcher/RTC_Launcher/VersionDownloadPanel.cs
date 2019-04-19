using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTC_Launcher
{
    public partial class VersionDownloadPanel : Form
    {
        public VersionDownloadPanel()
        {
            InitializeComponent();
            cbDevBuids.Checked = File.Exists(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\dev.txt");

            if (cbDevBuids.Checked)
                lbOnlineVersions.BackColor = Color.FromArgb(32, 16, 16);

        }

        private void VersionDownloadPanel_Load(object sender, EventArgs e)
        {
            refreshVersions();
        }

        public static string getLatestVersion()
        {
            try { 
                var versionFile = MainForm.GetFileViaHttp($"{MainForm.webRessourceDomain}/rtc/releases/version.php");
                string str = Encoding.UTF8.GetString(versionFile);
                List<string> onlineVersions = new List<string>(str.Split('|').Where(it => !it.Contains("Launcher")).ToArray());

                return onlineVersions.OrderByDescending(x => x).Select(it => it.Replace(".zip", "")).ToArray()[0];
            }
            catch
            {
                return null;
            }
        }

        public void refreshVersions()
        {
            var versionFile = MainForm.GetFileViaHttp($"{MainForm.webRessourceDomain}/rtc/releases/version.php");
            string str = Encoding.UTF8.GetString(versionFile);

            //Ignores any build containing the word Launcher in it
            List<string> onlineVersions = new List<string>(str.Split('|').Where(it => !it.Contains("Launcher")).ToArray());

            lbOnlineVersions.Items.Clear();
            lbOnlineVersions.Items.AddRange(onlineVersions.OrderByDescending(x => x).Select(it => it.Replace(".zip", "")).ToArray());

        }

        private void lbOnlineVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbOnlineVersions.SelectedIndex == -1)
                return;

            btnDownloadVersion.Visible = true;

        }

        private void btnDownloadVersion_Click(object sender, EventArgs e)
        {
            if (lbOnlineVersions.SelectedIndex == -1)
                return;

            string version = lbOnlineVersions.SelectedItem.ToString();

            if (Directory.Exists((MainForm.launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version)))
            {
                if (MessageBox.Show($"The version {version} is already installed.\nThis will DELETE version {version} and redownload it.\n\nWould you like to continue?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Directory.Delete(MainForm.launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version, true);
                }
                else
                {
                    return;
                }
            }

            string downloadUrl = $"{MainForm.webRessourceDomain}/rtc/releases/" + version + ".zip";
            string downloadedFile = MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar + version + ".zip";
            string extractDirectory = MainForm.launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version;

            MainForm.mf.DownloadFile(downloadUrl, downloadedFile, extractDirectory);



        }

        int devCounter = 0;
        private void cbDevBuids_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;


            devCounter++;
            bool devOn = File.Exists(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\dev.txt");

            if (devCounter >= 20 || devOn)
            {
                if (!devOn && MessageBox.Show((File.Exists(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\dev.txt") ? "Do you want to stay connected to the Dev Server?" : "Do you want to connect to the Dev Server?"), "Dev mode activation", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    File.WriteAllText(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\dev.txt", "DEV MODE ACTIVATED");
                    Application.Restart();
                }
                else
                {
                    if (devOn)
                    {
                        if (File.Exists(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\dev.txt"))
                            File.Delete(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\dev.txt");

                        Application.Restart();
                    }
                    else
                    {
                        devCounter = 0;
                        cbDevBuids.Checked = devOn;
                    }
                }
            }
            else
            {
                cbDevBuids.Checked = devOn;
            }

        }
    }
}
