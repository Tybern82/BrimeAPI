using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.clips {
    public class ChannelClipsRequest : BrimeAPIRequest<List<BrimeClip>> {

        private static readonly string GET_CLIPS_FOR_CHANNEL_REQUEST = "/v1/channel/{0}/clips";  // /v1/channel/:channelId/clips

        public string ChannelName { get; private set; }

        public ChannelClipsRequest(string channelName) : base(GET_CLIPS_FOR_CHANNEL_REQUEST) {
            this.ChannelName = channelName;
            this.RequestParameters = (() => {
                return new string[] { ChannelName };
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
