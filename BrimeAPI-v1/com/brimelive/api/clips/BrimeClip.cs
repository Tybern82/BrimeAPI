#nullable enable

using System;
using System.Text;
using BrimeAPI.com.brimelive.api.errors;
using BrimeAPI.com.brimelive.api.streams;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.clips {
    /// <summary>
    /// Identifies a single Clip made on a channel
    /// </summary>
    public class BrimeClip : JSONConvertable {

        /// <summary>
        /// Unique identifier for this clip
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// URL used to retrieve this clip
        /// </summary>
        public Uri URL { get; private set; }

        /// <summary>
        /// Name given to this clip
        /// </summary>
        public string ClipName { get; private set; }

        /// <summary>
        /// URL to the video for this clip
        /// </summary>
        public Uri ClipVideoURL { get; private set; }

        /// <summary>
        /// URL to a thumbnail for this clip
        /// </summary>
        public Uri ClipThumbnailURL { get; private set; }

        /// <summary>
        /// Date this clip was created
        /// </summary>
        public DateTime ClipDate { get; private set; }

        /// <summary>
        /// ID of the channel where this clip was taken from
        /// </summary>
        public string ChannelID { get; private set; }

        /// <summary>
        /// Information about the original stream being clipped
        /// </summary>
        public StreamDetails Stream { get; private set; }

        /// <summary>
        /// Timestamp of the start of this clip
        /// </summary>
        public DateTime SectionStart { get; private set; }

        /// <summary>
        /// Timestamp of the end of this clip
        /// </summary>
        public DateTime SectionEnd { get; private set; }

        /// <summary>
        /// Counter of the number of upvotes on this clip
        /// </summary>
        public int Upvotes { get; private set; }

        /// <summary>
        /// Create a new instance, processing the given response data
        /// </summary>
        /// <param name="jsonData">JSON response to process</param>
        public BrimeClip(JToken jsonData) {
            string? curr = jsonData.Value<string>("_id");
            if (curr == null) throw new BrimeAPIMalformedResponse("Missing _id on clip response");
            ID = curr;

            curr = jsonData.Value<string>("url");
            if (curr == null) throw new BrimeAPIMalformedResponse("Missing URL in clip response");
            URL = new Uri(curr);

            curr = jsonData.Value<string>("clipName");
            if (curr == null) throw new BrimeAPIMalformedResponse("Missing name for clip in response");
            ClipName = curr;

            curr = jsonData.Value<string>("clipVideoUrl");
            if (curr == null) throw new BrimeAPIMalformedResponse("Missing Video URL in clip response");
            ClipVideoURL = new Uri(curr);

            curr = jsonData.Value<string>("clipThumbnailUrl");
            if (curr == null) throw new BrimeAPIMalformedResponse("Missing Thumbnail URL in clip response");
            ClipThumbnailURL = new Uri(curr);

            curr = jsonData.Value<string>("channelID");
            if (curr == null) throw new BrimeAPIMalformedResponse("Missing Channel ID in clip response");
            ChannelID = curr;

            JToken? stream = jsonData.Value<JToken>("stream");
            if (stream == null) throw new BrimeAPIMalformedResponse("Missing stream information in clip response");

            Stream = new StreamDetails(stream);

            Upvotes = jsonData.Value<int>("upvotes");

            // TODO: Update when timestamp changes to milliseconds
            // Ensure toJSON is also updated

            long clipDate = jsonData.Value<long>("clipDate");
            ClipDate = DateTimeOffset.FromUnixTimeSeconds(clipDate).DateTime;
            // ClipDate = DateTimeOffset.FromUnixTimeMilliseconds(clipDate).DateTime;


            long sectionStart = jsonData.Value<long>("sectionStart");
            SectionStart = DateTimeOffset.FromUnixTimeSeconds(sectionStart).DateTime;
            // SectionStart = DateTimeOffset.FromUnixTimeMilliseconds(sectionStart).DateTime;

            long sectionEnd = jsonData.Value<long>("sectionEnd");
            SectionEnd = DateTimeOffset.FromUnixTimeSeconds(sectionEnd).DateTime;
            // SectionEnd = DateTimeOffset.FromUnixTimeMilliseconds(sectionEnd).DateTime;

            Upvotes = jsonData.Value<int>("upvotes");
        }

        /// <inheritdoc />
        public string toJSON() {
            StringBuilder _result = new StringBuilder();
            _result.Append("{")
                .Append(ID.toJSON("_id")).Append(", ")
                .Append(URL.AbsoluteUri.toJSON("url")).Append(", ")
                .Append(ClipName.toJSON("clipName")).Append(", ")
                .Append(ClipVideoURL.AbsoluteUri.toJSON("clipVideoUrl")).Append(", ")
                .Append(ClipThumbnailURL.AbsoluteUri.toJSON("clipThumbnailUrl")).Append(", ")
                .Append(new DateTimeOffset(ClipDate).ToUnixTimeSeconds().toJSON("clipDate")).Append(", ")
                // .Append(new DateTimeOffset(ClipDate).ToUnixTimeMilliseconds().toJSON("clipDate"))
                .Append(ChannelID.toJSON("channelID")).Append(", ")
                .Append(Stream.toJSON("stream")).Append(", ")
                .Append(new DateTimeOffset(SectionStart).ToUnixTimeSeconds().toJSON("sectionStart")).Append(", ")
                .Append(new DateTimeOffset(SectionEnd).ToUnixTimeSeconds().toJSON("sectionEnd")).Append(", ")
                // .Append(new DateTimeOffset(SectionStart).ToUnixTimeMilliseconds().toJSON("sectionStart"))
                // .Append(new DateTimeOffset(SectionEnd).ToUnixTimeMilliseconds().toJSON("sectionEnd"))
                .Append(Upvotes.toJSON("upvotes"))
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
    "_id": "6069f53185fa36ba0baad898",
    "url": "https://beta.brimelive.com/clip/6069f53185fa36ba0baad898",
    "clipName": "Clipped by llamafun",
    "clipVideoUrl": "https://content.brimecdn.com/brime/clip/6050a4804b90bf0430aec1e2/6069f53185fa36ba0baad898",
    "clipDate": 1617556786,
    "channelID": "6050a4804b90bf0430aec1e2",
    "stream": {
      "category": "BRIME™ OFFICIAL",
      "title": "Chatting with Brimer / Dev Updates"
    },
    "sectionStart": 1617556756,
    "sectionEnd": 1617556786,
    "clipperID": "605a58d321489a6c4a78709d",
    "clipper": "llamafun"
  }

{
  "data": {
    "_id": "607e67f1fb94b3cba84e80fb",
    "url": "https://clips.brimelive.com/607e67f1fb94b3cba84e80fb",
    "clipName": "Clipped by ItzLcyx",
    "clipDate": 1618896882,
    "clipVideoUrl": "https://content.brimecdn.com/brime/clip/6050a4804b90bf0430aec1e2/607e67f1fb94b3cba84e80fb",
    "clipThumbnailUrl": "https://content.brimecdn.com/brime/clip/6050a4804b90bf0430aec1e2/607e67f1fb94b3cba84e80fb/thumbnail",
    "channelID": "6050a4804b90bf0430aec1e2",
    "stream": {
      "title": "EXPERIMENTAL 120 FPS Stream",
      "category": {
        "_id": "606e8e43eb4c916b74360542",
        "igdb_id": 126459,
        "genres": [ "60568f1d5631c93404fe2eef", "60568f1d5631c93404fe2efa" ],
        "name": "VALORANT",
        "slug": "valorant",
        "summary": "Imagine this: tactical shooter meets hypernatural powers. Everyone’s got guns and a unique set of abilities, so how do you beat someone with the speed of wind? Use your own moves to outplay them and beat them to the shot. VALORANT is a game for bold strategists who dare to make the unexpected play, because if it wins, it works.",
        "cover": "https://images.igdb.com/igdb/image/upload/t_1080p/co2mvt.png",
        "type": "videogame"
      }
    },
    "sectionStart": 1618896852,
    "sectionEnd": 1618896882,
    "upvotes": 1
  }
}
*/