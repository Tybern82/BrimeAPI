#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.clips {

    public enum ClipSortOrder { 
        ASC,    // Oldest->Newest
        DESC    // Newest->Oldest
    }

    public class ChannelClipsRequest : BrimeAPIRequest<List<BrimeClip>> {

        private static readonly string GET_CLIPS_FOR_CHANNEL_REQUEST = "/v1/channel/{0}/clips?since={1}&limit={2}&skip={3}&sort={4}";  
        // /v1/channel/:channelId/clips?since=0&limit=50&skip=0&sort=desc

        public string ChannelName { get; set; }

        public int Since { get; set; } = 0; // 0 = All Time

        private int _Limit = 50;
        public int Limit { 
            get {
                return _Limit;
            }
            set {
                // Ensure Limit is set between 1 and 150 (platform maximum)
                _Limit = (value > 150) ? 150 : (value < 1) ? 1 : value;
            }
        }
        public int Skip { get; set;}
        public ClipSortOrder Sort { get; set;}

        public ChannelClipsRequest(string channelName) : base(GET_CLIPS_FOR_CHANNEL_REQUEST) {
            this.ChannelName = channelName;
            this.RequestParameters = (() => {
                return new string[] { ChannelName, Since, Limit, Skip, GetSortString(Sort) };
            });
        }

        public static string GetSortString(ClipSortOrder order) {
            switch (order) {
                case ClipSortOrder.ASC: 
                    return "asc";
                case ClipSortOrder.DESC: 
                default: 
                    return "desc";
            }
        }

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
