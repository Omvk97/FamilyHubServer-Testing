﻿namespace API.Contracts.V1
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
            public const string GetAllUsers = ControllerRoute + "/users";
            public const string GetUser = ControllerRoute;
            public const string UpdateUser = ControllerRoute;
            public const string DeleteUser = ControllerRoute;
            public const string GetUserFamily = ControllerRoute + "/family";
            public const string GetUserEvents = ControllerRoute + "/events";
        }
    }
}
