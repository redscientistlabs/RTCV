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

namespace RTCV.UI
{
	public partial class RTC_StashHistory_Form : ComponentForm, IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public bool DontLoadSelectedStash = false;

        public RTC_StashHistory_Form()
		{
			InitializeComponent();

            popoutAllowed = true;
            this.undockedSizable = true;

            this.MouseDoubleClick += ClearSelectedSKs;

            lbStashHistory.DataSource = StockpileManager_UISide.StashHistory;

        }

        public void btnAddStashToStockpile_Click(object sender, EventArgs e)
        {
            if (StockpileManager_UISide.CurrentStashkey != null && StockpileManager_UISide.CurrentStashkey.Alias != StockpileManager_UISide.CurrentStashkey.Key)
                AddStashToStockpile(false);
            else
                AddStashToStockpile(true);
        }

        public void AddStashToStockpile(bool askForName = true)
        {
            if (lbStashHistory.Items.Count == 0 || lbStashHistory.SelectedIndex == -1)
            {
                MessageBox.Show("Can't add the Stash to the Stockpile because none is selected in the Stash History");
                return;
            }

            string Name = "";
            string value = "";

            if (askForName)
            {
                if (GetInputBox("Glitch Harvester", "Enter the new Stash name:", ref value) == DialogResult.OK)
                {
                    Name = value.Trim();
                }
                else
                {
                    return;
                }
            }
            else
                Name = StockpileManager_UISide.CurrentStashkey.Alias;

            StockpileManager_UISide.CurrentStashkey = (StashKey)lbStashHistory.SelectedItem;

            if (String.IsNullOrWhiteSpace(Name))
                StockpileManager_UISide.CurrentStashkey.Alias = StockpileManager_UISide.CurrentStashkey.Key;
            else
                StockpileManager_UISide.CurrentStashkey.Alias = Name;


            StashKey sk = StockpileManager_UISide.CurrentStashkey;

            sk.BlastLayer.RasterizeVMDs();


            DataGridViewRow dataRow = S.GET<RTC_StockpileManager_Form>().dgvStockpile.Rows[S.GET<RTC_StockpileManager_Form>().dgvStockpile.Rows.Add()];
            dataRow.Cells["Item"].Value = sk;
            dataRow.Cells["GameName"].Value = sk.GameName;
            dataRow.Cells["SystemName"].Value = sk.SystemName;
            dataRow.Cells["SystemCore"].Value = sk.SystemCore;

            S.GET<RTC_StockpileManager_Form>().RefreshNoteIcons();

            StockpileManager_UISide.StashHistory.Remove(sk);

            RefreshStashHistory();

            DontLoadSelectedStash = true;
            lbStashHistory.ClearSelected();
            DontLoadSelectedStash = false;

            int nRowIndex = S.GET<RTC_StockpileManager_Form>().dgvStockpile.Rows.Count - 1;

            S.GET<RTC_StockpileManager_Form>().dgvStockpile.ClearSelection();
            S.GET<RTC_StockpileManager_Form>().dgvStockpile.Rows[nRowIndex].Selected = true;

            StockpileManager_UISide.StockpileChanged();

            S.GET<RTC_StockpileManager_Form>().UnsavedEdits = true;
        }

        public void RefreshStashHistory()
        {
            DontLoadSelectedStash = true;
            var lastSelect = lbStashHistory.SelectedIndex;

            DontLoadSelectedStash = true;
            lbStashHistory.DataSource = null;
            lbStashHistory.SelectedIndex = -1;

            DontLoadSelectedStash = true;
            //lbStashHistory.BeginUpdate();
            lbStashHistory.DataSource = StockpileManager_UISide.StashHistory;
            //lbStashHistory.EndUpdate();

            DontLoadSelectedStash = true;
            if (lastSelect < lbStashHistory.Items.Count)
                lbStashHistory.SelectedIndex = lastSelect;

            DontLoadSelectedStash = false;
        }

        private void lbStashHistory_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);

                ContextMenuStrip columnsMenu = new ContextMenuStrip();

                ((ToolStripMenuItem)columnsMenu.Items.Add("Open Selected Item in Blast Editor", null, new EventHandler((ob, ev) =>
                {
                    if (S.GET<RTC_NewBlastEditor_Form>() != null)
                    {
                        StashKey sk = StockpileManager_UISide.StashHistory[lbStashHistory.SelectedIndex];
                        RTC_NewBlastEditor_Form.OpenBlastEditor(sk);
                    }
                }))).Enabled = lbStashHistory.SelectedIndex != -1;

                columnsMenu.Items.Add(new ToolStripSeparator());

                ((ToolStripMenuItem)columnsMenu.Items.Add("Rename selected item", null, new EventHandler((ob, ev) =>
                {
                    StashKey sk = StockpileManager_UISide.StashHistory[lbStashHistory.SelectedIndex];
                    S.GET<RTC_StockpileManager_Form>().renameStashKey(sk);
                    RefreshStashHistory();
                }))).Enabled = lbStashHistory.SelectedIndex != -1;

                ((ToolStripMenuItem)columnsMenu.Items.Add("Generate VMD from Selected Item", null, new EventHandler((ob, ev) =>
                {
                    StashKey sk = StockpileManager_UISide.StashHistory[lbStashHistory.SelectedIndex];
                    sk.BlastLayer.RasterizeVMDs();
                    MemoryDomains.GenerateVmdFromStashkey(sk);
                    S.GET<RTC_VmdPool_Form>().RefreshVMDs();
                }))).Enabled = lbStashHistory.SelectedIndex != -1;

                columnsMenu.Items.Add(new ToolStripSeparator());

                ((ToolStripMenuItem)columnsMenu.Items.Add("Merge Selected Stashkeys", null, new EventHandler((ob, ev) =>
                {
                    List<StashKey> sks = new List<StashKey>();
                    foreach (StashKey sk in lbStashHistory.SelectedItems)
                        sks.Add((StashKey)sk);

                    StockpileManager_UISide.MergeStashkeys(sks);

                    RefreshStashHistory();
                }))).Enabled = (lbStashHistory.SelectedIndex != -1 && lbStashHistory.SelectedItems.Count > 1);

                /*
				if (!RTC_NetcoreImplementation.isStandaloneUI)
				{
					columnsMenu.Items.Add(new ToolStripSeparator());
					((ToolStripMenuItem)columnsMenu.Items.Add("[Multiplayer] Pull State from peer", null, new EventHandler((ob, ev) =>
						{
							S.GET<RTC_Multiplayer_Form>().cbPullStateToGlitchHarvester.Checked = true;
							RTC_NetcoreImplementation.Multiplayer.SendCommand(new RTC_Command(CommandType.PULLSTATE), false);
						}))).Enabled = RTC_NetcoreImplementation.Multiplayer != null && RTC_NetcoreImplementation.Multiplayer.side != NetworkSide.DISCONNECTED;
				}*/

                columnsMenu.Show(this, locate);
            }
        }

        private void RTC_StashHistory_Form_Load(object sender, EventArgs e)
        {
            RefreshStashHistory();
        }

        public void RefreshStashHistorySelectLast()
        {
            RefreshStashHistory();
            DontLoadSelectedStash = true;
            lbStashHistory.ClearSelected();
            DontLoadSelectedStash = true;
            lbStashHistory.SelectedIndex = lbStashHistory.Items.Count - 1;
        }

        private void lbStashHistory_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                lbStashHistory.Enabled = false;
                btnStashUP.Enabled = false;
                btnStashDOWN.Enabled = false;
                btnAddStashToStockpile.Enabled = false;


                if (DontLoadSelectedStash || lbStashHistory.SelectedIndex == -1)
                {
                    DontLoadSelectedStash = false;
                    return;
                }

                S.GET<RTC_StockpileManager_Form>().dgvStockpile.ClearSelection();
                S.GET<RTC_StockpilePlayer_Form>().dgvStockpile.ClearSelection();

                var blastForm = S.GET<RTC_GlitchHarvesterBlast_Form>();

                if (S.GET<RTC_GlitchHarvesterBlast_Form>().MergeMode)
                {
                    blastForm.ghMode = GlitchHarvesterMode.CORRUPT;
                    S.GET<RTC_StockpileManager_Form>().btnRenameSelected.Visible = true;
                    S.GET<RTC_StockpileManager_Form>().btnRemoveSelectedStockpile.Text = "  Remove Item";

                    if (blastForm.ghMode == GlitchHarvesterMode.CORRUPT)
                        blastForm.btnCorrupt.Text = "  Corrupt";
                    else if (blastForm.ghMode == GlitchHarvesterMode.INJECT)
                        blastForm.btnCorrupt.Text = "  Inject";
                    else if (blastForm.ghMode == GlitchHarvesterMode.ORIGINAL)
                        blastForm.btnCorrupt.Text = "  Original";
                }

                StockpileManager_UISide.CurrentStashkey = StockpileManager_UISide.StashHistory[lbStashHistory.SelectedIndex];

                if (!blastForm.LoadOnSelect)
                    return;

                blastForm.OneTimeExecute();
            }
            finally
            {
                lbStashHistory.Enabled = true;
                btnStashUP.Enabled = true;
                btnStashDOWN.Enabled = true;
                btnAddStashToStockpile.Enabled = true;
                ((Control)sender).Focus();
                S.GET<RTC_GlitchHarvesterBlast_Form>().RedrawActionUI();
            }
        }

        private void ClearSelectedSKs(object sender, MouseEventArgs e)
        {
            DontLoadSelectedStash = true;
            lbStashHistory.ClearSelected();
            DontLoadSelectedStash = true;
            S.GET<RTC_StockpileManager_Form>().dgvStockpile.ClearSelection();
            S.GET<RTC_GlitchHarvesterBlast_Form>().RedrawActionUI();

        }

        private void btnClearStashHistory_Click(object sender, EventArgs e)
        {
            StockpileManager_UISide.StashHistory.Clear();
            RefreshStashHistory();

            //Force clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void btnStashUP_Click(object sender, EventArgs e)
        {
            if (lbStashHistory.SelectedIndex == -1)
                return;

            if (lbStashHistory.SelectedIndex == 0)
            {
                lbStashHistory.ClearSelected();
                lbStashHistory.SelectedIndex = lbStashHistory.Items.Count - 1;
            }
            else
            {
                int newPos = lbStashHistory.SelectedIndex - 1;
                lbStashHistory.ClearSelected();
                lbStashHistory.SelectedIndex = newPos;

            }

        }

        private void btnStashDOWN_Click(object sender, EventArgs e)
        {

            if (lbStashHistory.SelectedIndex == -1)
                return;


            if (lbStashHistory.SelectedIndex == lbStashHistory.Items.Count - 1)
            {
                lbStashHistory.ClearSelected();
                lbStashHistory.SelectedIndex = 0;
            }
            else
            {
                int newPos = lbStashHistory.SelectedIndex + 1;
                lbStashHistory.ClearSelected();
                lbStashHistory.SelectedIndex = newPos;

            }

        }


    }
}
