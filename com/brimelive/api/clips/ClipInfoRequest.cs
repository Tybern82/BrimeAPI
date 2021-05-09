#nullable enable

using System;
using System.Collections.Generic;
using System.Text;

namespace BrimeAPI.com.brimelive.api.clips {
    public class ClipInfoRequest : BrimeAPIRequest<BrimeClip> {

        private static readonly string GET_CLIP_INFO_REQUEST = "/v1/clip/{0}";  // /v1/clip/:clipId

        public string ClipID { get; private set; }

        public ClipInfoRequest(string clipID) : base (GET_CLIP_INFO_REQUEST) {
            this.ClipID = clipID;
            this.RequestParameters = (() => {
                return new string[] { ClipID };
            });
        }

        public override BrimeClip getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new BrimeClip(response.Data);
        }
    }
}
