using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTCV.Launcher
{
    public partial class LaunchPanelV3 : Form
    {
        private LauncherConfJson lc;

        public LaunchPanelV3()
        {
            InitializeComponent();
            lbSelectedVersion.Visible = false;

            lc = new LauncherConfJson(MainForm.SelectedVersion);
        }

        public void DisplayVersion()
        {
            Size? btnSize = null;
            Point btnLocation = btnDefaultSize.Location;

            Controls.Remove(btnDefaultSize);

            int maxHorizontal = 4;
            int positionX = 0;
            int positionY = 0;

            foreach (var lcji in lc.Items)
            {
                Bitmap btnImage;
                using (var bmpTemp = new Bitmap(Path.Combine(lc.LauncherAssetLocation, lcji.ImageName)))
                {
                    btnImage = new Bitmap(bmpTemp);
                }

                if (btnSize == null)
                {
                    //The first image sets the parameters for display
                    btnSize = new Size(btnImage.Width + 1, btnImage.Height + 1);

                    //Checks how many fit horizontally
                    double screenspace = (this.Width - btnLocation.X); ;
                    double fullsizeItem = ((Size)btnSize).Width + btnLocation.X;
                    double howmanyfit = screenspace / fullsizeItem;
                    maxHorizontal = Convert.ToInt32(Math.Floor(howmanyfit));

                }

                Button newButton = new Button();
                newButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
                newButton.FlatAppearance.BorderSize = 0;
                newButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                newButton.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
                newButton.ForeColor = System.Drawing.Color.Black;
                newButton.Name = "btnDefaultSize";
                newButton.Size = (Size)btnSize;
                newButton.TabIndex = 134;
                newButton.TabStop = false;
                newButton.Tag = lcji;
                newButton.Text = "";
                newButton.UseVisualStyleBackColor = false;
                newButton.Click += this.btnBatchfile_Click;


                bool isAddon = !string.IsNullOrWhiteSpace(lcji.DownloadVersion);
                bool AddonInstalled = false;

                if (isAddon)
                {
                    AddonInstalled = Directory.Exists(Path.Combine(lc.VersionLocation, lcji.FolderName));
                    newButton.MouseDown += (sender, e) =>
                    {

                        if (e.Button == MouseButtons.Right)
                        {
                            Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

                            ContextMenuStrip columnsMenu = new ContextMenuStrip();
                            columnsMenu.Items.Add("Delete Addon", null, new EventHandler((ob, ev) => { DeleteAddon(lcji.FolderName); })).Enabled = AddonInstalled;
                            columnsMenu.Items.Add("Open Folder in Explorer", null, new EventHandler((ob, ev) =>
                            {
                                Process.Start(Path.Combine(MainForm.launcherDir, "VERSIONS", lc.Version, lcji.FolderName));
                                
                            })).Enabled = AddonInstalled;
                            columnsMenu.Show(this, locate);
                        }
                    };
                }

                if (isAddon)
                {
                    Pen p = new Pen((AddonInstalled ? Color.FromArgb(57, 255, 20) : Color.Red), 2);

                    int x1 = 8;
                    int y1 = btnImage.Height-8;
                    int x2 = 24;
                    int y2 = btnImage.Height - 8;
                    // Draw line to screen.
                    using (var graphics = Graphics.FromImage(btnImage))
                    {
                        graphics.DrawLine(p, x1, y1, x2, y2);
                    }
                }


                newButton.Image = btnImage;
                newButton.Location = new Point(btnLocation.X + (((Size)btnSize).Width * positionX + btnLocation.X * positionX), btnLocation.Y + (((Size)btnSize).Height * positionY + btnLocation.X * positionY));
                newButton.Visible = true;
                Controls.Add(newButton);

                positionX++;
                if (positionX >= maxHorizontal)
                {
                    positionX = 0;
                    positionY++;
                }
            }
            

            lbSelectedVersion.Text = lc.Version;
            lbSelectedVersion.Visible = true;

        }

        public void DeleteAddon(string AddonFolderName)
        {
            try
            {
                string targetFolder = Path.Combine(MainForm.launcherDir, "VERSIONS", lc.Version, AddonFolderName);

                if (Directory.Exists(targetFolder))
                    Directory.Delete(targetFolder, true);
            }
            catch (Exception ex)
            {
                var result = MessageBox.Show($"Could not delete addon {AddonFolderName} because of the following error:\n{ex.ToString()}", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    DeleteAddon(AddonFolderName);
                    return;
                }
            }

            MainForm.mf.RefreshKeepSelectedVersion();
            //MainForm.mf.RefreshInterface();
        }

        private void NewLaunchPanel_Load(object sender, EventArgs e)
        {
            DisplayVersion();
        }

        private void btnBatchfile_Click(object sender, EventArgs e)
        {
            Button currentButton = (Button)sender;

            var lcji = (LauncherConfJsonItem) currentButton.Tag;
            

            if(!String.IsNullOrEmpty(lcji.FolderName) && !Directory.Exists(Path.Combine(lc.VersionLocation, lcji.FolderName)))
            {
                LauncherConfJson lcCandidateForPull = getFolderFromPreviousVersion(lcji.DownloadVersion);
                if(lcCandidateForPull != null)
                {

                    var resultAskPull = MessageBox.Show($"The component {lcji.FolderName} could be imported from {lcCandidateForPull.Version}\nDo you wish import it?", "Import candidate found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(resultAskPull == DialogResult.Yes)
                    {
                        LauncherConfJsonItem candidate = lcCandidateForPull.Items.FirstOrDefault(it => it.DownloadVersion == lcji.DownloadVersion);
                        //handle it here
                        try
                        {
                            RTC_Extensions.RecursiveCopyNukeReadOnly(new DirectoryInfo(Path.Combine(lcCandidateForPull.VersionLocation, candidate.FolderName)), new DirectoryInfo(Path.Combine(lc.VersionLocation, lcji.FolderName)));
                            RTC_Extensions.RecursiveDeleteNukeReadOnly(new DirectoryInfo(Path.Combine(lcCandidateForPull.VersionLocation, candidate.FolderName)));
                            MainForm.mf.RefreshKeepSelectedVersion();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Couldn't copy {Path.Combine(lcCandidateForPull.VersionLocation, candidate?.FolderName ?? "NULL") ?? "NULL"} to {lcji.FolderName}.\nIs the file in use?\nException:{ex.Message}");
                            try
                            {
                                RTC_Extensions.RecursiveDeleteNukeReadOnly(new DirectoryInfo(Path.Combine(lc.VersionLocation, lcji.FolderName)));
                            }
                            catch (Exception _ex) //f
                            {
                                Console.WriteLine(_ex);
                            }
                        }
                        return;

                    }

                }

                var result = MessageBox.Show($"The following component is missing: {lcji.FolderName}\nDo you wish to download it?", "Additional download required", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if(result == DialogResult.Yes)
                {
                    string downloadUrl = $"{MainForm.webRessourceDomain}/rtc/addons/" + lcji.DownloadVersion + ".zip";
                    string downloadedFile = Path.Combine(MainForm.launcherDir, "PACKAGES", lcji.DownloadVersion + ".zip");
                    string extractDirectory = Path.Combine(lc.VersionLocation, lcji.FolderName);

                    MainForm.mf.DownloadFile(downloadUrl, downloadedFile, extractDirectory);
                }

                return;
            }

            lcji.Execute();
        }

        private LauncherConfJson getFolderFromPreviousVersion(string downloadVersion)
        {
            foreach(string ver in MainForm.mf.lbVersions.Items.Cast<string>())
            {
                if (downloadVersion == ver)
                    continue;

                var _lc = new LauncherConfJson(ver);
                LauncherConfJsonItem lcji = _lc.Items.FirstOrDefault(it => it.DownloadVersion == downloadVersion);
                if (lcji != null)
                {
                    if (Directory.Exists(Path.Combine(_lc.VersionLocation, lcji.FolderName)))
                        return _lc;
                }
            }

            return null;
        }
    }
}
