#nullable enable

using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.users {
    /// <summary>
    /// Query for clips associated with a given user.
    /// </summary>
    public class UserClipsRequest : BrimeAPIRequest<UserClipsResponse> {

        private static readonly string GET_CLIPS_FOR_USER_REQUEST = "/user/{0}/clips"; // /v1/user/:user/clips

        /// <summary>
        /// Name of user to retrieve clips from
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Create a new request for the given user
        /// </summary>
        /// <param name="userName">name of user to retrieve clips from</param>
        public UserClipsRequest(string userName) : base(GET_CLIPS_FOR_USER_REQUEST, true) {
            this.UserName = userName;
            this.RequestParameters = (() => {
                return new string[] { UserName };
            });
        }

        /// <inheritdoc />
        public override UserClipsResponse getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new UserClipsResponse(response.Data);
        }
    }

    /// <summary>
    /// Placeholder class for response from user clip request.
    /// </summary>
    public class UserClipsResponse {

        /// <summary>
        /// Create a new instance using the given JSON data
        /// </summary>
        /// <param name="jsonData">JSON data to process</param>
        public UserClipsResponse(JToken jsonData) {
            // TODO: No sample response provide in Public API docs
        }
    }
}
