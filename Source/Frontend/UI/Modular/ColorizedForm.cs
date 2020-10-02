namespace RTCV.UI.Modular
{
    using System;
    using System.Windows.Forms;

    public class ColorizedForm : Form, RTCV.Common.IColorize
    {
        public ColorizedForm() : base()
        {
            RTCV.Common.S.RegisterColorizable(this);
            Load += Colorize;
            FormClosed += DeregisterColorizable;
        }

        private void DeregisterColorizable(object sender, FormClosedEventArgs e)
        {
            RTCV.Common.S.DeregisterColorizable(this);
        }

        private void Colorize(object sender, EventArgs e) => Recolor();
        public void Recolor(bool propagate = true)
        {
            Colors.SetRTCColor(Colors.GeneralColor, this, propagate);
        }
    }
}
