#nullable enable

using System.Collections.Generic;
using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.vods {

    /// <summary>
    /// Requests a range of VODs for a particular channel
    /// </summary>
    public class ChannelVodsRequest : BrimeAPIRequest<List<BrimeVOD>> {

        private static readonly string GET_VODS_FOR_CHANNEL_REQUEST = "/v1/channel/{0}/vods?limit={1}&skip={2}&sort={3}";    // /v1/channel/:channelId/vods

        /// <summary>
        /// Specify the Channel the VODs are associated with
        /// </summary>
        public string ChannelName { get; private set; }

        /// <summary>
        /// Identify how many VODs to return for this request. Default is 50
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
        /// Specify how many VODs to skip before selecting the set to return. Allows paging through the 
        /// list of VODs. TODO: Identify whether Skip is counted in individual VODs or in multiples of Limit
        /// </summary>
        public int Skip { get; set; } = 0;
        
        /// <summary>
        /// Defines the order to sort the VODs before selecting the set to return. With DESC default, will return the
        /// latest 50 VODs, while if set to ASC, will return the oldest 50 VODs.
        /// </summary>
        public SortOrder Sort { get; set; } = SortOrder.DESC;

        /// <summary>
        /// Create a new instance of this request, for the specified channel.
        /// </summary>
        /// <param name="channelName">Specify the Channel to request VODs from</param>
        public ChannelVodsRequest(string channelName) : base(GET_VODS_FOR_CHANNEL_REQUEST) {
            this.ChannelName = channelName;
            this.RequestParameters = (() => {
                return new string[] { 
                    ChannelName,
                    Limit.ToString(),
                    Skip.ToString(),
                    Sort.GetSortString()
                };
            });
        }

        /// <summary>
        /// Retrieve the list of VODs for this request. 
        /// </summary>
        /// <returns>List of <c>BrimeVOD</c> entries, between 0 and <c>Limit</c> items</returns>
        public override List<BrimeVOD> getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            List<BrimeVOD> _result;
            JArray? vods = response.Data.Value<JArray>("streams");
            if (vods != null) {
                _result = new List<BrimeVOD>(vods.Count);
                foreach (JToken item in vods) {
                    _result.Add(new BrimeVOD(item));
                }
            } else {
                _result = new List<BrimeVOD>();
            }
            return _result;
        }
    }
}
