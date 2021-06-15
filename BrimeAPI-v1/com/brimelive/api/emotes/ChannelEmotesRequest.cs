#nullable enable

using System.Collections.Generic;
using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.emotes {
    /// <summary>
    /// Query for list of emotes associated with the given channel
    /// </summary>
    public class ChannelEmotesRequest : BrimeAPIRequest<List<BrimeEmote>> {

        private static readonly string GET_CHANNEL_EMOTES_REQUEST = "/channel/{0}/emotes"; // "/channel/:channel/emotes"

        /// <summary>
        /// Channel to request (appears to support both name and ID)
        /// </summary>
        public string ChannelID { get; set; }

        /// <summary>
        /// Create a new request for the given channel (appears to support both ID and name)
        /// </summary>
        /// <param name="ID">ID / Name of channel to request</param>
        public ChannelEmotesRequest(string ID) : base(GET_CHANNEL_EMOTES_REQUEST) {
            ChannelID = ID;
            this.RequestParameters = (() => {
                return new string[] { ChannelID };
            });
        }

        /// <inheritdoc />
        public override List<BrimeEmote> getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            JArray? emotes = response.Data.Value<JArray>("emotes");
            List<BrimeEmote> Emotes = new List<BrimeEmote>((emotes == null) ? 0 : emotes.Count);
            if (emotes != null) {
                foreach (JToken emote in emotes) Emotes.Add(new BrimeEmote(emote));
            }
            return Emotes;
        }
    }
}
