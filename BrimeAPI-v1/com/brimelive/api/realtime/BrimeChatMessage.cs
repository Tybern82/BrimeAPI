#nullable enable

using System;
using System.Collections.Generic;
using BrimeAPI.com.brimelive.api.errors;
using BrimeAPI.com.brimelive.api.users;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.realtime {
    /// <summary>
    /// Identify a single chat message
    /// </summary>
    public class BrimeChatMessage {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Unique identifier for this chat message
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// Channel ID for the channel this message was sent to
        /// </summary>
        public string ChannelID { get; private set; }

        /// <summary>
        /// Contents of the chat message
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Used by BrimeBot
        /// </summary>
        public string RichContents { get; private set; }

        /// <summary>
        /// Identify the user who sent this chat message
        /// </summary>
        public BrimeUser Sender { get; private set; }

        /// <summary>
        /// Identify the list of emotes included in this message
        /// </summary>
        public Dictionary<string, BrimeChatEmote> Emotes { get; private set; } = new Dictionary<string, BrimeChatEmote>();

        /// <summary>
        /// Identify the timestamp for when this message was sent
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Create a new instance from the given JSON data
        /// </summary>
        /// <param name="message">JSON data to process</param>
        public BrimeChatMessage(JToken message) {
            // Logger.Trace("Loading Chat Message: " + message);
            string? __notice = message.Value<string>("__notice");
            if (!string.IsNullOrWhiteSpace(__notice))
                Logger.Info("Notice: " + __notice);

            string? curr = message.Value<string>("channelID");
            ChannelID = curr ?? "";

            curr = message.Value<string>("_id");
            ID = curr ?? "";

            curr = message.Value<string>("message");
            Message = curr ?? "";

            curr = message.Value<string>("richContents");   // used by BrimeBot
            RichContents = curr ?? "";

            JToken? sender = message["sender"];
            if (sender == null) throw new BrimeAPIMalformedResponse("Missing sender details on chat message");
            Sender = new BrimeUser(sender);
            // Sender = (sender == null) ? new BrimeUser() : new BrimeUser(sender);

            JToken? emotes = message["emotes"];
            if (emotes != null) {
                foreach (JToken item in emotes) {
                    if (item != null) {
                        JProperty? prop = item.ToObject<JProperty>();
                        if (prop != null) {
                            string name = prop.Name;
                            string? id = prop.Value.Value<string>("_id");
                            if (id != null) {
                                Emotes.Add(name, new BrimeChatEmote(id));
                            }
                        }
                    }
                }
            }
            try {
                long tstamp = message.Value<long>("timestamp");
                DateTimeOffset offset = DateTimeOffset.FromUnixTimeMilliseconds(tstamp);
                Timestamp = offset.DateTime;
            } catch (Exception e) {
                Logger.Error(e.ToString());
            }
        }

        /// <inheritdoc />
        public override string ToString() {
            string _result = "ChannelID: " + ChannelID + "\n";
            _result += "Message: \"" + Message + "\"\n";
            _result += "Sender: " + Sender + "\n";
            _result += "TimeStamp: " + Timestamp + "\n";
            _result += "Emotes: {\n";
            foreach (KeyValuePair<string,BrimeChatEmote> item in Emotes) {
                _result += "   " + item.Key + ": \"" + item.Value.EmoteID + "\",\n";
            }
            _result += "}";
            return _result;
        }
    }

    /// <summary>
    /// Identify an emote used in a particular message. 
    /// </summary>
    public class BrimeChatEmote {

        /// <summary>
        /// ID for the emote
        /// </summary>
        public string EmoteID { get; private set; }

        /// <summary>
        /// Create a new instance for the given emote ID
        /// </summary>
        /// <param name="id">ID for the emote</param>
        public BrimeChatEmote(string id) {
            this.EmoteID = id;
        }
    }
} 