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
    /// Appears to require special access (requires v1.user.subcheck).
    /// </summary>
    public class ChannelSubscriptionRequest : BrimeAPIRequest<BrimeSubscription> {

        private static readonly string GET_CHANNEL_SUB_REQUEST = "/channel/{0}/subcheck/{1}";     // /v1/channel/:channel/subcheck/:user

        /// <summary>
        /// Name of the channel to check subscription for
        /// </summary>
        public string ChannelName { get; private set; }

        /// <summary>
        /// Name of the user to check subscription of
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Create a new instance to check subscription status of given user for specified channel
        /// </summary>
        /// <param name="channelName">name of channel to check</param>
        /// <param name="userName">name of user to check</param>
        public ChannelSubscriptionRequest(string channelName, string userName) : base(GET_CHANNEL_SUB_REQUEST, true) {
            this.ChannelName = channelName;
            this.UserName = userName;
            this.RequestParameters = (() => {
                return new string[] { ChannelName, UserName };
            });
        }

        /// <inheritdoc />
        public override BrimeSubscription getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new BrimeSubscription(response.Data);
        }
    }
}
