using NLog;
using System;
using System.Text;

namespace VideoLibrary.Logger
{
    public class CommonLogger
    {
        private static NLog.Logger log = LogManager.GetCurrentClassLogger();

        public static void LogError(Exception ex)
        {
            StringBuilder message = new StringBuilder(Environment.NewLine);
            message.Append(Environment.NewLine);
            message.Append("---------------------------ERROR---------------------------");
            message.Append(Environment.NewLine);
            message.Append("-----------------------------------------------------------");
            message.Append(Environment.NewLine);
            message.Append(string.Format("Message: {0}", ex.Message));
            message.Append(Environment.NewLine);
            message.Append(string.Format("StackTrace: {0}", ex.StackTrace));
            message.Append(Environment.NewLine);
            message.Append(string.Format("Source: {0}", ex.Source));
            message.Append(Environment.NewLine);
            message.Append(string.Format("TargetSite: {0}", ex.TargetSite.ToString()));
            message.Append(Environment.NewLine);
            message.Append("-----------------------------------------------------------");
            message.Append(Environment.NewLine);
            message.Append("-----------------------------------------------------------");

            log.Error(message);
            //We can use NLog config as well to define error log format and layout
            //log.Error(ex, "Application Exception");
        }

        public static void LogError(string message)
        {
            log.Error(message);
        }
    }
}
