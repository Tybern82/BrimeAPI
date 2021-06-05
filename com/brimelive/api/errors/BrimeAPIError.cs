#nullable enable


namespace BrimeAPI.com.brimelive.api.errors {

    /// <summary>
    /// Defines the states possible when checking for errors in API response. Can be either a VALID response, a response containing
    /// one, or more, ERROR conditions, or a request that needs to RETRY due to a temporary issue (currently only Rate Limit Exceeded).
    /// </summary>
    public enum ErrorCheckResult {
        /// <summary>API Response contained the requested data</summary>
        VALID, 
        /// <summary>API Request produced an error, will need to check request before trying again</summary>
        ERROR, 
        /// <summary>API Request did not successfully complete, will need to be retried</summary>
        RETRY
    }

    /// <summary>
    /// <c>BrimeAPIError</c> defines/implements the various error responses possible with an API request. Note this class does not
    /// represent a C# Exception, for that we use <c>BrimeAPIException</c> and its subclasses. This class is used to wrap/interpret
    /// the responses provided from the API which are then converted into the C# Exceptions users of the library should be detecting.
    /// </summary>
    public class BrimeAPIError {
        /// <summary>Class Logging instance.</summary>
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /* Currently unused.
        public static BrimeAPIError INTERNAL_ERROR      = new BrimeAPIError("INTERNAL_ERROR",       500);   // INTERNAL_ERROR: Internal server error.
        public static BrimeAPIError NOT_IMPLEMENTED     = new BrimeAPIError("NOT_IMPLEMENTED",      501);   // NOT_IMPLEMENTED: Not implemented.
        public static BrimeAPIError MISSING_PARAMETER   = new BrimeAPIError("MISSING_PARAMETER",    400);   // MISSING_PARAMETER: Missing required parameter "client_id"
        public static BrimeAPIError INVALID_CLIENT_ID   = new BrimeAPIError("INVALID_CLIENT_ID",    401);   // INVALID_CLIENT_ID: Invalid client id.
        public static BrimeAPIError MISSING_SCOPE       = new BrimeAPIError("MISSING_SCOPE",        401);   // MISSING_SCOPE: Missing required scope "READ_USER_EMAIL"
        public static BrimeAPIError INVALID_CHANNEL     = new BrimeAPIError("INVALID_CHANNEL",      404);   // INVALID_CHANNEL: Invalid channel name "notGeeken"
        public static BrimeAPIError RATE_LIMIT_EXCEEDED = new BrimeAPIError("RATE_LIMIT_EXCEEDED",  429);   // RATE_LIMIT_EXCEEDED: Rate limit exceeded
        */

        /// <summary>
        /// Error response name. This name is used to identify the specific error which has occurred, as the <c>ErrorCode</c>
        /// can be shared by multiple different errors.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Error response code. This code is generally related to the associated HTTP response code which would be returned
        /// by this particular error. This code may be the default if the Name was not recognized as a known error response.
        /// </summary>
        public int ErrorCode { get; private set; }

        /// <summary>
        /// Descriptive message provided with the error. This message may contain more specific information on the cause of the
        /// error, or what may be needed to correct it. 
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Create a new instance, using given Name, ErrorCode and Message. Constructors for this class are <c>private</c>
        /// since they should only be created in response to parsing the API response JSON.
        /// </summary>
        /// <param name="name">Identifying name of the error, as provided in the API response</param>
        /// <param name="errorCode">Error code associated with this error, if known, or default if unknown error</param>
        /// <param name="message">Descriptive message provided with this error in API response</param>
        private BrimeAPIError(string name, int errorCode, string message) {
            this.Name = name;
            this.ErrorCode = errorCode;
            this.Message = message;
        }

        /// <summary>
        /// Create a new instance, using an empty message component.
        /// </summary>
        /// <param name="name">Identifying name of the error</param>
        /// <param name="errorCode">Status code associated with this error (may be default if unknown error)</param>
        private BrimeAPIError(string name, int errorCode) : this(name, errorCode, "") { }

        /// <summary>
        /// Runs check over given API response to identify whether an error has occured with the request. Will also 
        /// identify if the request needs to be retried due to temporary issues.
        /// </summary>
        /// <param name="response">API response to check</param>
        /// <returns><c>ErrorCheckResult</c> will be VALID if no issues, RETRY if Rate limited, or ERROR for other issues</returns>
        public static ErrorCheckResult CheckError(BrimeAPIResponse response) {
            ErrorCheckResult _result = ErrorCheckResult.VALID;
            foreach (BrimeAPIError error in response.Errors) {
                if (error.ErrorCode == 429)
                    _result = ErrorCheckResult.RETRY;
                else {
                    return _result; // once one error detected, return immediately
                }
            }
            return _result; // result will either be VALID if no error present, or set to RETRY if RateLimit detected (ERROR returns early)
        }

        /// <summary>
        /// Throws exception for the first detected error in the response.
        /// </summary>
        /// <param name="response">API response to validate</param>
        public static void ThrowException(BrimeAPIResponse response) {
            // Note: Despite this being a foreach, this will only throw the first error detected. Using foreach to
            //       simplify difference between majority of cases which have no errors, and those which do. Could
            //       potentially be switched to if (response.Errors.Count > 0) switch (response.Errors[0]) ...
            foreach (BrimeAPIError error in response.Errors) {
                switch (error.Name) {
                    case "INTERNAL_ERROR":      throw new BrimeAPIInternalError(error);
                    case "NOT_IMPLEMENTED":     throw new BrimeAPINotImplemented(error);
                    case "MISSING_PARAMETER":   throw new BrimeAPIMissingParameter(error);
                    case "INVALID_CLIENT_ID":   throw new BrimeAPIInvalidClientID(error);
                    case "MISSING_SCOPE":       throw new BrimeAPIMissingScope(error);
                    case "INVALID_CHANNEL":     throw new BrimeAPIInvalidChannel(error);
                    case "RATE_LIMIT_EXCEEDED": throw new BrimeAPIRateLimitExceeded(error);
                    case "MISSING_ACCESS":      throw new BrimeAPIMissingAccess(error);
                    default:                    throw new BrimeAPIException(error);
                }
            }
        }

        /// <summary>
        /// Primary public constructor for this class. Processes the given error message (which should have been supplied in
        /// the JSON response to an API request) to produce an instance of this class.
        /// </summary>
        /// <param name="errorMessage">Should be in the format "NAME: Message" as provided by API response data in JSON</param>
        /// <returns>Parses the contents of the given message and creates an appropriate instance of this class</returns>
        public static BrimeAPIError lookupError(string errorMessage) {
            int idx = errorMessage.IndexOf(':');
            if (idx == -1) {
                // If no ':' in message, this is likely an unknown/invalid error, will attempt to lookup appropriate code,
                // however is likely to be default
                Logger.Warn("Missing ':' in error response: " + errorMessage);
                return new BrimeAPIError(errorMessage, getErrorCode(errorMessage));
            } else {
                // Splits the message into a Name and descriptive Message component, then calls helper to identify an appropriate
                // ErrorCode to include.
                string name = errorMessage.Substring(0, idx);
                string message = errorMessage.Substring(idx + 1).Trim();
                return new BrimeAPIError(name, getErrorCode(name), message);
            }
        }

        /// <summary>
        /// Internal helper method to lookup the associated ErrorCode for a given Name
        /// </summary>
        /// <param name="errorName">Identifying Name of the error</param>
        /// <returns>ErrorCode for this name, or 418 if the name was not recognized</returns>
        private static int getErrorCode(string errorName) {
            // Entries in this switch will be added/removed to match the known list of responses from the API
            switch (errorName) {
                case "INTERNAL_ERROR":      return 500; // HTTP status code 500: Internal Server Error
                case "NOT_IMPLEMENTED":     return 501; // HTTP status code 501: Not Implemented
                case "MISSING_PARAMETER":   return 400; // HTTP status code 400: Bad Request
                case "INVALID_CLIENT_ID":   return 401; // HTTP status code 401: Unauthorized
                case "MISSING_SCOPE":       return 401;       
                case "INVALID_CHANNEL":     return 404; // HTTP status code 404: Not Found
                case "RATE_LIMIT_EXCEEDED": return 429; // HTTP status code 429: Too Many Requests

                // Following items are currently being included, but not officially listing in API spec
                case "MISSING_ACCESS":      return 403; // Identified as response to requests requiring special access - HTTP status code 403: Forbidden
                case "MALFORMED_RESPONSE":  return 422; // Internal API error - HTTP status code 422: Unprocessable Entity

                default:
                    // This specific name is not recognized - may be a newly implemented name, or an invalid message (ie not actually
                    // part of an API response). Logs a warning and returns a default code of 418.
                    Logger.Warn("Unknown BrimeAPI Error: " + errorName);
                    return 418;                         // Unlikely to be used as a valid error - HTTP status code 418: I'm a Teapot
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) {
            if (obj == null) return (this == null);
            return (!(obj is BrimeAPIError error)) ? false : Name.Equals(error.Name);
        }


        /// <inheritdoc/>
        public override string ToString() {
            return Name + ": " + Message;
        }


        /// <inheritdoc/>
        public override int GetHashCode() {
            return Name.GetHashCode();
        }
    }
}