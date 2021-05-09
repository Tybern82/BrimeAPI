#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
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
        public string CoverURL { get; private set; }
        public string Type { get; private set; }

        public BrimeCategory(JToken jsonData) {
            string? curr = jsonData.Value<string>("_id");
            ID = (curr == null) ? "" : curr;

            IGDB = jsonData.Value<int>("igdb_id");

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
            Name = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("slug");
            Slug = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("summary");
            Summary = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("cover");
            CoverURL = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("type");
            Type = (curr == null) ? "" : curr;
        }

        public BrimeCategory() {
            ID = "";
            IGDB = 0;
            Name = "";
            Slug = "";
            Summary = "";
            CoverURL = "";
            Type = "";
        }

        public override string ToString() {
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
                JsonConvert.ToString(ID),
                JsonConvert.ToString(IGDB),
                JSONUtil.ToString(Genres),
                JsonConvert.ToString(Name),
                JsonConvert.ToString(Slug),
                JsonConvert.ToString(Summary),
                JsonConvert.ToString(CoverURL),
                JsonConvert.ToString(Type));
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