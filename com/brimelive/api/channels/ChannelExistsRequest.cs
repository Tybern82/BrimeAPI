#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrimeAPI.com.brimelive.api.errors;

namespace BrimeAPI.com.brimelive.api.channels {
    public class ChannelExistsRequest : BrimeAPIRequest<bool> {

        public ChannelExistsRequest() : base(ChannelRequest.GET_CHANNEL_REQUEST) {} // No parameters

        public override bool getResponse() {
            BrimeAPIResponse response = doRequest();
            try {
                BrimeAPIError.ThrowException(response);
            } catch (BrimeAPIInvalidChannel) {
                return false;
            }
            return true;
        }
    }
}
