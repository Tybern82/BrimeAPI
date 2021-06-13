#nullable enable


namespace BrimeAPI.com.brimelive.api.errors {

    /// <summary>
    /// Exception thrown when API request was made with an identifier that does not include the required scope for
    /// this request. 
    /// </summary>
    public class BrimeAPIMissingScope : BrimeAPIException {

        /// <summary>
        /// Create a new exception. <c>BrimeAPIError.Name</c> should be "MISSING_SCOPE"
        /// </summary>
        /// <param name="error">API Error which triggered this exception</param>
        public BrimeAPIMissingScope(BrimeAPIError error) : base(error) { } 
    }
}
