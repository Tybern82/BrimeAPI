#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrimeAPI.com.brimelive.api.realtime {
    /// <summary>
    /// Implements a listener used to record all triggers to log
    /// </summary>
    public class TracedBrimeListener : BrimeRealtimeListener {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public void onOpen() { Logger.Trace("Connection opened."); }
        /// <inheritdoc />
        public void onClose() { Logger.Trace("Connetion closed."); }

        /// <inheritdoc />
        public void onFollow(string username, string id) { Logger.Trace(username + " has followed (" + id + ")"); }
        /// <inheritdoc />
        public void onRaid(string channelName, string channelID, int viewerCount) { Logger.Trace(channelName + " has raided with " + viewerCount + " viewers"); }

        /// <inheritdoc />
        public void onSubscribe(string username, string userID, bool isResub) {
            Logger.Trace(() => {
                return string.Format("{0} from {1}", (isResub ? "Resubscription" : "Subscription"), username);
            });
        }

        /// <inheritdoc />
        public void onJoin(string username) { Logger.Trace(username + " has joined."); }
        /// <inheritdoc />
        public void onLeave(string username) { Logger.Trace(username + " has left."); }

        /// <inheritdoc />
        public void onChat(BrimeChatMessage chatMessage) {
            Logger.Info("MSG from <" + chatMessage.Sender.DisplayName + ">: \"" + EncodeNonAsciiCharacters(chatMessage.Message) + "\"");
        }

        /// <inheritdoc />
        public void onDeleteChat(string messageID) {
            Logger.Info("DELETED MESSAGE <" + messageID + ">");
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
}
