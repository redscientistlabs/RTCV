using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using NLog.Layouts;

namespace RTCV.Common.Forms
{
    public partial class LogConsoleForm : Form
    {
        /// <summary>
        /// Creates a LogConsoleForm using the global logger
        /// </summary>
        public LogConsoleForm()
        {
            InitializeComponent();
            LogConsole.InitializeFromGlobalLogger();
        }

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
        }

        public Logger Logger => LogConsole.Logger;
    }
}
