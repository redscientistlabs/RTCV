using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTCV.UI.Components.Controls
{
    public partial class OpenToolButton : UserControl
    {
        public OpenToolButton(string name, string buttonText, Action onClickedAction)
        {
            InitializeComponent();
            groupBox1.Text = Name;
            btnOpenTool.Text = buttonText;
            btnOpenTool.Click += delegate
            {
                onClickedAction.Invoke();
            };
        }
    }
}
