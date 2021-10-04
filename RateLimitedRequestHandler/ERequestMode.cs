using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateLimitedRequestHandler {
    public enum ERequestMode {
        /// <summary>
        /// Request is expected to return API response data, default request mode
        /// </summary>
        GET,

        /// <summary>
        /// Request is expected to trigger on server, normally no response data. (ie Used when using API to create clip on channel.)
        /// </summary>
        POST
    }
}
