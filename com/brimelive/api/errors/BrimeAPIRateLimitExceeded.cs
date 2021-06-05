#nullable enable

namespace BrimeAPI.com.brimelive.api.errors {

    /// <summary>
    /// Exception thrown when client has sent too many requests recently and has been rate limited. Indicates caller needs
    /// to wait and retry the request.
    /// </summary>
    public class BrimeAPIRateLimitExceeded : BrimeAPIException {

        /// <summary>
        /// Create a new exception. <c>BrimeAPIError.Name</c> should be "RATE_LIMIT_EXCEEDED"
        /// </summary>
        /// <param name="error">API Error which triggered this exception</param>
        public BrimeAPIRateLimitExceeded(BrimeAPIError error) : base(error) { }
    }
}
