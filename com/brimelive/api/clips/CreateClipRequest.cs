using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrimeAPI.com.brimelive.api.clips {
    public class CreateClipRequest : BrimeAPIRequest<NoAPIResponse> {

        private static readonly string CREATE_CLIP_REQUEST = "/v1/clip/{0}/create";    // /v1/clip/:channel/create

        public string ChannelName { get; private set; }

        public CreateClipRequest(string channelName) : base(CREATE_CLIP_REQUEST) {
            this.ChannelName = channelName;
            this.RequestParameters = (() => {
                return new string[] { ChannelName };
            });
        }

        public override NoAPIResponse getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new NoAPIResponse(response.Data);
        }
    }
}
