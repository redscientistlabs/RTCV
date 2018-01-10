using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTCV.GlitchHarvester
{
    public partial class GH_CoreForm : Form
    {
        //This form traps events and forwards them.
        //It contains the single GH_CanvasForm instance.

        public static GH_CoreForm thisForm;
        public static GH_CanvasForm cfForm;


        //Vallues used for padding and scaling properly in high dpi
        public static int xPadding;
        public static int coreYPadding; // height of the top bar
        public static int yPadding;


        public GH_CoreForm()
        {
            InitializeComponent();
            thisForm = this;

            cfForm = new GH_CanvasForm();
            cfForm.TopLevel = false;
            cfForm.Dock = DockStyle.Fill;
            this.Controls.Add(cfForm);
            cfForm.Location = new Point(0, pnTopBar.Size.Height);
            cfForm.Show();
            cfForm.BringToFront();

            xPadding = (Width - cfForm.Width);
            coreYPadding = pnTopBar.Height;
            yPadding = (Height - cfForm.Height) - coreYPadding;
            

        }

        private void GH_CoreForm_Load(object sender, EventArgs e)
        {
            NetCoreServer.StartLoopback();
        }

        public void SetSize(int x, int y)
        {
            this.Size = new Size(x + xPadding, y + yPadding + coreYPadding);
        }

        private void GH_CoreForm_ResizeBegin(object sender, EventArgs e)
        {
            //Sends event to SubForm 
            if(cfForm.spForm != null)
                cfForm.spForm.Parent_ResizeBegin();
            
        }


        private void GH_CoreForm_ResizeEnd(object sender, EventArgs e)
        {
            //Sends event to SubForm
            if (cfForm.spForm != null)
                cfForm.spForm?.Parent_ResizeEnd();
        }

        FormWindowState? LastWindowState = null;
        private void GH_CoreForm_Resize(object sender, EventArgs e)
        {
            // When window state changes
            if (WindowState != LastWindowState)
            {
                /*
                if (WindowState == FormWindowState.Maximized)
                {
                    // Maximized!
                }
                if (WindowState == FormWindowState.Normal)
                {
                    // Restored!
                }
                */

                if (cfForm.spForm != null)
                    cfForm.spForm?.Parent_ResizeEnd();

                LastWindowState = WindowState;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Test button, creates forms using class names and coordinates.

            var GlitchHarvester = new CanvasGrid(6, 3);
            GlitchHarvester.SetTileForm("GH_SavestateManager", 0, 0);
            GlitchHarvester.SetTileForm("GH_Engine_Blast", 1, 0);
            GlitchHarvester.SetTileForm("GH_BlastManipulator", 2, 0);
            GlitchHarvester.SetTileForm("GH_BlastParameters", 1, 1);
            GlitchHarvester.SetTileForm("GH_StashHistory", 2, 1);
            GlitchHarvester.SetTileForm("GH_RenderOutput", 1, 2);
            GlitchHarvester.SetTileForm("GH_StockpileManager", 3, 0);

            var EngineForm = new CanvasGrid(4, 3);

            EngineForm.SetTileForm("GH_Engine_Intensity", 0, 0);
            EngineForm.SetTileForm("GH_Engine_MemoryDomains", 2, 1);

            var TestForm = new CanvasGrid(5, 4);
            TestForm.SetTileForm("GH_DummyTileForm3x1", 0, 0);
            TestForm.SetTileForm("GH_DummyTileForm2x1", 3, 2);
            TestForm.SetTileForm("GH_DummyTileForm2x2", 3, 0);
            TestForm.SetTileForm("GH_DummyTileForm1x1", 4, 3);
            TestForm.SetTileForm("GH_DummyTileForm3x3", 0, 1);

            var multiGrid = new MultiGrid(
                EngineForm,
                GlitchHarvester,
                TestForm
            );

            multiGrid.Load();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //test button, loads a dummy form in SubForm mode

            if (cfForm.spForm == null)
            {
                cfForm.ShowSubForm("GH_DummySubForm");
            }
            else
                cfForm.CloseSubForm();
        }
    }
}
