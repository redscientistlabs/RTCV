using System;
using System.Collections.Generic;
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

        public string[] btnParentKeys = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
        public string[] btnAttachedRom = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };


        Dictionary<string, TextBox> StateBoxes = new Dictionary<string, TextBox>();

        public RTC_SavestateManager_Form()
		{
			InitializeComponent();

			popoutAllowed = false;



            #region textbox states to dico

            StateBoxes.Add("01", tbSavestate01);
            StateBoxes.Add("02", tbSavestate02);
            StateBoxes.Add("03", tbSavestate03);
            StateBoxes.Add("04", tbSavestate04);
            StateBoxes.Add("05", tbSavestate05);
            StateBoxes.Add("06", tbSavestate06);
            StateBoxes.Add("07", tbSavestate07);
            StateBoxes.Add("08", tbSavestate08);
            StateBoxes.Add("09", tbSavestate09);
            StateBoxes.Add("10", tbSavestate10);
            StateBoxes.Add("11", tbSavestate11);
            StateBoxes.Add("12", tbSavestate12);
            StateBoxes.Add("13", tbSavestate13);
            StateBoxes.Add("14", tbSavestate14);
            StateBoxes.Add("15", tbSavestate15);
            StateBoxes.Add("16", tbSavestate16);
            StateBoxes.Add("17", tbSavestate17);
            StateBoxes.Add("18", tbSavestate18);
            StateBoxes.Add("19", tbSavestate19);
            StateBoxes.Add("20", tbSavestate20);
            StateBoxes.Add("21", tbSavestate21);
            StateBoxes.Add("22", tbSavestate22);
            StateBoxes.Add("23", tbSavestate23);
            StateBoxes.Add("24", tbSavestate24);
            StateBoxes.Add("25", tbSavestate25);
            StateBoxes.Add("26", tbSavestate26);
            StateBoxes.Add("27", tbSavestate27);
            StateBoxes.Add("28", tbSavestate28);
            StateBoxes.Add("29", tbSavestate29);
            StateBoxes.Add("30", tbSavestate30);
            StateBoxes.Add("31", tbSavestate31);
            StateBoxes.Add("32", tbSavestate32);
            StateBoxes.Add("33", tbSavestate33);
            StateBoxes.Add("34", tbSavestate34);
            StateBoxes.Add("35", tbSavestate35);
            StateBoxes.Add("36", tbSavestate36);
            StateBoxes.Add("37", tbSavestate37);
            StateBoxes.Add("38", tbSavestate38);
            StateBoxes.Add("39", tbSavestate39);
            StateBoxes.Add("40", tbSavestate40);

            #endregion textbox states to dico


        }

        private void btnLoadSavestateList_Click(object sender, EventArgs e)
        {
            loadSavestateList();
        }

        public void RefreshSavestateTextboxes()
        {
            //fill text/state controls/dico
            for (int i = 1; i < 41; i++)
            {
                string key = i.ToString().PadLeft(2, '0');

                StateBoxes[key].Visible = StockpileManager_UISide.SavestateStashkeyDico.ContainsKey(key);
            }
        }

        private void RTC_SavestateManager_Form_Load(object sender, EventArgs e)
        {
            pnSavestateHolder.AllowDrop = true;
            pnSavestateHolder.DragDrop += pnSavestateHolder_DragDrop;
            pnSavestateHolder.DragEnter += pnSavestateHolder_DragEnter;

            RefreshSavestateTextboxes();

        }

        public void btnSavestate_Click(object sender, EventArgs e)
        {
            try
            {
                ((Button)sender).Visible = false;

                foreach (var item in pnSavestateHolder.Controls)
                    if (item is Button button)
                        button.ForeColor = Color.FromArgb(192, 255, 192);

                Button clickedButton = ((Button)sender);
                clickedButton.ForeColor = Color.OrangeRed;
                clickedButton.BringToFront();

                StockpileManager_UISide.CurrentSavestateKey = clickedButton.Text;
                StashKey psk = StockpileManager_UISide.GetCurrentSavestateStashkey();

                if (psk != null && !File.Exists(psk.RomFilename))
                {
                    if (DialogResult.Yes == MessageBox.Show($"Can't find file {psk.RomFilename}\nGame name: {psk.GameName}\nSystem name: {psk.SystemName}\n\n Would you like to provide a new file for replacement?", "Error: File not found", MessageBoxButtons.YesNo))
                    {
                        OpenFileDialog ofd = new OpenFileDialog
                        {
                            DefaultExt = "*",
                            Title = "Select Replacement File",
                            Filter = "Any file|*.*",
                            RestoreDirectory = true
                        };
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            string filename = ofd.FileName.ToString();
                            string oldFilename = psk.RomFilename;
                            for (int i = 1; i < 41; i++)
                            {
                                string key = i.ToString().PadLeft(2, '0');

                                if (StockpileManager_UISide.SavestateStashkeyDico.ContainsKey(key))
                                {
                                    StashKey sk = StockpileManager_UISide.SavestateStashkeyDico[key];
                                    if (sk.RomFilename == oldFilename)
                                        sk.RomFilename = filename;
                                }
                            }
                        }
                        else
                        {
                            clickedButton.ForeColor = Color.FromArgb(192, 255, 192);
                            StockpileManager_UISide.CurrentSavestateKey = null;
                            return;
                        }
                    }
                    else
                    {
                        clickedButton.ForeColor = Color.FromArgb(192, 255, 192);
                        StockpileManager_UISide.CurrentSavestateKey = null;
                        return;
                    }
                }


                if (cbSavestateLoadOnClick.Checked)
                {
                    btnSaveLoad.Text = "LOAD";
                    btnSaveLoad_Click(null, null);
                }
                //StockpileManager_UISide.LoadState(StockpileManager_UISide.getCurrentSavestateStashkey());
            }
            finally
            {
                ((Button)sender).Visible = true;
            }
        }

        private void btnToggleSaveLoad_Click(object sender, EventArgs e)
        {
            if (btnSaveLoad.Text == "LOAD")
            {
                btnSaveLoad.Text = "SAVE";
                btnSaveLoad.ForeColor = Color.OrangeRed;
            }
            else
            {
                btnSaveLoad.Text = "LOAD";
                btnSaveLoad.ForeColor = Color.FromArgb(192, 255, 192);
            }
        }

        public void btnSaveLoad_Click(object sender, EventArgs e)
        {
            if (btnSaveLoad.Text == "LOAD")
            {
                StashKey psk = StockpileManager_UISide.GetCurrentSavestateStashkey();
                if (psk != null)
                {
                    if (!File.Exists(psk.RomFilename))
                        if (DialogResult.Yes == MessageBox.Show($"Can't find file {psk.RomFilename}\nGame name: {psk.GameName}\nSystem name: {psk.SystemName}\n\n Would you like to provide a new file for replacement?", "Error: File not found", MessageBoxButtons.YesNo))
                        {
                            OpenFileDialog ofd = new OpenFileDialog
                            {
                                DefaultExt = "*",
                                Title = "Select Replacement File",
                                Filter = "Any file|*.*",
                                RestoreDirectory = true
                            };
                            if (ofd.ShowDialog() == DialogResult.OK)
                            {
                                string filename = ofd.FileName.ToString();
                                string oldFilename = psk.RomFilename;
                                for (int i = 1; i < 41; i++)
                                {
                                    string key = i.ToString().PadLeft(2, '0');

                                    if (StockpileManager_UISide.SavestateStashkeyDico.ContainsKey(key))
                                    {
                                        StashKey sk = StockpileManager_UISide.SavestateStashkeyDico[key];
                                        if (sk.RomFilename == oldFilename)
                                            sk.RomFilename = filename;
                                    }
                                }
                            }
                            else
                                return;
                        }

                    StockpileManager_UISide.LoadState(psk);
                }
                else
                    MessageBox.Show("Savestate box is empty");
            }
            else
            {
                if (StockpileManager_UISide.CurrentSavestateKey == null)
                {
                    MessageBox.Show("No Savestate Box is currently selected in the Glitch Harvester's Savestate Manager");
                    return;
                }

                StashKey sk = StockpileManager_UISide.SaveState(true);

                btnSaveLoad.Text = "LOAD";
                btnSaveLoad.ForeColor = Color.FromArgb(192, 255, 192);

                RefreshSavestateTextboxes();
            }
        }

        private void loadSavestateList(string fileName = null)
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

            //Commit any used states to the SESSION folder
            commitUsedStatesToSession();

            try
            {

                Stockpile.EmptyFolder(Path.DirectorySeparatorChar + "WORKING\\TEMP");
                if (!Stockpile.Extract(fileName, Path.DirectorySeparatorChar + "WORKING\\SSK", "keys.json"))
                    return;

                using (FileStream fs = File.Open(CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + "SSK\\keys.json", FileMode.OpenOrCreate))
                {
                    ssk = JsonHelper.Deserialize<SaveStateKeys>(fs);
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


            for (int i = 1; i < 41; i++)
            {
                StashKey key = ssk.StashKeys[i];

                if (key == null)
                    continue;

                //We have to set this first as we then change the other stuff
                key.StateLocation = StashKeySavestateLocation.SSK;

                string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name
                string newStatePath = CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + key.StateLocation + Path.DirectorySeparatorChar + statefilename;

                key.StateFilename = newStatePath;
                key.StateShortFilename = Path.GetFileName(newStatePath);
            }

            //clear the stockpile dico
            StockpileManager_UISide.SavestateStashkeyDico.Clear();

            //fill text/state controls/dico
            for (int i = 1; i < 41; i++)
            {
                string key = i.ToString().PadLeft(2, '0');

                if (ssk.StashKeys[i] != null)
                {
                    if (!StockpileManager_UISide.SavestateStashkeyDico.ContainsKey(key))
                        StockpileManager_UISide.SavestateStashkeyDico.Add(key, ssk.StashKeys[i]);
                    else
                        StockpileManager_UISide.SavestateStashkeyDico[key] = ssk.StashKeys[i];
                }

                StateBoxes[key].Text = "";

                if (ssk.Text[i] != null)
                    StateBoxes[key].Text = ssk.Text[i];
            }


            RefreshSavestateTextboxes();
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
            foreach (var sk in allStashKeys.Where(x => x?.StateLocation == StashKeySavestateLocation.SSK))
            {
                try
                {
                    var stateName = sk.GameName + "." + sk.ParentKey + ".timejump.State"; // get savestate name
                    File.Copy(CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + "SSK" + Path.DirectorySeparatorChar + stateName
                        , CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + "SESSION" + Path.DirectorySeparatorChar + stateName, true);
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

                    foreach (var item in pnSavestateHolder.Controls)
                    {
                        if (item is Button)
                            (item as Button).ForeColor = Color.FromArgb(192, 255, 192);

                        if (item is TextBox)
                            (item as TextBox).Text = "";
                    }

                    for (int i = 1; i < 41; i++)
                    {
                        string key = i.ToString().PadLeft(2, '0');

                        if (key == null)
                            continue;

                        if (StockpileManager_UISide.SavestateStashkeyDico.ContainsKey(key))
                            StockpileManager_UISide.SavestateStashkeyDico.Remove(key);

                    }

                    StockpileManager_UISide.CurrentSavestateKey = null;

                    RefreshSavestateTextboxes();
                }));

                columnsMenu.Show(this, locate);
            }
        }

        private void btnSaveSavestateList_Click(object sender, EventArgs e)
        {
            try
            {
                SaveStateKeys ssk = new SaveStateKeys();

                for (int i = 1; i < 41; i++)
                {
                    string key = i.ToString().PadLeft(2, '0');

                    if (StockpileManager_UISide.SavestateStashkeyDico.ContainsKey(key))
                    {
                        ssk.StashKeys[i] = StockpileManager_UISide.SavestateStashkeyDico[key];
                        ssk.Text[i] = StateBoxes[key].Text;
                    }
                    else
                    {
                        ssk.StashKeys[i] = null;
                        ssk.Text[i] = null;
                    }
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
                    //ShortFilename = Filename.Substring(Filename.LastIndexOf(Path.DirectorySeparatorChar) + 1, Filename.Length - (Filename.LastIndexOf(Path.DirectorySeparatorChar) + 1));
                    ShortFilename = Path.GetFileName(Filename);
                }
                else
                    return;

                //clean temp folder
                Stockpile.EmptyFolder(Path.DirectorySeparatorChar + "WORKING\\TEMP");

                for (int i = 1; i < 41; i++)
                {
                    StashKey key = ssk.StashKeys[i];

                    if (key == null)
                        continue;

                    string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name

                    if (File.Exists(CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + key.StateLocation.ToString() + Path.DirectorySeparatorChar + statefilename))
                        File.Copy(CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + key.StateLocation.ToString() + Path.DirectorySeparatorChar + statefilename, CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP" + Path.DirectorySeparatorChar + statefilename); // copy savestates to temp folder
                    else
                    {
                        MessageBox.Show("Couldn't find savestate " + CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar +
                                        key.StateLocation.ToString() + Path.DirectorySeparatorChar + statefilename +
                                        "!\n\n. This is savestate index " + i + 1 + ".\nAborting save");
                        Stockpile.EmptyFolder(Path.DirectorySeparatorChar + "WORKING\\TEMP");
                        return;
                    }

                }

                //Use two separate loops here in case the first one aborts. We don't want to update the StateLocation unless we know we're good
                for (int i = 1; i < 41; i++)
                {
                    StashKey key = ssk.StashKeys[i];

                    if (key == null)
                        continue;
                    key.StateLocation = StashKeySavestateLocation.SSK;
                }

                //Create keys.json
                using (FileStream fs = File.Open(CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP\\keys.json", FileMode.OpenOrCreate))
                {
                    JsonHelper.Serialize(ssk, fs, Formatting.Indented);
                    fs.Close();
                }

                //7z the temp folder to destination filename
                //string[] stringargs = { "-c", Filename, RTC_Core.rtcDir + Path.DirectorySeparatorChar + "TEMP4" + Path.DirectorySeparatorChar };
                //FastZipProgram.Exec(stringargs);

                string tempFilename = Filename + ".temp";

                System.IO.Compression.ZipFile.CreateFromDirectory(CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP" + Path.DirectorySeparatorChar, tempFilename, System.IO.Compression.CompressionLevel.Fastest, false);

                if (File.Exists(Filename))
                    File.Delete(Filename);

                File.Move(tempFilename, Filename);

                //Move all the files from temp into SSK
                Stockpile.EmptyFolder(Path.DirectorySeparatorChar + "WORKING\\SSK");
                foreach (string file in Directory.GetFiles(CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP"))
                    //File.Move(file, RTC_Core.workingDir + Path.DirectorySeparatorChar + "SSK" + Path.DirectorySeparatorChar + (file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1, file.Length - (file.LastIndexOf(Path.DirectorySeparatorChar) + 1))));
                    File.Move(file, CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + "SSK" + Path.DirectorySeparatorChar + Path.GetFileName(file));
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

        private void btnBackPanelPage_Click(object sender, EventArgs e)
        {
            if (pnSavestateHolder.Location.X != 0)
                pnSavestateHolder.Location = new Point(pnSavestateHolder.Location.X + 150, pnSavestateHolder.Location.Y);
        }

        private void btnForwardPanelPage_Click(object sender, EventArgs e)
        {
            if (pnSavestateHolder.Location.X != -450)
                pnSavestateHolder.Location = new Point(pnSavestateHolder.Location.X - 150, pnSavestateHolder.Location.Y);
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
                loadSavestateList(files[0]);
            }

            //Bring the UI back to normal after a drag+drop to prevent weird merge stuff 
            RedrawActionUI();
        }


    }
}
