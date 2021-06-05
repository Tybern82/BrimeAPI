#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrimeAPI.com.brimelive.api.realtime {

    public class ViewCountTracker : BrimeRealtimeListener {
        public void onOpen() { }
        public void onClose() { }
        public void onFollow(string username, string id) { }
        public void onChat(BrimeChatMessage chatMessage) { }

        public void onSubscribe(string username, string userID, bool isResub) { }

        public int ViewCount { get; set; }

        public ViewCountTracker(int viewCount) {
            this.ViewCount = viewCount;
        }

        public void onJoin(string username) { this.ViewCount++; }
        public void onLeave(string username) { this.ViewCount--; }
    }
}
