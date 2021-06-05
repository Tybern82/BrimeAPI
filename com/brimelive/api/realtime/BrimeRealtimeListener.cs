#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrimeAPI.com.brimelive.api.realtime {

    public interface BrimeRealtimeListener {
        public void onOpen();
        public void onClose();
        public void onFollow(string username, string id);
        public void onJoin(string username);
        public void onLeave(string username);
        public void onChat(BrimeChatMessage chatMessage);
        public void onSubscribe(string username, string userId, bool isResub);
    }
}
