#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using BrimeAPI.com.brimelive.api.errors;

namespace BrimeAPI.com.brimelive.api.categories {
    /// <summary>
    /// Gets information for a specific category.
    /// </summary>
    public class CategoryRequest : BrimeAPIRequest<BrimeCategory> {
        /// <summary>
        /// [API]/v1/category/:category
        /// </summary>
        private static readonly string GET_CATEGORY_REQUEST = "/v1/category/{0}";  // /v1/category/:category

        /// <summary>
        /// Specify the Category to request by either Name, ID, or Slug.
        /// </summary>
        public string Category { get; set; }  // can be Name, ID, or Slug

        /// <summary>
        /// <c>CategoryRequest</c> constructor. Requires parameter specifying the category to get information from.
        /// </summary>
        /// <param name="category">The category name, id, or slug.</param>
        public CategoryRequest(string category) : base (GET_CATEGORY_REQUEST) {
            this.Category = category;
            this.RequestParameters = (() => {
                return new string[] { Category };
            });
        }

        /// <summary>
        /// Generate BrimeCategory from API response.
        /// </summary>
        /// <returns><c>BrimeCategory</c> for the requested category name/id/slug.</returns>
        /// <exception cref="BrimeAPIMalformedResponse">If there is an error in the returned Category.</exception>
        /// <seealso cref="BrimeAPIRequest{ResponseType}.getResponse"/>
        public override BrimeCategory getResponse() {
            // Trigger the request and process the JSON response.
            BrimeAPIResponse response = doRequest();

            // Trigger exceptions for any detected errors in the response.
            BrimeAPIError.ThrowException(response);

            // Generate the new Category from the response.
            return new BrimeCategory(response.Data);
        }
    }
}
