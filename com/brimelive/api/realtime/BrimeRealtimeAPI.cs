#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IO.Ably;
using IO.Ably.Realtime;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.realtime {
    public class BrimeRealtimeAPI {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string ABLY_TOKEN = "TTKZpg.6NfkQA:gAXhL8RYoA8iQ2o7";
        public static readonly int VIEW_UPDATE = 300000;        // re-sync viewer count every 5 minutes (realtime updated by ENTER/LEAVE messages)

        private AblyRealtime ably;

        private HashSet<BrimeRealtimeListener> listeners = new HashSet<BrimeRealtimeListener>(2); // normally will only have a single listener, plus the view tracker

        private bool isConnected = false;

        public ViewCountTracker ViewCountTracker { get; private set; }

        public BrimeRealtimeAPI(string channelName) {
            ClientOptions options = new ClientOptions(ABLY_TOKEN);
            options.AutoConnect = false;
            options.LogHandler = new AblyLogHandler(Logger);
            options.LogLevel = LogLevel.None;
            this.ably = new AblyRealtime(options);

            this.ably.Connection.On((state) => {
                // onOpen / onClose
                switch (state.Current) {
                    case IO.Ably.Realtime.ConnectionState.Connected:
                        isConnected = true;
                        foreach (BrimeRealtimeListener listener in listeners) listener.onOpen();
                        break;

                    case IO.Ably.Realtime.ConnectionState.Closed:
                    case IO.Ably.Realtime.ConnectionState.Failed:
                        isConnected = false;
                        foreach (BrimeRealtimeListener listener in listeners) listener.onClose();
                        break;
                }
            });

            this.ably.Channels.Get(channelName.ToLower() + "/alerts").Subscribe((message) => {
                // Follow Alerts
                if (message.Name == "alert") {
                    JObject data = JObject.Parse((string)message.Data);
                    string? follower = data.Value<string>("follower");
                    string? followerID = data.Value<string>("followerID");
                    if (follower == null) follower = "";
                    if (followerID == null) followerID = "";
                    foreach (BrimeRealtimeListener listener in listeners) listener.onFollow(follower, followerID);
                }
            });

            IRealtimeChannel channel = this.ably.Channels.Get(channelName.ToLower());

            IEnumerable<PresenceMessage> msgs = channel.Presence.GetAsync().Result;
            int viewCount = 0; foreach (PresenceMessage msg in msgs) viewCount++;
            ViewCountTracker = new ViewCountTracker(viewCount);
            registerListener(ViewCountTracker); // register for tracking updates

            Task.Run(() => {
                while (isConnected) {
                    System.Threading.Thread.Sleep(VIEW_UPDATE);
                    IEnumerable<PresenceMessage> msgs = channel.Presence.GetAsync().Result;
                    int viewCount = 0; foreach (PresenceMessage msg in msgs) viewCount++;
                    ViewCountTracker.ViewCount = viewCount;
                }
            });

            channel.Presence.Subscribe((message) => {
                // onJoin / onLeave
                switch (message.Action) {
                    case PresenceAction.Absent:
                    case PresenceAction.Leave:
                        foreach (BrimeRealtimeListener listener in listeners) listener.onLeave(message.ClientId);
                        break;

                    case PresenceAction.Enter:
                    case PresenceAction.Present:
                    case PresenceAction.Update:
                        foreach (BrimeRealtimeListener listener in listeners) listener.onJoin(message.ClientId);
                        break;
                }
            });

            channel.Subscribe((message) => {
                // onChat
                if (message.Name == "greeting") {
                    Logger.Trace("Message Data: " + message.Data);
                    BrimeChatMessage chatMessage = new BrimeChatMessage(JObject.Parse((string)message.Data));
                    foreach (BrimeRealtimeListener listener in listeners) listener.onChat(chatMessage);
                } else {
                    Logger.Trace("Message Name: " + message.Name);
                }
            });
        }

        public void registerListener(BrimeRealtimeListener realtimeListener) {
            if (listeners.Contains(realtimeListener)) listeners.Remove(realtimeListener);
            listeners.Add(realtimeListener);
        }

        public void removeListener(BrimeRealtimeListener realtimeListener) {
            if (listeners.Contains(realtimeListener)) listeners.Remove(realtimeListener);
        }

        public void connect() {
            Logger.Trace("Connecting to Brime Realtime");
            this.ably.Connect();
        }

        public void close() {
            Logger.Trace("Disconnecting from Brime Realtime");
            this.ably.Close();
        }
    }

    public interface BrimeRealtimeListener {
        public void onOpen();
        public void onClose();
        public void onFollow(string username, string id);
        public void onJoin(string username);
        public void onLeave(string username);
        public void onChat(BrimeChatMessage chatMessage);
    }

    public class ViewCountTracker : BrimeRealtimeListener {
        public void onOpen() { }
        public void onClose() { }
        public void onFollow(string username, string id) { }
        public void onChat(BrimeChatMessage chatMessage) { }

        public int ViewCount { get; set; }

        public ViewCountTracker(int viewCount) {
            this.ViewCount = viewCount;
        }

        public void onJoin(string username) { this.ViewCount++; }
        public void onLeave(string username) { this.ViewCount--; }
    }

    public class TracedBrimeListener : BrimeRealtimeListener {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public void onOpen() { Logger.Trace("Connection opened."); }
        public void onClose() { Logger.Trace("Connetion closed."); }

        public void onFollow(string username, string id) { Logger.Trace(username + " has followed (" + id + ")"); }

        public void onJoin(string username) { Logger.Trace(username + " has joined."); }
        public void onLeave(string username) { Logger.Trace(username + " has left."); }

        public void onChat(BrimeChatMessage chatMessage) {
            Logger.Info("MSG from <" /*+ chatMessage.Sender.DisplayName*/ + ">: \"" + EncodeNonAsciiCharacters(chatMessage.Message) + "\"");
        }
        static string EncodeNonAsciiCharacters(string value) {
            StringBuilder sb = new StringBuilder();
            foreach (char c in value) {
                if (c > 127) {
                    // This character is too big for ASCII
                    string encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                } else {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }

    public class AblyLogHandler : ILoggerSink {

        private NLog.Logger Logger;

        public AblyLogHandler(NLog.Logger logger) {
            this.Logger = logger;
        }

        public void LogEvent(LogLevel level, string message) {
            switch (level) {
                case LogLevel.Debug:    Logger.Debug(message);  break;
                case LogLevel.Error:    Logger.Error(message);  break;
                case LogLevel.Warning:  Logger.Warn(message);   break;
                default:                Logger.Info(message);   break;
            }
        }
    }
}
