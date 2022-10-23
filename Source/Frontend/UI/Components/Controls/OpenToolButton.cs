namespace RTCV.UI.Components.Controls
{
    using System;
    using System.Windows.Forms;

    public partial class OpenToolButton : UserControl
    {
        public OpenToolButton(string name, string buttonText, Action onClickedAction)
        {
            InitializeComponent();
            btnOpenTool.Name = name;
            btnOpenTool.Text = buttonText;
            btnOpenTool.Click += delegate
            {
                onClickedAction.Invoke();
            };
        }
    }
}
