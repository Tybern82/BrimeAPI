#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrimeAPI.com.brimelive.api.emotes;

namespace BrimeAPI.com.brimelive.api {
    /// <summary>
    /// Contains static data for BrimeAPI connection
    /// </summary>
    public class BrimeAPI {

        /// <summary>
        /// Specifies the specific API endpoint to use in this request. Currently defaults to STAGING, will be updated to PRODUCTION once
        /// this version of the API is finalized. 
        /// </summary>
        public static BrimeAPIEndpoint APIEndpoint { get; private set; } = BrimeAPIEndpoint.STAGING;   // TODO: Update default to PRODUCTION

        /// <summary>
        /// Specifies the Client-ID sent with requests to the API. Note that in order to use this library you will need to have a valid 
        /// Client-ID assigned. This field is static so will only need to be specified once.
        /// </summary>
        public static string ClientID { get; set; } = "";

        /// <summary>
        /// Used to retrieve previously requested EmoteSets
        /// </summary>
        public static Dictionary<string, BrimeEmoteSet> EmoteSets { get; private set; } = new Dictionary<string, BrimeEmoteSet>();

        /// <summary>
        /// Used to retrieve the list of global emote sets
        /// </summary>
        public static List<BrimeEmoteSet> GlobalEmotes {
            get {
                if (_globalEmotes == null) {
                    GlobalEmotesRequest req = new GlobalEmotesRequest();
                    _globalEmotes = req.getResponse();
                }
                return new List<BrimeEmoteSet>(_globalEmotes);
            }
        }
        private static List<BrimeEmoteSet>? _globalEmotes = null;

        /// <summary>
        /// Retrieve the given set of emotes
        /// </summary>
        /// <param name="setID">ID for emote set to retrieve</param>
        /// <returns>the requested emote set (or null, if no set previously requested)</returns>
        public static BrimeEmoteSet? lookupEmoteSet(string setID) {
            BrimeAPI.EmoteSets.TryGetValue(setID, out BrimeEmoteSet _result);
            return _result;
        }


        // TODO: Implement cached request commands (ie Channel lookup, etc) which will save requests and serve from in-memory cache
        //       of requests to reduce request load on the server. 
    }
}
