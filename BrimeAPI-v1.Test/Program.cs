using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrimeAPI.com.brimelive.api.categories;
using BrimeAPI.com.brimelive.api.channels;
using BrimeAPI.com.brimelive.api.clips;
using BrimeAPI.com.brimelive.api.emotes;
using BrimeAPI.com.brimelive.api.streams;
using BrimeAPI.com.brimelive.api.users;
using BrimeAPI.com.brimelive.api.vods;
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
                ChannelClipsRequest req = new ChannelClipsRequest(channelName) {
                    Limit = 5
                };
                List<BrimeClip> clips = req.getResponse();
                Logger.Info("Detected " + clips.Count + "/5 clips");
                foreach (BrimeClip clip in clips) {
                    Logger.Info("Detected clip <" + clip.ClipName + "> made on " + clip.ClipDate);
                    Logger.Trace(clip.ToString());
                }
            } catch (Exception e) {
                Logger.Error(e.ToString());
            }

            string vodID = "60c49720d73886e6de6bc554";   // Finished
            Logger.Info("Checking VodInfoRequest");
            try {
                VodInfoRequest req = new VodInfoRequest(vodID);
                BrimeVOD vod = req.getResponse();
                Logger.Info("Detected VOD from " + vod.StartDate + " on channel " + vod.Channel.ChannelName);
                if (vod.State == VODState.FINISHED) Logger.Info("Finish time: " + vod.EndDate);
                Logger.Trace(vod.toJSON());
            } catch (Exception e) {
                Logger.Error(e.ToString());
            }

            Logger.Info("Checking ChannelVodsRequest");
            try {
                ChannelVodsRequest req = new ChannelVodsRequest(channelName) {
                    Limit = 5
                };
                List<BrimeVOD> vods = req.getResponse();
                foreach (BrimeVOD vod in vods) {
                    Logger.Info("Detected VOD from " + vod.StartDate + " on channel " + vod.Channel.ChannelName);
                    if (vod.State == VODState.FINISHED) Logger.Info("Finish time: " + vod.EndDate);
                }
            } catch (Exception e) {
                Logger.Error(e.ToString());
            }

            string testCategory = "uncategorized";
            Logger.Info("Checking LiveCategoriesRequest");
            try {
                LiveCategoriesRequest req = new LiveCategoriesRequest();
                List<CategoryStreams> live = req.getResponse();
                List<string> names = new List<string>();
                int count = 0;
                Random r = new Random();
                foreach (CategoryStreams cat in live) {
                    names.Add(cat.Category.Name);
                    // Select largest category, randomly select between categories of same size
                    if ((cat.Streams.Count > count) || ((cat.Streams.Count == count) && (r.Next(2) == 1))) {
                        testCategory = cat.Category.ID;
                        count = cat.Streams.Count;
                    }
                }
                Logger.Info("Currently live streams in: " + string.Join(", ", names));
            } catch (Exception e) {
                Logger.Error(e.ToString());
            }

            Logger.Info("Checking LivestreamsByCategoryRequest");
            try {
                LivestreamsByCategoryRequest req = new LivestreamsByCategoryRequest(testCategory);
                CategoryStreams live = req.getResponse();
                Logger.Info("Found " + live.Streams.Count + " streams currently under " + live.Category.Name);
            } catch (Exception e) {
                Logger.Error(e.ToString());
            }

            Logger.Info("Checking CategoryRequest");
            try {
                CategoryRequest req = new CategoryRequest(testCategory);
                BrimeCategory cat = req.getResponse();
                Logger.Info("Checking category " + cat.Name + " in group " + cat.Type);
                Logger.Info(cat.Summary);
                Logger.Trace(cat.toJSON());
            } catch (Exception e) {
                Logger.Error(e.ToString());
            }

            Logger.Info("Checking GlobalEmotesRequest");
            try {
                GlobalEmotesRequest req = new GlobalEmotesRequest();
                List<BrimeEmoteSet> sets = req.getResponse();
                foreach (BrimeEmoteSet eset in sets) {
                    List<string> emotes = new List<string>();
                    foreach (BrimeEmote e in eset.Emotes) emotes.Add(e.Name);
                    Logger.Info("Global Emote Set <" + eset.Name + ">: " + string.Join(", ", emotes));
                }
            } catch (Exception e) {
                Logger.Error(e.ToString());
            }

            /* Appears that this endpoint is not currently operational? Returning Internal Server Error
             * Have tried both the set ID from API doc (which appears to be global emote set, and set ID
             * returned from channel request (geeken).
            string emoteID = "607b681d05cac13b60fe161c";
            Logger.Info("Checking EmoteSetRequest");
            try {
                EmoteSetRequest req = new EmoteSetRequest(emoteID);
                BrimeEmoteSet eset = req.getResponse();
                List<string> emotes = new List<string>();
                foreach (BrimeEmote e in eset.Emotes) emotes.Add(e.Name);
                Logger.Info("Emote Set <" + eset.Name + ">: " + string.Join(", ", emotes));
            } catch (Exception e) {
                Logger.Error(e.ToString());
            }
            */
            Logger.Info("Skipping checks for EmoteSetRequest - appears to be a server error with this endpoint");

            Logger.Info("Checking ChannelEmotesRequest");
            try {
                ChannelEmotesRequest req = new ChannelEmotesRequest(channelName);
                List<BrimeEmote> eset = req.getResponse();
                List<string> emotes = new List<string>();
                foreach (BrimeEmote e in eset) {
                    emotes.Add(e.Name);
                }
                Logger.Info("Emotes for <" + channelName + ">: " + string.Join(", ", emotes));
            } catch (Exception e) {
                Logger.Error(e.ToString());
            }

            Logger.Info("Checking GlobalBadgesRequest");
            try {
                GlobalBadgesRequest req = new GlobalBadgesRequest();
                Dictionary<UserRoles, BrimeBadge> badges = req.getResponse();
                foreach (UserRoles role in badges.Keys) {
                    Logger.Info("Badge for role <" + Enum.GetName(typeof(UserRoles), role) + ">: " + badges[role].ImageLink);
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
