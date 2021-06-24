#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrimeAPI.com.brimelive.api.errors;
using BrimeAPI.com.brimelive.api.users;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.emotes {
    /// <summary>
    /// Identify default badges for the various user roles
    /// </summary>
    public class GlobalBadgesRequest : BrimeAPIRequest<Dictionary<UserRoles, BrimeBadge>> {
        private static readonly string GET_GLOBAL_BADGES = "/global/badges";

        /// <summary>
        /// Create a new request - no parameters for this request type
        /// </summary>
        public GlobalBadgesRequest() : base(GET_GLOBAL_BADGES) {}   // No parameters to request

        /// <inheritdoc />
        public override Dictionary<UserRoles, BrimeBadge> getResponse() {
            BrimeAPIResponse response = doRequest();
            BrimeAPIError.ThrowException(response);
            Dictionary<UserRoles, BrimeBadge> _result = new Dictionary<UserRoles, BrimeBadge>();
            foreach (UserRoles role in Enum.GetValues(typeof(UserRoles))) {
                string name = Enum.GetName(typeof(UserRoles), role).ToUpper();  // ensures name is in uppercase
                if (response.Data.HasValue(name)) {
                    JToken? item = response.Data.Value<JToken>(name);
                    if ((item != null) && (item.Type != JTokenType.Null)) {
                        if (item.HasValue("imageLink")) {
                            string? imageLink = item.Value<string>("imageLink");
                            bool isDefault = item.Value<bool>("isDefault");
                            if (imageLink != null) {
                                BrimeBadge badge = new BrimeBadge(imageLink, isDefault);
                                _result.Add(role, badge);
                            }
                        }
                    }
                }
            }
            return _result;
        }
    }

    /// <summary>
    /// Specify a Brime Badge
    /// </summary>
    public class BrimeBadge {
        /// <summary>
        /// Link to the badge image
        /// </summary>
        public Uri ImageLink { get; private set; }

        /// <summary>
        /// Identify whether badge is set as a default
        /// </summary>
        public bool IsDefault { get; private set; }

        /// <summary>
        /// Create a new instance for the specified badge
        /// </summary>
        /// <param name="imageLink">link to the image</param>
        /// <param name="isDefault">identify whether set as a default</param>
        public BrimeBadge(string imageLink, bool isDefault) {
            this.ImageLink = new Uri(imageLink);
            this.IsDefault = isDefault;
        }
    }
}
