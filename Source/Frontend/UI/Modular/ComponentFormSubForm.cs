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

        public bool SubForm_HasLeftButton => true;
        public bool SubForm_HasRightButton => false;
        public string SubForm_LeftButtonText => "Exit";
        public string SubForm_RightButtonText => "Peter Griffin";

        public void SubForm_LeftButton_Click()
        {
        }

        public void SubForm_RightButton_Click()
        {
        }

        public void OnShown()
        {
        }

        public void OnHidden()
        {
        }

        private void ComponentFormSubForm_Load(object sender, EventArgs e)
        {
        }
    }
}
