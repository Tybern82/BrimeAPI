#nullable enable

using System.Collections.Generic;
using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.streams {
    /// <summary>
    /// Load details of currently live streams
    /// </summary>
    public class LiveStreamsRequest : BrimeAPIRequest<List<BrimeStream>> {

        private static readonly string GET_LIVE_STREAMS_REQUEST = "/streams";    // /v1/streams

        /// <summary>
        /// Create a new instance to request live streams
        /// </summary>
        public LiveStreamsRequest() : base(GET_LIVE_STREAMS_REQUEST) { }    // No parameters

        /// <inheritdoc />
        public override List<BrimeStream> getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            List<BrimeStream> _result;
            JArray? streams = response.Data.Value<JArray>("streams");
            if (streams != null) {
                _result = new List<BrimeStream>(streams.Count);
                foreach (JToken item in streams) {
                    _result.Add(new BrimeStream(item));
                }
            } else {
                _result = new List<BrimeStream>();
            }
            return _result;
        }
    }
}
