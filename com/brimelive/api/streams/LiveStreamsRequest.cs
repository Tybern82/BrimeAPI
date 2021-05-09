using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.streams {
    public class LiveStreamsRequest : BrimeAPIRequest<List<BrimeStream>> {

        private static readonly string GET_LIVE_STREAMS_REQUEST = "/v1/streams";    // /v1/streams

        public LiveStreamsRequest() : base(GET_LIVE_STREAMS_REQUEST) { }    // No parameters

        public override List<BrimeStream> getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            List<BrimeStream> _result;
            JArray? streams = response.Data.Value<JArray>("streams");
            if (streams != null) {
                _result = new List<BrimeStream>(streams.Count);
                foreach (JToken item in streams) {
                    _result.Add(new BrimeStream(item));
                }
            } else {
                _result = new List<BrimeStream>();
            }
            return _result;
        }
    }
}
