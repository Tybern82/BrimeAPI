#nullable enable


namespace BrimeAPI.com.brimelive.api.errors {

    /// <summary>
    /// Represents an Internal Server error when making API request.
    /// </summary>
    public class BrimeAPIInternalError : BrimeAPIException { 

        /// <summary>
        /// Create a new exception. <c>BrimeAPIError.Name</c> should be "INTERNAL_ERROR"
        /// </summary>
        /// <param name="error">API Error which triggered this exception</param>
        public BrimeAPIInternalError(BrimeAPIError error) : base(error) { } 
    }
}
