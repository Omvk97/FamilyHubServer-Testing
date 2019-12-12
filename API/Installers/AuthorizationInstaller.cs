using System;
using System.Text;
using API.Helpers.JwtHelper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Installers
{
    public class AuthorizationInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var jwtKey = Encoding.ASCII.GetBytes(configuration["JWT:KEY"]);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = JwtHelper
                .GetTokenValidationParameters(configuration);
            });

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("AdministratorOnly", policy => policy.RequireClaim("accountType", AccountType.Administrator.ToString()));
            });
        }
    }
}
