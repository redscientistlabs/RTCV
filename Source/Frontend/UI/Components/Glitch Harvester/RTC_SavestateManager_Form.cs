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
        private BindingSource savestateBindingSource = new BindingSource(new BindingList<Tuple<StashKey, string>>(), null);


        public RTC_SavestateManager_Form()
		{
			InitializeComponent();

            popoutAllowed = true;
            this.undockedSizable = false;

            savestateList.DataSource = savestateBindingSource;
        }

        private void btnLoadSavestateList_Click(object sender, EventArgs e)
        {
            loadSavestateList();
        }


        private void RTC_SavestateManager_Form_Load(object sender, EventArgs e)
        {
            savestateList.AllowDrop = true;
            savestateList.DragDrop += pnSavestateHolder_DragDrop;
            savestateList.DragEnter += pnSavestateHolder_DragEnter;

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
            savestateBindingSource.Clear();

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

            for (var i = 0; i < ssk.StashKeys.Count; i++)
            {
                var key = ssk.StashKeys[i];
                if (key == null)
                    continue;

                //We have to set this first as we then change the other stuff
                key.StateLocation = StashKeySavestateLocation.SSK;

                string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name
                string newStatePath = CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + key.StateLocation + Path.DirectorySeparatorChar + statefilename;

                key.StateFilename = newStatePath;
                key.StateShortFilename = Path.GetFileName(newStatePath);

                savestateBindingSource.Add(new Tuple<StashKey, string>(key, ssk.Text[i]));
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
                    savestateBindingSource.Clear();

                }));

                columnsMenu.Show(this, locate);
            }
        }

        private void btnSaveSavestateList_Click(object sender, EventArgs e)
        {
            try
            {
                SaveStateKeys ssk = new SaveStateKeys();

                foreach (Tuple<StashKey, string> x in savestateBindingSource.List)
                {
                    ssk.StashKeys.Add(x.Item1);
                    ssk.Text.Add(x.Item2);
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
            S.GET<RTC_GlitchHarvesterBlast_Form>().RedrawActionUI();
        }
    }
}
