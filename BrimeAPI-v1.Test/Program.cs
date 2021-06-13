using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrimeAPI.com.brimelive.api.channels;
using BrimeAPI.com.brimelive.api.clips;
using BrimeAPI.com.brimelive.api.streams;
using BrimeAPI.com.brimelive.api.users;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.Test {
    class Program {
        /// <summary>Class Logger instance</summary>
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args) {
            /* Requires Special Access 
            UserClipsRequest;
            UserFollowingRequest;
            UserRequest;
            ChannelSubscriptionRequest
            */
            string curr;
            try {
                string appPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                if (appPath == null) appPath = "./";
                string fname = Path.Combine(appPath, "MultiChat.json");
                if (File.Exists(fname)) {
                    // Load JSON data
                    JObject jsonData = JObject.Parse(File.ReadAllText(fname));

                    curr = jsonData.Value<string>("clientID");
                    if (!string.IsNullOrWhiteSpace(curr))
                        BrimeAPI.com.brimelive.api.BrimeAPI.ClientID = curr;
                }
            } catch (Exception) {}

            string channelName = "geeken";

            Logger.Info("Checking TotalUsersRequest...");
            try {
                TotalUsersRequest req = new TotalUsersRequest();
                int totalUsers = req.getResponse();
                Logger.Info("Identified " + totalUsers + " total registered users.");
            } catch (Exception e) {
                Logger.Error(e.ToString());
            }

            Logger.Info("Checking ChannelExistsRequest");
            try {
                ChannelExistsRequest req = new ChannelExistsRequest(channelName);
                bool hasChannel = req.getResponse();
                Logger.Info("Channel <" + channelName + "> " + (hasChannel ? "exists" : "does not exist"));
            } catch (Exception e) {
                Logger.Error(e.ToString());
            }

            Logger.Info("Checking ChannelRequest");
            try {
                ChannelRequest req = new ChannelRequest(channelName);
                BrimeChannel channel = req.getResponse();
                Logger.Info("Identified channel " + channelName);
                Logger.Trace(channel.ToString());
            } catch (Exception e) {
                Logger.Error(e.ToString());
            }

            string liveChannel = channelName;
            Logger.Info("Checking LiveStreamsRequest");
            try {
                LiveStreamsRequest req = new LiveStreamsRequest();
                List<BrimeStream> live = req.getResponse();
                Logger.Info("Detected " + live.Count + " live streams:");
                List<string> names = new List<string>(live.Count);
                foreach (BrimeStream s in live) {
                    names.Add(s.ChannelName);
                    Logger.Trace(s.ToString());
                }
                Logger.Info(string.Join(", ", names.ToArray()));
                if (live.Count > 0) {
                    liveChannel = live[0].ChannelName;
                    Logger.Info(live[0].ChannelName + " started streaming at " + live[0].PublishTime);
                }
            } catch (Exception e) {
                Logger.Error(e.ToString());
            }

            Logger.Info("Checking StreamRequest");
            try {
                StreamRequest req = new StreamRequest(liveChannel);
                BrimeStream stream = req.getResponse();
                Logger.Info("Detected " + stream.ChannelName + " is streaming <" + stream.Category.Name + ">");
                Logger.Info("Title: " + stream.Title);
                Logger.Trace(stream.ToString());
            } catch (Exception e) {
                Logger.Error(e.ToString());
            }

            string clipID = "607e67f1fb94b3cba84e80fb";
            Logger.Info("Checking ClipInfoRequest");
            try {
                ClipInfoRequest req = new ClipInfoRequest(clipID);
                BrimeClip clip = req.getResponse();
                Logger.Info("Detected clip <" + clip.ClipName + "> made on " + clip.ClipDate);
                Logger.Trace(clip.ToString());
            } catch (Exception e) {
                Logger.Error(e.ToString());
            }

            Logger.Info("Checking ChannelClipsRequest");
            try {
                ChannelClipsRequest req = new ChannelClipsRequest(channelName);
                req.Limit = 5;
                List<BrimeClip> clips = req.getResponse();
                Logger.Info("Detected " + clips.Count + "/5 clips");
                foreach (BrimeClip clip in clips) {
                    Logger.Info("Detected clip <" + clip.ClipName + "> made on " + clip.ClipDate);
                    Logger.Trace(clip.ToString());
                }
            } catch (Exception e) {
                Logger.Error(e.ToString());
            }

            Logger.Info("Checks complete");

            Console.WriteLine("Press <return> to close...");
            Console.ReadLine().Trim();
        }
    }
}
