#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrimeAPI.com.brimelive.api.channels {
    public class ChannelSubscriptionRequest : BrimeAPIRequest<BrimeSubscription> {

        private static readonly string GET_CHANNEL_SUB_REQUEST = "/v1/channel/{0}/subcheck/{1}";     // /v1/channel/:channel/subcheck/:user

        public string ChannelName { get; private set; }
        public string UserName { get; private set; }

        public ChannelSubscriptionRequest(string channelName, string userName) : base(GET_CHANNEL_SUB_REQUEST) {
            this.ChannelName = channelName;
            this.UserName = userName;
            this.RequestParameters = (() => {
                return new string[] { ChannelName, UserName };
            });
        }

        public override BrimeSubscription getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new BrimeSubscription(response.Data);
        }
    }
}
