using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;
using RTCV.NetCore;
using Newtonsoft.Json;

namespace RTCV.UI
{
	public partial class RTC_SavestateManager_Form : ComponentForm, IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);


        Dictionary<string, TextBox> StateBoxes = new Dictionary<string, TextBox>();
        private BindingSource savestateBindingSource = new BindingSource(new BindingList<SaveStateKey>(), null);

        private bool LoadSavestateOnClick = false;
        public StashKey CurrentSaveStateStashKey => savestateList.CurrentSaveStateStashKey;

        public RTC_SavestateManager_Form()
		{
			InitializeComponent();

            popoutAllowed = true;
            this.undockedSizable = false;

            savestateList.DataSource = savestateBindingSource;
        }

        private void btnLoadSavestateList_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
                loadSavestateList();
        }


        private void RTC_SavestateManager_Form_Load(object sender, EventArgs e)
        {
            savestateList.AllowDrop = true;
            savestateList.DragDrop += pnSavestateHolder_DragDrop;
            savestateList.DragEnter += pnSavestateHolder_DragEnter;

        }

        private void loadSavestateList(bool import = false, string fileName = null)
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
                    return;
            }

            if (!File.Exists(fileName))
            {
                MessageBox.Show("The Savestate Keys file wasn't found");
                return;
            }

            SaveStateKeys ssk;

            if (!import)
            {
                //Commit any used states to the SESSION folder
                commitUsedStatesToSession();
                savestateBindingSource.Clear();
            }

            var extractFolder = import ? "TEMP" : "SSK";

            //Extract the ssk
            if (!Stockpile.Extract(fileName, Path.Combine("WORKING", extractFolder), "keys.json"))
                return;

            //Read in the ssk
            try
            {
                using (FileStream fs = File.Open(Path.Combine(RtcCore.workingDir, extractFolder, "keys.json"), FileMode.OpenOrCreate))
                {
                    ssk = CorruptCore.JsonHelper.Deserialize<SaveStateKeys>(fs);
                    fs.Close();
                }

            }
            catch (Exception ex)
            {
                string additionalInfo = "The Savestate Keys file could not be loaded\n\n";

                var ex2 = new CustomException(ex.Message, additionalInfo + ex.StackTrace);

                if (CloudDebug.ShowErrorDialog(ex2, true) == DialogResult.Abort)
                    throw new RTCV.NetCore.AbortEverythingException();

                return;
            }


            if (ssk == null)
            {
                MessageBox.Show("The Savestate Keys file was empty (null).\n");
                return;
            }

            var s = (string) RTCV.NetCore.AllSpec.VanguardSpec?[VSPEC.NAME] ?? "ERROR";
            if (!String.IsNullOrEmpty(ssk.VanguardImplementation) && !ssk.VanguardImplementation.Equals(s, StringComparison.OrdinalIgnoreCase) && ssk.VanguardImplementation != "ERROR")
            {
                MessageBox.Show($"The ssk you loaded is for a different Vanguard implementation.\nThe ssk reported {ssk.VanguardImplementation} but you're connected to {s}.\nThis is a fatal error. Aborting load.");
                return;
            }

            if (import)
            {
                var allCopied = new List<string>();
                //Copy from temp to sks
                foreach (string file in Directory.GetFiles(Path.Combine(RtcCore.workingDir, "TEMP")))
                {
                    if (!file.Contains(".ssk"))
                    {
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
                                File.Delete(f);
                            return;

                        }
                    }
                }

                Stockpile.EmptyFolder(Path.Combine("WORKING", "TEMP"));
            }

            for (var i = 0; i < ssk.StashKeys.Count; i++)
            {
                var key = ssk.StashKeys[i];
                if (key == null)
                    continue;

                //We have to set this first as we then change the other stuff
                key.StateLocation = StashKeySavestateLocation.SSK;

                string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name
                string newStatePath = Path.Combine(CorruptCore.RtcCore.workingDir, key.StateLocation.ToString(), statefilename);

                key.StateFilename = newStatePath;
                key.StateShortFilename = Path.GetFileName(newStatePath);

                savestateBindingSource.Add(new SaveStateKey(key, ssk.Text[i]));
            }
        }

        private void commitUsedStatesToSession()
        {
            var allStashKeys = new List<StashKey>();
            //Commit any used states to SESSION so we can safely unload the sk
            foreach (var row in S.GET<RTC_StockpileManager_Form>().dgvStockpile.Rows.Cast<DataGridViewRow>().ToList())
            {
                if (row.Cells[0].Value is StashKey sk)
                    allStashKeys.Add(sk);
            }
            allStashKeys.AddRange(StockpileManager_UISide.StashHistory);
            allStashKeys.AddRange(S.GET<RTC_NewBlastEditor_Form>().GetStashKeys());
            allStashKeys.AddRange(S.GET<RTC_BlastGenerator_Form>().GetStashKeys());
            bool notified = false;
            foreach (var sk in allStashKeys.Where(x => x?.StateLocation == StashKeySavestateLocation.SSK))
            {
                try
                {
                    var stateName = sk.GameName + "." + sk.ParentKey + ".timejump.State"; // get savestate name
                    if(File.Exists(Path.Combine(CorruptCore.RtcCore.workingDir, "SSK", stateName))) //it SHOULD be here. If it's not, let's hunt for it
                        File.Copy(Path.Combine(CorruptCore.RtcCore.workingDir, "SSK", stateName), Path.Combine(CorruptCore.RtcCore.workingDir, "SESSION", stateName), true);
                    else if (File.Exists(Path.Combine(CorruptCore.RtcCore.workingDir, "TEMP", stateName)))
                        File.Copy(Path.Combine(CorruptCore.RtcCore.workingDir, "TEMP", stateName), Path.Combine(CorruptCore.RtcCore.workingDir, "SESSION", stateName), true);
                    else if (File.Exists(Path.Combine(CorruptCore.RtcCore.workingDir, "SKS", stateName)))
                        File.Copy(Path.Combine(CorruptCore.RtcCore.workingDir, "SKS", stateName), Path.Combine(CorruptCore.RtcCore.workingDir, "SESSION", stateName), true);
                    else if (File.Exists(Path.Combine(CorruptCore.RtcCore.workingDir, "SESSION", stateName)))
                        continue;
                    else if (!notified)
                    {
                        MessageBox.Show($"Couldn't locate savestate {stateName}.\nIf you remember the course of actions that lead here, report to the RTC devs.\nSome of your non-stockpiled stashkeys may be broken.");
                        notified = true;
                    }
                        

                    sk.StateLocation = StashKeySavestateLocation.SESSION;
                }
                catch (IOException e)
                {
                    throw new CustomException("Couldn't copy savestate " + sk.StateShortFilename + " to SESSION! " + e.Message, e.StackTrace);
                }
            }
        }

        private void btnLoadSavestateList_MouseDown(object sender, MouseEventArgs e)
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

        private void btnSaveSavestateList_Click(object sender, EventArgs e)
        {
            try
            {
                SaveStateKeys ssk = new SaveStateKeys();

                foreach (SaveStateKey x in savestateBindingSource.List)
                {
                    ssk.StashKeys.Add(x.StashKey);
                    ssk.Text.Add(x.Text);
                }

                string Filename;
                string ShortFilename;

                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    DefaultExt = "ssk",
                    Title = "Savestate Keys File",
                    Filter = "SSK files|*.ssk",
                    RestoreDirectory = true
                };

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Filename = saveFileDialog1.FileName;
                    ShortFilename = Path.GetFileName(Filename);
                }
                else
                    return;

                //clean temp folder
                Stockpile.EmptyFolder(Path.Combine("WORKING", "TEMP"));

                //Commit any states in use
                commitUsedStatesToSession();

                foreach(var key in ssk.StashKeys)
                {
                    if (key == null)
                        continue;

                    string stateFilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name

                    string statePath = Path.Combine(CorruptCore.RtcCore.workingDir, key.StateLocation.ToString(), stateFilename);
                    string tempPath = Path.Combine(CorruptCore.RtcCore.workingDir, "TEMP", stateFilename);

                    if (File.Exists(statePath))
                        File.Copy(statePath, tempPath); // copy savestates to temp folder
                    else
                    {

                        MessageBox.Show("Couldn't find savestate " + statePath + "!\n\n. This is savestate index " +  ssk.StashKeys.IndexOf(key) + 1 + ".\nAborting save");
                        Stockpile.EmptyFolder(Path.Combine("WORKING", "TEMP"));
                        return;
                    }

                }

                //Use two separate loops here in case the first one aborts. We don't want to update the StateLocation unless we know we're good
                foreach(var key in ssk.StashKeys)
                {
                    if (key == null)
                        continue;
                    key.StateLocation = StashKeySavestateLocation.SSK;
                }

                //Create keys.json
                using (FileStream fs = File.Open(Path.Combine(CorruptCore.RtcCore.workingDir, "TEMP", "keys.json"), FileMode.OpenOrCreate))
                {
                    JsonHelper.Serialize(ssk, fs, Formatting.Indented);
                    fs.Close();
                }

                string tempFilename = Filename + ".temp";
                //If there's already a temp file from a previous failed save, delete it
                try
                {
                    if (File.Exists(tempFilename))
                        File.Delete(tempFilename);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                string tempFolderPath = Path.Combine(CorruptCore.RtcCore.workingDir, "TEMP");
                System.IO.Compression.ZipFile.CreateFromDirectory(tempFolderPath, tempFilename, System.IO.Compression.CompressionLevel.Fastest, false);

                if (File.Exists(Filename))
                    File.Delete(Filename);

                File.Move(tempFilename, Filename);

                //Move all the files from temp into SSK
                Stockpile.EmptyFolder(Path.Combine("WORKING", "SSK"));
                foreach (string file in Directory.GetFiles(tempFolderPath))
                    File.Move(file, Path.Combine(CorruptCore.RtcCore.workingDir, "SSK", Path.GetFileName(file)));
            }
            catch (Exception ex)
            {

                string additionalInfo = "The Savestate Keys file could not be saved\n\n";

                var ex2 = new CustomException(ex.Message, additionalInfo + ex.StackTrace);

                if (CloudDebug.ShowErrorDialog(ex2, true) == DialogResult.Abort)
                    throw new RTCV.NetCore.AbortEverythingException();

                return;
            }
        }

        internal void DisableFeature()
        {
            Controls.Clear();
        }

        private void pnSavestateHolder_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void pnSavestateHolder_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (files?.Length > 0 && files[0]
                .Contains(".ssk"))
            {
                loadSavestateList(false, files[0]);
            }

            //Bring the UI back to normal after a drag+drop to prevent weird merge stuff 
            S.GET<RTC_GlitchHarvesterBlast_Form>().RedrawActionUI();
        }

        private void cbSavestateLoadOnClick_CheckedChanged(object sender, EventArgs e)
        {
            LoadSavestateOnClick = cbSavestateLoadOnClick.Checked;
        }

        private void RTC_SavestateManager_Form_Shown(object sender, EventArgs e)
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
