using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWCFService
{
    public interface ILogger
    {
        void AddLog(string log, LogType type);
        void Clear();
    }
    public enum LogType
    {
        Error = 0,
        Information = 1,
        Warning = 2
    }
    public class LoggerEventLog : ILogger
    {
        private string nameLog = "";

        public LoggerEventLog(string nameLog)
        {
            this.nameLog = nameLog;
        }
        public void AddLog(string log, LogType type)
        {
            try
            {
                var el = GetLog();
                el.WriteEntry(log, LogTypeToLogEntryType(type));
            }

            catch
            {
                // ignored
            }
        }

        private EventLog GetLog()
        {
            if (!EventLog.SourceExists(nameLog))
            {
                EventLog.CreateEventSource(nameLog, nameLog);
            }
            return new EventLog { Source = nameLog };
        }

        public void Clear()
        {
            var EventLog = GetLog();
            EventLog.Clear();
        }

        private EventLogEntryType LogTypeToLogEntryType(LogType lt)
        {
            switch (lt)
            {
                case LogType.Error: return EventLogEntryType.Error;
                case LogType.Information: return EventLogEntryType.Information;
                case LogType.Warning: return EventLogEntryType.Warning;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lt), lt, null);
            }
        }
    }
}
