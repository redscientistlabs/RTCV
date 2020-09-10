namespace RTCV.UI
{
    using System;
    using RTCV.UI.Modular;

    public partial class ComponentFormSubForm : ComponentForm, ISubForm, IBlockable
    {
        public ComponentFormSubForm()
        {
            InitializeComponent();

            Colors.SetRTCColor(Colors.GeneralColor, this);
        }

        public bool HasLeftButton => true;
        public bool HasRightButton => false;
        public string LeftButtonText => "Exit";
        public string RightButtonText => "Peter Griffin";

        public void LeftButtonClick()
        {
        }

        public void RightButtonClick()
        {
        }

        public void OnShown()
        {
        }

        public void OnHidden()
        {
        }
    }
}
