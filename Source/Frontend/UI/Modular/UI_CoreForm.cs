using RTCV.NetCore.StaticTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI
{
    public partial class UI_CoreForm : Form
    {
        //This form traps events and forwards them.
        //It contains the single UI_CanvasForm instance.

        public static UI_CoreForm thisForm;
        public static UI_CanvasForm cfForm;


        CanvasGrid ecGrid = null;
        CanvasGrid ghGrid = null;
        CanvasGrid spGrid = null;


        //Vallues used for padding and scaling properly in high dpi
        public static int xPadding;
        public static int corePadding; // height of the top bar
        public static int yPadding;


        public UI_CoreForm()
        {
            InitializeComponent();
            thisForm = this;

            UICore.SetRTCColor(UICore.GeneralColor, this);

            cfForm = new UI_CanvasForm();
            cfForm.TopLevel = false;
            cfForm.Dock = DockStyle.Fill;
            this.Controls.Add(cfForm);
            cfForm.Location = new Point(0, pnTopBar.Size.Height);
            cfForm.Show();
            cfForm.BringToFront();

            //For Horizontal tab-style menu in coreform
            //xPadding = (Width - cfForm.Width);
            //coreYPadding = pnTopBar.Height;
            //yPadding = (Height - cfForm.Height) - coreYPadding;

            //For Vertical tab-style menu in coreform
            yPadding = (Height - cfForm.Height);
            corePadding = pnTopBar.Width;
            xPadding = (Width - cfForm.Width) - corePadding;


        }

        private void UI_CoreForm_Load(object sender, EventArgs e)
        {

        }

        public void SetSize(int x, int y)
        {
            //this.Size = new Size(x + xPadding, y + yPadding + coreYPadding); //For Horizontal tab-style menu in coreform
            this.Size = new Size(x + xPadding + corePadding, y + yPadding); //For Vertical tab-style menu in coreform
        }

        private void UI_CoreForm_ResizeBegin(object sender, EventArgs e)
        {
            //Sends event to SubForm 
            if(cfForm.spForm != null)
                cfForm.spForm.Parent_ResizeBegin();
            
        }


        private void UI_CoreForm_ResizeEnd(object sender, EventArgs e)
        {
            //Sends event to SubForm
            if (cfForm.spForm != null)
                cfForm.spForm?.Parent_ResizeEnd();
        }

        FormWindowState? LastWindowState = null;
        private void UI_CoreForm_Resize(object sender, EventArgs e)
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
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //test button, loads a dummy form in SubForm mode

            if (cfForm.spForm == null)
            {
                cfForm.ShowSubForm("UI_ComponentFormSubForm");
            }
            else
                cfForm.CloseSubForm();
        }

        private void btnEngineConfig_Click(object sender, EventArgs e)
        {
            //Test button, creates forms using class names and coordinates.
            /*
            var GlitchHarvester = new CanvasGrid(6, 3);
            GlitchHarvester.SetTileForm("UI_SavestateManager", 0, 0);
            GlitchHarvester.SetTileForm("UI_Engine_Blast", 1, 0);
            GlitchHarvester.SetTileForm("UI_BlastManipulator", 2, 0);
            GlitchHarvester.SetTileForm("UI_BlastParameters", 1, 1);
            GlitchHarvester.SetTileForm("UI_StashHistory", 2, 1);
            GlitchHarvester.SetTileForm("UI_RenderOutput", 1, 2);
            GlitchHarvester.SetTileForm("UI_StockpileManager", 3, 0);
            */

            //EngineForm.SetTileForm("UI_Engine_MemoryDomains", 0, 2);
            /*
            var TestForm = new CanvasGrid(5, 4);
            TestForm.SetTileForm("UI_DummyTileForm3x1", 0, 0);
            TestForm.SetTileForm("UI_DummyTileForm2x1", 3, 2);
            TestForm.SetTileForm("UI_DummyTileForm2x2", 3, 0);
            TestForm.SetTileForm("UI_DummyTileForm1x1", 4, 3);
            TestForm.SetTileForm("UI_DummyTileForm3x3", 0, 1);
            */

            /*
            var multiGrid = new MultiGrid(
                EngineGrid,
                GlitchHarvester,
                TestForm
            );

            multiGrid.Load();
            */

            ecGrid = new CanvasGrid(15, 12, "Engine Config");

            Form mtForm = new RTC_SelectBox_Form(new ComponentForm[] {
                S.GET<RTC_VmdNoTool_Form>(),
                S.GET<RTC_VmdPool_Form>(),
                S.GET<RTC_VmdGen_Form>(),
                S.GET<RTC_VmdAct_Form>(),
                S.GET<RTC_ListGen_Form>(),
            })
            {
                popoutAllowed = false,
                Text = "Advanced Memory Tools",
            };

            Form gpForm = S.GET<RTC_GeneralParameters_Form>();
            Form mdForm = S.GET<RTC_MemoryDomains_Form>();
            Form ceForm = S.GET<RTC_CorruptionEngine_Form>();

            ecGrid.SetTileForm(gpForm, 0, 0, 5, 5);
            ecGrid.SetTileForm(ceForm, 5, 0, 10, 5);
            ecGrid.SetTileForm(mdForm, 0, 5, 5, 7);
            ecGrid.SetTileForm(mtForm, 5, 5, 10, 7);

            ecGrid.LoadToMain();

            /*
            var TestGrid = new CanvasGrid(13, 8, "Glitch Harvester");
            TestGrid.SetTileForm("Glitch Harvester", 0, 0, 9, 8, false);
            TestGrid.LoadToNewWindow();
            */


            /*
            var multiGrid = new MultiGrid(
                EngineGrid,
                TestGrid
            );
            */

            //multiGrid.Load();

            //var tileForm = (UI_ComponentFormTile)UI_CanvasForm.getTileForm("UI_ComponentFormTile");
            //tileForm.SetCompoentForm("ComponentForm host", 4, 4);

        }

        private void pnAutoKillSwitch_MouseHover(object sender, EventArgs e)
        {
            lbAks.ForeColor = Color.FromArgb(32, 32, 32);
            pnAutoKillSwitch.BackColor = Color.White;
        }

        private void pnAutoKillSwitch_MouseLeave(object sender, EventArgs e)
        {
            lbAks.ForeColor = Color.White;
            pnAutoKillSwitch.BackColor = Color.Transparent;
        }

        private void btnGlitchHarvester_Click(object sender, EventArgs e)
        {

        }
    }
}
