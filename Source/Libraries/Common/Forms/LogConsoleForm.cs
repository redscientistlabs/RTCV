using System.Windows.Forms;
using NLog;
using NLog.Layouts;
using NLog.Windows.Forms;

namespace RTCV.Common.Forms
{
    public partial class LogConsoleForm : Form
    {
        Logger _logger = null;
        public Logger Logger
        {
            get
            {
                if (_logger != null)
                    return _logger;
                InitializeLogger();
                return _logger;
            }
        }

        public void InitializeLogger(int maxLines = 1000, Layout layout = null)
        {
            if(layout == null)
                layout = "${level} ${logger} ${message} ${onexception:|${newline}EXCEPTION OCCURRED\\:${exception:format=type,message,method:maxInnerExceptionLevel=5:innerFormat=shortType,message,method}${newline}";

            var config = new NLog.Config.LoggingConfiguration();
            var LogTextboxTarget = new RichTextBoxTarget()
            {
                FormName = this.Name,
                TargetRichTextBox = this.tbLog,
                Layout = layout,
                MaxLines = maxLines,
                AutoScroll = true,
                UseDefaultRowColoringRules = false,
            };

            LogTextboxTarget.RowColoringRules.Add(new RichTextBoxRowColoringRule(
                "level == LogLevel.Trace", // condition
                "LightGray", // font color
                tbLog.BackColor.ToString()
            ));
            LogTextboxTarget.RowColoringRules.Add(new RichTextBoxRowColoringRule(
                "level == LogLevel.Debug", // condition
                "Purple", // font colore
                tbLog.BackColor.ToString()
            ));
            LogTextboxTarget.RowColoringRules.Add(new RichTextBoxRowColoringRule(
                "level == LogLevel.Warn", // condition
                "Yellow", // font color
                tbLog.BackColor.ToString()
            ));
            LogTextboxTarget.RowColoringRules.Add(new RichTextBoxRowColoringRule(
                "level == LogLevel.Error", // condition
                "Red", // font color
                tbLog.BackColor.ToString()
            ));
            LogTextboxTarget.RowColoringRules.Add(new RichTextBoxRowColoringRule(
                "level == LogLevel.Info", // condition
                "White", // font color
                tbLog.BackColor.ToString()
            ));

            config.AddRule(LogLevel.Trace, LogLevel.Fatal, LogTextboxTarget);
            _logger = new LogFactory(config).GetCurrentClassLogger();
        }


        public LogConsoleForm()
        {
            InitializeComponent();
            this.FormClosing += ConsoleForm_FormClosing;
        }
        public LogConsoleForm(int maxLines, Layout customLayout)
        {
            InitializeComponent();
            this.FormClosing += ConsoleForm_FormClosing;
            InitializeLogger(maxLines, customLayout);
        }

        private void ConsoleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
