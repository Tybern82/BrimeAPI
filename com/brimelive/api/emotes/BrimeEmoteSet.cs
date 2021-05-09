#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.emotes {
    public class BrimeEmoteSet : IEnumerable<BrimeEmote> {

        public static Dictionary<string, BrimeEmoteSet> EmoteSets { get; private set; } = new Dictionary<string, BrimeEmoteSet>();

        private static List<BrimeEmoteSet>? _globalEmotes = null;
        public static List<BrimeEmoteSet> GlobalEmotes {
            get {
                if (_globalEmotes == null) {
                    GlobalEmotesRequest req = new GlobalEmotesRequest();
                    _globalEmotes = req.getResponse().GlobalEmoteSets;
                }
                return new List<BrimeEmoteSet>(_globalEmotes);
            }
        }

        public string ID { get; private set; }
        public string Name { get; private set; }
        public string ChannelID { get; private set; }

        public List<BrimeEmote> Emotes { get; private set; }

        public BrimeEmoteSet(JToken emoteSet) {
            try {
                string? curr = emoteSet.Value<string>("_id");
                ID = (curr == null) ? "" : curr;

                curr = emoteSet.Value<string>("name");
                Name = (curr == null) ? "" : curr;

                curr = emoteSet.Value<string>("channelID");
                ChannelID = (curr == null) ? "" : curr;

                if (!string.IsNullOrWhiteSpace(ID)) EmoteSets[ID] = this;

                JArray? emotes = emoteSet.Value<JArray>("emotes");
                Emotes = new List<BrimeEmote>((emotes == null) ? 0 : emotes.Count);
                if (emotes != null) {
                    foreach (JToken emote in emotes) Emotes.Add(new BrimeEmote(emote));
                }
            } catch (Exception e) {
                throw new BrimeAPIMalformedResponse(e.ToString());
            }
        }

        public static BrimeEmoteSet? lookupEmoteSet(string setID) {
            EmoteSets.TryGetValue(setID, out BrimeEmoteSet _result);
            return _result;
        }

        public override string ToString() {
            string _result = "EmoteSet<" + ID + ": " + Name + "> for <" + ChannelID + "> {";
            bool isFirst = true;
            foreach (BrimeEmote emote in Emotes) {
                if (!isFirst) _result += ", ";
                _result += emote;
                isFirst = false;
            }
            _result += "}";
            return _result;
        }

        public IEnumerator<BrimeEmote> GetEnumerator() {
            return ((IEnumerable<BrimeEmote>)Emotes).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)Emotes).GetEnumerator();
        }
    }
} 