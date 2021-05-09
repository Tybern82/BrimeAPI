using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrimeAPI.com.brimelive.api.users {
    public class TotalUsersRequest : BrimeAPIRequest<int> {

        private static readonly string GET_USERS_REQUEST = "/v1/users"; ///v1/users

        public TotalUsersRequest() : base(GET_USERS_REQUEST) { }        // No parameters

        public override int getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return response.Data.Value<int>("total");
        }
    }
}
