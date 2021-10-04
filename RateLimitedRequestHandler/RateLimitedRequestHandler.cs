using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace RateLimitedRequestHandler {
    /// <summary>
    /// Helper class used to provide a rate-limiter on API requests.
    /// </summary>
    public class RateLimitedRequestHandler {

        /// <summary>
        /// Singleton instance for this class - ensures all requests use the same limiter instance.
        /// </summary>
        public static RateLimitedRequestHandler Instance = new RateLimitedRequestHandler();

        /// <summary>
        /// Set maximum requests per second for this interface. Note this can only be changed before the first request is
        /// made. 
        /// </summary>
        public ushort RequestsPerSecond { get; set; } = 5;

        private RateLimitedRequestHandler() { }

        private ResetSemaphore rateLimiter;

        /// <summary>
        /// Perform the given API request, using the given parameters. This method will wait to ensure that no more then 5 requests
        /// are issued per second as per rate limit in API spec. 
        /// </summary>
        /// <param name="request">full URL for API request</param>
        /// <param name="requestMode">request mode (either GET or POST)</param>
        /// <param name="headers">headers to add into the WebRequest, will be used to add Client-ID as header instead of Query parameter</param>
        /// <param name="postBody">data to use for POST request, generally empty string</param>
        /// <returns>processed response to API request</returns>
        public string doRequest(string request, ERequestMode requestMode, KeyValuePair<string, string>[] headers, string postBody) {
            // Initialize the semaphore, if it doesn't already exist (allows changing the RPS setting before the first call).
            if (rateLimiter == null) rateLimiter = new ResetSemaphore(RequestsPerSecond, RequestsPerSecond, 1000);

            rateLimiter.Wait(); // make sure we're not overloading the rate-limiter

            // Create the request, including any provided Headers
            WebRequest req = WebRequest.Create(request);
            foreach (KeyValuePair<string, string> item in headers)
                req.Headers.Add(item.Key, item.Value);

            // Only send data if this is a POST request.
            if (requestMode == ERequestMode.POST) {
                byte[] data = Encoding.UTF8.GetBytes(postBody);
                req.Method = "POST";
                req.ContentType = "application/json";
                req.ContentLength = data.Length;

                using (Stream stream = req.GetRequestStream()) {
                    stream.Write(data, 0, data.Length);
                }
            }

            // Get response from API
            WebResponse response = req.GetResponse();
            string _result = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return _result;
        }
    }
}
