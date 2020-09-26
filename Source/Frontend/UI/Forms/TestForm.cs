namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using RTCV.Common;

    public partial class TestForm : Modular.ColorizedForm
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
        }
    }

    public class TestClass
    {
        public List<long[]> ListLongArr { get; set; } = new List<long[]>();

        public TestClass()
        {
        }
    }
}
