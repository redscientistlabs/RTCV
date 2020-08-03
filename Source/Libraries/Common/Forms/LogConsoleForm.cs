namespace RTCV.Common.Forms
{
    using System.Drawing;
    using System.Windows.Forms;
    using NLog;
    using NLog.Layouts;

    public partial class LogConsoleForm : Form
    {
        public bool HideOnClose = false;

        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                LogConsole.ForeColor = value;
                base.ForeColor = value;
            }
        }
        public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                LogConsole.BackColor = value;
                base.BackColor = value;
            }
        }


        /// <summary>
        /// Creates a LogConsoleForm using the global logger
        /// </summary>
        public LogConsoleForm()
        {
            InitializeComponent();
            LogConsole.InitializeFromGlobalLogger();
        }

        #pragma warning disable CA1801,IDE0060 //maxLines is unused but should be left in for external plugins
        /// <summary>
        /// Creates a LogConsoleForm using the global logger
        /// </summary>
        /// <param name="maxLines">Maximum lines to display</param>
        /// <param name="layout">Layout</param>
        /// <param name="fileName">Optional file log</param>
        public LogConsoleForm(int maxLines = 1000, Layout layout = null, string fileName = null)
        {
            InitializeComponent();
            LogConsole.InitializeCustomLogger(maxLines, layout, fileName);
            this.FormClosing += LogConsoleForm_FormClosing;
        }

        private void LogConsoleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (HideOnClose)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        public Logger Logger => LogConsole.Logger;
    }
}
