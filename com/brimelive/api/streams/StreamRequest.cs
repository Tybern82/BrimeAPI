#nullable enable 

using BrimeAPI.com.brimelive.api.errors;

namespace BrimeAPI.com.brimelive.api.streams {

    /// <summary>
    /// Queries for a stream by the given channel and returns the data for that stream.
    /// </summary>
    public class StreamRequest : BrimeAPIRequest<BrimeStream> {

        private static readonly string GET_STREAM_REQUEST = "/v1/stream/{0}";      // /v1/stream/:channel

        public string ChannelName { get; private set; }

        public StreamRequest(string channelName) : base(GET_STREAM_REQUEST) {
            this.ChannelName = channelName;
            this.RequestParameters = (() => {
                return new string[] { ChannelName };
            });
        }
        public override BrimeStream getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new BrimeStream(response.Data);
        }
    }
}
