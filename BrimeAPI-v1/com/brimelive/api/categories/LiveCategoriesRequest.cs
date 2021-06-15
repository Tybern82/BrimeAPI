#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.categories {
    /// <summary>
    /// Queries for all currently live categories (ie where there is a current stream broadcasting in this category)
    /// </summary>
    public class LiveCategoriesRequest : BrimeAPIRequest<List<CategoryStreams>> {

        private static readonly string GET_LIVE_CATEGORIES_REQUEST = "/categories/live"; // /v1/categories/live

        /// <summary>
        /// Create a new instance - no parameters as this queries for all currently live categories
        /// </summary>
        public LiveCategoriesRequest() : base(GET_LIVE_CATEGORIES_REQUEST) { }  // no parameters

        /// <inheritdoc />
        public override List<CategoryStreams> getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            JArray? items = response.Data.Value<JArray>("categories");
            if (items != null) {
                List<CategoryStreams> _result = new List<CategoryStreams>(items.Count);
                foreach (JToken item in items) {
                    if (item != null) _result.Add(new CategoryStreams(item));
                }
                return _result;
            } else {
                return new List<CategoryStreams>();
            }
        }
    }
}
