#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using BrimeAPI.com.brimelive.api.errors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.users {

    /// <summary>
    /// Identify the roles assocated with Users
    /// </summary>
    public enum UserRoles {
        /// <summary>
        /// Employees of Brime
        /// </summary>
        STAFF,

        /// <summary>
        /// Platform-wide moderators
        /// </summary>
        PLATFORM_MOD,

        /// <summary>
        /// A verified user account
        /// </summary>
        VERIFIED,

        /// <summary>
        /// User with Brime Pro
        /// </summary>
        BRIME_PRO,

        /// <summary>
        /// A chat bot
        /// </summary>
        BOT,

        /// <summary>
        /// A normal user
        /// </summary>
        USER,

        /// <summary>
        /// The owner of the current channel (chat)
        /// </summary>
        CREATOR,

        /// <summary>
        /// A user that is subscribed to the current channel (chat)
        /// </summary>
        SUBSCRIBER,

        /// <summary>
        /// A user that is following the current channel (chat)
        /// </summary>
        FOLLOWER,

        /// <summary>
        /// A moderator of the current channel (chat)
        /// </summary>
        MODERATOR
    }

    /// <summary>
    /// Identifies a Brime User. Note that most requests which retrieve User details require Special Access
    /// on the client-ID.
    /// </summary>
    public class BrimeUser : JSONConvertable {

        /// <summary>
        /// Unique identifier for this user
        /// </summary>
        public string UserID { get; private set; }

        /// <summary>
        /// Identify the username of this user
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Identify how this username should be displayed 
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Link to user's avatar image
        /// </summary>
        public Uri Avatar { get; private set; }

        /// <summary>
        /// Identify the color this username should display as in chat
        /// </summary>
        public string Color { get; private set; }

        /// <summary>
        /// Identify roles associated with this user
        /// </summary>
        public List<string> Roles { get; private set; }

        /// <summary>
        /// Identify badges associated with this user
        /// </summary>
        public List<string> Badges { get; private set; }

        /// <summary>
        /// Identify whether this user has subscribed to BrimePro
        /// </summary>
        public bool isBrimePro { get; private set; } = false;

        /// <summary>
        /// Identify whether this user has enabled extended VOD storage (requires BrimePro subscription)
        /// </summary>
        public bool extendedVODsEnabled { get; private set; } = false;

        /// <summary>
        /// Create a new instance based on the given JSON data
        /// </summary>
        /// <param name="jsonData">JSON data to process</param>
        public BrimeUser(JToken jsonData) {
            string? curr = jsonData.Value<string>("_id");
            if (curr == null) throw new BrimeAPIMalformedResponse("Missing ID in user details");
            UserID = curr;

            curr = jsonData.Value<string>("username");
            if (curr == null) throw new BrimeAPIMalformedResponse("Missing username in user details");
            Username = curr;

            curr = jsonData.Value<string>("displayname");
            DisplayName = curr ?? Username; // default displayname to username if missing

            curr = jsonData.Value<string>("avatar");
            if (curr == null) throw new BrimeAPIMalformedResponse("Missing Avatar in user details");
            Avatar = new Uri(curr);

            curr = jsonData.Value<string>("color");
            Color = curr ?? "#FFFFFF";  // default to white

            JArray? roles = jsonData.Value<JArray>("roles");
            Roles = new List<string>();
            if (roles != null) { 
                foreach (JToken role in roles) {
                    Roles.Add(role.ToString());
                }
            }

            JArray? badges = jsonData.Value<JArray>("badges");
            Badges = new List<string>();
            if (badges != null) {
                foreach (JToken badge in badges) {
                    Badges.Add(badge.ToString());
                }
            }

            isBrimePro = jsonData.Value<bool>("isBrimePro");
            extendedVODsEnabled = jsonData.Value<bool>("extendedVodsEnabled");
        }

        /// <inheritdoc />
        public override string ToString() {
            return toJSON();
        }

        /// <inheritdoc />
        public string toJSON() {
            StringBuilder _result = new StringBuilder();
            _result.Append("{")
                .Append(UserID.toJSON("_id")).Append(", ")
                .Append(Username.toJSON("username")).Append(", ")
                .Append(DisplayName.toJSON("displayname")).Append(", ")
                .Append(Avatar.toJSON("avatar")).Append(", ")
                .Append(Color.toJSON("color")).Append(", ")
                .Append(Roles.toJSON<string>("roles")).Append(", ")
                .Append(Badges.toJSON<string>("badges")).Append(", ")
                .Append(isBrimePro.toJSON("isBrimePro")).Append(", ")
                .Append(extendedVODsEnabled.toJSON("extendedVodsEnabled"))
                .Append("}");
            return _result.ToString();
        }
    }
}
