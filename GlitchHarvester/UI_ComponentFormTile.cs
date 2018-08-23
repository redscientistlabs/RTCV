using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTCV.UI
{
    public partial class UI_ComponentFormTile : Form, ITileForm
    {

        string childForm = null; //change to ComponentForm
        public int SizeX = 2;
        public int SizeY = 2;

        public UI_ComponentFormTile()
        {
            InitializeComponent();

        }

        public void SetCompoentForm(string _childForm, int _sizeX, int _sizeY, bool DisplayHeader)
        {
            childForm = _childForm;
            SizeX = _sizeX;
            SizeY = _sizeY;


            


            this.Size = new Size(
                (SizeX * UI_CanvasForm.tileSize) + ((SizeX - 1) * UI_CanvasForm.spacerSize),
                (SizeY * UI_CanvasForm.tileSize) + ((SizeY - 1) * UI_CanvasForm.spacerSize)
                );


            //childForm.AttachToPanel(pnComponentFormHost); //ATTACH HERE


            if (DisplayHeader)
            {
                lbComponentFormName.Text = childForm; // replace that with the childform's text property
            }
            else
            {
                lbComponentFormName.Visible = false;
                pnComponentFormHost.Location = new Point(0, 0);
                pnComponentFormHost.Size = this.Size;
            }
        }

        public bool CanPopout { get; set; } = false;
        public int TilesX { get { return SizeX; } set { SizeX = value; } }
        public int TilesY { get { return SizeY; } set { SizeY = value; } }
    }
}
