namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using RTCV.Common;

    public partial class UI_CanvasForm : Form, IBlockable
    {
        public static UI_CanvasForm mainForm;
        public static List<UI_CanvasForm> extraForms = new List<UI_CanvasForm>();
        private static Dictionary<string, UI_CanvasForm> allExtraForms = new Dictionary<string, UI_CanvasForm>();
        public UI_ShadowPanel spForm;

        public Panel blockPanel { get; set; } = null;

        public static int spacerSize;
        public static int tileSize;
        private static Dictionary<Form, UI_ComponentFormTile> loadedTileForms = new Dictionary<Form, UI_ComponentFormTile>();

        public bool SubFormMode
        {
            get => (spForm != null);
            set
            {
                if (value == false && spForm != null)
                {
                    CloseSubForm();
                }
            }
        }

        public UI_CanvasForm(bool extraForm = false)
        {
            InitializeComponent();

            UICore.SetRTCColor(UICore.GeneralColor, this);

            if (!extraForm)
            {
                mainForm = this;
                spacerSize = pnScale.Location.X;
                tileSize = pnScale.Size.Width;
                Controls.Remove(pnScale);
            }
        }

        public static UI_ComponentFormTile getTileForm(Form componentForm, int? newSizeX = null, int? newSizeY = null, bool DisplayHeader = true)
        {
            if (!loadedTileForms.ContainsKey(componentForm))
            {
                var newForm = (UI_ComponentFormTile)Activator.CreateInstance(typeof(UI_ComponentFormTile));
                loadedTileForms[componentForm] = newForm;

                if (newSizeX != null && newSizeY != null)
                {
                    newForm.SetCompoentForm(componentForm, newSizeX.Value, newSizeY.Value, DisplayHeader);
                }
            }
            else
            {
                var foundForm = loadedTileForms[componentForm];
                foundForm.SetCompoentForm(componentForm, newSizeX.Value, newSizeY.Value, DisplayHeader);
            }
            /*
            else
            {
                componentForm.Size = new Size(newSizeX.Value, newSizeY.Value);
            }
            */
            return loadedTileForms[componentForm];
        }

        public static int getTilePos(int gridPos)
        {
            return (gridPos * tileSize) + (gridPos * spacerSize) + spacerSize;
        }

        public static Point getTileLocation(int x, int y)
        {
            return new Point(getTilePos(x), getTilePos(y));
        }

        public static void unloadTileForms()
        {
            clearExtraTileForms();
            loadedTileForms.Clear();
        }

        public static void clearExtraTileForms()
        {
            foreach (Form frm in extraForms)
            {
                frm.Controls.Clear();
                frm.Close();
            }

            extraForms.Clear();
            loadedTileForms.Clear();
        }

        public static void clearMainTileForm()
        {
            mainForm.Controls.Clear();

            loadedTileForms.Clear();
        }

        public static UI_CanvasForm GetExtraForm(string windowTitle)
        {
            allExtraForms.TryGetValue(windowTitle, out UI_CanvasForm outForm);
            return outForm;
        }

        public void ResizeCanvas(UI_CanvasForm targetForm, CanvasGrid canvasGrid)
        {
            this.SetSize(getTilePos(canvasGrid.x), getTilePos(canvasGrid.y));
        }

        public void SetSize(int x, int y)
        {
            if (this.TopLevel)
            {
                this.Size = new Size(x + UI_CoreForm.xPadding, y + UI_CoreForm.yPadding);
            }
            else
            {
                UI_CoreForm.thisForm.SetSize(x, y);
            }
        }

        public static void loadTileForm(UI_CanvasForm targetForm, CanvasGrid canvasGrid)
        {
            targetForm.ResizeCanvas(targetForm, canvasGrid);

            for (int x = 0; x < canvasGrid.x; x++)
            {
                for (int y = 0; y < canvasGrid.y; y++)
                {
                    if (canvasGrid.gridComponent[x, y] != null)
                    {
                        targetForm.Text = canvasGrid.GridName;
                        bool DisplayHeader = (canvasGrid.gridComponentDisplayHeader[x, y].HasValue ? canvasGrid.gridComponentDisplayHeader[x, y].Value : false);
                        var size = canvasGrid.gridComponentSize[x, y];
                        UI_ComponentFormTile tileForm = getTileForm(canvasGrid.gridComponent[x, y], size?.Width, size?.Height, DisplayHeader);
                        tileForm.TopLevel = false;
                        targetForm.Controls.Add(tileForm);
                        tileForm.Location = getTileLocation(x, y);

                        tileForm.Anchor = tileForm.childForm.Anchor;

                        tileForm.Show();
                    }
                }
            }

            targetForm.MinimumSize = targetForm.Size;
        }

        //public void BlockView() => (this as IBlockable)?.BlockView();
        //public void UnblockView() => (this as IBlockable)?.UnblockView();

        public static void loadTileFormExtraWindow(CanvasGrid canvasGrid, string WindowHeader, bool silent = false)
        {
            UI_CanvasForm extraForm;

            if (allExtraForms.ContainsKey(WindowHeader))
            {
                extraForm = allExtraForms[WindowHeader];

                foreach (Control ctr in extraForm?.Controls)
                {
                    if (ctr is UI_ComponentFormTile cft)
                    {
                        cft.ReAnchorToPanel();
                    }
                }
            }
            else
            {
                extraForm = new UI_CanvasForm(true);
                allExtraForms[WindowHeader] = extraForm;
                extraForms.Add(extraForm);
            }

            extraForm.Controls.Clear();
            extraForm.Text = WindowHeader;

            UICore.registerFormEvents(extraForm);
            UICore.registerHotkeyBlacklistControls(extraForm);
            loadTileForm(extraForm, canvasGrid);

            if (canvasGrid.isResizable)
            {
                extraForm.MaximizeBox = true;
                extraForm.FormBorderStyle = FormBorderStyle.Sizable;
            }
            else
            {
                extraForm.MaximizeBox = false;
                extraForm.FormBorderStyle = FormBorderStyle.FixedSingle;
            }

            if (!silent)
            {
                extraForm.Show();
                extraForm.Focus();
            }
        }

        public static void loadTileFormMain(CanvasGrid canvasGrid)
        {
            clearMainTileForm();
            loadTileForm(mainForm, canvasGrid);

            if (mainForm.Parent is Form f)
            {
                if (canvasGrid.isResizable)
                {
                    f.FormBorderStyle = FormBorderStyle.Sizable;
                    f.MaximizeBox = true;
                }
                else
                {
                    f.FormBorderStyle = FormBorderStyle.FixedSingle;
                    f.MaximizeBox = false;
                }
            }

            //thisForm.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        }

        public int getTileSpacesX()
        {
            int sizeX = this.Size.Width;
            int tilesSpace = sizeX - spacerSize;
            int nbSpaces = (int)((double)tilesSpace / (spacerSize + tileSize));

            return nbSpaces;
        }

        public int getTileSpacesY()
        {
            int sizeY = this.Size.Height;
            int tilesSpace = sizeY - spacerSize;
            int nbSpaces = (int)((double)tilesSpace / (spacerSize + tileSize));

            return nbSpaces;
        }

        public void OpenSubForm(ISubForm reqForm, bool lockSidebar = false)
        {
            //sets program to SubForm mode, darkens screen and displays flating form.
            //Start by giving type of Form class. Implement interface SubForms.UI_SubForm for Cancel/Ok buttons

            //See DummySubForm for example

            if (lockSidebar)
            {
                S.GET<UI_CoreForm>().LockSideBar();
            }

            if (spForm != null)
            {
                CloseSubForm();
            }

            spForm = new UI_ShadowPanel(this, reqForm)
            {
                TopLevel = false
            };
            this.Controls.Add(spForm);

            spForm.Show();
            spForm.BringToFront();
            spForm.Refresh();
        }

        public void CloseSubForm()
        {
            //Closes subform and exists SubForm mode.
            //is automatically called when Cancel/Ok is pressed in SubForm.

            S.GET<UI_CoreForm>().UnlockSideBar();

            if (spForm != null)
            {
                ((ISubForm)spForm.subForm).OnHidden();
                spForm.subForm = null;
                spForm.Hide();
                spForm = null;
            }
        }

        private void UI_CanvasForm_Load(object sender, EventArgs e)
        {
        }

        private void UI_CanvasForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                //S.GET<RTC_Core_Form>().btnGlitchHarvester.Text = S.GET<RTC_Core_Form>().btnGlitchHarvester.Text.Replace("○ ", "");

                if (this.Text == "Glitch Harvester")
                {
                    S.GET<UI_CoreForm>().pnGlitchHarvesterOpen.Visible = false;
                }

                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
