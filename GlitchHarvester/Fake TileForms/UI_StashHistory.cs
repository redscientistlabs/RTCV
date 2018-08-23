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
    public partial class UI_StashHistory : Form, ITileForm
    {
        public UI_StashHistory()
        {
            InitializeComponent();
        }

        public bool CanPopout { get; set; } = false;
        public int TilesX { get; set; } = 1;
        public int TilesY { get; set; } = 2;

    }


}
