#nullable enable

using System;
using System.Collections.Generic;
using System.Text;

namespace BrimeAPI.com.brimelive.api {

    public class BrimeAPIException : Exception {

        public BrimeAPIError Error { get; private set; }
        public BrimeAPIException(BrimeAPIError apiError) : base(apiError.ToString()) {
            this.Error = apiError;
        }
    }

    public class BrimeAPIInternalError : BrimeAPIException { public BrimeAPIInternalError(BrimeAPIError error) : base(error) { } }
    public class BrimeAPINotImplemented : BrimeAPIException { public BrimeAPINotImplemented(BrimeAPIError error) : base(error) { } }
    public class BrimeAPIMissingParameter : BrimeAPIException { public BrimeAPIMissingParameter(BrimeAPIError error) : base(error) { } }
    public class BrimeAPIInvalidClientID : BrimeAPIException { public BrimeAPIInvalidClientID(BrimeAPIError error) : base(error) { } }
    public class BrimeAPIMissingScope : BrimeAPIException { public BrimeAPIMissingScope(BrimeAPIError error) : base(error) { } }
    public class BrimeAPIInvalidChannel : BrimeAPIException { public BrimeAPIInvalidChannel(BrimeAPIError error) : base(error) { } }
    public class BrimeAPIRateLimitExceeded : BrimeAPIException { public BrimeAPIRateLimitExceeded(BrimeAPIError error) : base(error) { } }
    public class BrimeAPIMalformedResponse : BrimeAPIException { 
        public BrimeAPIMalformedResponse(BrimeAPIError error) : base(error) { }
        public BrimeAPIMalformedResponse(string message) : base(BrimeAPIError.lookupError("MALFORMED_RESPONSE: " + message)) { }
    }

    public enum ErrorCheckResult {
        VALID, ERROR, RETRY
    }

    public class BrimeAPIError {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static BrimeAPIError INTERNAL_ERROR      = new BrimeAPIError("INTERNAL_ERROR",       500);   // INTERNAL_ERROR: Internal server error.
        public static BrimeAPIError NOT_IMPLEMENTED     = new BrimeAPIError("NOT_IMPLEMENTED",      501);   // NOT_IMPLEMENTED: Not implemented.
        public static BrimeAPIError MISSING_PARAMETER   = new BrimeAPIError("MISSING_PARAMETER",    400);   // MISSING_PARAMETER: Missing required parameter "client_id"
        public static BrimeAPIError INVALID_CLIENT_ID   = new BrimeAPIError("INVALID_CLIENT_ID",    401);   // INVALID_CLIENT_ID: Invalid client id.
        public static BrimeAPIError MISSING_SCOPE       = new BrimeAPIError("MISSING_SCOPE",        401);   // MISSING_SCOPE: Missing required scope "READ_USER_EMAIL"
        public static BrimeAPIError INVALID_CHANNEL     = new BrimeAPIError("INVALID_CHANNEL",      404);   // INVALID_CHANNEL: Invalid channel name "notGeeken"
        public static BrimeAPIError RATE_LIMIT_EXCEEDED = new BrimeAPIError("RATE_LIMIT_EXCEEDED",  429);   // RATE_LIMIT_EXCEEDED: Rate limit exceeded

        public string Name { get; private set; }
        public int ErrorCode { get; private set; }
        public string Message { get; private set; }

        private BrimeAPIError(string name, int errorCode, string message) {
            this.Name = name;
            this.ErrorCode = errorCode;
            this.Message = message;
        }

        private BrimeAPIError(string name, int errorCode) : this(name, errorCode, "") { }

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

        public static void ThrowException(BrimeAPIResponse response) {
            foreach (BrimeAPIError error in response.Errors) {
                switch (error.Name) {
                    case "INTERNAL_ERROR": throw new BrimeAPIInternalError(error);
                    case "NOT_IMPLEMENTED": throw new BrimeAPINotImplemented(error);
                    case "MISSING_PARAMETER": throw new BrimeAPIMissingParameter(error);
                    case "INVALID_CLIENT_ID": throw new BrimeAPIInvalidClientID(error);
                    case "MISSING_SCOPE": throw new BrimeAPIMissingScope(error);
                    case "INVALID_CHANNEL": throw new BrimeAPIInvalidChannel(error);
                    case "RATE_LIMIT_EXCEEDED": throw new BrimeAPIRateLimitExceeded(error);
                    default: throw new BrimeAPIException(error);
                }
            }
        }

        public static BrimeAPIError lookupError(string errorMessage) {
            int idx = errorMessage.IndexOf(':');
            if (idx == -1) {
                Logger.Warn("Missing ':' in error response: " + errorMessage);
                return new BrimeAPIError(errorMessage, getErrorCode(errorMessage));
            } else {
                string name = errorMessage.Substring(0, idx);
                string message = errorMessage.Substring(idx + 1).Trim();
                return new BrimeAPIError(name, getErrorCode(name), message);
            }
        }

        private static int getErrorCode(string errorName) {
            switch (errorName) {
                case "INTERNAL_ERROR": return 500;
                case "NOT_IMPLEMENTED": return 501;
                case "MISSING_PARAMETER": return 400;
                case "INVALID_CLIENT_ID": return 401;
                case "MISSING_SCOPE": return 401;
                case "INVALID_CHANNEL": return 404;
                case "RATE_LIMIT_EXCEEDED": return 429;
                default:
                    Logger.Warn("Unknown BrimeAPI Error: " + errorName);
                    return -1;
            }
        }

        public override bool Equals(object? obj) {
            if (obj == null) return (this == null);
            BrimeAPIError? error = obj as BrimeAPIError;
            return (error == null) ? false : Name.Equals(error.Name);
        }

        public override string ToString() {
            return Name + ": " + Message;
        }

        public override int GetHashCode() {
            return Name.GetHashCode();
        }
    }
}