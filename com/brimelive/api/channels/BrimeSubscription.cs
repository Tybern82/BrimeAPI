#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.channels {
    public class BrimeSubscription {

        public string ChannelID { get; private set; }
        public string SubscriptionID { get; private set; }
        public bool HasSubscribedBefore { get; private set; }
        public bool IsSubscribed { get; private set; }
        public string Tier { get; private set; }

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

/*
"data": {
    "channelID": "6050a4804b90bf0430aec1e2",
    "subID": "604e1cf62cbb31a8fe5e1de5",
    "hasSubscribedBefore": true,
    "isSubscribed": true,
    "tier": "tier_1"
  }
*/