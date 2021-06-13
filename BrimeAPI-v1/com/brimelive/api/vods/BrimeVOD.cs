#nullable enable

using System;
using System.Text;
using BrimeAPI.com.brimelive.api.categories;
using BrimeAPI.com.brimelive.api.errors;
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
        FINISHED
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
    public class BrimeVOD {

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
        public BrimeVODStreamInfo Stream { get; private set; }

        /// <summary>
        /// Current state of the VOD (used to identify whether VOD is still currently broadcasting)
        /// </summary>
        public string State { get; private set; }   // TODO: Update to enum once full set of states available

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
            Stream = new BrimeVODStreamInfo(streamInfo);

            curr = jsonData.Value<string>("state");
            if (curr == null) {
                Logger.Error("Missing state in VOD Response");
                throw new BrimeAPIMalformedResponse("Missing state in VOD Response");
            }
            State = curr;

            StartDate = jsonData.Value<DateTime>("startDate");
            EndDate = jsonData.Value<DateTime>("endDate");
            ExpiresAt = jsonData.Value<DateTime>("expiresAt");
        }
    }

    /**
     * <summary>
     * <para>Class <c>BrimeVODStreamInfo</c> is used to record the stream information assocated with a VOD record. This item
     * records the Stream Title and Stream Category used when the original stream was broadcast.</para>
     * <example>Example: <code>
     * "stream": {
     *     "title": "Brime Api Example",
     *     "category": {
     *         "_id": "606e93525fa50e5780970135",
     *         "genres": [],
     *         "name": "Uncategorized",
     *         "slug": "uncategorized",
     *         "summary": "",
     *         "cover": "https://content.brimecdn.com/brime/category_images/uncategorized.png",
     *         "type": "entertainment"
     *     }
     * }
     * </code></example>
     * </summary>
     */
    public class BrimeVODStreamInfo : JSONConvertable {

        /// <summary>
        /// Error message when missing Title element in JSON data.
        /// </summary>
        private const string ERR_MissingTitle = "Missing Title in VOD Stream Info";
        
        /// <summary>
        /// Error message when missing Category element in JSON data.
        /// </summary>
        private const string ERR_MisingCategory = "Missing Category in VOD Stream Info";
        
        /// <summary>
        /// ID for Title element when loading/storing to JSON.
        /// </summary>
        private const string ID_Title = "title";
        
        /// <summary>
        /// ID for Category element when loading/storing to JSON.
        /// </summary>
        private const string ID_Category = "category";

        /// <summary>
        /// Local NLog logging class.
        /// </summary>
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Property <c>Title</c> specifies the Stream Title of the original broadcast.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Property <c>Category</c> specifies the Stream Category of the original broadcast.
        /// </summary>
        public BrimeCategory Category { get; private set; }


        ///
        /// <summary>Construct a new <c>BrimeVODStreamInfo</c> record based on the given JSON response data.</summary>
        /// <param name="jsonData"><c>JToken</c> containing the JSON formatted record from API response.</param>
        /// <exception cref="BrimeAPIMalformedResponse">Thrown if provided JSON data is missing either <c>title</c> or <c>category</c> elements.</exception>
        ///
        public BrimeVODStreamInfo(JToken jsonData) {
            string? curr = jsonData.Value<string>(ID_Title);
            if (curr == null) {
                Logger.Error(ERR_MissingTitle);
                throw new BrimeAPIMalformedResponse(ERR_MissingTitle);
            }
            Title = curr;

            JToken? category = jsonData.Value<JToken>(ID_Category);
            if (category == null) {
                Logger.Error(ERR_MisingCategory);
                throw new BrimeAPIMalformedResponse(ERR_MisingCategory);
            }
            Category = new BrimeCategory(category);
        }

        /// <summary>
        /// Convert this record to its equivalent JSON formatted text.
        /// </summary>
        /// <returns><c>String</c> containing JSON formatted data for this record</returns>
        public string toJSON() {
            StringBuilder _result = new StringBuilder();
            _result.Append("{")
                .Append(Title.toJSON(ID_Title)).Append(", ")
                .Append(Category.toJSON(ID_Category))
                .Append("}");
            return _result.ToString();
        }

        /// <summary>
        /// Convert this record to a <c>string</c> value suitable for display/logging.
        /// </summary>
        /// <returns>"BrimeVODStreamInfo: {JSON object}"</returns>
        public override string ToString() {
            return "BrimeVODStreamInfo: " + toJSON();
        }
    }
}