﻿namespace RTCV.UI
{
    using System.Windows.Forms;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public partial class VmdNoToolForm : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public VmdNoToolForm()
        {
            InitializeComponent();

            popoutAllowed = false;
        }
    }
}