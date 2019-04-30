using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI
{
    public partial class UI_ComponentFormSubForm : ComponentForm, ISubForm
    {
        public UI_ComponentFormSubForm()
        {
            InitializeComponent();

            UICore.SetRTCColor(UICore.GeneralColor, this);
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

        private void UI_ComponentFormSubForm_Load(object sender, EventArgs e)
        {
        }

    }
}
