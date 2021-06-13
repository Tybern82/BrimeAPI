#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using BrimeAPI.com.brimelive.api.errors;

namespace BrimeAPI.com.brimelive.api.clips {
    /// <summary>
    /// Queries for information on a particular clip
    /// </summary>
    public class ClipInfoRequest : BrimeAPIRequest<BrimeClip> {

        private static readonly string GET_CLIP_INFO_REQUEST = "/clip/{0}";  // /v1/clip/:clipId

        /// <summary>
        /// ID of the clip to retrieve information about
        /// </summary>
        public string ClipID { get; private set; }

        /// <summary>
        /// Create a new request for information on the given clip
        /// </summary>
        /// <param name="clipID">ID of the clip to request</param>
        public ClipInfoRequest(string clipID) : base (GET_CLIP_INFO_REQUEST) {
            this.ClipID = clipID;
            this.RequestParameters = (() => {
                return new string[] { ClipID };
            });
        }

        /// <inheritdoc />
        public override BrimeClip getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new BrimeClip(response.Data);
        }
    }
}
