using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrimeAPI.com.brimelive.api {
    /// <summary>
    /// Contains static data for BrimeAPI connection
    /// </summary>
    public class BrimeAPI {

        /// <summary>
        /// Specifies the specific API endpoint to use in this request. Currently defaults to STAGING, will be updated to PRODUCTION once
        /// this version of the API is finalized. 
        /// </summary>
        public static BrimeAPIEndpoint APIEndpoint { get; private set; } = BrimeAPIEndpoint.STAGING;   // TODO: Update default to PRODUCTION

        /// <summary>
        /// Specifies the Client-ID sent with requests to the API. Note that in order to use this library you will need to have a valid 
        /// Client-ID assigned. This field is static so will only need to be specified once.
        /// </summary>
        public static string ClientID { get; set; } = "";
    }
}
