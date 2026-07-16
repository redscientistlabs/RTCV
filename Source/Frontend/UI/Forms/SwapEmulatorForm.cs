namespace RTCV.UI
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using RTCV.Common;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.UI.Modular;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Xml.Linq;

    public partial class SwapEmulatorForm : ComponentForm, IBlockable
    {
        private string _LauncherAssetLocation;
        private string _LauncherConfLocation;
        private Dictionary<string, EmulatorData> emulatorCards = new Dictionary<string, EmulatorData>();

        private string _currentEmulator = String.Empty;

        private class EmulatorData
        {
            public string ImageName;
            public string FolderName;
            public bool Downloaded;
        }

        public SwapEmulatorForm()
        {
            InitializeComponent();
        }

        // Updates the selected emulator (if a new one was swapped to) and updates the available emulators list
        // to match any changes to selected cards or the directory
        public void UpdateSelectedEmulator(string selectedEmulator = null)
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                lbConnectionStatus.Text = "Connected to: " + (string)AllSpec.VanguardSpec[VSPEC.NAME];
                _currentEmulator = selectedEmulator ?? _currentEmulator;

                string rtcRootDir = Path.Combine(RtcCore.RtcDir, "..\\..");
                DirectoryInfo directory = new DirectoryInfo(rtcRootDir);
                List<string> folderNames = directory.GetDirectories().Select(d => d.Name).ToList();

                foreach (string name in emulatorCards.Keys)
                {
                    EmulatorData emulatorCard = emulatorCards[name];
                    emulatorCard.Downloaded = folderNames.Contains(emulatorCard.FolderName);
                }

                UpdateEmulatorList();
            });
        }

        private void SwapEmulatorForm_Load(object sender, EventArgs e)
        {
            lbConnectionStatus.Text = "Connected to: " + (string)AllSpec.VanguardSpec[VSPEC.NAME];
            _currentEmulator = new DirectoryInfo((string)AllSpec.VanguardSpec?[VSPEC.EMUDIR]).Name.ToUpper();

            string rtcRootDir = Path.Combine(RtcCore.RtcDir, "..\\..");
            DirectoryInfo directory = new DirectoryInfo(rtcRootDir);
            List<string> folderNames = directory.GetDirectories().Select(d => d.Name).ToList();


            _LauncherAssetLocation = Path.Combine(rtcRootDir, "Launcher");
            _LauncherConfLocation = Path.Combine(_LauncherAssetLocation, "launcher.json");

            if (Directory.Exists(_LauncherAssetLocation))
            {
                if (!File.Exists(_LauncherConfLocation))
                {
                    return;
                }

                // Grab the launcher configuration JSON file and deserialize it
                string launcherJson = File.ReadAllText(_LauncherConfLocation);
                JToken result = JsonConvert.DeserializeObject<JToken>(launcherJson);

                // Loop through each entry in the config file and check if it's an 
                // item that can be downloaded
                foreach (JToken entry in result)
                {
                    if (entry["FolderName"] != null && entry["ImageName"] != null)
                    {
                        string folderName = entry["FolderName"].ToString();
                        string imageName = entry["ImageName"].ToString();

                        // TODO: Temporary workaround until I change these implementations
                        // so they immediately connect
                        List<string> blacklist = new List<string> {
                            "Launcher",
                            "FileStub",
                            "ProcessStub",
                            "ClipStub"
                        };

                        bool containsKeyword = blacklist.Any(keyword =>
                        folderName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
                        if (!containsKeyword)
                        {
                            emulatorCards[folderName] = new EmulatorData
                            {
                                ImageName = imageName.ToString(),
                                FolderName = folderName.ToString(),
                                Downloaded = folderNames.Contains(folderName)
                            };
                        }
                    }
                }
            }

            UpdateEmulatorList();

            // Set up a file watch system to detect when emulators are downloaded or deleted
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = directory.FullName;
            watcher.NotifyFilter = NotifyFilters.DirectoryName;
            watcher.Changed += OnFolderCountChanged;
            watcher.Created += OnFolderCountChanged;
            watcher.Deleted += OnFolderCountChanged;
            watcher.EnableRaisingEvents = true;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void UpdateEmulatorList()
        {
            pnCurrentEmulator.Controls.Clear();
            pnAvailableEmulators.Controls.Clear();

            foreach (string name in emulatorCards.Keys)
            {
                EmulatorData emulatorCard = emulatorCards[name];

                if (emulatorCard.Downloaded)
                {
                    string imageName = emulatorCard.ImageName;
                    string folderName = emulatorCard.FolderName;

                    var enabled = _currentEmulator == folderName ? false : true;
                    var newButton = CreateCard(imageName, name, enabled);
                    pnAvailableEmulators.Controls.Add(newButton);

                    if (enabled)
                    {
                        newButton.Click += OnSwapEmulator;
                    }

                    if (_currentEmulator == folderName)
                    {
                        var currentCard = CreateCard(imageName, name, true);
                        pnCurrentEmulator.Controls.Add(currentCard);
                    }
                }
            }
        }

        private Button CreateCard(string imageName, string cardName, bool enabled)
        {
            Bitmap cardImage;
            Size? cardSize = null;
            using (var bmpTemp = new Bitmap(new MemoryStream(File.ReadAllBytes(Path.Combine(_LauncherAssetLocation, imageName)))))
            {
                cardImage = new Bitmap(bmpTemp);
                cardSize = new Size(cardImage.Width + 1, cardImage.Height + 1);
            }

            var newButton = new Button();
            newButton.Size = cardSize.Value;
            newButton.Image = cardImage;
            newButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            newButton.FlatAppearance.BorderSize = 0;
            newButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            newButton.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            newButton.ForeColor = System.Drawing.Color.Black;
            newButton.Name = cardName;
            newButton.TabIndex = 134;
            newButton.TabStop = false;
            newButton.Text = string.Empty;
            newButton.UseVisualStyleBackColor = false;

            newButton.Visible = true;

            if (!enabled)
            {
                using (Graphics g = Graphics.FromImage(newButton.Image))
                {
                    using (Brush darkBrush = new SolidBrush(Color.FromArgb(150, Color.Black)))
                    {
                        g.FillRectangle(darkBrush, 0, 0, newButton.Image.Width, newButton.Image.Height);
                    }
                }
            }

            return newButton;
        }

        private async void OnSwapEmulator(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                if (!(await VanguardImplementation.SwapImplementation(btn.Name, true)))
                    return;
            }
        }

        private void OnFolderCountChanged(object sender, FileSystemEventArgs e)
        {
            UpdateSelectedEmulator();
        }
    }
}
