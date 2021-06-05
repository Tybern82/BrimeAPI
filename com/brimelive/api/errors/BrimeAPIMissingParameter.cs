#nullable enable


namespace BrimeAPI.com.brimelive.api.errors {
    /// <summary>
    /// Exception thrown when API request was missing a required parameter. This will also be the exception thrown
    /// when the request failed to include a Client-ID (though if the ID was included, but not valid this will 
    /// instead generate a <c>BrimeAPIInvalidClientID</c>).
    /// </summary>
    public class BrimeAPIMissingParameter : BrimeAPIException {

        /// <summary>
        /// Create a new exception. <c>BrimeAPIError.Name</c> should be "MISSING_PARAMETER"
        /// </summary>
        /// <param name="error">API Error which triggered this exception</param>
        public BrimeAPIMissingParameter(BrimeAPIError error) : base(error) { } 
    }
}
