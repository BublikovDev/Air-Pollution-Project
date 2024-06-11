namespace Client.Static
{
    internal static class APIEndpoints
    {
#if DEBUG
        internal const string ServerBaseUrl = "https://localhost:7059";
#else
        internal const string ServerBaseUrl = "https://airpollutionserver.azurewebsites.net";
#endif

        internal readonly static string s_refreshToken = $"{ServerBaseUrl}/api/auth/refreshtoken";
        internal readonly static string s_signIn = $"{ServerBaseUrl}/api/auth/signin";
        internal readonly static string s_signUp = $"{ServerBaseUrl}/api/auth/signup";
        internal readonly static string s_logOut = $"{ServerBaseUrl}/api/auth/logout";
        internal readonly static string s_userGetData = $"{ServerBaseUrl}/api/user/GetById";
        internal readonly static string s_userPutData = $"{ServerBaseUrl}/api/user/userdata";


        internal readonly static string s_ = $"{ServerBaseUrl}/api/data";

        internal readonly static string s_getData = string.Format("{0}/api/data/getdata/{1}", ServerBaseUrl, "{0}");
        internal readonly static string s_putData = string.Format("{0}/api/data/putdata/{1}", ServerBaseUrl, "{0}");
    }
}
