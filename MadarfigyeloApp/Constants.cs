namespace MadarfigyeloApp
{
    public static class Constants
    {
        // URLs
        public const string LocalBaseUrlHttp = "http://10.0.2.2:5000/api";
        public const string LocalBaseUrlHttps = "https://10.0.2.2:5001/api";
        public const string BaseUrlHttp = "http://wolfkonig-001-site1.site4future.com/api";
        public const string BaseUrlHttps = "https://wolfkonig-001-site1.site4future.com/api";

        // Routes
        public const string RouteNewLatogatas = "newLatogatas";
        public const string RouteNewOdutelep = "newOdutelep";
        public const string RouteNewOdu = "newOdu";
        public const string RouteLogin = "login";
        public const string RouteHome = "home";
        public const string RouteOdutelepek = "odutelep";
        public const string RouteOduk = "odu";
        public const string RouteLatogatasok = "latogatas";
        public const string RouteMainPage = "mainPage";

        // Preferences keys
        public const string KeyLoggedInUserEmail = "loggedInUserEmail";
        public const string KeyLoggedInUserFirstName = "loggedInUserFirstName";
        public const string KeyLoggedInUserLastName = "loggedInUserLastName";

        public const string KeyLoggedInUserPassword = "loggedInUserPassword";
        public const string KeyLoggedInUserToken = "loggedInUserToken";
        public const string KeyLoggedInUserTokenExpDate = "loggedInUserTokenExpDate";

        // Paremeters
        public const string ParamOduTelepId = "oduTelepId";
        public const string ParamOduId = "oduId";
    }
}
