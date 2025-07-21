using RTCV.UI.Components.Controls;

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
            if (!loadedTileForms.TryGetValue(componentForm, out ComponentFormTile foundForm))
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
                foundForm.SetComponentForm(componentForm, newSizeX.Value, newSizeY.Value, DisplayHeader);
            }
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
            this.SetSize(getTilePos(canvasGrid.Width), getTilePos(canvasGrid.Height), getTilePos(canvasGrid.MinimumWidth), getTilePos(canvasGrid.MinimumHeight));
        }

        public void SetSize(int width, int height, int minWidth, int minHeight)
        {
            if (this.TopLevel)
            {
                this.MinimumSize = new Size(minWidth + CoreForm.xPadding, minHeight + CoreForm.yPadding);
                this.Size = new Size(width + CoreForm.xPadding, height + CoreForm.yPadding);
            }
            else
            {
                CoreForm.thisForm.SetSize(width, height, minWidth, minHeight);
            }
        }

        private static void loadTileForm(CanvasForm targetForm, CanvasGrid canvasGrid, bool dontAnchor = false)
        {
            targetForm.ResizeCanvas(canvasGrid);

            for (int x = 0; x < canvasGrid.Width; x++)
            {
                for (int y = 0; y < canvasGrid.Height; y++)
                {
                    if (canvasGrid.gridComponent[x, y] is { } form)
                    {
                        if (dontAnchor && form.Parent == null)
                        {
                            form.Show();
                            continue;
                        }
                        targetForm.Text = canvasGrid.GridName;
                        
                        bool displayHeader = canvasGrid.gridComponentDisplayHeader[x, y].HasValue ? canvasGrid.gridComponentDisplayHeader[x, y].Value : false;
                        AnchorStyles anchor = canvasGrid.gridComponentAnchor[x, y].HasValue ? canvasGrid.gridComponentAnchor[x, y].Value : AnchorStyles.None;
                        var size = canvasGrid.gridComponentSize[x, y];
                        
                        ComponentFormTile tileForm = getTileForm(canvasGrid.gridComponent[x, y], size?.Width, size?.Height, displayHeader);
                        
                        tileForm.TopLevel = false;
                        targetForm.Controls.Add(tileForm);
                        tileForm.Location = getTileLocation(x, y);
                        tileForm.ChildForm.Anchor = anchor;
                        tileForm.Anchor = tileForm.ChildForm.Anchor;

                        tileForm.Show();
                    }
                }
            }

            //targetForm.MinimumSize = new Size(getTilePos(canvasGrid.MinimumWidth), getTilePos(canvasGrid.MinimumHeight));
        }

        //public void BlockView() => (this as IBlockable)?.BlockView();
        //public void UnblockView() => (this as IBlockable)?.UnblockView();

        internal static void loadTileFormExtraWindow(CanvasGrid canvasGrid, string WindowHeader, bool silent = false, bool dontAnchor = false)
        {
            if (allExtraForms.TryGetValue(WindowHeader, out var extraForm))
            {
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
            loadTileForm(extraForm, canvasGrid, dontAnchor);

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
                if (extraForm.Location.X < -30000)
                {
                    // when a window is minimized, it's location is set to -32000, -32000
                    // if it's closed while minimized, it will maintain that location for some reason upon reopening
                    extraForm.Location = new Point(0, 0);
                    // we have to call loadTileForm again as well and i don't know why
                    loadTileForm(extraForm, canvasGrid, dontAnchor);
                }
            }
        }

        internal static void loadTileFormMain(CanvasGrid canvasGrid, bool dontAnchor = false)
        {
            clearMainTileForm();
            loadTileForm(mainForm, canvasGrid, dontAnchor);

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
        
        public void ShowToast(Toast toast)
        {
            toast.SuspendLayout();
            toast.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            toast.Location = new Point(this.ClientRectangle.Width - toast.Width - 10, 10);
            
            this.Controls.Add(toast);
            
            toast.Show();
            toast.BringToFront();
            toast.ResumeLayout();
            toast.Refresh();
        }

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //S.GET<RTC_Core_Form>().btnGlitchHarvester.Text = S.GET<RTC_Core_Form>().btnGlitchHarvester.Text.Replace("○ ", "");

                if (Text == "Glitch Harvester")
                {
                    S.GET<CoreForm>().pnGlitchHarvesterOpen.Visible = false;
                }

                e.Cancel = true;
                Hide();
            }
        }
    }
}
