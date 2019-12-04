using System;
namespace API.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Identity
        {
            public const string ControllerRoute = Base + "/identity";

            public const string Login = "login";

            public const string Register = Base + "register";

            public const string Refresh = Base + "refresh";
        }
    }
}
