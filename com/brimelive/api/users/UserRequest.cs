using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrimeAPI.com.brimelive.api.users {
    public class UserRequest : BrimeAPIRequest<BrimeUser> {
        /*
         * (REQUIRES SPECIAL ACCESS)
         */

        private static readonly string GET_USER_REQUEST = "/v1/user/{0}";     // /v1/user/:user

        public string UserName { get; private set; }    // Will Accept User ID for query

        public UserRequest(string userName) : base(GET_USER_REQUEST, true) {
            this.UserName = userName;
            this.RequestParameters = (() => {
                return new string[] { UserName };
            });
        }

        public override BrimeUser getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new BrimeUser(response.Data);
        }
    }
}
