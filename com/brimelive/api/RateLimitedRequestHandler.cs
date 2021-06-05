#nullable enable

using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace BrimeAPI.com.brimelive.api {

    class ResetSemaphore {

        private readonly int maxThreads;
        private int availThreads;

        private Semaphore localSemaphore;

        /// <summary>
        /// Constructs a semaphore with an automatic reset thread count. This is used to control Rate Limiting.
        /// </summary>
        /// <param name="avail">Number of requests that are initially available</param>
        /// <param name="max">Maximum number of requests available per timeout period</param>
        /// <param name="timeout">Number of milliseconds before thread counts reset</param>
        public ResetSemaphore(int avail, int max, int timeout) {
            this.availThreads = avail;
            this.maxThreads = max;
            this.localSemaphore = new Semaphore(avail, max);

            Thread t = new Thread(new ThreadStart(() => {
                Thread.Sleep(timeout);
                try {
                    lock (this) {
                        if (availThreads < maxThreads) {
                            // Should only be releasing if items have been allocated
                            localSemaphore.Release(maxThreads - availThreads);
                            availThreads = maxThreads;
                        }
                    }
                } catch (SemaphoreFullException) { }
            })) {
                Priority = ThreadPriority.BelowNormal,
                IsBackground = true
            };
            t.Start();
        }

        /// <summary>
        /// Call to request entry to the Semaphore
        /// </summary>
        public void Wait() {
            localSemaphore.WaitOne();
            lock (this) {
                availThreads--;
            }
        }
    }

    /// <summary>
    /// Helper class used to provide a rate-limiter on API requests.
    /// </summary>
    public class RateLimitedRequestHandler {

        /// <summary>
        /// Singleton instance for this class - ensures all requests use the same limiter instance.
        /// </summary>
        public static RateLimitedRequestHandler Instance = new RateLimitedRequestHandler();

        private RateLimitedRequestHandler() {}

        private ResetSemaphore rateLimiter = new ResetSemaphore(5, 5, 1000);    // starts with 5 requests, has a maximum of 5 requests

        /// <summary>
        /// Perform the given API request, using the given parameters. This method will wait to ensure that no more then 5 requests
        /// are issued per second as per rate limit in API spec. 
        /// </summary>
        /// <param name="request">full URL for API request</param>
        /// <param name="requestMode">request mode (either GET or POST)</param>
        /// <param name="postBody">data to use for POST request, generally empty string</param>
        /// <returns>processed response to API request</returns>
        public BrimeAPIResponse doRequest(string request, BrimeRequestMode requestMode, string[] headers, string postBody) {
            rateLimiter.Wait(); // make sure we're not overloading the rate-limiter

            // Create the request, including any provided Headers
            WebRequest req = WebRequest.Create(request);
            foreach (string item in headers)
                req.Headers.Add(item);

            // Only send data if this is a POST request.
            if (requestMode == BrimeRequestMode.POST) {
                byte[] data = Encoding.UTF8.GetBytes(postBody);
                req.Method = "POST";
                req.ContentType = "application/json";
                req.ContentLength = data.Length;

                using (var stream = req.GetRequestStream()) {
                    stream.Write(data, 0, data.Length);
                }
            }

            // Get response from API
            WebResponse response = req.GetResponse();
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

            // Process the JSON response
            return new BrimeAPIResponse(json);
        }
    }
}
