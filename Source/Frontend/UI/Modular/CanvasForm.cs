namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using RTCV.Common;
    using RTCV.UI.Modular;

    #pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    public partial class CanvasForm : ColorizedForm, IBlockable
    {
        internal static CanvasForm mainForm { get; private set; }
        internal static List<CanvasForm> extraForms { get; private set; } = new List<CanvasForm>();
        private static Dictionary<string, CanvasForm> allExtraForms = new Dictionary<string, CanvasForm>();
        public ShadowPanel spForm { get; private set; }

        public Panel blockPanel { get; set; } = null;

        internal static int spacerSize { get; private set; }
        internal static int tileSize { get; private set; }
        private static Dictionary<Form, ComponentFormTile> loadedTileForms = new Dictionary<Form, ComponentFormTile>();

        public CanvasForm(bool extraForm = false)
        {
            InitializeComponent();

            if (!extraForm)
            {
                mainForm = this;
                spacerSize = pnScale.Location.X;
                tileSize = pnScale.Size.Width;
                Controls.Remove(pnScale);
            }
        }

        private static ComponentFormTile getTileForm(Form componentForm, int? newSizeX = null, int? newSizeY = null, bool DisplayHeader = true)
        {
            if (!loadedTileForms.ContainsKey(componentForm))
            {
                var newForm = (ComponentFormTile)Activator.CreateInstance(typeof(ComponentFormTile));
                loadedTileForms[componentForm] = newForm;

                if (newSizeX != null && newSizeY != null)
                {
                    newForm.SetComponentForm(componentForm, newSizeX.Value, newSizeY.Value, DisplayHeader);
                }
            }
            else
            {
                var foundForm = loadedTileForms[componentForm];
                foundForm.SetComponentForm(componentForm, newSizeX.Value, newSizeY.Value, DisplayHeader);
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

        public static CanvasForm GetExtraForm(string windowTitle)
        {
            allExtraForms.TryGetValue(windowTitle, out CanvasForm outForm);
            return outForm;
        }

        private void ResizeCanvas(CanvasGrid canvasGrid)
        {
            this.SetSize(getTilePos(canvasGrid.X), getTilePos(canvasGrid.Y));
        }

        public void SetSize(int x, int y)
        {
            if (this.TopLevel)
            {
                this.Size = new Size(x + CoreForm.xPadding, y + CoreForm.yPadding);
            }
            else
            {
                CoreForm.thisForm.SetSize(x, y);
            }
        }

        private static void loadTileForm(CanvasForm targetForm, CanvasGrid canvasGrid)
        {
            targetForm.ResizeCanvas(canvasGrid);

            for (int x = 0; x < canvasGrid.X; x++)
            {
                for (int y = 0; y < canvasGrid.Y; y++)
                {
                    if (canvasGrid.gridComponent[x, y] != null)
                    {
                        targetForm.Text = canvasGrid.GridName;
                        bool DisplayHeader = (canvasGrid.gridComponentDisplayHeader[x, y].HasValue ? canvasGrid.gridComponentDisplayHeader[x, y].Value : false);
                        var size = canvasGrid.gridComponentSize[x, y];
                        ComponentFormTile tileForm = getTileForm(canvasGrid.gridComponent[x, y], size?.Width, size?.Height, DisplayHeader);
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

        internal static void loadTileFormExtraWindow(CanvasGrid canvasGrid, string WindowHeader, bool silent = false)
        {
            CanvasForm extraForm;

            if (allExtraForms.ContainsKey(WindowHeader))
            {
                extraForm = allExtraForms[WindowHeader];

                foreach (Control ctr in extraForm?.Controls)
                {
                    if (ctr is ComponentFormTile cft)
                    {
                        cft.ReAnchorToPanel();
                    }
                }
            }
            else
            {
                extraForm = new CanvasForm(true);
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

        internal static void loadTileFormMain(CanvasGrid canvasGrid)
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

        public void OpenSubForm(ISubForm reqForm, bool lockSidebar = false)
        {
            //sets program to SubForm mode, darkens screen and displays flating form.
            //Start by giving type of Form class. Implement interface SubForms.UI_SubForm for Cancel/Ok buttons

            //See DummySubForm for example

            if (lockSidebar)
            {
                S.GET<CoreForm>().LockSideBar();
            }

            if (spForm != null)
            {
                CloseSubForm();
            }

            spForm = new ShadowPanel(this, reqForm)
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

            S.GET<CoreForm>().UnlockSideBar();

            if (spForm != null)
            {
                ((ISubForm)spForm.subForm).OnHidden();
                spForm.subForm = null;
                spForm.Hide();
                spForm = null;
            }
        }

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //S.GET<RTC_Core_Form>().btnGlitchHarvester.Text = S.GET<RTC_Core_Form>().btnGlitchHarvester.Text.Replace("○ ", "");

                if (this.Text == "Glitch Harvester")
                {
                    S.GET<CoreForm>().pnGlitchHarvesterOpen.Visible = false;
                }

                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
