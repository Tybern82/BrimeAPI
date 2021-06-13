#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.users {
    public class BrimeUser : JSONConvertable {

        public string UserID { get; private set; }
        public string Username { get; private set; }
        public string DisplayName { get; private set; }
        public string Avatar { get; private set; }
        public string Color { get; private set; }
        public List<string> Roles { get; private set; }
        public List<string> Badges { get; private set; }
        public bool isBrimePro { get; private set; } = false;
        public bool extendedVODsEnabled { get; private set; } = false;

        public BrimeUser() {
            UserID = "";
            Username = "";
            DisplayName = "";
            Avatar = "";
            Color = "";
            Roles = new List<string>();
            Badges = new List<string>();
        }

        public BrimeUser(JToken jsonData) {
            string? curr = jsonData.Value<string>("_id");
            UserID = curr ?? "";

            curr = jsonData.Value<string>("username");
            Username = curr ?? "";

            curr = jsonData.Value<string>("displayname");
            DisplayName = curr ?? "";

            curr = jsonData.Value<string>("avatar");
            Avatar = curr ?? "";

            curr = jsonData.Value<string>("color");
            Color = curr ?? "";

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

        public override string ToString() {
            return toJSON();
        }

        public string toJSON() {
            StringBuilder _result = new StringBuilder();
            _result.Append("{")
                .Append(UserID.toJSON("_id")).Append(", ")
                .Append(Username.toJSON("username")).Append(", ")
                .Append(DisplayName.toJSON("displayname")).Append(", ")
                .Append(Avatar.toJSON("avatar")).Append(", ")
                .Append(Color.toJSON("color")).Append(", ")
                .Append(Roles.toJSON<string>("roles")).Append(", ")
                .Append(Badges.toJSON<string>("badges"))
                .Append("}");
            return _result.ToString();
        }
    }
}
