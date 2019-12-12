using System.Text;
using API.Data;
using API.V1.Repositories.EventRepo;
using API.V1.Repositories.FamilyRepo;
using API.V1.Repositories.UserRepo;
using API.V1.DTO;
using API.Helpers.Hashing;
using API.Helpers.JwtHelper;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using API.V1.Repositories.IdentityRepo;
using System;
using Npgsql;
using API.Data.Models;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            }
            );

            NpgsqlConnection.GlobalTypeMapper.MapEnum<WeekDay>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<FrequencyOption>();

            services.AddDbContext<DataContext>(options => options.UseNpgsql(_configuration["DATABASE_CONNECTION_STRING"]));

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

            // Authorization - JWT
            var jwtKey = Encoding.ASCII.GetBytes(_configuration["JWT:KEY"]);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = JwtHelper
                .GetTokenValidationParameters(_configuration);
            });

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("AdministratorOnly", policy => policy.RequireClaim("accountType", AccountType.Administrator.ToString()));
            });

            services.AddCors();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "FamilyHub API",
                    Description = "REST API for Family Hub at VK-Media",
                    Contact = new OpenApiContact
                    {
                        Name = "Oliver Marco van Komen",
                        Email = string.Empty,
                        Url = new Uri("https://github.com/omvk97")
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                Console.WriteLine("IF THIS IS RUN I AM IN DEVELOPMENT :O");
                app.UseDeveloperExceptionPage();
            }

            UpdateDatabase(app);

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FamilyHub API v1");
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<DataContext>();
            context.Database.EnsureCreated();
        }
    }
}
