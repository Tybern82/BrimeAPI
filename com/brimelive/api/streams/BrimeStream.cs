using System;
using System.Collections.Generic;
using System.Text;
using BrimeAPI.com.brimelive.api.categories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.streams {
    public class BrimeStream {

        public string ID { get; private set; }
        public string ChannelName { get; private set; }
        public BrimeCategory Category { get; private set; }
        public string Title { get; private set; }
        public bool IsLive { get; private set; }
        public DateTime PublishTime { get; private set; }
        public int Bandwidth { get; private set; }
        public List<string> Resolutions { get; private set; } = new List<string>();
        public string VCodec { get; private set; }
        public string ACodec { get; private set; }
        public string Protocol { get; private set; }

        public BrimeStream(JToken jsonData, BrimeCategory category) {
            string? curr = jsonData.Value<string>("_id");
            ID = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("channel");
            ChannelName = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("title");
            Title = (curr == null) ? "" : curr;

            IsLive = jsonData.Value<bool>("isLive");

            DateTime? dt = jsonData.Value<DateTime>("publishTime");
            if (dt == null) {
                PublishTime = DateTime.Now;
            } else {
                PublishTime = (DateTime)dt;
            }

            // curr = jsonData.Value<string>("publishTime");
            // PublishTime = (curr == null) ? DateTime.UtcNow : DateTime.Parse(curr);

            Bandwidth = jsonData.Value<int>("bandwidth");

            JArray? resolutions = jsonData.Value<JArray>("resolutions");
            if (resolutions != null) {
                foreach (string? s in resolutions) {
                    if (s != null) Resolutions.Add(s);
                }
            }

            curr = jsonData.Value<string>("vcodec");
            VCodec = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("acodec");
            ACodec = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("protocol");
            Protocol = (curr == null) ? "" : curr;
            this.Category = category;
        }

        public BrimeStream(JToken jsonData) {
            string? curr = jsonData.Value<string>("_id");
            ID = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("channel");
            ChannelName = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("title");
            Title = (curr == null) ? "" : curr;

            IsLive = jsonData.Value<bool>("isLive");

            curr = jsonData.Value<string>("publishTime");
            PublishTime = (curr == null) ? DateTime.UtcNow : DateTime.Parse(curr);

            Bandwidth = jsonData.Value<int>("bandwidth");

            JArray? resolutions = jsonData.Value<JArray>("resolutions");
            if (resolutions != null) {
                foreach (string? s in resolutions) {
                    if (s != null) Resolutions.Add(s);
                }
            }

            curr = jsonData.Value<string>("vcodec");
            VCodec = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("acodec");
            ACodec = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("protocol");
            Protocol = (curr == null) ? "" : curr;

            JToken? category = jsonData.Value<JToken>("category");
            Category = (category == null) ? new BrimeCategory() : new BrimeCategory(category);
        }

        public override string ToString() {
            string format = "{{" +
                "\"_id\": {0}," +
                "\"channel\": {1}," +
                "\"category\": {2}," +
                "\"title\": {3}," +
                "\"isLive\": {4}," +
                "\"publishTime\": {5}," +
                "\"bandwidth\": {6}," +
                "\"resolutions\": {7}," +
                "\"vcodec\": {8}," +
                "\"acodec\": {9}," +
                "\"protocol\": {10}" +
                "}}";
            return string.Format(format,
                JsonConvert.ToString(ID),
                JsonConvert.ToString(ChannelName),
                Category.ToString(),
                JsonConvert.ToString(Title),
                JsonConvert.ToString(IsLive),
                JsonConvert.ToString(PublishTime.ToUniversalTime().ToString("yyyy-mm-ddTHH:mm:ss.fffZ")),
                JsonConvert.ToString(Bandwidth),
                JSONUtil.ToString(Resolutions),
                JsonConvert.ToString(VCodec),
                JsonConvert.ToString(ACodec),
                JsonConvert.ToString(Protocol)
                );
        }
    }
}

/*
    "streams":
      {
        "_id": "6050a4804b90bf0430aec1e2",
        "channel": "geeken",
        "category": {
          "_id": "606e93525fa50e5780970135",
          "genres": [],
          "name": "Uncategorized",
          "slug": "uncategorized",
          "summary": "",
          "cover": "",
          "type": "entertainment"
        },
        "title": "It's BrimeTime™!",
        "isLive": true,
        "publishTime": "2021-03-26T19:25:53.000Z",
        "bandwidth": 9154256,
        "resolutions": [
          "1280x720"
        ],
        "vcodec": "avc1.64001f",
        "acodec": "mp4a.40.2",
        "protocol": "RTMP"
      }
*/