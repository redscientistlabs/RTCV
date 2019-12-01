using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace RTCV.Launcher
{
    public partial class MainForm : Form
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int
            HT_CAPTION = 0x2,
            HT_LEFT = 0xA,
            HT_RIGHT = 0xB,
            HT_TOP = 0xC,
            HT_TOPLEFT = 0xD,
            HT_TOPRIGHT = 0xE,
            HT_BOTTOM = 0xF,
            HT_BOTTOMLEFT = 0x10,
            HT_BOTTOMRIGHT = 0x11;
        


        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();



        public static string launcherDir = Path.GetDirectoryName(Application.ExecutablePath);
        public static string webRessourceDomain = "http://redscientist.com/software";

        public static MainForm mf = null;
        public static VersionDownloadPanel vdppForm = null;
        public static DownloadForm dForm = null;
        public static Form lpForm = null;

        public static int launcherVer = 22;


        public static int devCounter = 0;
        internal static string SelectedVersion = null;
        internal static string lastSelectedVersion = null;

        public MainForm()
        {
            InitializeComponent();

            mf = this;
            lbVersions.AutoSize = true;
            RewireMouseMove();


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

        private void RewireMouseMove()
        {
            foreach (Control control in Controls)
            {
                control.MouseMove -= RedirectMouseMove;
                control.MouseMove += RedirectMouseMove;
            }
                

            this.MouseMove -= MainForm_MouseMove;
            this.MouseMove += MainForm_MouseMove;
        }

        public void DownloadFile(string downloadURL, string downloadedFile, string extractDirectory)
        {
            MainForm.mf.clearAnchorRight();

            MainForm.dForm = new DownloadForm(downloadURL, downloadedFile, extractDirectory);

            MainForm.mf.pnLeftSide.Visible = false;

            MainForm.mf.btnVersionDownloader.Enabled = false;

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
                Action a = () =>
                {
                    var motdFile = GetFileViaHttp($"{MainForm.webRessourceDomain}/rtc/releases/MOTD.txt");
                    string motd = "";
                    if (motdFile == null)
                        motd = "Couldn't load the RTC MOTD from Redscientist.com";
                    else
                        motd = Encoding.UTF8.GetString(motdFile);

                    this.Invoke(new MethodInvoker(() => { lbMOTD.Text = motd; }));
                };
                Task.Run(a);
            }
            catch
            {
                lbMOTD.Text = "Couldn't load the RTC MOTD from Redscientist.com";
                MessageBox.Show("Couldn't connect to the server.");
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
            lbVersions.Items.AddRange(versions.OrderByNaturalDescending(x => x).Select(it => getFilenameFromFullFilename(it)).ToArray<object>());
            this.PerformLayout();
            SelectedVersion = null;


            Action a = () =>
            {
                string latestVersion = VersionDownloadPanel.getLatestVersion();
                this.Invoke(new MethodInvoker(() => { pbNewVersionNotification.Visible = !versions.Select(it => it.Substring(it.LastIndexOf('\\') + 1)).Contains(latestVersion); }));
            };
            Task.Run(a);
        }

        public static byte[] GetFileViaHttp(string url)
        {
            //Windows does the big dumb: part 11
            WebRequest.DefaultWebProxy = null;

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(60000);
                byte[] b = null;
                try
                {
                    b = client.GetByteArrayAsync(url).Result;
                }
                catch (AggregateException e)
                {
                    Console.WriteLine($"{url} timed out.");
                }

                return b;
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
                lastSelectedVersion = SelectedVersion;
            }


            if (File.Exists(Path.Combine(MainForm.launcherDir, "VERSIONS", SelectedVersion, "Launcher", "launcher.json")))
                MainForm.lpForm = new LaunchPanelV3();
            else if (File.Exists(Path.Combine(MainForm.launcherDir, "VERSIONS", SelectedVersion, "Launcher", "launcher.ini")))
                MainForm.lpForm = new LaunchPanelV2();
            else
                MainForm.lpForm = new LaunchPanelV1();


            MainForm.lpForm.Size = pnAnchorRight.Size;
            MainForm.lpForm.TopLevel = false;
            pnAnchorRight.Controls.Add(MainForm.lpForm);
            foreach (Control c in MainForm.lpForm.Controls)
                c.MouseMove += MainForm_MouseMove;
            MainForm.lpForm.MouseMove += MainForm_MouseMove;

            MainForm.lpForm.Dock = DockStyle.Fill;
            MainForm.lpForm.Show();
        }



        public void InvokeUI(Action a)
        {
            this.BeginInvoke(new MethodInvoker(a));
        }

        private void UpdateLauncher(string extractDirectory)
        {
            string batchLocation = extractDirectory + Path.DirectorySeparatorChar + "Launcher\\update.bat";
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Path.GetFileName(batchLocation);
            psi.WorkingDirectory = Path.GetDirectoryName(batchLocation);
            Process.Start(psi);
            Environment.Exit(0);
        }

        public void DownloadComplete(string downloadedFile, string extractDirectory)
        {

            try
            {
                if (!Directory.Exists(extractDirectory))
                    Directory.CreateDirectory(extractDirectory);

                try
                {
                    System.IO.Compression.ZipFile.ExtractToDirectory(downloadedFile, extractDirectory);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during extraction, rolling back changes.\n\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (Directory.Exists(extractDirectory))
                        RTC_Extensions.RecursiveDeleteNukeReadOnly(extractDirectory);
                    return;
                }


                //This checks every extracted files against the contents of the zip file
                using (ZipArchive za = System.IO.Compression.ZipFile.OpenRead(downloadedFile))
                {
                    bool foundLockBefore = false; //this flag prompts a message to skip all 
                    bool skipLock = false; //file locked messages and sents the flag below

                    foreach (var entry in za.Entries.Where(it => !it.FullName.EndsWith("/")))
                    {
                        string targetFile = Path.Combine(extractDirectory, entry.FullName.Replace("/", "\\"));
                        if (File.Exists(targetFile))
                        {
                            string ext = entry.FullName.ToUpper().Substring(entry.FullName.Length - 3);
                            if (ext == "EXE" || ext == "DLL")
                            {
                                FileStream readCheck = null;
                                try
                                {
                                    readCheck = File.OpenRead(targetFile); //test if file can be read
                                    foundLockBefore = true;
                                }
                                catch
                                {
                                    if (!skipLock)
                                    {
                                        if (foundLockBefore)
                                        {
                                            if (MessageBox.Show($"Another file has been found locked/inaccessible.\nThere might be many more messages like this coming up.\n\nWould you like skip any remaining lock messages?", "Error",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                                                skipLock = true;
                                        }
                                    }

                                    if (!skipLock)
                                    {
                                        MessageBox.Show($"An error occurred during extraction,\n\nThe file \"targetFile\" seems to have been locked/made inaccessible by an external program. It might be caused by your antivirus.", "Error",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }

                                readCheck?.Close(); //close file immediately
                            }
                        }
                        else
                        {
                            MessageBox.Show($"An error occurred during extraction, rolling back changes.\n\nThe file \"{targetFile}\" could not be found. It might have been deleted by your antivirus.", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                            if (Directory.Exists(extractDirectory))
                                RTC_Extensions.RecursiveDeleteNukeReadOnly(extractDirectory);
                        }


                    }
                }

                //check if files are all present here

                if (File.Exists(downloadedFile))
                    File.Delete(downloadedFile);


                var preReqChecker = Path.Combine(extractDirectory, "Launcher", "PrereqChecker.exe");
                if (File.Exists(preReqChecker))
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = Path.GetFileName(preReqChecker);
                    psi.WorkingDirectory = Path.GetDirectoryName(preReqChecker);
                    Process.Start(psi)?.WaitForExit();
                }

                if (File.Exists(Path.Combine(extractDirectory, "Launcher", "ver.ini")))
                {
                    int newVer = Convert.ToInt32(File.ReadAllText(Path.Combine(extractDirectory, "Launcher", "ver.ini")));
                    if (newVer > launcherVer)
                    {

                        if (File.Exists(Path.Combine(extractDirectory, "Launcher", "minver.ini")) && //Do we have minver
                            Convert.ToInt32(File.ReadAllText(Path.Combine(extractDirectory, "Launcher", "minver.ini"))) > launcherVer) //Is minver > launcherVer
                        {
                            if (MessageBox.Show("A mandatory launcher update is required to use this version. Click \"OK\" to update the launcher.",
                                    "Launcher update required",
                                    MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Exclamation,
                                    MessageBoxDefaultButton.Button1,
                                    MessageBoxOptions.DefaultDesktopOnly) == DialogResult.OK)
                            {
                                UpdateLauncher(extractDirectory);
                            }
                            else
                            {
                                MessageBox.Show("Launcher update is required. Cancelling.");
                                RTC_Extensions.RecursiveDeleteNukeReadOnly(extractDirectory);
                                return;
                            }
                        }

                        if (MessageBox.Show("The downloaded package contains a new launcher update.\n\nDo you want to update the Launcher?", "Launcher update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            UpdateLauncher(extractDirectory);
                        }
                    }
                }
            }
            finally
            {

                lbVersions.SelectedIndex = -1;

                RefreshInstalledVersions();

                MainForm.mf.pnLeftSide.Visible = true;

                if (MainForm.vdppForm != null)
                {
                    MainForm.vdppForm.lbOnlineVersions.SelectedIndex = -1;
                    MainForm.vdppForm.btnDownloadVersion.Visible = false;
                }


                dForm.Close();
                dForm = null;

                RefreshKeepSelectedVersion();
            }
        }


        public void RefreshKeepSelectedVersion()
        {
            if (lastSelectedVersion != null)
            {
                int index = -1;
                for (int i = 0; i < lbVersions.Items.Count; i++)
                {
                    var item = lbVersions.Items[i];
                    if (item.ToString() == lastSelectedVersion)
                    {
                        index = i;
                        break;
                    }
                }

                lbVersions.SelectedIndex = -1;
                lbVersions.SelectedIndex = index;
            }
        }

        public void DeleteSelected()
        {
            if (lbVersions.SelectedIndex == -1)
                return;

            Directory.SetCurrentDirectory(launcherDir); //Move our working dir back
            string version = lbVersions.SelectedItem.ToString();

            if (File.Exists(launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar + version + ".zip"))
                File.Delete(launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar + version + ".zip");

            if (Directory.Exists((launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version)))
            {
            }

            {
                var failed = RTC_Extensions.RecursiveDeleteNukeReadOnly(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version);
                if (failed.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var l in failed)
                    {
                        sb.AppendLine(Path.GetFileName(l));
                    }

                    MessageBox.Show($"Failed to delete some files!\nSomething may be locking them (is the RTC still running?)\n\nList of failed files:\n{sb.ToString()}");
                }
            }
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

            lastSelectedVersion = null;

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

        const int _ = 10; // you can rename this variable if you like

        Rectangle Top => new Rectangle(0, 0, this.ClientSize.Width, _);
        Rectangle Left => new Rectangle(0, 0, _, this.ClientSize.Height);
        Rectangle Bottom => new Rectangle(0, this.ClientSize.Height - _, this.ClientSize.Width, _);
        Rectangle Right => new Rectangle(this.ClientSize.Width - _, 0, _, this.ClientSize.Height);
        Rectangle TopLeft => new Rectangle(0, 0, _, _);
        Rectangle TopRight => new Rectangle(this.ClientSize.Width - _, 0, _, _);
        Rectangle BottomLeft => new Rectangle(0, this.ClientSize.Height - _, _, _);
        Rectangle BottomRight => new Rectangle(this.ClientSize.Width - _, this.ClientSize.Height - _, _, _);



        private void ResizeWindow(MouseEventArgs e, int wParam)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, wParam, 0);
            }
        }
        private void RedirectMouseMove(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            Point screenPoint = control.PointToScreen(new Point(e.X, e.Y));
            Point formPoint = PointToClient(screenPoint);
            MouseEventArgs args = new MouseEventArgs(e.Button, e.Clicks,
                formPoint.X, formPoint.Y, e.Delta);
            OnMouseMove(args);
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Default;
            var cursor = this.PointToClient(Cursor.Position);
            if (TopLeft.Contains(cursor))
            {
                Cursor.Current = Cursors.SizeNWSE;
                ResizeWindow(e, HT_TOPLEFT);
            }
            else if (TopRight.Contains(cursor))
            {
                Cursor.Current = Cursors.SizeNESW;
                ResizeWindow(e, HT_TOPRIGHT);
            }
            else if (BottomLeft.Contains(cursor))
            {
                Cursor.Current = Cursors.SizeNESW;
                ResizeWindow(e, HT_BOTTOMLEFT);
            }
            else if (BottomRight.Contains(cursor))
            {
                Cursor.Current = Cursors.SizeNWSE;
                ResizeWindow(e, HT_BOTTOMRIGHT);
            }
            else if (Top.Contains(cursor))
            {
                Cursor.Current = Cursors.SizeNS;
                ResizeWindow(e, HT_TOP);
            }
            else if (Left.Contains(cursor))
            {
                Cursor.Current = Cursors.SizeWE;
                ResizeWindow(e, HT_LEFT);
            }
            else if (Right.Contains(cursor))
            {
                Cursor.Current = Cursors.SizeWE;
                ResizeWindow(e, HT_RIGHT);
            }
            else if (Bottom.Contains(cursor))
            {
                Cursor.Current = Cursors.SizeNS;
                ResizeWindow(e, HT_BOTTOM);
            }
            else if (pnTopPanel.ClientRectangle.Contains(cursor))
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
        }
    }
}
