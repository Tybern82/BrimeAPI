#nullable enable

using IO.Ably;

namespace BrimeAPI.com.brimelive.api.realtime {

    /// <summary>
    /// Link Ably logging into an existing NLog logger
    /// </summary>
    public class AblyLogHandler : ILoggerSink {

        private readonly NLog.Logger Logger;

        /// <summary>
        /// Create a new instance, using the given logger
        /// </summary>
        /// <param name="logger">logging instance/class to use</param>
        public AblyLogHandler(NLog.Logger logger) {
            this.Logger = logger;
        }

        /// <summary>
        /// Triggered by Ably, will pass the log message back to the NLog 
        /// </summary>
        /// <param name="level">Ably Logging Level, used to select the appropriate NLog level</param>
        /// <param name="message">message to log</param>
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
