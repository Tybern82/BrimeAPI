#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrimeAPI.com.brimelive.api.errors;

namespace BrimeAPI.com.brimelive.api.channels {

    /// <summary>
    /// Queries for a given channel and returns the data on that channel. Keep in mind that not every user has a channel, 
    /// attempting to query a nonexistent channel will return INVALID_CHANNEL even if the given user exists.
    /// </summary>
    public class ChannelRequest : BrimeAPIRequest<BrimeChannel> {

        /// <summary>
        /// Format for channel request string. This request format is public as it is used by both this request,
        /// as well as the ChannelExistsRequest to identify when a channel is actually present. 
        /// </summary>
        public static readonly string GET_CHANNEL_REQUEST = "/channel/{0}";    // /v1/channel/:channel

        /// <summary>
        /// Identifies the name of the channel to request information on.
        /// </summary>
        public string ChannelName { get; private set; }

        /// <summary>
        /// Create a new instance for the given channel
        /// </summary>
        /// <param name="channelName">Name of the channel to request</param>
        public ChannelRequest(string channelName) : base(GET_CHANNEL_REQUEST) {
            this.ChannelName = channelName;
            this.RequestParameters = (() => {
                return new string[] { ChannelName };
            });
        }

        /// <inheritdoc />
        public override BrimeChannel getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new BrimeChannel(response.Data);
        }
    }
}
