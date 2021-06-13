#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrimeAPI.com.brimelive.api.realtime {
    /// <summary>
    /// Brime Listener which processes a running count of viewers in chat.
    /// </summary>
    public class ViewCountTracker : BrimeRealtimeListener {
        /// <inheritdoc />
        public void onOpen() { }
        /// <inheritdoc />
        public void onClose() { }
        /// <inheritdoc />
        public void onFollow(string username, string id) { }
        /// <inheritdoc />
        public void onChat(BrimeChatMessage chatMessage) { }
        /// <inheritdoc />
        public void onDeleteChat(string messageID) { }
        /// <inheritdoc />
        public void onSubscribe(string username, string userID, bool isResub) { }
        /// <inheritdoc />
        public void onRaid(string channelName, string channelID, int viewerCount) { }

        /// <summary>
        /// Used to access the current view count. Public access to allow the RealtimeAPI to
        /// update from externally. This ensures the count is synchronized every 5 minutes
        /// (by default) to ensure we aren't missing any join/leave messages.
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// Create a new tracker instance, using the given initial view count
        /// </summary>
        /// <param name="viewCount">initial count of viewers</param>
        public ViewCountTracker(int viewCount) {
            this.ViewCount = viewCount;
        }

        /// <summary>
        /// Record join request to increase number of viewers
        /// </summary>
        /// <param name="username">ignored</param>
        public void onJoin(string username) { this.ViewCount++; }
        /// <summary>
        /// Record leave request to reduce number of viewers
        /// </summary>
        /// <param name="username">ignored</param>
        public void onLeave(string username) { this.ViewCount--; }
    }
}
