using System;
using API.Helpers.Hashing;
using API.Helpers.JwtHelper;
using API.V1.DTO;
using API.V1.Repositories.EventRepo;
using API.V1.Repositories.FamilyRepo;
using API.V1.Repositories.IdentityRepo;
using API.V1.Repositories.UserRepo;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace API.Installers
{
    public class RepositoryAndHelpersInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            // Repository dependency injection
            services.AddScoped<IIdentityRepo, IdentityRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IFamilyRepo, FamilyRepo>();
            services.AddScoped<IEventRepo, EventRepo>();

            // Helper injection
            services.AddSingleton<IHashing, Hashing>();
            services.AddSingleton<IJwtHelper, JwtHelper>();

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddCors();
        }
    }
}
