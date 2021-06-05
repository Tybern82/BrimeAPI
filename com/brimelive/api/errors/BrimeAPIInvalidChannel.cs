#nullable enable


namespace BrimeAPI.com.brimelive.api.errors {

    /// <summary>
    /// Exception thrown when the API request was for a non-existent or invalid channel.
    /// </summary>
    public class BrimeAPIInvalidChannel : BrimeAPIException {

        /// <summary>
        /// Create a new exception. <c>BrimeAPIError.Name</c> should be "INVALID_CHANNEL"
        /// </summary>
        /// <param name="error">API Error which triggered this exception</param>
        public BrimeAPIInvalidChannel(BrimeAPIError error) : base(error) { } 
    }
}
