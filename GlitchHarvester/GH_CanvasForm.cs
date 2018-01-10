using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RTCV.GlitchHarvester.TileForms;

namespace RTCV.GlitchHarvester
{
    public partial class GH_CanvasForm : Form
    {
        public static GH_CanvasForm thisForm;
        public static List<GH_CanvasForm> extraForms = new List<GH_CanvasForm>();
        public GH_ShadowPanel spForm;

        public static int spacerSize;
        public static int tileSize;

        static Dictionary<string, Form> loadedTileForms = new Dictionary<string, Form>();

        public bool SubFormMode
        {
            get {
                return (spForm != null);
            }
            set {
                if (value == false && spForm != null)
                    CloseSubForm();
                }
        }

        public GH_CanvasForm(bool extraForm = false)
        {
            InitializeComponent();

            if (!extraForm)
            {
                thisForm = this;
                spacerSize = pnScale.Location.X;
                tileSize = pnScale.Size.Width;
            }


        }

        public static Form getTileForm(string tileForm)
        {
            if (!loadedTileForms.ContainsKey(tileForm))
            {
                loadedTileForms[tileForm] = (Form)Activator.CreateInstance(Type.GetType("RTCV.GlitchHarvester.TileForms." + tileForm));
            }

            return loadedTileForms[tileForm];
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
            clearTileForms();
            loadedTileForms.Clear();
        }

        public static void clearTileForms()
        {
            thisForm.Controls.Clear();
            foreach (Form frm in extraForms)
            {
                frm.Controls.Clear();
                frm.Close();
            }
            extraForms.Clear();

        }

        public void ResizeCanvas(GH_CanvasForm targetForm, CanvasGrid canvasGrid)
        {
            this.SetSize(getTilePos(canvasGrid.x), getTilePos(canvasGrid.y));
        }

        public void SetSize(int x, int y)
        {
            if (this.TopLevel)
                this.Size = new Size(x + GH_CoreForm.xPadding, y + GH_CoreForm.yPadding);
            else
                GH_CoreForm.thisForm.SetSize(x, y);
        }

        public static void loadMultiGrid(MultiGrid mg)
        {
            mg.Load();
        }

        public static void loadTileFormExtra(CanvasGrid canvasGrid)
        {
            GH_CanvasForm extraForm = new GH_CanvasForm(true);
            extraForm.Controls.Clear();
            extraForms.Add(extraForm);
            extraForm.FormBorderStyle = FormBorderStyle.FixedSingle;
            extraForm.MaximizeBox = false;
            extraForm.Text = "RTC Extra Form";
            loadTileForm(extraForm, canvasGrid);
            extraForm.Show();


        }

        public static void loadTileFormMain(CanvasGrid canvasGrid)
        {
            clearTileForms();
            loadTileForm(thisForm, canvasGrid);
        }

        public static void loadTileForm(GH_CanvasForm targetForm, CanvasGrid canvasGrid)
        {

            targetForm.ResizeCanvas(targetForm, canvasGrid);

            for (int x = 0; x < canvasGrid.x; x++)
                for (int y = 0; y < canvasGrid.y; y++)
                    if (canvasGrid.grid[x, y] != null)
                    {
                        Form tileForm = getTileForm(canvasGrid.grid[x, y]);
                        tileForm.TopLevel = false;
                        targetForm.Controls.Add(tileForm);
                        tileForm.Location = getTileLocation(x,y);
                        tileForm.Show();
                    }



        }

        private void button4_Click(object sender, EventArgs e)
        {
            //test button, to delete later

            if (spForm == null)
            {
                ShowSubForm("GH_DummySubForm");
            }
            else
                CloseSubForm();
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


        public void ShowSubForm(string _type)
        {

            //sets program to SubForm mode, darkens screen and displays flating form.
            //Start by giving type of Form class. Implement interface SubForms.GH_SubForm for Cancel/Ok buttons

            //See DummySubForm for example

            if (spForm != null)
                CloseSubForm();

            spForm = new GH_ShadowPanel(thisForm, _type);
            spForm.TopLevel = false;
            thisForm.Controls.Add(spForm);
            spForm.Show();
            spForm.BringToFront();
        }

        public void CloseSubForm()
        {
            //Closes subform and exists SubForm mode.
            //is automatically called when Cancel/Ok is pressed in SubForm.

            if (GH_ShadowPanel.subForm != null) {
                GH_ShadowPanel.subForm.Close();
                GH_ShadowPanel.subForm = null;
            }

            if (spForm != null)
            {
                spForm.Close();
                spForm = null;
            }
        }

        private void GH_CanvasForm_Load(object sender, EventArgs e)
        {
            
        }
        
    }
}
