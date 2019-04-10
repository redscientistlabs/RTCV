using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTCV.UI
{
    public partial class UI_ComponentFormSubForm : Form, ISubForm
    {
        public UI_ComponentFormSubForm()
        {
            InitializeComponent();
        }


        public bool HasCancelButton
        {
            get { return false; }
            set { }
        }

        public void SubForm_Cancel()
        {

        }

        public void SubForm_Ok()
        {

        }

    }
}
