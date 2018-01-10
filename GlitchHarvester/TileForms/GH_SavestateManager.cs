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
    public partial class GH_SavestateManager : Form, ITileForm
    {
        public GH_SavestateManager()
        {
            InitializeComponent();
        }

        public bool CanPopout { get; set; } = false;
        public int TilesX { get; set; } = 3;
        public int TilesY { get; set; } = 1;
    }
}
