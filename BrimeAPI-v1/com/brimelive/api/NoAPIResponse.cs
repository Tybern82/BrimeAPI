#nullable enable

using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api {

    /// <summary>
    /// Class used to identify an API request that provides no actual response. Used for POST requests.
    /// </summary>
    public class NoAPIResponse {

        /// <summary>
        /// Create a new instance. Constructor ignores the parameter as this should be empty for no-response 
        /// requests.
        /// </summary>
        /// <param name="jsonData">JSON data included in the API response, should be empty</param>
        public NoAPIResponse(JToken jsonData) {}
    }
}
