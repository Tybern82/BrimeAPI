#nullable enable

using BrimeAPI.com.brimelive.api.errors;

namespace BrimeAPI.com.brimelive.api.channels {
    /// <summary>
    /// Helper request - used to identify whether a given channel exists
    /// </summary>
    public class ChannelExistsRequest : BrimeAPIRequest<bool> {

        /// <summary>
        /// Identifies the name of the channel to request information on.
        /// </summary>
        public string ChannelName { get; private set; }

        /// <summary>
        /// Create new instance to check if the given channel exists
        /// </summary>
        public ChannelExistsRequest(string channelName) : base(ChannelRequest.GET_CHANNEL_REQUEST) {
            this.ChannelName = channelName;
            this.RequestParameters = (() => {
                return new string[] { ChannelName };
            });
        }

        /// <inheritdoc />
        public override bool getResponse() {
            BrimeAPIResponse response = doRequest();
            try {
                BrimeAPIError.ThrowException(response);
            } catch (BrimeAPIInvalidChannel) {
                // Thrown if the channel does not exist
                return false;
            }
            // If no exception thrown, channel should exist
            return true;
        }
    }
}
