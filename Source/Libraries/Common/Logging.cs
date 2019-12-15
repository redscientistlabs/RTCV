using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.LayoutRenderers;

namespace RTCV.Common
{
    public static class Logging
    {
        public static Logger GlobalLogger = LogManager.GetLogger("Global");
        public static void StartLogging(string filename)
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var errorLayout = new NLog.Layouts.SimpleLayout("${longdate}|${level:uppercase=true}|${logger}|${message}${onexception:|${newline}EXCEPTION OCCURRED\\:${exception:format=type,message,method:maxInnerExceptionLevel=5:innerFormat=shortType,message,method}${newline}");
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = filename , Layout = errorLayout};
            var logconsole = new NLog.Targets.ColoredConsoleTarget("logconsole") {Layout = errorLayout};
            
            
            

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;

            GlobalLogger = LogManager.GetLogger("Global");
        }
    }

    public class ExtendedRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            throw new NotImplementedException();
        }
    }
}
