namespace RTCV.UI.Forms
{
    using System.Windows.Forms;

    public partial class InputBox : Form
    {
        public static DialogResult ShowDialog(string title, string promptText, ref string value)
        {
            var form = new InputBox
            {
                Text = title
            };
            form.label.Text = promptText;
            var result = form.ShowDialog();
            value = form.inputTextBox.Text;
            return result;
        }

        public InputBox()
        {
            InitializeComponent();
        }
    }
}
