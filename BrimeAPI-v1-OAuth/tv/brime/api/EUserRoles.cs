using System;

namespace BrimeAPIv1OAuth.tv.brime.api {
    [Flags] public enum EUserRoles : ushort {
        NONE = 0,
        STAFF = 1,
        PLATFORM_MOD = 2,
        VERIFIED = 4,
        BRIME_PRO = 8,
        BOT = 16,
        USER = 32,
        CREATOR = 64,
        SUBSCRIBER = 128,
        FOLLOWER = 256,
        MODERATOR = 512
    }
}