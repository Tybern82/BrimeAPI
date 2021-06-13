#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.clips {

    /// <summary>
    /// Query for the clips associated with a given channel. Can be paged
    /// </summary>
    public class ChannelClipsRequest : BrimeAPIRequest<List<BrimeClip>> {

        private static readonly string GET_CLIPS_FOR_CHANNEL_REQUEST = "/channel/{0}/clips";  
        // /v1/channel/:channelId/clips?since=0&limit=50&skip=0&sort=desc

        /// <summary>
        /// Identify the name of the channel to request clips from
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// Identify time period to start looking for clips (will produce only clips made after this date)
        /// Defaults to '0' indicating clips from all time
        /// </summary>
        public long Since { get; set; } = 0; // 0 = All Time

        /// <summary>
        /// Identify how many clips to return in the list (between 1 and 150, default 50)
        /// </summary>
        public int Limit { 
            get {
                return _Limit;
            }
            set {
                // Ensure Limit is set between 1 and 150 (platform maximum)
                _Limit = (value > 150) ? 150 : (value < 1) ? 1 : value;
            }
        }
        private int _Limit = 50;

        /// <summary>
        /// Identify how many clips to skip before selecting the ones to return
        /// </summary>
        public long Skip { get; set; } = 0;

        /// <summary>
        /// Identify what order to sort the clips. Defaults to Descending order (Newest to Oldest)
        /// </summary>
        public SortOrder Sort { get; set; } = SortOrder.DESC;

        /// <summary>
        /// Create a new request for the given channel name
        /// </summary>
        /// <param name="channelName">name of the channel to retrieve clips from</param>
        public ChannelClipsRequest(string channelName) : base(GET_CLIPS_FOR_CHANNEL_REQUEST) {
            this.ChannelName = channelName;
            this.RequestParameters = (() => {
                return new string[] { ChannelName };
            });
            this.QueryParameters = (() => {
                List<KeyValuePair<string, string>> queryParams = new List<KeyValuePair<string, string>>();
                // Add in parameters if they are not default
                if (Since != 0) queryParams.Add(new KeyValuePair<string, string>("since", Since.ToString()));
                if (Limit != 50) queryParams.Add(new KeyValuePair<string, string>("limit", Limit.ToString()));
                if (Skip != 0) queryParams.Add(new KeyValuePair<string, string>("skip", Skip.ToString()));
                if (Sort != SortOrder.DESC) queryParams.Add(new KeyValuePair<string, string>("sort", Sort.GetSortString()));
                return queryParams.ToArray();
            });
        }

        /// <inheritdoc />
        public override List<BrimeClip> getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);

            JArray? clips = response.Data.Value<JArray>("clips");
            if (clips != null) {
                List<BrimeClip> _result = new List<BrimeClip>(clips.Count);
                foreach (JToken? clip in clips) {
                    if (clip != null) _result.Add(new BrimeClip(clip));
                }
                return _result;
            } else {
                return new List<BrimeClip>();
            }
        }
    }
}
