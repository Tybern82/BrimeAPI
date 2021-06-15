#nullable enable

using System.Text;
using BrimeAPI.com.brimelive.api.categories;
using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.streams {

    /// <summary>
    /// Contains the stream information for the stream where the clip was taken
    /// </summary>
    public class StreamDetails : JSONConvertable {
        /// <summary>
        /// Category the original stream was broadcast under
        /// </summary>
        public BrimeCategory Category { get; private set; }

        /// <summary>
        /// Original title of the stream
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Create a new instance based on the given JSON data
        /// </summary>
        /// <param name="jsonData">JSON data to process</param>
        public StreamDetails(JToken jsonData) {
            string? curr = jsonData.Value<string>("title");
            if (curr == null) throw new BrimeAPIMalformedResponse("Missing title in stream information for clip");
            Title = curr;

            JToken? category = jsonData.Value<JToken>("category");
            if (category == null) throw new BrimeAPIMalformedResponse("Missing category in stream information for clip");
            Category = new BrimeCategory(category);
        }

        /// <inheritdoc />
        public string toJSON() {
            StringBuilder _result = new StringBuilder();
            _result.Append("{")
                .Append(Title.toJSON("title")).Append(", ")
                .Append(Category.toJSON("category"))
                .Append("}");
            return _result.ToString();
        }

        /// <inheritdoc />
        public override string ToString() {
            return toJSON();
        }
    }
}
