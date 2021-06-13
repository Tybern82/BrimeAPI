#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using BrimeAPI.com.brimelive.api.errors;

namespace BrimeAPI.com.brimelive.api.emotes {
    public class ChannelEmotesRequest : BrimeAPIRequest<ChannelEmotesResponse> {

        private static string GET_CHANNEL_EMOTES_REQUEST = "/channel/{0}/emotes"; // "/channel/:channel/emotes"

        public string ChannelID { get; set; }

        public ChannelEmotesRequest() : this("") { }

        public ChannelEmotesRequest(string ID) : base(GET_CHANNEL_EMOTES_REQUEST) {
            ChannelID = ID;
            this.RequestParameters = (() => {
                return new string[] { ChannelID };
            });
        }

        public override ChannelEmotesResponse getResponse() {
            return new ChannelEmotesResponse(doRequest());
        }
    }

    public class ChannelEmotesResponse {

        public BrimeEmoteSet EmoteSet { get; private set; }

        public ChannelEmotesResponse(BrimeAPIResponse response) {
            try {
                // TODO: Check structure of API response - no current example
                EmoteSet = new BrimeEmoteSet(response.Data);
            } catch (Exception e) {
                throw new BrimeAPIMalformedResponse(e.ToString());
            }
        }
    }
}
