#nullable enable

using BrimeAPI.com.brimelive.api.errors;

namespace BrimeAPI.com.brimelive.api.emotes {
    /// <summary>
    /// Query for a list of emotes in a particular set
    /// </summary>
    public class EmoteSetRequest : BrimeAPIRequest<BrimeEmoteSet> {

        private static readonly string GET_EMOTE_SET_REQUEST = "/emoteset/{0}";  // "/emoteset/:emoteset"

        /// <summary>
        /// ID for the emote set to retrieve
        /// </summary>
        public string EmoteSetID { get; set; }

        /// <summary>
        /// Create a new request for the given emote set
        /// </summary>
        /// <param name="ID">ID for the set to retrieve</param>
        public EmoteSetRequest(string ID) : base(GET_EMOTE_SET_REQUEST) {
            EmoteSetID = ID;
            this.RequestParameters = (() => {
                return new string[] { EmoteSetID };
            });
        }

        /// <inheritdoc />
        public override BrimeEmoteSet getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new BrimeEmoteSet(response.Data);
        }
    }
}
