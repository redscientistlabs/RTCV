using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace RTC_Launcher
{
    public partial class MainForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();



        public static string launcherDir = Directory.GetCurrentDirectory();
        public static string webRessourceDomain = "http://redscientist.com/software";

        public static MainForm mf = null;
        public static VersionDownloadPanel vdppForm = null;
        public static DownloadForm dForm = null;
        public static Form lpForm = null;

        public static int launcherVer = 4;


        public static int devCounter = 0;
        internal static string SelectedVersion = null;

        public MainForm()
        {
            InitializeComponent();

            mf = this;

            //creating default folders
            if (!Directory.Exists(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar))
                Directory.CreateDirectory(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar);

            if (!Directory.Exists(launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar))
                Directory.CreateDirectory(launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar);

            if (File.Exists(launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\dev.txt"))
                webRessourceDomain = "http://cc.r5x.cc";



            //Will trigger after an update from the original launcher
            if (Directory.Exists(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + "Update_Launcher"))
            {
                Directory.Delete(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + "Update_Launcher", true);
                if (File.Exists(launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar + "Update_Launcher.zip"))
                    File.Delete(launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar + "Update_Launcher.zip");
            }

        }

        public void DownloadFile(string downloadURL, string downloadedFile, string extractDirectory)
        {
            MainForm.mf.clearAnchorRight();

            MainForm.dForm = new DownloadForm(downloadURL, downloadedFile, extractDirectory);

            MainForm.mf.pnLeftSide.Visible = false;


            MainForm.dForm.TopLevel = false;
            MainForm.dForm.Location = new Point(0, 0);
            MainForm.dForm.Dock = DockStyle.Fill;
            MainForm.mf.Controls.Add(MainForm.dForm);
            MainForm.dForm.Show();
            MainForm.dForm.Focus();
            MainForm.dForm.BringToFront();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            RefreshInstalledVersions();


            try
            {

                var motdFile = GetFileViaHttp($"{MainForm.webRessourceDomain}/rtc/releases/MOTD.txt");
                string motd = Encoding.UTF8.GetString(motdFile);

                lbMOTD.Text = motd;

            }
            catch
            {
                lbMOTD.Text = "Couldn't load the RTC MOTD from Redscientist.com";
            }

            lbMOTD.Visible = true;

            SetRTCColor(Color.FromArgb(120, 180, 155));

        }

        public void SetRTCColor(Color color, Form form = null)
        {
            //Recolors all the RTC Forms using the general skin color

            List<Control> allControls = new List<Control>();

            if (form == null)
            {

                allControls.AddRange(this.Controls.getControlsWithTag());
                allControls.Add(this);


            }
            else
                allControls.AddRange(form.Controls.getControlsWithTag());

            var lightColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:light"));
            var normalColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:normal"));
            var darkColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:dark"));
            var darkerColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:darker"));

            foreach (Control c in lightColorControls)
                c.BackColor = color.ChangeColorBrightness(0.30f);

            foreach (Control c in normalColorControls)
                c.BackColor = color;

            //spForm.dgvStockpile.BackgroundColor = color;
            //ghForm.dgvStockpile.BackgroundColor = color;

            foreach (Control c in darkColorControls)
                c.BackColor = color.ChangeColorBrightness(-0.30f);

            foreach (Control c in darkerColorControls)
                c.BackColor = color.ChangeColorBrightness(-0.75f);

        }


        public void RefreshInstalledVersions()
        {
            lbVersions.Items.Clear();
            List<string> versions = new List<string>(Directory.GetDirectories(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar));
            lbVersions.Items.AddRange(versions.OrderByDescending(x => x).Select(it => getFilenameFromFullFilename(it)).ToArray<object>());
            SelectedVersion = null;

            string latestVersion = VersionDownloadPanel.getLatestVersion();
            pbNewVersionNotification.Visible = !versions.Select(it => it.Substring(it.LastIndexOf('\\') + 1)).Contains(latestVersion);


        }

        public static byte[] GetFileViaHttp(string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadData(url);
            }
        }

        public string getFilenameFromFullFilename(string fullFilename)
        {
            return fullFilename.Substring(fullFilename.LastIndexOf('\\') + 1);
        }

        public string removeExtension(string filename)
        {
            return filename.Substring(0, filename.LastIndexOf('.'));
        }


        private void lbVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearAnchorRight();


            if (lbVersions.SelectedIndex == -1)
            {
                SelectedVersion = null;
                return;
            }
            else
            {
                SelectedVersion = lbVersions.SelectedItem.ToString();
            }

            if (Directory.Exists(MainForm.launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + SelectedVersion + Path.DirectorySeparatorChar + "Launcher"))
                MainForm.lpForm = new NewLaunchPanel();
            else
                MainForm.lpForm = new OldLaunchPanel();


            MainForm.lpForm.Size = pnAnchorRight.Size;
            MainForm.lpForm.TopLevel = false;
            pnAnchorRight.Controls.Add(MainForm.lpForm);

            MainForm.lpForm.Dock = DockStyle.Fill;
            MainForm.lpForm.Show();
        }



        public void InvokeUI(Action a)
        {
            this.BeginInvoke(new MethodInvoker(a));
        }

        public void DownloadComplete(string downloadedFile, string extractDirectory)
        {

            if (!Directory.Exists(extractDirectory))
                Directory.CreateDirectory(extractDirectory);

            try { 
            System.IO.Compression.ZipFile.ExtractToDirectory(downloadedFile, extractDirectory);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"An error occurred during extraction, rolling back changes.\n\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (Directory.Exists(extractDirectory))
                    Directory.Delete(extractDirectory, true);

            }

            if (File.Exists(downloadedFile))
                File.Delete(downloadedFile);


            if(File.Exists(extractDirectory + Path.DirectorySeparatorChar + "Launcher\\ver.ini"))
            {
                int newVer = Convert.ToInt32(File.ReadAllText(extractDirectory + Path.DirectorySeparatorChar + "Launcher\\ver.ini"));
                if(newVer > launcherVer)
                {
                    var result = MessageBox.Show("The downloaded package contains a new launcher update.\n\nDo you want to update the Launcher?", "Launcher update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(result == DialogResult.Yes)
                    {
                        string batchLocation = extractDirectory + Path.DirectorySeparatorChar + "Launcher\\update.bat";
                        ProcessStartInfo psi = new ProcessStartInfo();
                        psi.FileName = Path.GetFileName(batchLocation);
                        psi.WorkingDirectory = Path.GetDirectoryName(batchLocation);
                        Process.Start(psi);
                        Application.Exit();
                    }
                }
            }


            lbVersions.SelectedIndex = -1;

            RefreshInstalledVersions();

            MainForm.mf.pnLeftSide.Visible = true;

            if(MainForm.vdppForm != null)
            {
                MainForm.vdppForm.lbOnlineVersions.SelectedIndex = -1;
                MainForm.vdppForm.btnDownloadVersion.Visible = false;
            }
            

            dForm.Close();
            dForm = null;



        }

        public void DeleteSelected()
        {
            if (lbVersions.SelectedIndex == -1)
                return;

            string version = lbVersions.SelectedItem.ToString();

            if (File.Exists(launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar + version + ".zip"))
                File.Delete(launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar + version + ".zip");

            if (Directory.Exists((launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version)))
                Directory.Delete(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version, true);

            RefreshInterface();

        }

        public void RefreshInterface()
        {
            lbVersions.SelectedIndex = -1;
            RefreshInstalledVersions();
        }

        public void OpenFolder()
        {
            if (lbVersions.SelectedIndex == -1)
                return;

            string version = lbVersions.SelectedItem.ToString();

            if (Directory.Exists((launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version)))
                Process.Start(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version);


        }

        private void lbVersions_MouseDown(object sender, MouseEventArgs e)
        {
            if (lbVersions.SelectedIndex == -1)
                return;

            if (e.Button == MouseButtons.Right)
            {
                Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y + pnTopPanel.Height);

                ContextMenuStrip columnsMenu = new ContextMenuStrip();
                columnsMenu.Items.Add("Open Folder", null, new EventHandler((ob, ev) => { OpenFolder(); }));
                columnsMenu.Items.Add(new ToolStripSeparator());
                columnsMenu.Items.Add("Delete", null, new EventHandler((ob, ev) => { DeleteSelected(); }));
                columnsMenu.Show(this, locate);
            }
        }



        private void btnOnlineGuide_Click(object sender, EventArgs e)
        {
            Process.Start("https://corrupt.wiki/");
        }

        public void clearAnchorRight()
        {
            foreach (Control c in pnAnchorRight.Controls)
                if (c is Form)
                {
                    pnAnchorRight.Controls.Remove(c);
                    (c as Form).Close();
                }
        }


        private void btnVersionDownloader_Click(object sender, EventArgs e)
        {
            lbVersions.SelectedIndex = -1;

            clearAnchorRight();

            MainForm.vdppForm = new VersionDownloadPanel();
            MainForm.vdppForm.TopLevel = false;
            pnAnchorRight.Controls.Add(MainForm.vdppForm);
            MainForm.vdppForm.Dock = DockStyle.Fill;
            MainForm.vdppForm.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pnAnchorRight_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lbRtcLauncher_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnQuit_MouseEnter(object sender, EventArgs e)
        {
            btnQuit.BackColor = Color.FromArgb(230, 46, 76);
        }

        private void btnQuit_MouseLeave(object sender, EventArgs e)
        {
            btnQuit.BackColor = Color.FromArgb(64, 64, 64);
        }

        private void btnDiscord_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.corrupt.wiki/");
        }

    }
}
