#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using BrimeAPI.com.brimelive.api.categories;
using BrimeAPI.com.brimelive.api.users;
using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.streams {
    /// <summary>
    /// Identifies details about a stream
    /// </summary>
    public class BrimeStream : JSONConvertable {

        /// <summary>
        /// Stream ID
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// Name of channel streaming
        /// </summary>
        public string ChannelName { get; private set; }

        /// <summary>
        /// Identifies the current streaming category
        /// </summary>
        public BrimeCategory Category { get; private set; }

        /// <summary>
        /// Identifies the current title of the stream
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Identify whether this stream is currently live
        /// </summary>
        public bool IsLive { get; private set; }

        /// <summary>
        /// Identify the time this stream started
        /// </summary>
        public DateTime PublishTime { get; private set; }

        /// <summary>
        /// Identify the available streaming sources
        /// </summary>
        public List<BrimeStreamSource> Streams { get; private set; }

        /// <summary>
        /// Identify the user currently broadcasting this stream
        /// </summary>
        public BrimeUser BroadcastingUser { get; private set; }

        /// <summary>
        /// Identify the URL of the stream thumbnail
        /// </summary>
        public Uri? ThumbnailURL { get; private set; }

        /// <summary>
        /// Create new instance, overriding the loaded category with the provided value
        /// </summary>
        /// <param name="jsonData">JSON data to process</param>
        /// <param name="category">Category to set</param>
        public BrimeStream(JToken jsonData, BrimeCategory category) : this(jsonData) {
            this.Category = category;
        }

        /// <summary>
        /// Create new instance, loading data from the given JSON data
        /// </summary>
        /// <param name="jsonData">JSON data to process</param>
        public BrimeStream(JToken jsonData) {
            string? curr = jsonData.Value<string>("_id");
            if (curr == null) throw new BrimeAPIMalformedResponse("Missing ID on stream details");
            ID = curr;

            curr = jsonData.Value<string>("channel");
            if (curr == null) throw new BrimeAPIMalformedResponse("Missing channel ID on stream details");
            ChannelName = curr;

            JToken? category = jsonData.Value<JToken>("category");
            Category = (category == null) ? new BrimeCategory() : new BrimeCategory(category);

            curr = jsonData.Value<string>("title");
            Title = curr ?? "";

            IsLive = jsonData.Value<bool>("isLive");

            PublishTime = DateTimeOffset.FromUnixTimeMilliseconds(jsonData.Value<Int64>("publishTime")).DateTime;
            
            JArray? streams = jsonData.Value<JArray>("streams");
            if (streams != null) {
                Streams = new List<BrimeStreamSource>(streams.Count);
                foreach (JToken s in streams) {
                    Streams.Add(new BrimeStreamSource(s));
                }
            } else {
                Streams = new List<BrimeStreamSource>();
            }

            JToken? bUser = jsonData.Value<JToken>("broadcastingUser");
            if (bUser == null) throw new BrimeAPIMalformedResponse("Missing Broadcasting user on stream details");
            BroadcastingUser = new BrimeUser(bUser);

            curr = jsonData.Value<string>("streamThumbnailUrl");
            if (curr != null) ThumbnailURL = new Uri(curr);
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
                .Append(PublishTime.Ticks.toJSON("publishTime")).Append(", ")
                .Append(Streams.toJSON<BrimeStreamSource>("streams")).Append(", ")
                .Append(BroadcastingUser.toJSON("broadcastingUser")).Append(", ");
            if (ThumbnailURL != null)
                _result.Append(ThumbnailURL.toJSON("streamThumbnailUrl"));
            _result.Append("}");
            return _result.ToString();
        }

        /// <inheritdoc />
        public override string ToString() {
            return toJSON();
        }
    }

    /// <summary>
    /// Defines the stream details for an individual broadcast resolution
    /// </summary>
    public class BrimeStreamSource : JSONConvertable {

        /// <summary>
        /// Identify the bandwidth of this stream
        /// </summary>
        public int Bandwidth { get; private set; }

        /// <summary>
        /// Identify the resolution for the stream
        /// </summary>
        public string Resolution { get; private set; }

        /// <summary>
        /// Utility method to get X-Resolution
        /// </summary>
        public int ResolutionX {
            get {
                if (_resx == -1) {
                    Match m = Regex.Match(Resolution, "(?<resx>[0-9]*)x(?<resy>[0-9]*)");
                    if (m.Success) {
                        int.TryParse(m.Groups["resx"].Value, out _resx);
                        int.TryParse(m.Groups["resy"].Value, out _resy);
                    }
                }
                return _resx;
            }
        }
        private int _resx = -1;

        /// <summary>
        /// Utility method to get Y-Resolution
        /// </summary>
        public int ResolutionY { 
            get {
                if (_resy == -1) {
                    Match m = Regex.Match(Resolution, "(?<resx>[0-9]*)x(?<resy>[0-9]*)");
                    if (m.Success) {
                        int.TryParse(m.Groups["resx"].Value, out _resx);
                        int.TryParse(m.Groups["resy"].Value, out _resy);
                    }
                }
                return _resy;
            } 
        }
        private int _resy = -1;

        /// <summary>
        /// Identify the video codec
        /// </summary>
        public string VCodec { get; private set; }

        /// <summary>
        /// Identify the audio codec
        /// </summary>
        public string ACodec { get; private set; }

        /// <summary>
        /// Identify the broadcast protocol
        /// </summary>
        public string Protocol { get; private set; }

        /// <summary>
        /// Identify whether this is the original source resolution
        /// </summary>
        public bool IsSource { get; private set; }

        /// <summary>
        /// Create a new instance based on the given JSON data
        /// </summary>
        /// <param name="jsonData">JSON data to process</param>
        public BrimeStreamSource(JToken jsonData) {
            Bandwidth = jsonData.Value<int>("bandwidth");

            string? curr = jsonData.Value<string>("resolution");
            Resolution = curr ?? "";

            curr = jsonData.Value<string>("vcodec");
            VCodec = curr ?? "";

            curr = jsonData.Value<string>("acodec");
            ACodec = curr ?? "";

            curr = jsonData.Value<string>("protocol");
            Protocol = curr ?? "";

            IsSource = jsonData.Value<bool>("isSource");
        }

        /// <summary>
        /// Convert this object to JSON data
        /// </summary>
        /// <returns>JSON encoding of this object</returns>
        public string toJSON() {
            StringBuilder _result = new StringBuilder();
            _result.Append("{")
                .Append(Bandwidth.toJSON("bandwidth")).Append(", ")
                .Append(Resolution.toJSON("resolution")).Append(", ")
                .Append(VCodec.toJSON("vcodec")).Append(", ")
                .Append(ACodec.toJSON("acodec")).Append(", ")
                .Append(Protocol.toJSON("protocol")).Append(", ")
                .Append(IsSource.toJSON("isSource"))
                .Append("}");
            return _result.ToString();
        }

        /// <inheritdoc />
        public override string ToString() {
            return toJSON();
        }
    }
}


/* TODO: Updated structure
"data": {
    "_id": "6050a4804b90bf0430aec1e2",
    "channel": "Geeken",
    "channelId": "6050a4804b90bf0430aec1e2",
    "streamThumbnailUrl": "https://content.brimecdn.com/live/VibeFM/thumbnail.jpg",
    "category": {
      "_id": "606e924e5fa50e5780970134",
      "genres": [],
      "name": "Just Chatting",
      "slug": "just-chatting",
      "summary": "",
      "cover": "https://content.brimecdn.com/brime/category_images/just_chatting.png",
      "type": "entertainment"
    },
    "title": "Geeken's Stream",
    "isLive": true,
    "publishTime": "2021-05-17T10:10:12.000Z",
    "streams": [
      {
        "bandwidth": 7078172,
        "resolution": "1920x1080",
        "vcodec": "avc1.4d4028",
        "acodec": "mp4a.40.2",
        "protocol": "RTMP",
        "isSource": true
      },
      {
        "bandwidth": 6021359,
        "resolution": "1280x720",
        "vcodec": "avc1.42c01f",
        "acodec": "mp4a.40.2",
        "protocol": "RTMP",
        "isSource": false
      },
      {
        "bandwidth": 3557832,
        "resolution": "854x480",
        "vcodec": "avc1.42c01f",
        "acodec": "mp4a.40.2",
        "protocol": "RTMP",
        "isSource": false
      },
      {
        "bandwidth": 1326782,
        "resolution": "426x240",
        "vcodec": "avc1.42c015",
        "acodec": "mp4a.40.2",
        "protocol": "RTMP",
        "isSource": false
      }
    ],
    "broadcastingUser": {
      "_id": "603b901b0a8fe286fc6f1229",
      "username": "geeken",
      "displayname": "Geeken",
      "avatar": "https://content.brimecdn.com/brime/user/603b901b0a8fe286fc6f1229/avatar",
      "badges": [
        "https://beta.brimelive.com/brime_verified.png"
      ],
      "roles": [
        "admin"
      ],
      "color": "#00e025",
      "isBrimePro": false,
      "extendedVodsEnabled": false,
      "channels": [
        "6050a4804b90bf0430aec1e2"
      ]
    }
  }
*/