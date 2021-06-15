#nullable enable

using System.Collections.Generic;
using System.Text;
using BrimeAPI.com.brimelive.api.categories;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.channels {
    /// <summary>
    /// Identifies a channel on Brime
    /// </summary>
    public class BrimeChannel {

        /// <summary>
        /// Unique ID for this channel
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// Name of the channel
        /// </summary>
        public string ChannelName { get; private set; }

        /// <summary>
        /// Currently assigned category for this channel
        /// </summary>
        public BrimeCategory Category { get; private set; }

        /// <summary>
        /// Currently assigned title for this channel
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Identify whether this channel is currently live
        /// </summary>
        public bool IsLive { get; private set; }

        /// <summary>
        /// Identify current follower count for this channel
        /// </summary>
        public int FollowerCount { get; private set; }

        /// <summary>
        /// Identify the current subscriber count for this channel
        /// </summary>
        public int SubscriberCount { get; private set; }

        /// <summary>
        /// Identify the currently set description for this channel
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Identify the user(s) who own this channel
        /// </summary>
        public List<string> Owners { get; private set; }

        /// <summary>
        /// Create a new instance using the given JSON data
        /// </summary>
        /// <param name="jsonData">JSON data to process</param>
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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