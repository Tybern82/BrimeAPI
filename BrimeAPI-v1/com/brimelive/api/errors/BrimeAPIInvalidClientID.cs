#nullable enable


namespace BrimeAPI.com.brimelive.api.errors {

    /// <summary>
    /// Exception thrown when the API request included an invalid Client-ID. Note that the Error will 
    /// generally be a missing parameter if the Client-ID was not sent at all.
    /// </summary>
    /// <seealso cref="BrimeAPIMissingParameter"/>
    public class BrimeAPIInvalidClientID : BrimeAPIException {

        /// <summary>
        /// Create a new exception. <c>BrimeAPIError.Name</c> should be "INVALID_CLIENT_ID"
        /// </summary>
        /// <param name="error">API Error which triggered this exception</param>
        public BrimeAPIInvalidClientID(BrimeAPIError error) : base(error) { } 
    }
}
