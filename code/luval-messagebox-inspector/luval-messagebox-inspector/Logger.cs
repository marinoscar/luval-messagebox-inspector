using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval_messagebox_inspector
{
    public static class Logger
    {

        public static void WriteConsole(string format, params object[] parameters)
        {
            Console.WriteLine(format, parameters);
        }

        public static void WriteError(string format, params object[] parameters)
        {
            WriteToEventLog(EventLogEntryType.Error, format, parameters);
        }

        public static void WriteWarning(string format, params object[] parameters)
        {
            WriteToEventLog(EventLogEntryType.Warning, format, parameters);
        }

        public static void WriteInfo(string format, params object[] parameters)
        {
            WriteToEventLog(EventLogEntryType.Information, format, parameters);
        }
        public static void WriteToEventLog(EventLogEntryType type, string format, params object[] parameters)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                var message = string.Format(format, parameters);
                eventLog.Source = "Application";
                eventLog.WriteEntry(message, type);
                Console.WriteLine("Type: {0} Message: {1}", type, message);
            }
        }
    }
}
