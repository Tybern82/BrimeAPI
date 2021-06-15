#nullable enable

using BrimeAPI.com.brimelive.api.errors;

namespace BrimeAPI.com.brimelive.api.categories {
    /// <summary>
    /// Query for streams currently live in a specific category
    /// </summary>
    public class LivestreamsByCategoryRequest : BrimeAPIRequest<CategoryStreams> {
        private static readonly string GET_LIVESTREAM_BY_CATEGORY_REQUEST = "/category/{0}/live";    // /v1/category/:category/live

        /// <summary>
        /// Category to request live streams from (can be specified by Name, ID or Slug)
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Create a new request for the given category
        /// </summary>
        /// <param name="category">name, ID or slug of category to request</param>
        public LivestreamsByCategoryRequest(string category) : base(GET_LIVESTREAM_BY_CATEGORY_REQUEST) {
            this.Category = category;
            this.RequestParameters = (() => {
                return new string[] { Category };
            });
        }

        /// <inheritdoc />
        public override CategoryStreams getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new CategoryStreams(response.Data);
        }
    }
}
