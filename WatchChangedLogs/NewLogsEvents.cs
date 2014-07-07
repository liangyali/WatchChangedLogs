using System;

namespace WatchChangedLogs
{
    public class NewLogsEvents : EventArgs
    {
        private readonly string _logs;

        public NewLogsEvents(string logs)
        {
            _logs = logs;
        }

        public string Logs
        {
            get { return _logs; }
        }
    }
}
