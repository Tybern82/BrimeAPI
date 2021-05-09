#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.emotes {
    public class GlobalEmotesRequest : BrimeAPIRequest<GlobalEmotesResponse> {

        private static string GLOBAL_EMOTES_REQUEST = "/emotesets";

        // No Parameters

        public GlobalEmotesRequest() : base(GLOBAL_EMOTES_REQUEST) {
            // No parameters on GlobalEmotes request
        }

        public override GlobalEmotesResponse getResponse() {
            return new GlobalEmotesResponse(doRequest());
        }
    }

    public class GlobalEmotesResponse {

        public List<BrimeEmoteSet> GlobalEmoteSets { get; private set; }

        public GlobalEmotesResponse(BrimeAPIResponse apiResponse) {
            BrimeAPIError.ThrowException(apiResponse);
            try {
                JArray? emoteSets = apiResponse.Data.Value<JArray>("emoteSets");
                GlobalEmoteSets = new List<BrimeEmoteSet>((emoteSets == null) ? 0 : emoteSets.Count);
                if (emoteSets != null) {
                    foreach (JToken set in emoteSets) GlobalEmoteSets.Add(new BrimeEmoteSet(set));
                }
            } catch (Exception e) {
                throw new BrimeAPIMalformedResponse(e.ToString());
            }
        }
    }
} 