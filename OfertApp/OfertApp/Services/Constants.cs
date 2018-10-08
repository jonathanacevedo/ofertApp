namespace OfertApp.Services
{
    public static class Constants
    {
        public static string AppName = "OfertApp";

		// OAuth
		// For Google login, configure at https://console.developers.google.com/
		public static string iOSClientId = "<insert IOS client ID here>";
		public static string AndroidClientId = "364014280769-n853ibkeagms631c6nmv73438jblb00o.apps.googleusercontent.com";

		// These values do not need changing
		public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
		public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
		public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v3/token";
		public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

        public const string IP = "http://192.168.7.191";

        // Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
        public static string iOSRedirectUrl = "<insert IOS redirect URL here>:/oauth2redirect";
        
        public static string AndroidRedirectUrl = "com.googleusercontent.apps.364014280769-n853ibkeagms631c6nmv73438jblb00o:/oauth2redirect";

        //public static string AndroidRedirectUrl = "<insert Android redirect URL here>:/oauth2redirect";
    }
}
