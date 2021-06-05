#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using BrimeAPI.com.brimelive.api.users;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.realtime {
    public class BrimeChatMessage {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static readonly string IMAGE_URL_FORMAT = "https://content.brimecdn.com/brime/emote/{0}/{1}";

        public string ID { get; private set; }
        public string ChannelID { get; private set; }
        public string Message { get; private set; }
        public BrimeUser Sender { get; private set; }
        public Dictionary<string, BrimeChatEmote> Emotes { get; private set; } = new Dictionary<string, BrimeChatEmote>();
        public DateTime Timestamp { get; private set; }

        public BrimeChatMessage(JToken message) {
            // Logger.Trace("Loading Chat Message: " + message);
            string? __notice = message.Value<string>("__notice");
            if (!string.IsNullOrWhiteSpace(__notice))
                Logger.Info("Notice: " + __notice);

            string? curr = message.Value<string>("channelID");
            ChannelID = (curr == null) ? "" : curr;

            curr = message.Value<string>("_id");
            ID = (curr == null) ? "" : curr;

            curr = message.Value<string>("message");
            Message = (curr == null) ? "" : curr;

            JToken? sender = message["sender"];
            Sender = (sender == null) ? new BrimeUser() : new BrimeUser(sender);

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
            Timestamp = message.Value<DateTime>("timestamp");
        }

        public override string ToString() {
            string _result = "ChannelID: " + ChannelID + "\n";
            _result += "Message: " + Message + "\n";
            _result += "Sender: " + Sender + "\n";
            _result += "Emotes: {\n";
            foreach (KeyValuePair<string,BrimeChatEmote> item in Emotes) {
                _result += "   " + item.Key + ": " + item.Value.EmoteID + ",\n";
            }
            _result += "}";
            return _result;
        }
    }

    public class BrimeChatEmote {
        public string EmoteID { get; private set; }

        public BrimeChatEmote(string id) {
            this.EmoteID = id;
        }

        public string get1xImageUrl() {
            return string.Format(BrimeChatMessage.IMAGE_URL_FORMAT, EmoteID, "1x");
        }

        public string get2xImageUrl() {
            return string.Format(BrimeChatMessage.IMAGE_URL_FORMAT, EmoteID, "2x");
        }

        public string get3xImageUrl() {
            return string.Format(BrimeChatMessage.IMAGE_URL_FORMAT, EmoteID, "3x");
        }
    }
}
