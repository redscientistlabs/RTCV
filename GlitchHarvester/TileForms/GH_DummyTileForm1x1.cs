using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTCV.GlitchHarvester.TileForms
{
    public partial class GH_DummyTileForm1x1 : Form, ITileForm
    {
        public GH_DummyTileForm1x1()
        {
            InitializeComponent();
        }

        public bool CanPopout { get; set; } = false;
        public int TilesX { get; set; } = 1;
        public int TilesY { get; set; } = 1;
    }
}
