using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.users {

    /// <summary>
    /// Request Brime for Follower details. This request requires special access and will not 
    /// be available for a standard Client-ID.
    /// </summary>
    public class UserFollowingRequest : BrimeAPIRequest<UserFollowingResponse> {

        private static readonly string GET_USER_FOLLOWING_REQUEST = "/user/{0}/following";     // /v1/user/:user/following?live_only=true

        /// <summary>
        /// Name of user to retrieve follower details.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Specifiy whether to only include Live results.
        /// </summary>
        public bool LiveOnly { get; set; } = false;

        /// <summary>
        /// Create a new request instance.
        /// </summary>
        /// <param name="userName">Identifies which user to request information on</param>
        public UserFollowingRequest(string userName) : base(GET_USER_FOLLOWING_REQUEST, true) {
            this.UserName = userName;
            this.RequestParameters = (() => {
                return new string[] { UserName };
            });
            this.QueryParameters = (() => {
                return new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>("live_only", LiveOnly.ToString())
                };
            });
        }

        /// <inheritdoc />
        public override UserFollowingResponse getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new UserFollowingResponse(response.Data);
        }
    }

    /// <summary>
    /// Defines the response from the UserFollowingRequest.
    /// </summary>
    public class UserFollowingResponse {

        /// <summary>
        /// Create a new instance based on the given response data
        /// </summary>
        /// <param name="jsonData">JSON response from server</param>
        public UserFollowingResponse(JToken jsonData) {
            // TODO: No example structure provided in Public API docs
        }
    }
}
