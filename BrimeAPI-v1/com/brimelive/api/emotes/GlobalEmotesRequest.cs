#nullable enable

using System.Collections.Generic;
using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.emotes {
    /// <summary>
    /// Query for global emote sets
    /// </summary>
    public class GlobalEmotesRequest : BrimeAPIRequest<List<BrimeEmoteSet>> {

        private static readonly string GLOBAL_EMOTES_REQUEST = "/emotesets";

        /// <summary>
        /// Create a new request - no parameters as this retrieves all global emote sets
        /// </summary>
        public GlobalEmotesRequest() : base(GLOBAL_EMOTES_REQUEST) {
            // No parameters on GlobalEmotes request
        }

        /// <inheritdoc /> 
        public override List<BrimeEmoteSet> getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            JArray? emoteSets = response.Data.Value<JArray>("emoteSets");
            List<BrimeEmoteSet> _result = new List<BrimeEmoteSet>((emoteSets == null) ? 0 : emoteSets.Count);
            if (emoteSets != null) {
                foreach (JToken set in emoteSets) _result.Add(new BrimeEmoteSet(set));
            }
            return _result;
        }
    }
} 