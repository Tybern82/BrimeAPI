#nullable enable


namespace BrimeAPI.com.brimelive.api.errors {

    class BrimeAPIMissingAccess : BrimeAPIException {

        /// <summary>
        /// Exception thrown when request is made which is missing required access. <c>BrimeAPIError.Name</c> should be "MISSING_ACCESS"
        /// which appears to be returned when request made which requires <c>BrimeAPIRequest.RequiresSpecialAccess</c>
        /// </summary>
        /// <param name="apiError">API Error which triggered this exception</param>
        public BrimeAPIMissingAccess(BrimeAPIError apiError) : base(apiError) { }
    }
}
