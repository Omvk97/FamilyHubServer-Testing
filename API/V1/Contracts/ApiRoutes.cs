namespace API.V1.Contracts
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class IdentityRoutes
        {
            private const string ControllerRoute = Base + "/identity";

            public const string Login = ControllerRoute + "/login";

            public const string Register = ControllerRoute +  "/register";
        }

        public static class UserRoutes
        {
            private const string ControllerRoute = Base + "/user";

            public const string GetAllUsers = Base + "/users";

            public const string GetUser = ControllerRoute;

            public const string UpdateUser = ControllerRoute;

            public const string DeleteUser = ControllerRoute;

            public const string GetUserFamily = ControllerRoute + "/family";

            public const string GetUserEvents = ControllerRoute + "/events";
        }

        public static class FamilyRoutes
        {
            private const string ControllerRoute = Base + "/family";

            public const string CreateFamily = ControllerRoute;

            public const string GetAllFamilies = Base + "/families";

            public const string GetFamily = ControllerRoute;

            public const string UpdateFamily = ControllerRoute;

            public const string DeleteFamily = ControllerRoute;
        }

        public static class EventRoutes
        {
            private const string ControllerRoute = Base + "/event";

            public const string CreateEvent = ControllerRoute;

            public const string GetAllEvents = Base + "/events";

            public const string GetEvent = ControllerRoute;

            public const string UpdateEvent = ControllerRoute;

            public const string DeleteEvent = ControllerRoute;
        }
    }
}
