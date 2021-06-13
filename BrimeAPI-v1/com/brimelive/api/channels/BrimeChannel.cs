#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrimeAPI.com.brimelive.api.categories;
using BrimeAPI.com.brimelive.api.users;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.channels {
    public class BrimeChannel {

        public string ID { get; private set; }
        public string ChannelName { get; private set; }
        public BrimeCategory Category { get; private set; }
        public string Title { get; private set; }
        public bool IsLive { get; private set; }
        public int FollowerCount { get; private set; }
        public int SubscriberCount { get; private set; }
        public string Description { get; private set; }
        public List<string> Owners { get; private set; }

        public BrimeChannel(JToken jsonData) {
            string? curr = jsonData.Value<string>("_id");
            ID = curr ?? "";

            curr = jsonData.Value<string>("channel");
            ChannelName = curr ?? "";

            JToken? category = jsonData.Value<JToken>("category");
            Category = (category == null) ? new BrimeCategory() : new BrimeCategory(category);

            curr = jsonData.Value<string>("title");
            Title = curr ?? "";

            IsLive = jsonData.Value<bool>("isLive");
            FollowerCount = jsonData.Value<int>("followerCount");
            SubscriberCount = jsonData.Value<int>("subscriberCount");

            curr = jsonData.Value<string>("description");
            Description = curr ?? "";

            JArray? owners = jsonData.Value<JArray>("owners");
            if (owners != null) {
                Owners = new List<string>(owners.Count);
                foreach (string? item in owners) {
                    if (item != null) Owners.Add(item);
                }
            } else {
                Owners = new List<string>();
            }
        }

        public string toJSON() {
            StringBuilder _result = new StringBuilder();
            _result.Append("{")
                .Append(ID.toJSON("_id")).Append(", ")
                .Append(ChannelName.toJSON("channel")).Append(", ")
                .Append(Category.toJSON("category")).Append(", ")
                .Append(Title.toJSON("title")).Append(", ")
                .Append(IsLive.toJSON("isLive")).Append(", ")
                .Append(FollowerCount.toJSON("followerCount")).Append(", ")
                .Append(SubscriberCount.toJSON("subscriberCount")).Append(", ")
                .Append(Description.toJSON("description")).Append(", ")
                .Append(Owners.toJSON<string>("owners"))
                .Append("}");
            return _result.ToString();
        }

        public override string ToString() {
            return toJSON();
        }
    }
}

/*
"data": {
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
    "title": "Gaming, then back to dev.",
    "isLive": false,
    "followerCount": 73,
    "subscriberCount": -1,
    "description": "",
// Apparently changed to "owners": [ "603b901b0a8fe286fc6f1229" ]
    "owners": [
      {
        "_id": "603b901b0a8fe286fc6f1229",
        "username": "geeken",
        "avatar": "https://i.ibb.co/fMF9pQJ/Doge-Tri-Poloski.png",
        "badges": [
          "https://beta.brimelive.com/brime_verified.png"
        ],
        "roles": [
          "admin"
        ]
      }
    ]
  }
*/