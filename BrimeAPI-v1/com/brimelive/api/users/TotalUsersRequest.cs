using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrimeAPI.com.brimelive.api.errors;

namespace BrimeAPI.com.brimelive.api.users {
    /// <summary>
    /// Retrieve the total number of users currently registered on the platform.
    /// {"data":{"total": [value] }}
    /// </summary>
    public class TotalUsersRequest : BrimeAPIRequest<int> {

        private static readonly string GET_USERS_REQUEST = "/users"; ///v1/users
        
        /// <summary>
        /// Create a new instance. No parameters required for this request.
        /// </summary>
        public TotalUsersRequest() : base(GET_USERS_REQUEST) { }        // No parameters

        /// <inheritdoc />
        public override int getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return response.Data.Value<int>("total");
        }
    }
}
