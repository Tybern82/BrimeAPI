#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.emotes {
    public enum BrimeEmoteSize { X1, X2, X3 }

    public class BrimeEmote {

        public string ID { get; private set; }
        public string Name { get; private set; }

        private string _EmoteSet;
        public BrimeEmoteSet? EmoteSet => BrimeEmoteSet.lookupEmoteSet(_EmoteSet);

        public BrimeEmote(JToken emote) {
            try {
                string? curr = emote.Value<string>("_id");
                ID = (curr == null) ? "" : curr;

                curr = emote.Value<string>("name");
                Name = (curr == null) ? "" : curr;

                curr = emote.Value<string>("emoteSet");
                _EmoteSet = (curr == null) ? "" : curr;
            } catch (Exception e) {
                throw new BrimeAPIMalformedResponse(e.ToString());
            }
        }

        public string getImageURL(BrimeEmoteSize sz) {
            string URLFormat = "https://content.brimecdn.com/brime/emote/{0}/{1}";
            return string.Format(URLFormat, new string[] { Name, getSize(sz) });
        }

        private string getSize(BrimeEmoteSize sz) {
            switch (sz) {
                case BrimeEmoteSize.X3: return "x3";
                case BrimeEmoteSize.X2: return "x2";
                case BrimeEmoteSize.X1:
                default:                return "x1";
            }
        }

        public override string ToString() {
            return "EMOTE<" + ID + ": " + Name + ">";
        }
    }
}