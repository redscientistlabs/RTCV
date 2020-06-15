namespace RTCV.Common
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Linq;
    using NLog;
    using NLog.LayoutRenderers;
    using NLog.Layouts;
    using NLog.LayoutRenderers.Wrappers;
    using NLog.Config;

    public static class Logging
    {
        public static Logger GlobalLogger = LogManager.GetLogger("Global");

        private static readonly SimpleLayout defaultLayout = new NLog.Layouts.SimpleLayout("${longdate}|${level:uppercase=true}|${logger}|${message}${onexception:|${newline}EXCEPTION OCCURRED\\:${InvariantCulture:${exception:format=type,message,method:maxInnerExceptionLevel=5:innerFormat=shortType,message,method}${newline}");
        private static readonly SimpleLayout traceLayout = new NLog.Layouts.SimpleLayout("${longdate}|${level:uppercase=true}|${logger}|${callsite}|${message}${onexception:|${newline}EXCEPTION OCCURRED\\:${InvariantCulture:${exception:format=type,message,method:maxInnerExceptionLevel=5:innerFormat=shortType,message,method}${newline}");

        public static Layout CurrentLayout = defaultLayout;
        private static readonly LogLevel minLevel = LogLevel.Trace;
        private const int logsToKeep = 5;

        public static void StartLogging(string filename)
        {
            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("InvariantCulture", typeof(InvariantCultureLayoutRendererWrapper));
            var config = new NLog.Config.LoggingConfiguration();


            for (int i = logsToKeep; i >= 0; i--)
            {
                var _filename = getFormattedLogFilename(filename, i - 1);
                if (File.Exists(_filename))
                {
                    var newName = getFormattedLogFilename(filename, i);

                    if (string.IsNullOrEmpty(newName)) //If something went wrong generating the name, just give up
                    {
                        break;
                    }

                    try
                    {
                        File.Copy(_filename, newName, true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Failed to rotate log file {_filename} to {newName}\n{e}");
                    }
                }
            }

            try
            {
                File.Delete(filename);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to delete old log!\n{e}");
            }

            if (minLevel == LogLevel.Trace)
            {
                CurrentLayout = traceLayout;
            }

            // var logfile = new NLog.Targets.FileTarget("logfile") { FileName = filename, Layout = layout };
            var logconsole = new NLog.Targets.ColoredConsoleTarget("logconsole") { Layout = CurrentLayout };

            bool isDebug = false;
            Debug.Assert(isDebug = true);
            if (Environment.GetCommandLineArgs().Contains("-TRACE") || isDebug)
                config.AddRule(LogLevel.Trace, LogLevel.Fatal, logconsole);
            else
                config.AddRule(LogLevel.Debug, LogLevel.Fatal, logconsole);
            // Rules for mapping loggers to targets
            //config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);

            // Apply config
            NLog.LogManager.Configuration = config;
            Common.ConsoleHelper.CreateConsole(filename);
            if (!Environment.GetCommandLineArgs().Any(x => string.Equals(x, "-CONSOLE", StringComparison.OrdinalIgnoreCase)))
            {
                Common.ConsoleHelper.HideConsole();
            }

            GlobalLogger = LogManager.GetLogger("Global");
        }


        private static string getFormattedLogFilename(string path, int num)
        {
            try
            {
                var dir = Path.GetDirectoryName(path);
                var filename = Path.GetFileNameWithoutExtension(path);
                var ext = Path.GetExtension(path);

                return Path.Combine(dir, num == 0 ? $"{filename}{ext}" : $"{filename}.{num}{ext}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"getFormattedLogFilename failed {e}");
                return string.Empty;
            }
        }
    }


    public class ExtendedRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            throw new NotImplementedException();
        }
    }

    [LayoutRenderer("InvariantCulture")]
    [ThreadAgnostic]
    public sealed class InvariantCultureLayoutRendererWrapper : WrapperLayoutRendererBase
    {
        protected override string Transform(string text)
        {
            return text;
        }

        protected override string RenderInner(LogEventInfo logEvent)
        {
            var currentCulture = Thread.CurrentThread.CurrentUICulture;
            try
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                return base.RenderInner(logEvent);
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = currentCulture;
            }
        }
    }
}
