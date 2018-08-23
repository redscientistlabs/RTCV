using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTCV.UI.TileForms
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

        public void SetCompoentForm(string _childForm, int _sizeX, int _sizeY)
        {
            childForm = _childForm;
            SizeX = _sizeX;
            SizeY = _sizeY;

            this.Size = new Size(SizeX, SizeY);

            //childForm.AttachToPanel(pnComponentFormHost);
            lbComponentFormName.Text = childForm;
        }

        public bool CanPopout { get; set; } = false;
        public int TilesX { get { return SizeX; } set { SizeX = value; } }
        public int TilesY { get { return SizeY; } set { SizeY = value; } }
    }
}
