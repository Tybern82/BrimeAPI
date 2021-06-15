#nullable enable

using System.Collections.Generic;
using BrimeAPI.com.brimelive.api.streams;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.categories {
    /// <summary>
    /// Response data for category requests (used by both request for all live categorys, and when
    /// requesting a specific category).
    /// </summary>
    public class CategoryStreams {
        /// <summary>
        /// Identify the category for the provided streams
        /// </summary>
        public BrimeCategory Category { get; private set; }

        /// <summary>
        /// List of streams currently live in this category
        /// </summary>
        public List<BrimeStream> Streams { get; private set; }

        /// <summary>
        /// Process given response to request
        /// </summary>
        /// <param name="jsonData">JSON data to process</param>
        public CategoryStreams(JToken jsonData) {
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
