namespace RTCV.Plugins.ScriptHost.Controls
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using CSScriptLibrary;
    using NLog;
    using NLog.Layouts;
    using NLog.Windows.Forms;
    using ScintillaNET;

    public partial class ScriptManager : UserControl
    {
        Logger _logger = null;
        Logger logger
        {
            get
            {
                if (_logger != null)
                    return _logger;

                var config = new NLog.Config.LoggingConfiguration();
                SimpleLayout layout = "${level} ${logger} ${message} ${onexception:|${newline}EXCEPTION OCCURRED\\:${exception:format=type,message,method:maxInnerExceptionLevel=5:innerFormat=shortType,message,method}${newline}";

                var logtextbox = new RichTextBoxTarget()
                {
                    FormName = this.Name,
                    TargetRichTextBox = this.tbLog,
                    Layout = layout,
                    MaxLines = 10000,
                    AutoScroll = true,
                    UseDefaultRowColoringRules = false,
                };
                logtextbox.RowColoringRules.Add(new RichTextBoxRowColoringRule(
                        "level == LogLevel.Trace", // condition
                        "LightGray", // font color
                        tbLog.BackColor.ToString()
                    ));
                logtextbox.RowColoringRules.Add(new RichTextBoxRowColoringRule(
                        "level == LogLevel.Debug", // condition
                        "Purple", // font colore
                        tbLog.BackColor.ToString()
                    ));
                logtextbox.RowColoringRules.Add(new RichTextBoxRowColoringRule(
                        "level == LogLevel.Warn", // condition
                        "Yellow", // font color
                        tbLog.BackColor.ToString()
                    ));
                logtextbox.RowColoringRules.Add(new RichTextBoxRowColoringRule(
                        "level == LogLevel.Error", // condition
                        "Red", // font color
                        tbLog.BackColor.ToString()
                    ));
                logtextbox.RowColoringRules.Add(new RichTextBoxRowColoringRule(
                        "level == LogLevel.Info", // condition
                        "White", // font color
                        tbLog.BackColor.ToString()
                    ));


                config.AddRule(LogLevel.Trace, LogLevel.Fatal, logtextbox);
                _logger = new LogFactory(config).GetCurrentClassLogger();
                return _logger;
            }
        }

        public string FilePath;


        public ScriptManager(bool darkTheme = true)
        {
            InitializeComponent();
            //CSScript.EvaluatorConfig.Engine = EvaluatorEngine.Roslyn;
            if (darkTheme)
                ConfigureScintillaDark();
            else
            {
                ConfigureScintilla();
            }
            //CSScript.EvaluatorConfig.Access = EvaluatorAccess.Singleton;
        }

        public bool LoadScript(string path)
        {
            if (path == null || !File.Exists(path))
            {
                logger.Error("File {path} doesn't exist.", path ?? "DEFAULT");
                return false;
            }

            string script = "";
            try
            {
                script = File.ReadAllText(path);
            }
            catch (FileNotFoundException e)
            {
                logger.Error("File {file} not found. Message:{e}", e.Message);
                return false;
            }
            catch (Exception e)
            {
                logger.Error(e, "An unknown error has occurred");
            }

            FilePath = path;
            scintilla.Text = script;
            return true;
        }
        public string GetScript()
        {
            return scintilla.Text;
        }

        private void ConfigureScintilla()
        {
            // Configuring the default style with properties
            // we have common to every lexer style saves time.
            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 10;
            scintilla.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            scintilla.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            scintilla.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
            scintilla.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
            scintilla.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            scintilla.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
            scintilla.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
            scintilla.Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
            scintilla.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;
            scintilla.Lexer = Lexer.Cpp;

            // Set the keywords
            scintilla.SetKeywords(0, "abstract as base break case catch checked continue default delegate do else event explicit extern false finally fixed for foreach goto if implicit in interface internal is lock namespace new null object operator out override params private protected public readonly ref return sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe using virtual while");
            scintilla.SetKeywords(1, "var dynamic bool byte char class const decimal double enum float int long sbyte short static string struct uint ulong ushort void");
        }
        private void ConfigureScintillaDark()
        {
            // Configuring the default style with properties
            // we have common to every lexer style saves time.
            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 10;
            scintilla.Styles[Style.Default].BackColor = Color.FromArgb(30, 30, 30);
            scintilla.Styles[Style.Default].ForeColor = Color.FromArgb(220, 220, 220);
            scintilla.CaretForeColor = Color.White;
            scintilla.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            scintilla.Styles[Style.Cpp.Default].ForeColor = Color.LightGray;
            scintilla.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
            scintilla.Styles[Style.Cpp.Number].ForeColor = Color.FromArgb(181, 206, 168);
            scintilla.Styles[Style.Cpp.Word].ForeColor = Color.FromArgb(86, 156, 214);
            scintilla.Styles[Style.Cpp.Word2].ForeColor = Color.FromArgb(86, 156, 214);
            scintilla.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(214, 157, 133);
            scintilla.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(214, 157, 133);
            scintilla.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(214, 157, 133);
            scintilla.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
            scintilla.Styles[Style.Cpp.Operator].ForeColor = Color.FromArgb(220, 20, 220);
            scintilla.Styles[Style.Cpp.Preprocessor].ForeColor = Color.OrangeRed;
            scintilla.Lexer = Lexer.Cpp;

            // Set the keywords
            scintilla.SetKeywords(0, "abstract as base break case catch checked continue default delegate do else event explicit extern false finally fixed for foreach goto if implicit in interface internal is lock namespace new null object operator out override params private protected public readonly ref return sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe using virtual while");
            scintilla.SetKeywords(1, "var dynamic bool byte char class const decimal double enum float int long sbyte short static string struct uint ulong ushort void");
        }

        private void btnRunSynchronously_Click(object sender, EventArgs e)
        {
            MethodDelegate scr = null;
            try
            {
                scr = CSScript.CodeDomEvaluator.CreateDelegate(scintilla.Text);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception generating script.");
                return;
            }
            try
            {
                scr.Invoke(new[] { logger });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception running script.");
                return;
            }
        }

        private async void btnRunAsync_Click(object sender, EventArgs e)
        {
            MethodDelegate scr = null;
            try
            {
                scr = await Task.Run(() => CSScript.CodeDomEvaluator.CreateDelegate(scintilla.Text));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception generating script.");
                return;
            }
            try
            {
                scr.BeginInvoke(new[] { logger }, null, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception running script.");
                return;
            }
        }
    }
}
