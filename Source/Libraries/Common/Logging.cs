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
        private const int logsToKeep = 5;

        public static Logger GlobalLogger { get; private set; } = LogManager.GetLogger("Global");
        public static Layout CurrentLayout { get; private set; }

        private static readonly LogLevel minLevel = LogLevel.Trace;

        public static void StartLogging(string filename)
        {
            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("InvariantCulture", typeof(InvariantCultureLayoutRendererWrapper));
            var config = new LoggingConfiguration();

            //We have to define these after we register InvariantCulture
            var defaultLayout = new SimpleLayout("${longdate}|${level:uppercase=true}|${logger}|${message}${onexception:|${newline}EXCEPTION OCCURRED\\:${InvariantCulture:${exception:format=type,message,method:maxInnerExceptionLevel=5:innerFormat=shortType,message,method}${newline}");
            var traceLayout = new SimpleLayout("${longdate}|${level:uppercase=true}|${logger}|${callsite}|${message}${onexception:|${newline}EXCEPTION OCCURRED\\:${InvariantCulture:${exception:format=type,message,method:maxInnerExceptionLevel=5:innerFormat=shortType,message,method}${newline}");
            CurrentLayout = defaultLayout;

            for (var i = logsToKeep; i >= 0; i--)
            {
                var formattedFilename = GetFormattedLogFilename(filename, i - 1);
                if (File.Exists(formattedFilename))
                {
                    var newName = GetFormattedLogFilename(filename, i);

                    if (string.IsNullOrEmpty(newName)) //If something went wrong generating the name, just give up
                    {
                        break;
                    }

                    try
                    {
                        File.Copy(formattedFilename, newName, true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Failed to rotate log file {formattedFilename} to {newName}\n{e}");
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

            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logconsole);
            
            // Rules for mapping loggers to targets
            //config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);

            // Apply config
            LogManager.Configuration = config;
            ConsoleHelper.CreateConsole(filename);
            if (!Environment.GetCommandLineArgs().Any(x => string.Equals(x, "-CONSOLE", StringComparison.OrdinalIgnoreCase)))
            {
                ConsoleHelper.HideConsole();
            }

            GlobalLogger = LogManager.GetLogger("Global");
        }


        private static string GetFormattedLogFilename(string path, int num)
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
            CultureInfo currentCulture = Thread.CurrentThread.CurrentUICulture;
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
