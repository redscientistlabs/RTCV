namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class FreezeEngineControl : EngineConfigControl
    {
        public FreezeEngineControl(CorruptionEngineForm parent) : base(new Point(parent.gbSelectedEngine.Location.X, parent.gbSelectedEngine.Location.Y))
        {
            InitializeComponent();

            btnClearAllFreezes.Click += parent.ClearCheats;
            cbClearFreezesOnRewind.CheckedChanged += parent.OnClearRewindToggle;
        }
    }
}
