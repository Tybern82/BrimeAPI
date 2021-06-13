#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrimeAPI.com.brimelive.api.realtime {
    /// <summary>
    /// Interface for BrimeAPI Realtime callbacks
    /// </summary>
    public interface BrimeRealtimeListener {
        /// <summary>
        /// Called when API connection is opened
        /// </summary>
        public void onOpen();

        /// <summary>
        /// Called when API connection is closed
        /// </summary>
        public void onClose();

        /// <summary>
        /// Called when a Follower notification is received
        /// </summary>
        /// <param name="username">username of new follower</param>
        /// <param name="id">user ID of new follower</param>
        public void onFollow(string username, string id);

        /// <summary>
        /// Called when new user joins the chat/stream
        /// </summary>
        /// <param name="username">name of new viewer/chatter</param>
        public void onJoin(string username);

        /// <summary>
        /// Called when user leaves the chat/stream
        /// </summary>
        /// <param name="username">name of departing viewer/chatter</param>
        public void onLeave(string username);

        /// <summary>
        /// Called when receiving a new chat message
        /// </summary>
        /// <param name="chatMessage">chat message received</param>
        public void onChat(BrimeChatMessage chatMessage);

        /// <summary>
        /// Called when deleting a chat message
        /// </summary>
        /// <param name="messageID">ID of message to delete</param>
        public void onDeleteChat(string messageID);

        /// <summary>
        /// Called when new user has subscribed to the channel
        /// </summary>
        /// <param name="username">name of new subscriber</param>
        /// <param name="userId">user ID of new subscriber</param>
        /// <param name="isResub">identify whether this is a re-subscription</param>
        public void onSubscribe(string username, string userId, bool isResub);

        /// <summary>
        /// Called when a channel has raided
        /// </summary>
        /// <param name="channelName">name of the raiding channel</param>
        /// <param name="channelID">channel ID of raider</param>
        /// <param name="viewerCount">number of viewers in the raid</param>
        public void onRaid(string channelName, string channelID, int viewerCount);
    }
}
