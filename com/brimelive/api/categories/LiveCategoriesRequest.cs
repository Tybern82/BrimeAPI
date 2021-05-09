#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.categories {
    public class LiveCategoriesRequest : BrimeAPIRequest<List<LivestreamsByCategoryResponse>> {

        private static readonly string GET_LIVE_CATEGORIES_REQUEST = "/v1/categories/live"; // /v1/categories/live

        public LiveCategoriesRequest() : base(GET_LIVE_CATEGORIES_REQUEST) { }  // no parameters

        public override List<LivestreamsByCategoryResponse> getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            JArray? items = response.Data.Value<JArray>("categories");
            if (items != null) {
                List<LivestreamsByCategoryResponse> _result = new List<LivestreamsByCategoryResponse>(items.Count);
                foreach (JToken item in items) {
                    if (item != null) _result.Add(new LivestreamsByCategoryResponse(item));
                }
                return _result;
            } else {
                return new List<LivestreamsByCategoryResponse>();
            }
        }
    }
}
