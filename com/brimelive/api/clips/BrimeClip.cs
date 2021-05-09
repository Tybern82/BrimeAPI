#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api.clips {
    public class BrimeClip {

        public string ID { get; private set; }
        public string URL { get; private set; }
        public string ClipName { get; private set; }
        public string ClipVideoURL { get; private set; }
        public DateTime ClipDate { get; private set; }
        public string ChannelID { get; private set; }
        public StreamDetails Stream { get; private set; }
        public DateTime SectionStart { get; private set; }
        public DateTime SectionEnd { get; private set; }
        public string ClipperID { get; private set; }
        public string Clipper { get; private set; }

        public BrimeClip(JToken jsonData) {
            string? curr = jsonData.Value<string>("_id");
            ID = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("url");
            URL = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("clipName");
            ClipName = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("clipVideoUrl");
            ClipVideoURL = (curr == null) ? "" : curr;

            int clipDate = jsonData.Value<int>("clipDate");
            ClipDate = new DateTime(clipDate, DateTimeKind.Utc);

            curr = jsonData.Value<string>("channelID");
            ChannelID = (curr == null) ? "" : curr;

            JToken? stream = jsonData.Value<JToken>("stream");
            Stream = (stream == null) ? new StreamDetails() : new StreamDetails(stream);

            int sectionStart = jsonData.Value<int>("sectionStart");
            SectionStart = new DateTime(sectionStart, DateTimeKind.Utc);

            int sectionEnd = jsonData.Value<int>("sectionEnd");
            SectionEnd = new DateTime(sectionEnd, DateTimeKind.Utc);

            curr = jsonData.Value<string>("clipperID");
            ClipperID = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("clipper");
            Clipper = (curr == null) ? "" : curr;
        }

        public BrimeClip() {
            this.ID = "";
            this.URL = "";
            this.ClipName = "";
            this.ClipVideoURL = "";
            this.ClipDate = DateTime.Now;
            this.ChannelID = "";
            this.Stream = new StreamDetails();
            this.SectionStart = DateTime.Now;
            this.SectionEnd = DateTime.Now;
            this.ClipperID = "";
            this.Clipper = "";
        }

        public override string ToString() {
            string format = "{{" +
                "\"_id\": {0}," +
                "\"url\": {1}," +
                "\"clipName\": {2}," +
                "\"clipVideoURL\": {3}," +
                "\"clipDate\": {4}," +
                "\"channelID\": {5}," +
                "\"stream\": {6}," +
                "\"sectionStart\": {7}," +
                "\"sectionEnd\": {8}," +
                "\"clipperID\": {9}," +
                "\"clipper\": {10}" +
                "}}";
            return string.Format(format,
                JsonConvert.ToString(ID),
                JsonConvert.ToString(URL),
                JsonConvert.ToString(ClipName),
                JsonConvert.ToString(ClipVideoURL),
                JsonConvert.ToString(ClipDate.Ticks),
                JsonConvert.ToString(ChannelID),
                Stream.ToString(),
                JsonConvert.ToString(SectionStart.Ticks),
                JsonConvert.ToString(SectionEnd.Ticks),
                JsonConvert.ToString(ClipperID),
                JsonConvert.ToString(Clipper)
                );
        }
    }

    public class StreamDetails {
        public string Category { get; private set; }
        public string Title { get; private set; }

        public StreamDetails(JToken jsonData) {
            string? curr = jsonData.Value<string>("category");
            Category = (curr == null) ? "" : curr;

            curr = jsonData.Value<string>("title");
            Title = (curr == null) ? "" : curr;
        }

        public StreamDetails() {
            this.Category = "";
            this.Title = "";
        }

        public override string ToString() {
            string format = "{{" +
                "\"category\": {0}," +
                "\"title\": {1}" +
                "}}";
            return string.Format(format,
                JsonConvert.ToString(Category),
                JsonConvert.ToString(Title)
                );
        }
    }
}

/*
  "data": {
    "_id": "6069f53185fa36ba0baad898",
    "url": "https://beta.brimelive.com/clip/6069f53185fa36ba0baad898",
    "clipName": "Clipped by llamafun",
    "clipVideoUrl": "https://content.brimecdn.com/brime/clip/6050a4804b90bf0430aec1e2/6069f53185fa36ba0baad898",
    "clipDate": 1617556786,
    "channelID": "6050a4804b90bf0430aec1e2",
    "stream": {
      "category": "BRIME™ OFFICIAL",
      "title": "Chatting with Brimer / Dev Updates"
    },
    "sectionStart": 1617556756,
    "sectionEnd": 1617556786,
    "clipperID": "605a58d321489a6c4a78709d",
    "clipper": "llamafun"
  }
*/