using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.users {
    public class UserFollowingRequest : BrimeAPIRequest<UserFollowingResponse> {

        private static readonly string GET_USER_FOLLOWING_REQUEST = "/v1/user/{0}/following";     // /v1/user/:user/following

        public string UserName { get; private set; }

        public UserFollowingRequest(string userName) : base(GET_USER_FOLLOWING_REQUEST) {
            this.UserName = userName;
            this.RequestParameters = (() => {
                return new string[] { UserName };
            });
        }

        public override UserFollowingResponse getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new UserFollowingResponse(response.Data);
        }
    }

    public class UserFollowingResponse {

        public UserFollowingResponse(JToken jsonData) {
            // TODO: No example structure provided in Public API docs
        }
    }
}
