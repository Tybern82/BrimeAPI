using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.users {
    public class UserClipsRequest : BrimeAPIRequest<UserClipsResponse> {

        private static readonly string GET_CLIPS_FOR_USER_REQUEST = "/v1/user/{0}/clips"; // /v1/user/:user/clips

        public string UserName { get; private set; }

        public UserClipsRequest(string userName) : base(GET_CLIPS_FOR_USER_REQUEST) {
            this.UserName = userName;
            this.RequestParameters = (() => {
                return new string[] { UserName };
            });
        }

        public override UserClipsResponse getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new UserClipsResponse(response.Data);
        }
    }

    public class UserClipsResponse {

        public UserClipsResponse(JToken jsonData) {
            // TODO: No sample response provide in Public API docs
        }
    }
}
