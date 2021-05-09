#nullable enable

using System;
using System.Collections.Generic;
using System.Text;

namespace BrimeAPI.com.brimelive.api.categories {
    public class CategoryRequest : BrimeAPIRequest<BrimeCategory> {

        private static readonly string GET_CATEGORY_REQUEST = "/v1/category/{0}";  // /v1/category/:category

        public string Category { get; set; }  // can be Name, ID, or Slug

        public CategoryRequest(string category) : base (GET_CATEGORY_REQUEST) {
            this.Category = category;
            this.RequestParameters = (() => {
                return new string[] { Category };
            });
        }

        public override BrimeCategory getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new BrimeCategory(response.Data);
        }
    }
}
