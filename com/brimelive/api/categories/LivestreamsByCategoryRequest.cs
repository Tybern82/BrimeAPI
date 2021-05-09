#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using BrimeAPI.com.brimelive.api.streams;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.categories {
    public class LivestreamsByCategoryRequest : BrimeAPIRequest<LivestreamsByCategoryResponse> {
        private static readonly string GET_LIVESTREAM_BY_CATEGORY_REQUEST = "/v1/category/{0}/live";    // /v1/category/:category/live

        public string Category { get; set; }  // can be Name, ID, or Slug
        public LivestreamsByCategoryRequest(string category) : base(GET_LIVESTREAM_BY_CATEGORY_REQUEST) {
            this.Category = category;
            this.RequestParameters = (() => {
                return new string[] { Category };
            });
        }

        public override LivestreamsByCategoryResponse getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            return new LivestreamsByCategoryResponse(response);
        }
    }

    public class LivestreamsByCategoryResponse {

        public BrimeCategory Category { get; private set; }
        public List<BrimeStream> Streams { get; private set; }

        public LivestreamsByCategoryResponse(BrimeCategory category, List<BrimeStream> streams) {
            this.Category = category;
            this.Streams = streams;
        }

        public LivestreamsByCategoryResponse(BrimeAPIResponse response) : this(response.Data) {}

        public LivestreamsByCategoryResponse(JToken jsonData) {
            JToken? tCategory = jsonData.Value<JToken>("category");
            BrimeCategory? category = (tCategory == null) ? null : new BrimeCategory(tCategory);
            JArray? streams = jsonData.Value<JArray>("streams");
            if (streams != null) {
                this.Streams = new List<BrimeStream>(streams.Count);
                foreach (JToken item in streams) {
                    if (category != null)
                        Streams.Add(new BrimeStream(item, category));
                    else
                        Streams.Add(new BrimeStream(item));
                }
            } else {
                this.Streams = new List<BrimeStream>();
            }
            this.Category = category ?? new BrimeCategory();
        }
    }
}
