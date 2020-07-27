namespace RTCV.Common.Forms
{
    using System.Drawing;
    using System.Windows.Forms;
    using NLog;
    using NLog.Layouts;
    using NLog.Windows.Forms;

    public partial class LogConsole : UserControl
    {
        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                tbLog.ForeColor = value;
                base.ForeColor = value;
            }
        }
        public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                tbLog.BackColor = value;
                base.BackColor = value;
            }
        }

        Logger _logger = null;
        public Logger Logger
        {
            get
            {
                if (_logger != null)
                    return _logger;
                InitializeFromGlobalLogger();
                return _logger;
            }
        }

        private RichTextBoxTarget GetRichTextBoxTarget(int maxLines, Layout layout)
        {
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
            return LogTextboxTarget;
        }

        public void InitializeFromGlobalLogger()
        {
            var config = NLog.LogManager.Configuration;
            var t = GetRichTextBoxTarget(1000, RTCV.Common.Logging.CurrentLayout);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, t);
            _logger = new LogFactory(config).GetCurrentClassLogger();
        }

        #pragma warning disable CA1801,IDE0060 //maxLines is unused but should be left in for external plugins
        public void InitializeCustomLogger(int maxLines, Layout layout, string fileName = null)
        {
            if (layout == null)
                layout = "${level} ${logger} ${message} ${onexception:|${newline}EXCEPTION OCCURRED\\:${exception:format=type,message,method:maxInnerExceptionLevel=5:innerFormat=shortType,message,method}${newline}";

            var config = new NLog.Config.LoggingConfiguration();

            if (!string.IsNullOrEmpty(fileName))
            {
                var logfile = new NLog.Targets.FileTarget("logfile") { FileName = fileName, Layout = layout };
                config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);
            }

            var t = GetRichTextBoxTarget(1000, layout);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, t);

            _logger = new LogFactory(config).GetCurrentClassLogger();
        }


        public LogConsole()
        {
            InitializeComponent();
        }
        public LogConsole(int maxLines, Layout customLayout, string fileName = null)
        {
            InitializeComponent();
            InitializeCustomLogger(maxLines, customLayout, fileName);
        }
    }
}
