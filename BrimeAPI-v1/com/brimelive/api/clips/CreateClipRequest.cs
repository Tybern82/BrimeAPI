#nullable enable

using BrimeAPI.com.brimelive.api.errors;

namespace BrimeAPI.com.brimelive.api.clips {
    /// <summary>
    /// Trigger creation of a new clip
    /// </summary>
    public class CreateClipRequest : BrimeAPIRequest<NoAPIResponse> {

        private static readonly string CREATE_CLIP_REQUEST = "/clip/{0}/create";    // /v1/clip/:channel/create

        /// <summary>
        /// Identify the channel to make clip against
        /// </summary>
        public string ChannelName { get; private set; }

        /// <summary>
        /// Creates a new clip on the given channel
        /// </summary>
        /// <param name="channelName"></param>
        public CreateClipRequest(string channelName) : base(CREATE_CLIP_REQUEST) {
            this.ChannelName = channelName;
            this.RequestParameters = (() => {
                return new string[] { ChannelName };
            });
        }

        /// <inheritdoc />
        public override NoAPIResponse getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new NoAPIResponse(response.Data);
        }
    }
}
