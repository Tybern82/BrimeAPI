using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrimeAPI.com.brimelive.api.vods {
    public class VodInfoRequest : BrimeAPIRequest<BrimeVod> {

        private static readonly string VOD_INFO_REQUEST = "/v1/vod/{0}";     // /v1/vod/:vodId

        public string VodID { get; private set; }

        public VodInfoRequest(string vodID) : base(VOD_INFO_REQUEST) {
            this.VodID = vodID;
            this.RequestParameters = (() => {
                return new string[] { VodID };
            });
        }

        public override BrimeVod getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new BrimeVod(response.Data);
        }
    }
}
