#nullable enable

using System;
using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.emotes {
    /// <summary>
    /// Specify the sizing options for Brime Emotes
    /// </summary>
    public enum BrimeEmoteSize { 
        /// <summary>
        /// Smallest size (x1)
        /// </summary>
        X1, 
        /// <summary>
        /// Intermediate size (x2)
        /// </summary>
        X2, 
        /// <summary>
        /// Largest size (x3)
        /// </summary>
        X3 
    }

    /// <summary>
    /// Identify an emote in the Brime system
    /// </summary>
    public class BrimeEmote {

        /// <summary>
        /// Identifier for this emote
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// Name for this emote
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Used to retrieve the EmoteSet this emote is a part of
        /// </summary>
        public BrimeEmoteSet? EmoteSet => BrimeAPI.lookupEmoteSet(_EmoteSet);
        private readonly string _EmoteSet;

        /// <summary>
        /// Create a new instance from the given JSON data
        /// </summary>
        /// <param name="emote">JSON data to process</param>
        public BrimeEmote(JToken emote) {
            try {
                string? curr = emote.Value<string>("_id");
                if (curr == null) throw new BrimeAPIMalformedResponse("Missing ID in emote");
                ID = curr;

                curr = emote.Value<string>("name");
                if (curr == null) throw new BrimeAPIMalformedResponse("Missing name for emote");
                Name = curr;

                curr = emote.Value<string>("emoteSet");
                if (curr == null) throw new BrimeAPIMalformedResponse("Missing emote set ID for emote");
                _EmoteSet = curr;
            } catch (Exception e) {
                throw new BrimeAPIMalformedResponse(e.ToString());
            }
        }

        /// <summary>
        /// Retrieve the URL for this emote with the given size
        /// </summary>
        /// <param name="sz">size of the emote to retrieve</param>
        /// <returns>URL for emote at this size</returns>
        public string getImageURL(BrimeEmoteSize sz) {
            return getImageURL(Name, sz);
        }

        /// <inheritdoc />
        public override string ToString() {
            return "EMOTE<" + ID + ": " + Name + ">";
        }

        /// <summary>
        /// Used to retrieve the Image URL. This method is public to allow both access from the BrimeEmote class itself, as well
        /// as remotely when loading emotes for BrimeChatEmote (which only includes the name).
        /// </summary>
        /// <param name="name">name of the emote to get a link to</param>
        /// <param name="sz">size of the emote to retreive</param>
        /// <returns>https://content.brimecdn.com/brime/emote/:Name:/:Size:</returns>
        public static string getImageURL(string name, BrimeEmoteSize sz) {
            string URLFormat = "https://content.brimecdn.com/brime/emote/{0}/{1}";
            return string.Format(URLFormat, new string[] { name, getSize(sz) });
        }

        private static string getSize(BrimeEmoteSize sz) => sz switch {
            BrimeEmoteSize.X3 => "x3",
            BrimeEmoteSize.X2 => "x2",
            _ => "x1",
        };
    }
}

