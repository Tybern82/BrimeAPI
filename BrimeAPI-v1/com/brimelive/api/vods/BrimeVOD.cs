#nullable enable

using System;
using System.Text;
using BrimeAPI.com.brimelive.api.categories;
using BrimeAPI.com.brimelive.api.channels;
using BrimeAPI.com.brimelive.api.errors;
using BrimeAPI.com.brimelive.api.streams;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.vods {

    /// <summary>
    /// TODO: Will be used for <c>BrimeVOD.State</c>
    /// Still requires identifying all options for this field.
    /// </summary>
    public enum VODState {
        /// <summary>
        /// VOD has completed broadcasting
        /// </summary>
        FINISHED,
        /// <summary>
        /// VOD is currently broadcasting
        /// </summary>
        IN_PROGRESS,
        /// <summary>
        /// Unknown / unrecognized state
        /// </summary>
        UNKNOWN
    }

    /// <summary>
    /// Helper class for extension methods for VODState
    /// </summary>
    public static class VODStateUtil {

        /// <summary>
        /// Convert given state into valid JSON string output
        /// </summary>
        /// <param name="state">state to convert</param>
        /// <returns>string suitable for inclusion in JSON</returns>
        public static string GetStateString(this VODState state) => state switch {
            VODState.FINISHED => "FINISHED",
            VODState.IN_PROGRESS => "IN_PROGRESS",
            _ => ""
        };
    }


    /**
     * <summary>
     * Class <c>BrimeVOD</c> is used to record the details of a single VOD. This record includes information about the original broadcast,
     * as well as when this VOD is scheduled to expire.
     * </summary>
     * <example>Example: <code>
     * "data": {
     *     "_id": "60a252ef93689e8e2708c0cd",
     *     "channelID": "6056ee164b90bf043002c32d",
     *     "vodVideoUrl": "https://content.brimecdn.com/brime-vods/vod/6056ee164b90bf043002c32d/60a252ef93689e8e2708c0cd/hls/chunks_dvr.m3u8",
     *     "vodThumbnailUrl": "https://content.brimecdn.com/brime-vods/vod/6056ee164b90bf043002c32d/60a252ef93689e8e2708c0cd/thumbnail",
     *     "stream": {
     *         "title": "Brime Api Example",
     *         "category": {
     *             "_id": "606e93525fa50e5780970135",
     *             "genres": [],
     *             "name": "Uncategorized",
     *             "slug": "uncategorized",
     *             "summary": "",
     *             "cover": "https://content.brimecdn.com/brime/category_images/uncategorized.png",
     *             "type": "entertainment"
     *         }
     *     },
     *     "state": "FINISHED",
     *     "startDate": 1621250799105,
     *     "endDate": 1621250905273,
     *     "expiresAt": 1622460505273
     * }
     * </code></example>
     */
    public class BrimeVOD : JSONConvertable {

        /// <summary>
        /// Local NLog logging class.
        /// </summary>
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// VOD Identifier
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// Channel ID for the channel which broadcast this VOD
        /// </summary>
        public string ChannelID { get; private set; }

        /// <summary>
        /// Retrieve the Channel Information
        /// </summary>
        public BrimeChannel Channel { 
            get {
                if (_Channel == null) {
                    // TODO: Update to call through BrimeAPI to allow caching of requests.
                    ChannelRequest req = new ChannelRequest(ChannelID);
                    _Channel = req.getResponse();
                }
                return _Channel;
            }
        }
        private BrimeChannel? _Channel;

        /// <summary>
        /// URL for the VOD video data
        /// </summary>
        public Uri VODVideoURL { get; private set; }

        /// <summary>
        /// URL for the VOD thumbnail
        /// </summary>
        public Uri VODThumbnailURL { get; private set; }

        /// <summary>
        /// Information about the original broadcast stream
        /// </summary>
        public StreamDetails Stream { get; private set; }

        /// <summary>
        /// Current state of the VOD (used to identify whether VOD is still currently broadcasting)
        /// </summary>
        public VODState State { get; private set; }   

        /// <summary>
        /// Identifies when this VOD started broadcasting
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// Identifies when this VOD finished broadcasting
        /// </summary>
        public DateTime EndDate { get; private set; }

        /// <summary>
        /// Identifies when this VOD will expire from storage and be deleted
        /// </summary>
        public DateTime ExpiresAt { get; private set; }

        /// <summary>
        /// Construct a new instance based on the given JSON data
        /// </summary>
        /// <param name="jsonData">JSON data to process</param>
        public BrimeVOD(JToken jsonData) {
            string? curr = jsonData.Value<string>("_id");
            if (curr == null) {
                Logger.Error("Missing _id in VOD Response");
                throw new BrimeAPIMalformedResponse("Missing _id in VOD Response");
            }
            ID = curr;

            curr = jsonData.Value<string>("channelID");
            if (curr == null) {
                Logger.Error("Missing channelID in VOD Response");
                throw new BrimeAPIMalformedResponse("Missing channelID in VOD Response");
            }
            ChannelID = curr;

            curr = jsonData.Value<string>("vodVideoUrl");
            if (curr == null) {
                Logger.Error("Missing vodVideoUrl in VOD Response");
                throw new BrimeAPIMalformedResponse("Mising vodVideoUrl in VOD Response");
            }
            VODVideoURL = new Uri(curr);

            curr = jsonData.Value<string>("vodThumbnailUrl");
            if (curr == null) {
                Logger.Error("Missing vodThumbnailUrl in VOD Response");
                throw new BrimeAPIMalformedResponse("Missing vodThumbnailUrl in VOD Response");
            }
            VODThumbnailURL = new Uri(curr);

            JToken? streamInfo = jsonData.Value<JToken>("stream");
            if (streamInfo == null) {
                Logger.Error("Missing stream information in VOD Response");
                throw new BrimeAPIMalformedResponse("Missing stream information in VOD Response");
            }
            Stream = new StreamDetails(streamInfo);

            curr = jsonData.Value<string>("state");
            if (curr == null) {
                Logger.Error("Missing state in VOD Response");
                throw new BrimeAPIMalformedResponse("Missing state in VOD Response");
            }
            State = curr switch {
                "FINISHED" => VODState.FINISHED,
                "IN_PROGRESS" => VODState.IN_PROGRESS,
                _ => VODState.UNKNOWN
            };

            StartDate = DateTimeOffset.FromUnixTimeMilliseconds(jsonData.Value<long>("startDate")).DateTime;
            EndDate = DateTimeOffset.FromUnixTimeMilliseconds(jsonData.Value<long>("endDate")).DateTime;
            ExpiresAt = DateTimeOffset.FromUnixTimeMilliseconds(jsonData.Value<long>("expiresAt")).DateTime;
        }

        /// <inheritdoc />
        public string toJSON() {
            StringBuilder _result = new StringBuilder();
            _result.Append("{")
                .Append(ID.toJSON("_id")).Append(", ")
                .Append(ChannelID.toJSON("channelID")).Append(", ")
                .Append(VODVideoURL.toJSON("vodVideoUrl")).Append(", ")
                .Append(VODThumbnailURL.toJSON("vodThumbnailUrl")).Append(", ")
                .Append(State.GetStateString().toJSON("state")).Append(", ")
                .Append(new DateTimeOffset(StartDate).ToUnixTimeMilliseconds().toJSON("startDate")).Append(", ")
                .Append(new DateTimeOffset(EndDate).ToUnixTimeMilliseconds().toJSON("endDate")).Append(", ")
                .Append(new DateTimeOffset(ExpiresAt).ToUnixTimeMilliseconds().toJSON("expiresAt")).Append(", ")
                .Append("}");
            return _result.ToString();
        }

        /// <inheritdoc />
        public override string ToString() {
            return toJSON();
        }
    }
}