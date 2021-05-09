using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.users {
    public class BrimeUser {

        public string UserID { get; private set; }
        public string Username { get; private set; }
        public string DisplayName { get; private set; }
        public string Avatar { get; private set; }
        public string Color { get; private set; }
        public List<string> Roles { get; private set; }
        public List<string> Badges { get; private set; }

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
            UserID = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("username");
            Username = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("displayname");
            DisplayName = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("avatar");
            Avatar = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("color");
            Color = (curr == null) ? "" : curr;

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
        }

        public override string ToString() {
            string format = "{{" +
                "\"_id\": {0}, " +
                "\"username\": {1}," +
                "\"displayname\": {2}," +
                "\"avatar\": {3}," +
                "\"color\": {4}," +
                "\"roles\": {5}," +
                "\"badges\": {6}" +
                "}}";
            try {
                return string.Format(
                    format,
                    JsonConvert.ToString(UserID),
                    JsonConvert.ToString(Username),
                    JsonConvert.ToString(DisplayName),
                    JsonConvert.ToString(Avatar),
                    JsonConvert.ToString(Color),
                    JSONUtil.ToString(Roles.ToArray()),
                    JSONUtil.ToString(Badges.ToArray())
                    );
            } catch (Exception e) {
                Console.WriteLine(e);
                return "";
            }
        }
    }
}
