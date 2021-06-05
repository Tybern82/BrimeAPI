#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.clips {

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
        public int Skip { get; set; } = 0;
        public SortOrder Sort { get; set; } = SortOrder.DESC;

        public ChannelClipsRequest(string channelName) : base(GET_CLIPS_FOR_CHANNEL_REQUEST) {
            this.ChannelName = channelName;
            this.RequestParameters = (() => {
                return new string[] { ChannelName, Since.ToString(), Limit.ToString(), Skip.ToString(), Sort.GetSortString() };
            });
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
