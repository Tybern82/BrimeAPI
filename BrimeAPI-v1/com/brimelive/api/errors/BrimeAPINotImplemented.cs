#nullable enable


namespace BrimeAPI.com.brimelive.api.errors {

    /// <summary>
    /// Exception thrown when API request was to an endpoint which is not currently implemented.
    /// </summary>
    public class BrimeAPINotImplemented : BrimeAPIException {

        /// <summary>
        /// Create a new exception. <c>BrimeAPIError.Name</c> should be "NOT_IMPLEMENTED"
        /// </summary>
        /// <param name="error">API Error which triggered this exception</param>
        public BrimeAPINotImplemented(BrimeAPIError error) : base(error) { } 
    }
}
