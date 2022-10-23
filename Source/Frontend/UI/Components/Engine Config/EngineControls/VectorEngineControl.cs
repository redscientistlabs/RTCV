namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Drawing;
    using RTCV.CorruptCore;

    public partial class VectorEngineControl : EngineConfigControl
    {
        internal VectorEngineControl(CorruptionEngineForm parent) : base(new Point(parent.gbSelectedEngine.Location.X, parent.gbSelectedEngine.Location.Y))
        {
            InitializeComponent();

            cbVectorValueList.DataSource = null;
            cbVectorLimiterList.DataSource = null;

            cbVectorValueList.DisplayMember = "Name";
            cbVectorLimiterList.DisplayMember = "Name";
            cbVectorValueList.ValueMember = "Value";
            cbVectorLimiterList.ValueMember = "Value";

            cbVectorLimiterList.SelectedIndexChanged += parent.UpdateVectorLimiterList;
            cbVectorValueList.SelectedIndexChanged += parent.UpdateVectorValueList;
            cbVectorUnlockPrecision.CheckedChanged += parent.UpdateVectorUnlockPrecision;

            //Do this here as if it's stuck into the designer, it keeps defaulting out
            cbVectorValueList.DataSource = RtcCore.ValueListBindingSource;
            cbVectorLimiterList.DataSource = RtcCore.LimiterListBindingSource;
        }

        public void ResyncEngineUI()
        {
            if (RtcCore.LimiterListBindingSource.Count > 0)
                cbVectorLimiterList.SelectedIndex = RtcCore.LimiterListBindingSource.Select(x => x.Value).ToList().IndexOf(VectorEngine.LimiterListHash);
            if (RtcCore.ValueListBindingSource.Count > 0)
                cbVectorValueList.SelectedIndex = RtcCore.ValueListBindingSource.Select(x => x.Value).ToList().IndexOf(VectorEngine.ValueListHash);
            cbVectorUnlockPrecision.Checked = VectorEngine.UnlockPrecision;
            //throw new NotImplementedException();
        }
    }
}
