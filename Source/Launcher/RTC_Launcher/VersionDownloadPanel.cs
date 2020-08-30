using RTCV.Launcher.Components;

namespace RTCV.Launcher
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class VersionDownloadPanel : Form
    {
        public string latestVersionString = " (Latest version)";
        private List<dynamic> onlineVersionsObjects = null;

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
            try
            {
                byte[] versionFile = MainForm.GetFileViaHttp(new Uri($"{MainForm.webRessourceDomain}/rtc/releases/version.php"));
                if (versionFile == null)
                    return null;

                var str = Encoding.UTF8.GetString(versionFile);
                var onlineVersions = new List<string>(str.Split('|').Where(it => !it.Contains("Launcher")).ToArray());

                var returnValue = onlineVersions.OrderByNaturalDescending(x => x).Select(it => it.Replace(".zip", "")).ToArray()[0];

                return returnValue;
            }
            catch
            {
                return null;
            }
        }


        public void refreshVersions()
        {
            Action a = () =>
            {
                byte[] versionFile = MainForm.GetFileViaHttp(new Uri($"{MainForm.webRessourceDomain}/rtc/releases/version.php"));

                if (versionFile == null)
                    return;

                var str = Encoding.UTF8.GetString(versionFile);

                //Ignores any build containing the word Launcher in it
                string[] onlineVersions = str.Split('|').Where(it => !it.Contains("Launcher")).OrderByNaturalDescending(x => x).Select(it => it.Replace(".zip", "")).ToArray();
                Invoke(new MethodInvoker(() =>
                {
                    onlineVersionsObjects = new List<dynamic>();

                    lbOnlineVersions.Items.Clear();
                    if (onlineVersions.Length > 0)
                    {
                        for (var i = 0; i < onlineVersions.Length; i++)
                        {
                            var value = onlineVersions[i];

                            if (i == 0)
                                onlineVersions[i] += latestVersionString;

                            var key = onlineVersions[i];

                            onlineVersionsObjects.Add(new {key = key, value = value});
                        }
                    }

                    lbOnlineVersions.DataSource = null;
                    lbOnlineVersions.DataSource = onlineVersionsObjects;

                    //lbOnlineVersions.Items.AddRange(onlineVersionsTuples);
                }));
            };
            Task.Run(a);
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

            dynamic itemData = lbOnlineVersions.SelectedItem;

            string version = itemData.value;
            version = version.Replace(latestVersionString, "");

            if (Directory.Exists(MainForm.launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version))
            {
                if (MessageBox.Show($"The version {version} is already installed.\nThis will DELETE version {version} and redownload it.\n\nWould you like to continue?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes)
                {
                    Directory.Delete(MainForm.launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version, true);
                }
                else
                {
                    return;
                }
            }

            var downloadUrl = $"{MainForm.webRessourceDomain}/rtc/releases/" + version + ".zip";
            var downloadedFile = MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar + version + ".zip";
            var extractDirectory = MainForm.launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version;

            MainForm.lastSelectedVersion = version;

            MainForm.DownloadFile(new Uri(downloadUrl), downloadedFile, extractDirectory);
        }

        private int devCounter = 0;

        private void cbDevBuids_CheckedChanged(object sender, EventArgs e)
        {
            if (!Visible)
                return;

            var devOn = File.Exists(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\dev.txt");

            if (!devOn && devCounter % 2 == 0)
                Console.Beep(220 + 20 * devCounter, 100);

            devCounter++;


            if (devCounter >= 20 || devOn)
            {
                if (!devOn)
                {
                    Console.Beep(220, 100);
                    Console.Beep(300, 100);
                    Console.Beep(400, 100);
                    Console.Beep(520, 108);
                }

                if (!devOn && MessageBox.Show(File.Exists(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\dev.txt") ? "Do you want to stay connected to the Dev Server?" : "Do you want to connect to the Dev Server?",
                    "Dev mode activation", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
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

        private void lbOnlineVersions_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            btnDownloadVersion_Click(sender, e);
        }

        private void lbOnlineVersions_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);

                var columnsMenu = new BuildContextMenu();
                columnsMenu.Items.Add("Download", null, new EventHandler((ob, ev) => { btnDownloadVersion_Click(sender, e); }));
                columnsMenu.Show(this, locate);
            }
        }
    }
}
