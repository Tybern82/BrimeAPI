#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Ably;

namespace BrimeAPI.com.brimelive.api.realtime {

    public class AblyLogHandler : ILoggerSink {

        private NLog.Logger Logger;

        public AblyLogHandler(NLog.Logger logger) {
            this.Logger = logger;
        }

        public void LogEvent(LogLevel level, string message) {
            switch (level) {
                case LogLevel.Debug: Logger.Debug(message); break;
                case LogLevel.Error: Logger.Error(message); break;
                case LogLevel.Warning: Logger.Warn(message); break;
                default: Logger.Info(message); break;
            }
        }
    }
}
