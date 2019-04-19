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

namespace RTC_Launcher
{
    public partial class NewLaunchPanel : Form
    {
        string launcherAssetLocation;
        string launcherConfLocation;
        string batchFilesLocation;
        string version;

        public NewLaunchPanel()
        {
            InitializeComponent();
            lbSelectedVersion.Visible = false;

            version = MainForm.SelectedVersion;
            launcherAssetLocation = MainForm.launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version + Path.DirectorySeparatorChar + "Launcher";
            launcherConfLocation = launcherAssetLocation + Path.DirectorySeparatorChar + "launcher.ini";
            batchFilesLocation = MainForm.launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version;
        }

        public void DisplayVersion()
        {
            

            Size? btnSize = null;
            Point btnLocation = btnDefaultSize.Location;

            Controls.Remove(btnDefaultSize);

            if (!File.Exists(launcherConfLocation))
            {
                lbSelectedVersion.Text = "launcher.ini could not be found";
                lbSelectedVersion.Visible = true;
                return;
            }

            string[] launcherConf = File.ReadAllLines(launcherConfLocation);

            int maxHorizontal = 4;
            int positionX = 0;
            int positionY = 0;

            foreach (string line in launcherConf)
            {
                string[] lineItems = line.Split('|');
                string imageLocation = launcherAssetLocation + Path.DirectorySeparatorChar + "" + lineItems[0];
                string batchLocation = batchFilesLocation + Path.DirectorySeparatorChar + "" + lineItems[1];
                string folderName = lineItems[2];
                string folderCheck = batchFilesLocation + Path.DirectorySeparatorChar + "" + folderName;
                string downloadLocation = lineItems[3];

                Button newButton = new Button();
                newButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
                newButton.FlatAppearance.BorderSize = 0;
                newButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                newButton.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
                newButton.ForeColor = System.Drawing.Color.Black;


                Bitmap btnImage;
                using (var bmpTemp = new Bitmap(imageLocation))
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


                newButton.Name = "btnDefaultSize";
                newButton.Size = (Size)btnSize;
                newButton.TabIndex = 134;
                newButton.TabStop = false;
                newButton.Tag = line;
                newButton.Text = "";
                newButton.UseVisualStyleBackColor = false;
                newButton.Click += new System.EventHandler(this.btnBatchfile_Click);




                bool isAddon = !string.IsNullOrWhiteSpace(downloadLocation);
                bool AddonInstalled = false;

                if (isAddon)
                {
                    AddonInstalled = Directory.Exists(folderCheck);
                    newButton.MouseDown += new MouseEventHandler((sender, e) =>
                    {

                        if (e.Button == MouseButtons.Right)
                        {
                            Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

                            ContextMenuStrip columnsMenu = new ContextMenuStrip();
                            columnsMenu.Items.Add("Delete", null, new EventHandler((ob, ev) => { DeleteAddon(folderName); })).Enabled = AddonInstalled;
                            columnsMenu.Show(this, locate);
                        }

                        return;
                    });
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
            

            lbSelectedVersion.Text = version;
            lbSelectedVersion.Visible = true;

        }

        public void DeleteAddon(string AddonFolderName)
        {
            try
            {
                string targetFolder = MainForm.launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version + Path.DirectorySeparatorChar + "" + AddonFolderName;

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

            MainForm.mf.RefreshInterface();
        }

        private void NewLaunchPanel_Load(object sender, EventArgs e)
        {
            DisplayVersion();
        }

        private void btnBatchfile_Click(object sender, EventArgs e)
        {
            Button currentButton = (Button)sender;

            string line = (string)currentButton.Tag;
            string[] lineItems = line.Split('|');

            string imageLocation = launcherAssetLocation + Path.DirectorySeparatorChar + "" + lineItems[0];
            string batchName = lineItems[1];
            string batchLocation = batchFilesLocation + Path.DirectorySeparatorChar + "" + batchName;
            string folderName = lineItems[2];
            string folderLocation = batchFilesLocation + Path.DirectorySeparatorChar + "" + folderName;
            string downloadVersion = lineItems[3];

            if(!Directory.Exists(folderLocation))
            {
                if(string.IsNullOrWhiteSpace(downloadVersion))
                {
                    MessageBox.Show($"A required folder is missing: {lineItems[2]}\nNo download location was provided", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var result = MessageBox.Show($"The following component is missing: {lineItems[2]}\nDo you wish to download it?", "Additional download required", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if(result == DialogResult.Yes)
                {

                    string downloadUrl = $"{MainForm.webRessourceDomain}/rtc/addons/" + downloadVersion + ".zip";
                    string downloadedFile = MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar + downloadVersion + ".zip";
                    string extractDirectory = folderLocation;

                    MainForm.mf.DownloadFile(downloadUrl, downloadedFile, extractDirectory);

                }

                return;
            }

            if(batchLocation.Contains("http"))
            {
                Process.Start(batchName);
                return;
            }

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Path.GetFileName(batchLocation);
            psi.WorkingDirectory = Path.GetDirectoryName(batchLocation);
            Process.Start(psi);
        }
    }
}
