#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.categories {
    public class BrimeCategory {

        public string ID { get; private set; }
        public int IGDB { get; private set; }

        public List<string> Genres { get; private set; } = new List<string>();
        public string Name { get; private set; }
        public string Slug { get; private set; }
        public string Summary { get; private set; }
        public Uri CoverURL { get; private set; }
        public string Type { get; private set; }

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
            Summary = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("cover");
            if (curr == null) throw new BrimeAPIMalformedResponse("Missing Cover URL in Category");
            try {
                CoverURL = new Uri(curr);
            } catch (Exception e) when (e is ArgumentNullException || e is UriFormatException) {
                throw new BrimeAPIMalformedResponse("Invalid Cover URL in Category", e);
            }

            curr = jsonData.Value<string>("type");
            Type = (curr == null) ? "" : curr;
        }

        public BrimeCategory() {
            ID = "";
            IGDB = -1;
            Name = "";
            Slug = "";
            Summary = "";
            CoverURL = new Uri("./");
            Type = "";
        }

        public override string ToString() {            
            List<string> items = new List<string>(8);   // up to 8 items in list
            items.Add(ID.ToJSONString().ToJSONEntry("_id"));
            if (IGDB != -1) items.Add(IGDB.ToJSONString().ToJSONEntry("igdb_id"));
            items.Add(Genres.ToJSONString().ToJSONEntry("genres"));
            items.Add(Name.ToJSONString().ToJSONEntry("name"));
            items.Add(Slug.ToJSONString().ToJSONEntry("slug"));
            items.Add(Summary.ToJSONString().ToJSONEntry("summary"));
            items.Add(CoverURL.ToJSONString().ToJSONEntry("cover"));
            items.Add(Type.ToJSONString().ToJSONEntry("type"));
            return items.TOJSONEntry();

            /*            
             string format = "{{" +
                            "\"_id\": {0}," +
                            "\"igdb_id\": {1}," +
                            "\"genres\": {2}," +
                            "\"name\": {3}," +
                            "\"slug\": {4}," +
                            "\"summary\": {5}," +
                            "\"cover\": {6}," +
                            "\"type\": {7}" +
                            "}}";
            return string.Format(format,
                            ID.ToJSONString(),
                            IGDB.ToJSONString(),
                            Genres.ToJSONString(),
                            Name.ToJSONString(),
                            Slug.ToJSONString(),
                            Summary.ToJSONString(),
                            CoverURL.ToJSONString(),
                            Type.ToJSONString()); 
            */
        }
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