#nullable enable

using System;

namespace BrimeAPI.com.brimelive.api.errors {

    /// <summary>
    /// Internal API Exception, thrown when the JSON response to an API request is missing required elements, or contains
    /// invalid/unparseable values for those elements. This exception generally indicates that the current API does not 
    /// fully match the API specification in this version of the library. 
    /// </summary>
    public class BrimeAPIMalformedResponse : BrimeAPIException {

        /// <summary>
        /// Construct a new exception with only an error message.
        /// </summary>
        /// <param name="message">Descriptive message for this error</param>
        public BrimeAPIMalformedResponse(string message) : base(BrimeAPIError.lookupError("MALFORMED_RESPONSE: " + message)) { }

        /// <summary>
        /// Construct a new exception with both error message, and a base exception which triggered the error.
        /// </summary>
        /// <param name="message">Descriptive message for this error</param>
        /// <param name="baseException">Base exception which caused the issue with processing the response</param>
        public BrimeAPIMalformedResponse(string message, Exception baseException) : base(BrimeAPIError.lookupError("MALFORMED_RESPONSE: " + message), baseException) { }
    }
}
