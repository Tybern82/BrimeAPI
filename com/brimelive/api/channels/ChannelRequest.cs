#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrimeAPI.com.brimelive.api.channels {
    public class ChannelRequest : BrimeAPIRequest<BrimeChannel> {

        public static readonly string GET_CHANNEL_REQUEST = "/v1/channel/{0}";    // /v1/channel/:channel

        public string ChannelName { get; private set; }

        public ChannelRequest(string channelName) : base(GET_CHANNEL_REQUEST) {
            this.ChannelName = channelName;
            this.RequestParameters = (() => {
                return new string[] { ChannelName };
            });
        }

        public override BrimeChannel getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new BrimeChannel(response.Data);
        }
    }
}
