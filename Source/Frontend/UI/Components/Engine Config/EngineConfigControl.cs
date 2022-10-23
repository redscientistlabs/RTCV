namespace RTCV.UI.Components.EngineConfig
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class EngineConfigControl : UserControl
    {
        internal EngineConfigControl(Point location)
        {
            InitializeComponent();

            Location = location;
        }

        // The forms designer requires a parameterless constructor for this to function. See https://stackoverflow.com/a/16326021/1222411
        [Obsolete("Designer only", true)]
        internal EngineConfigControl()
        {
            InitializeComponent();
        }
    }
}
