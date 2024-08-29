using System.IO.Compression;

namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Newtonsoft.Json;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    #pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    public partial class SavestateManagerForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        private BindingSource savestateBindingSource = new BindingSource(new BindingList<SaveStateKey>(), null);

        private bool LoadSavestateOnClick = false;
        public StashKey CurrentSaveStateStashKey => savestateList.CurrentSaveStateStashKey;
        private Color? _originalSaveButtonColor;
        private bool _unsavedEdits;
        public bool UnsavedEdits
        {
            get => _unsavedEdits;
            set
            {
                _unsavedEdits = value;

                if (_unsavedEdits)
                {
                    btnSaveSavestateList.BackColor = Color.Tomato;
                }
                else
                {
                    const float dark2 = -0.35f;
                    const float generalDarken = -0.50f;
                    Color c = Colors.GeneralColor.ChangeColorBrightness(generalDarken).ChangeColorBrightness(dark2);
                    btnSaveSavestateList.BackColor = c;
                    btnSaveSavestateList.FlatAppearance.BorderColor = c;
                }
            }
        }
        public SavestateManagerForm()
        {
            InitializeComponent();

            popoutAllowed = true;
            undockedSizable = false;

            savestateList.DataSource = savestateBindingSource;
            btnSaveSavestateList.BackColorChanged += (o, e) => UnsavedEdits = UnsavedEdits; //this is pretty cursed but it works
        }

        private void OnLoadSavestateListButtonMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                loadSavestateList();
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            savestateList.AllowDrop = true;
            savestateList.DragDrop += OnSavestateHolderDragDrop;
            savestateList.DragEnter += OnSavestateHolderDragEnter;
        }

        private void loadSSK(bool import, string fileName)
        {
            bool cancelLoad = false;
            SyncObjectSingleton.FormExecute(() =>
            {
                if (!import && UnsavedEdits
                            && DialogResult.No == MessageBox.Show(this,
                                "You have unsaved edits in the Glitch Harvester Savestate Manager. \n\n Are you sure you want to load without saving?",
                                "Unsaved edits in Savestate Manager", MessageBoxButtons.YesNo))
                {
                    cancelLoad = true;
                }
            });
            if (cancelLoad)
                return;
            
            decimal currentProgress = 0;
            decimal percentPerFile = 0;
            SaveStateKeys ssk;

            if (!import)
            {
                RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs("Committing used states to session", currentProgress += 5));
                //Commit any used states to the SESSION folder
                commitUsedStatesToSession();
                SyncObjectSingleton.FormExecute(() => savestateBindingSource.Clear());
            }

            var extractFolder = import ? "TEMP" : "SSK";

            //Extract the ssk
            RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs("Extracting the SSK", currentProgress += 50));
            if (Stockpile.Extract(fileName, Path.Combine("WORKING", extractFolder), "keys.json") is { Failed: true })
            {
                return;
            }

            //Read in the ssk
            try
            {
                RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs("Reading keys.json", currentProgress += 5));
                using (FileStream fs = File.Open(Path.Combine(RtcCore.workingDir, extractFolder, "keys.json"), FileMode.OpenOrCreate))
                {
                    ssk = JsonHelper.Deserialize<SaveStateKeys>(fs);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                if (CloudDebug.ShowErrorDialog(ex, true) == DialogResult.Abort)
                {
                    throw new AbortEverythingException();
                }

                return;
            }

            if (ssk == null)
            {
                MessageBox.Show("The Savestate Keys file was empty (null).\n");
                return;
            }

            var s = (string)AllSpec.VanguardSpec?[VSPEC.NAME] ?? "ERROR";
            if (!string.IsNullOrEmpty(ssk.VanguardImplementation) && !ssk.VanguardImplementation.Equals(s, StringComparison.OrdinalIgnoreCase) && ssk.VanguardImplementation != "ERROR")
            {
                MessageBox.Show($"The ssk you loaded is for a different Vanguard implementation.\nThe ssk reported {ssk.VanguardImplementation} but you're connected to {s}.\nThis is a fatal error. Aborting load.");
                return;
            }

            if (import)
            {
                var allCopied = new List<string>();
                var files = Directory.GetFiles(Path.Combine(RtcCore.workingDir, "TEMP"));
                percentPerFile = 20m / (files.Length + 1);
                //Copy from temp to sks
                foreach (string file in files)
                {
                    if (!file.Contains(".ssk"))
                    {
                        RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs($"Copying {Path.GetFileName(file)} to SSK", currentProgress += percentPerFile));
                        try
                        {
                            string dest = Path.Combine(RtcCore.workingDir, "SSK", Path.GetFileName(file));

                            //Only copy if a version doesn't exist
                            //This prevents copying over keys
                            if (!File.Exists(dest))
                            {
                                File.Copy(file, dest); // copy roms/stockpile/whatever to sks folder
                                allCopied.Add(dest);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unable to copy a file from temp to ssk. The culprit is " + file + ".\nCancelling operation.\n " + ex.ToString());
                            //Attempt to cleanup
                            foreach (var f in allCopied)
                            {
                                File.Delete(f);
                            }

                            return;
                        }
                    }
                }

                RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs($"Emptying TEMP", currentProgress += 5));
                Stockpile.EmptyFolder(Path.Combine("WORKING", "TEMP"));
            }
            UnsavedEdits = import;

            percentPerFile = 20m / (ssk.StashKeys.Count + 1);
            for (var i = 0; i < ssk.StashKeys.Count; i++)
            {
                var key = ssk.StashKeys[i];
                if (key == null)
                {
                    continue;
                }

                RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs($"Fixing up {key.Alias}", currentProgress += percentPerFile));

                //We have to set this first as we then change the other stuff
                key.StateLocation = StashKeySavestateLocation.SSK;

                string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name
                string newStatePath = Path.Combine(RtcCore.workingDir, key.StateLocation.ToString(), statefilename);

                key.StateFilename = newStatePath;
                key.StateShortFilename = Path.GetFileName(newStatePath);

                SyncObjectSingleton.FormExecute(() => savestateBindingSource.Add(new SaveStateKey(key, ssk.Text[i])));
            }
            RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs($"Done", 100));
        }

        internal async void loadSavestateList(bool import = false, string fileName = null)
        {
            if (fileName == null)
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    DefaultExt = "ssk",
                    Title = "Open Savestate Keys File",
                    Filter = "SSK files|*.ssk",
                    RestoreDirectory = true
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    fileName = ofd.FileName;
                }
                else
                {
                    return;
                }
            }
            if (!File.Exists(fileName))
            {
                MessageBox.Show("The Savestate Keys file wasn't found");
                return;
            }

            var ghForm = CanvasForm.GetExtraForm("Glitch Harvester");
            try
            {
                //We do this here and invoke because our unlock runs at the end of the awaited method, but there's a chance an error occurs
                //Thus, we want this to happen within the try block
                SyncObjectSingleton.FormExecute(() =>
                {
                    UICore.LockInterface(false, true);
                    S.GET<SaveProgressForm>().Dock = DockStyle.Fill;
                    ghForm?.OpenSubForm(S.GET<SaveProgressForm>());
                });

                await Task.Run(() =>
                {
                    loadSSK(import, fileName);
                });
            }
            finally
            {
                SyncObjectSingleton.FormExecute(() =>
                {
                    ghForm?.CloseSubForm();
                    UICore.UnlockInterface();
                });
            }
        }

        private static void commitUsedStatesToSession()
        {
            var allStashKeys = new List<StashKey>();
            //Commit any used states to SESSION so we can safely unload the sk
            foreach (var row in S.GET<StockpileManagerForm>().dgvStockpile.Rows.Cast<DataGridViewRow>().ToList())
            {
                if (row.Cells[0].Value is StashKey sk)
                {
                    allStashKeys.Add(sk);
                }
            }
            allStashKeys.AddRange(StockpileManagerUISide.StashHistory);
            allStashKeys.AddRange(S.GET<BlastEditorForm>().GetStashKeys());
            allStashKeys.AddRange(S.GET<BlastGeneratorForm>().GetStashKeys());
            bool notified = false;
            foreach (var sk in allStashKeys.Where(x => x?.StateLocation == StashKeySavestateLocation.SSK))
            {
                try
                {
                    var stateName = sk.GameName + "." + sk.ParentKey + ".timejump.State"; // get savestate name
                    if (File.Exists(Path.Combine(RtcCore.workingDir, "SSK", stateName))) //it SHOULD be here. If it's not, let's hunt for it
                    {
                        File.Copy(Path.Combine(RtcCore.workingDir, "SSK", stateName), Path.Combine(RtcCore.workingDir, "SESSION", stateName), true);
                    }
                    else if (File.Exists(Path.Combine(RtcCore.workingDir, "TEMP", stateName)))
                    {
                        File.Copy(Path.Combine(RtcCore.workingDir, "TEMP", stateName), Path.Combine(RtcCore.workingDir, "SESSION", stateName), true);
                    }
                    else if (File.Exists(Path.Combine(RtcCore.workingDir, "SKS", stateName)))
                    {
                        File.Copy(Path.Combine(RtcCore.workingDir, "SKS", stateName), Path.Combine(RtcCore.workingDir, "SESSION", stateName), true);
                    }
                    else if (File.Exists(Path.Combine(RtcCore.workingDir, "SESSION", stateName)))
                    {
                        continue;
                    }
                    else if (!notified)
                    {
                        MessageBox.Show($"Couldn't locate savestate {stateName}.\nIf you remember the course of actions that lead here, report to the RTC devs.\nSome of your non-stockpiled stashkeys may be broken.");
                        notified = true;
                    }

                    sk.StateLocation = StashKeySavestateLocation.SESSION;
                }
                catch (IOException e)
                {
                    throw new Exception("Couldn't copy savestate " + sk.StateShortFilename + " to SESSION! " + e.Message);
                }
            }
        }

        private void OnLoadSavestateListButtonMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

                ContextMenuStrip columnsMenu = new ContextMenuStrip();
                columnsMenu.Items.Add("Clear Savestate List", null, new EventHandler((ob, ev) =>
                {
                    //Commit any used states to disk
                    commitUsedStatesToSession();
                    savestateBindingSource.Clear();
                }));

                columnsMenu.Items.Add("Import SSK", null, (ob, ev) =>
                {
                    loadSavestateList(true);
                });

                columnsMenu.Show(this, locate);
            }
        }

        private void saveSSK(string path)
        {
            decimal currentProgress = 0;
            try
            {
                SaveStateKeys ssk = new SaveStateKeys();
                foreach (SaveStateKey x in savestateBindingSource.List)
                {
                    ssk.StashKeys.Add(x.StashKey);
                    ssk.Text.Add(x.Text);
                }
                RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs("Prepping TEMP", currentProgress += 5));
                //clean temp folder
                Stockpile.EmptyFolder(Path.Combine("WORKING", "TEMP"));

                //Commit any states in use
                RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs("Committing used states", currentProgress += 5));
                commitUsedStatesToSession();

                var percentPerFile = 30m / (ssk.StashKeys.Count + 1);
                foreach (var key in ssk.StashKeys)
                {
                    RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs($"Copying {key.GameName + "." + key.ParentKey + ".timejump.State"} to TEMP", currentProgress += percentPerFile));
                    string stateFilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name

                    string statePath = Path.Combine(RtcCore.workingDir, key.StateLocation.ToString(), stateFilename);
                    string tempPath = Path.Combine(RtcCore.workingDir, "TEMP", stateFilename);

                    if (File.Exists(statePath))
                    {
                        File.Copy(statePath, tempPath, true); // copy savestates to temp folder
                    }
                    else
                    {
                        MessageBox.Show("Couldn't find savestate " + statePath + "!\n\n. This is savestate index " + ssk.StashKeys.IndexOf(key) + 1 + ".\nAborting save");
                        Stockpile.EmptyFolder(Path.Combine("WORKING", "TEMP"));
                        return;
                    }
                }

                percentPerFile = 10m / (ssk.StashKeys.Count + 1);
                //Use two separate loops here in case the first one aborts. We don't want to update the StateLocation unless we know we're good
                foreach (var key in ssk.StashKeys)
                {
                    RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs($"Updating {key} location", currentProgress += percentPerFile));
                    if (key == null)
                    {
                        continue;
                    }

                    key.StateLocation = StashKeySavestateLocation.SSK;
                }

                //Create keys.json
                RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs("Creating keys.json", currentProgress += 10));
                using (FileStream fs = File.Open(Path.Combine(RtcCore.workingDir, "TEMP", "keys.json"), FileMode.OpenOrCreate))
                {
                    JsonHelper.Serialize(ssk, fs, Formatting.Indented);
                    fs.Close();
                }

                string tempFilename = path + ".temp";
                //If there's already a temp file from a previous failed save, delete it
                try
                {
                    if (File.Exists(tempFilename))
                    {
                        File.Delete(tempFilename);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                string tempFolderPath = Path.Combine(RtcCore.workingDir, "TEMP");

                RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs("Creating SSK", currentProgress += 20));
                CompressionLevel compressionLevel = Params.IsParamSet("COMPRESS_SAVESTATES")
                    ? CompressionLevel.Fastest
                    : CompressionLevel.NoCompression;
                ZipFile.CreateFromDirectory(tempFolderPath, tempFilename, compressionLevel, false);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs("Moving SSK to destination", currentProgress += 5));
                File.Move(tempFilename, path);

                //Move all the files from temp into SSK
                RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs("Emptying SSK", currentProgress += 5));
                Stockpile.EmptyFolder(Path.Combine("WORKING", "SSK"));

                var files = Directory.GetFiles(tempFolderPath);
                percentPerFile = 15m / files.Length;
                foreach (string file in files)
                {
                    RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs($"Moving {Path.GetFileName(file)} to SSK", currentProgress += percentPerFile));
                    File.Move(file, Path.Combine(RtcCore.workingDir, "SSK", Path.GetFileName(file)));
                }

                UnsavedEdits = false;
            }
            catch (Exception ex)
            {
                if (CloudDebug.ShowErrorDialog(ex, true) == DialogResult.Abort)
                {
                    throw new AbortEverythingException();
                }

                return;
            }
        }

        public async void SaveSavestateList(object sender, EventArgs e)
        {
            string filename;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                DefaultExt = "ssk",
                Title = "Savestate Keys File",
                Filter = "SSK files|*.ssk",
                RestoreDirectory = true
            };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = saveFileDialog1.FileName;
            }
            else
            {
                return;
            }

            var ghForm = CanvasForm.GetExtraForm("Glitch Harvester");
            try
            {
                //We do this here and invoke because our unlock runs at the end of the awaited method, but there's a chance an error occurs
                //Thus, we want this to happen within the try block
                SyncObjectSingleton.FormExecute(() =>
                {
                    UICore.LockInterface(false, true);
                    S.GET<SaveProgressForm>().Dock = DockStyle.Fill;
                    ghForm?.OpenSubForm(S.GET<SaveProgressForm>());
                });

                await Task.Run(() =>
                {
                    saveSSK(filename);
                });
            }
            finally
            {
                SyncObjectSingleton.FormExecute(() =>
                {
                    ghForm?.CloseSubForm();
                    UICore.UnlockInterface();
                });
            }
        }

        internal void DisableFeature()
        {
            Controls.Clear();
        }

        private void OnSavestateHolderDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void OnSavestateHolderDragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (files?.Length > 0 && files[0]
                .Contains(".ssk"))
            {
                loadSavestateList(false, files[0]);
            }

            //Bring the UI back to normal after a drag+drop to prevent weird merge stuff
            S.GET<GlitchHarvesterBlastForm>().RedrawActionUI();
        }

        private void UpdateLoadSavestateOnClick(object sender, EventArgs e)
        {
            LoadSavestateOnClick = cbSavestateLoadOnClick.Checked;
        }

        private void OnFormShown(object sender, EventArgs e)
        {
            object param = AllSpec.VanguardSpec[VSPEC.RENAME_SAVESTATE];
            if (param != null && param is string RenameTitle)
            {
                Text = RenameTitle;
                ParentComponentFormTitle.lbComponentFormName.Text = $"{RenameTitle} Manager";
            }
        }
    }
}
