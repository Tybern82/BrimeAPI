#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.categories {
    /// <summary>
    /// Identify the category for the stream
    /// </summary>
    public class BrimeCategory : JSONConvertable {

        /// <summary>
        /// Category Identifier
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// IGDB number
        /// </summary>
        public int IGDB { get; private set; }

        /// <summary>
        /// List of genres associated with this category
        /// </summary>
        public List<string> Genres { get; private set; } = new List<string>();

        /// <summary>
        /// Display name for this category
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Identifying slug for this category
        /// </summary>
        public string Slug { get; private set; }

        /// <summary>
        /// Provides a short description of this category
        /// </summary>
        public string Summary { get; private set; }

        /// <summary>
        /// Identify cover-art for this category
        /// </summary>
        public Uri? CoverURL { get; private set; }

        /// <summary>
        /// Identify the type of Category
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Construct a new instance, using the provided JSON data
        /// </summary>
        /// <param name="jsonData">JSON to load category from</param>
        public BrimeCategory(JToken jsonData) {
            string? curr = jsonData.Value<string>("_id");
            if (curr == null) throw new BrimeAPIMalformedResponse("Missing ID in Category");
            ID = curr;

            if (jsonData.HasValue("igdb_id")) {
                IGDB = jsonData.Value<int>("igdb_id");
            } else {
                IGDB = -1;
            }

            JArray? genres = jsonData.Value<JArray>("genres");
            if (genres != null) {
                foreach (JToken? i in genres) {
                    if (i != null) {
                        string? item = i.Value<string>();
                        if (item != null) Genres.Add(item);
                    }
                }
            }

            curr = jsonData.Value<string>("name");
            if (curr == null) throw new BrimeAPIMalformedResponse("Missing Name in Category");
            Name = curr;

            curr = jsonData.Value<string>("slug");
            if (curr == null) throw new BrimeAPIMalformedResponse("Missing Slug in Category");
            Slug = curr;

            curr = jsonData.Value<string>("summary");
            Summary = curr ?? "";

            curr = jsonData.Value<string>("cover");
            if (string.IsNullOrWhiteSpace(curr)) {
                CoverURL = null;
            } else {
                try {
                    CoverURL = new Uri(curr);
                } catch (Exception e) when (e is ArgumentNullException || e is UriFormatException) {
                    throw new BrimeAPIMalformedResponse("Invalid Cover URL in Category", e);
                }
            }

            curr = jsonData.Value<string>("type");
            Type = curr ?? "";
        }

        /// <summary>
        /// Create a new instance, providing empty/default parameters
        /// </summary>
        public BrimeCategory() {
            ID = "";
            IGDB = -1;
            Name = "";
            Slug = "";
            Summary = "";
            CoverURL = new Uri("./");
            Type = "";
        }

        /// <summary>
        /// Convert this object to JSON data
        /// </summary>
        /// <returns>JSON encoding of this object</returns>
        public string toJSON() {
            StringBuilder _result = new StringBuilder();
            _result.Append("{")
                .Append(ID.toJSON("_id")).Append(", ")
                .Append(Genres.toJSON<string>("genres")).Append(", ")
                .Append(Name.toJSON("name")).Append(", ")
                .Append(Slug.toJSON("slug")).Append(", ")
                .Append(Summary.toJSON("summary")).Append(", ");
            if (CoverURL != null) {
                _result.Append(CoverURL.toJSON("cover")).Append(", ");
            }
            _result.Append(Type.toJSON("type"));
            if (IGDB != -1) _result.Append(", ").Append(IGDB.toJSON("igdb_id"));
            _result.Append("}");
            return _result.ToString();
        }

        /// <inheritdoc />
        public override string ToString() => toJSON();
    }
}

/*
"data": {
    "_id": "606e89aceb4c916b7435b963",
    "igdb_id": 358,
    "genres": [
      "60568f1d5631c93404fe2ef1",
      "60568f1d5631c93404fe2efe"
    ],
    "name": "Super Mario Bros.",
    "slug": "super-mario-bros",
    "summary": "A side scrolling 2D platformer and first entry in the Super Mario franchise, Super Mario Bros. follows Italian plumber Mario as he treks across many levels of platforming challenges featuring hostile enemies to rescue Princess Peach from the evil king Bowser.",
    "cover": "https://images.igdb.com/igdb/image/upload/t_1080p/co2362.png",
    "type": "videogame"
  }
*/