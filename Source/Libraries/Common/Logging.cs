using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace RTCV.Common
{
    public static class Logging
    {
        public static Logger GlobalLogger = LogManager.GetLogger("Global");
        public static void StartLogging(string filename)
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = filename };
            var logconsole = new NLog.Targets.ColoredConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            
            // Apply config           
            NLog.LogManager.Configuration = config;

            GlobalLogger = LogManager.GetLogger("Global");
        }
    }
}
