#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.channels {
    /// <summary>
    /// Contains the details for a Channel Subscription
    /// </summary>
    public class BrimeSubscription {
        /// <summary>
        /// ID of the channel subscribed to
        /// </summary>
        /// <code>
        /// "data": {
        ///     "channelID": "6050a4804b90bf0430aec1e2",
        ///     "subID": "604e1cf62cbb31a8fe5e1de5",
        ///     "hasSubscribedBefore": true,
        ///     "isSubscribed": true,
        ///     "tier": "tier_1"
        /// }</code>
        public string ChannelID { get; private set; }

        /// <summary>
        /// ID for this subscription
        /// </summary>
        public string SubscriptionID { get; private set; }

        /// <summary>
        /// Identify whether this user has subscribed before
        /// </summary>
        public bool HasSubscribedBefore { get; private set; }

        /// <summary>
        /// Identify whether this subscription is active
        /// </summary>
        public bool IsSubscribed { get; private set; }

        /// <summary>
        /// Identify the tier of the subscription
        /// </summary>
        public string Tier { get; private set; }

        /// <summary>
        /// Process the given data to load subscription information
        /// </summary>
        /// <param name="jsonData">JSON data to process</param>
        public BrimeSubscription(JToken jsonData) {
            string? curr = jsonData.Value<string>("channelID");
            ChannelID = curr ?? "";

            curr = jsonData.Value<string>("subID");
            SubscriptionID = curr ?? "";

            HasSubscribedBefore = jsonData.Value<bool>("hasSubscribedBefore");
            IsSubscribed = jsonData.Value<bool>("isSubscribed");

            curr = jsonData.Value<string>("tier");
            Tier = curr ?? "";
        }

        private static readonly string FORMAT = "{{" +
            "\"channelID\": {0}," +
            "\"subID\": {1}," +
            "\"hasSubscribedBefore\": {2}," +
            "\"isSubscribed\": {3}," +
            "\"tier\": {4}" +
            "}}";

        /// <inheritdoc />
        public override string ToString() {
            return string.Format(FORMAT,
                JsonConvert.ToString(ChannelID),
                JsonConvert.ToString(SubscriptionID),
                JsonConvert.ToString(HasSubscribedBefore),
                JsonConvert.ToString(IsSubscribed),
                JsonConvert.ToString(Tier));
        }
    }
}
