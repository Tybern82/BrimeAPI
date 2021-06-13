using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrimeAPI.com.brimelive.api.errors;

namespace BrimeAPI.com.brimelive.api.users {
    /// <summary>
    /// Request details about a particular user. NOTE: This request requires special access and is not
    /// available with a regular Client-ID.
    /// </summary>
    public class UserRequest : BrimeAPIRequest<BrimeUser> {

        private static readonly string GET_USER_REQUEST = "/user/{0}";     // /v1/user/:user

        /// <summary>
        /// Records the user name / ID to request
        /// </summary>
        public string UserName { get; private set; }    // Will Accept User ID for query

        /// <summary>
        /// Construct a new request for the given user details
        /// </summary>
        /// <param name="userName">user name / ID to request</param>
        public UserRequest(string userName) : base(GET_USER_REQUEST, true) {
            this.UserName = userName;
            this.RequestParameters = (() => {
                return new string[] { UserName };
            });
        }

        /// <inheritdoc />
        public override BrimeUser getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new BrimeUser(response.Data);
        }
    }
}
