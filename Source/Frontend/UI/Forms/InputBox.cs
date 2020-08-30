namespace RTCV.UI.Forms
{
    using System;
    using System.Windows.Forms;

    public partial class InputBox : Form
    {
        public static DialogResult ShowDialog(string title, string promptText, ref string value)
        {
            var form = new InputBox
            {
                Text = title
            };
            form.label1.Text = promptText;
            var result = form.ShowDialog();
            value = form.textBox1.Text;
            return result;
        }

        public InputBox()
        {
            InitializeComponent();
        }
    }
}
