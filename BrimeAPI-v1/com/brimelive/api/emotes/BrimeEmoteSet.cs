#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.emotes {
    /// <summary>
    /// Specify a collection of emotes 
    /// </summary>
    public class BrimeEmoteSet : IEnumerable<BrimeEmote> {

        /// <summary>
        /// Unique ID for this emote set
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// Name for this emote set
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Associated channel for this emote set
        /// </summary>
        public string ChannelID { get; private set; }

        /// <summary>
        /// List of the emotes included in this set
        /// </summary>
        public List<BrimeEmote> Emotes { get; private set; }

        /// <summary>
        /// Create a new instance from the given JSON data
        /// </summary>
        /// <param name="emoteSet">JSON data to process</param>
        public BrimeEmoteSet(JToken emoteSet) {
            try {
                string? curr = emoteSet.Value<string>("_id");
                if (curr == null) throw new BrimeAPIMalformedResponse("Missing ID in emote set");
                ID = curr;

                curr = emoteSet.Value<string>("name");
                if (curr == null) throw new BrimeAPIMalformedResponse("Missing Name in emote set");
                Name = curr;

                curr = emoteSet.Value<string>("channelID");
                ChannelID = curr ?? "";     // NOTE: No channel ID for Global Emotes?

                if (!string.IsNullOrWhiteSpace(ID)) 
                    BrimeAPI.EmoteSets[ID] = this;      // record this set into the global list

                JArray? emotes = emoteSet.Value<JArray>("emotes");
                Emotes = new List<BrimeEmote>((emotes == null) ? 0 : emotes.Count);
                if (emotes != null) {
                    foreach (JToken emote in emotes) Emotes.Add(new BrimeEmote(emote));
                }
            } catch (Exception e) {
                throw new BrimeAPIMalformedResponse(e.ToString());
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public IEnumerator<BrimeEmote> GetEnumerator() {
            return ((IEnumerable<BrimeEmote>)Emotes).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)Emotes).GetEnumerator();
        }
    }
} 