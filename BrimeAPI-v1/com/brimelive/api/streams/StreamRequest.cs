#nullable enable 

using BrimeAPI.com.brimelive.api.errors;

namespace BrimeAPI.com.brimelive.api.streams {

    /// <summary>
    /// Queries for a stream by the given channel and returns the data for that stream.
    /// </summary>
    public class StreamRequest : BrimeAPIRequest<BrimeStream> {

        private static readonly string GET_STREAM_REQUEST = "/stream/{0}";      // /v1/stream/:channel

        /// <summary>
        /// Name of the channel to request current live stream information from
        /// </summary>
        public string ChannelName { get; private set; }

        /// <summary>
        /// Create a new instance, requesting stream information for given channel
        /// </summary>
        /// <param name="channelName">name of the channel to request live stream information</param>
        public StreamRequest(string channelName) : base(GET_STREAM_REQUEST) {
            this.ChannelName = channelName;
            this.RequestParameters = (() => {
                return new string[] { ChannelName };
            });
        }

        /// <inheritdoc />
        public override BrimeStream getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new BrimeStream(response.Data);
        }
    }
}
