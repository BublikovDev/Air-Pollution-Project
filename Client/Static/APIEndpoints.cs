namespace Client.Static
{
    internal static class APIEndpoints
    {
#if DEBUG
        internal const string ServerBaseUrl = "https://localhost:7059";
#else
        //Do prod url
#endif

        internal readonly static string s_refreshToken = $"{ServerBaseUrl}/api/auth/refreshtoken";
        internal readonly static string s_signIn = $"{ServerBaseUrl}/api/auth/signin";
        internal readonly static string s_signUp = $"{ServerBaseUrl}/api/auth/signup";
        internal readonly static string s_logOut = $"{ServerBaseUrl}/api/auth/logout";
        internal readonly static string s_userGetData = $"{ServerBaseUrl}/api/user/userdata";
        internal readonly static string s_userPutData = $"{ServerBaseUrl}/api/user/userdata";


        internal readonly static string s_ = $"{ServerBaseUrl}/api/user/userdata";

        //internal readonly static string s_liveStreamGetByIdFormat = string.Format("{0}/{1}/GetLiveStream/{2}", ServerBaseUrl, LiveStreamRoute, "{0}");
        //internal readonly static string s_liveStreamGetAllFormat = string.Format("{0}/{1}/GetAllLiveStreams", ServerBaseUrl, LiveStreamRoute);
        //internal readonly static string s_liveStreamCreateFormat = string.Format("{0}/{1}/CreateLiveStream", ServerBaseUrl, LiveStreamRoute);
        //internal readonly static string s_liveStreamGetThumbnailByIdFormat = string.Format("{0}/{1}/GetThumbnailUrl/{2}", ServerBaseUrl, LiveStreamRoute, "{0}");
        //internal readonly static string s_liveStreamStartByIdFormat = string.Format("{0}/{1}/StartLiveStream/start/{2}", ServerBaseUrl, LiveStreamRoute, "{0}");
        //internal readonly static string s_liveStreamStopByIdFormat = string.Format("{0}/{1}/StopLiveStream/stop/{2}", ServerBaseUrl, LiveStreamRoute, "{0}");

    }
}
