#nullable enable

using BrimeAPI.com.brimelive.api.errors;

namespace BrimeAPI.com.brimelive.api.vods {

    /// <summary>
    /// Requests information on a specific VOD.
    /// </summary>
    public class VodInfoRequest : BrimeAPIRequest<BrimeVOD> {

        /// <summary>
        /// Get Vod Info: /v1/vod/:vodId <br />
        /// vodId - ID for the VOD to retrieve
        /// </summary>
        private static readonly string VOD_INFO_REQUEST = "/v1/vod/{0}";

        /// <summary>
        /// Identifies the Vod ID being requested
        /// </summary>
        public string VodID { get; private set; }

        /// <summary>
        /// Create a new request for the specified VOD ID
        /// </summary>
        /// <param name="vodID">ID of the VOD being requested</param>
        public VodInfoRequest(string vodID) : base(VOD_INFO_REQUEST) {
            this.VodID = vodID;
            this.RequestParameters = (() => {
                return new string[] { VodID };
            });
        }

        /// <inheritdoc />
        public override BrimeVOD getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new BrimeVOD(response.Data);
        }
    }
}
