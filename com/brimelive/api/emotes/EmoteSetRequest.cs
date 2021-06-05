#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using BrimeAPI.com.brimelive.api.errors;

namespace BrimeAPI.com.brimelive.api.emotes {
    public class EmoteSetRequest : BrimeAPIRequest<BrimeEmoteSet> {

        private static readonly string GET_EMOTE_SET_REQUEST = "/emoteset/{0}";  // "/emoteset/:emoteset"

        public string EmoteSetID { get; set; }
        
        private EmoteSetRequest() : this("") { }

        public EmoteSetRequest(string ID) : base(GET_EMOTE_SET_REQUEST) {
            EmoteSetID = ID;
            this.RequestParameters = (() => {
                return new string[] { EmoteSetID };
            });
        }

        public override BrimeEmoteSet getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new BrimeEmoteSet(response.Data);
        }
    }
}
