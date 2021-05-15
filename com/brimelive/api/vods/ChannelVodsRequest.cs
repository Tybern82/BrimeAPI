using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.vods {
    public class ChannelVodsRequest : BrimeAPIRequest<List<BrimeVod>> {

        private static readonly string GET_VODS_FOR_CHANNEL_REQUEST = "/v1/channel/{0}/vods";    // /v1/channel/:channelId/vods

        public string ChannelName { get; private set; }

        public ChannelVodsRequest(string channelName) : base(GET_VODS_FOR_CHANNEL_REQUEST) {
            this.ChannelName = channelName;
            this.RequestParameters = (() => {
                return new string[] { ChannelName };
            });
        }

        public override List<BrimeVod> getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            List<BrimeVod> _result;
            JArray? vods = response.Data.Value<JArray>("streams");
            if (vods != null) {
                _result = new List<BrimeVod>(vods.Count);
                foreach (JToken item in vods) {
                    _result.Add(new BrimeVod(item));
                }
            } else {
                _result = new List<BrimeVod>();
            }
            return _result;
        }
    }
}
