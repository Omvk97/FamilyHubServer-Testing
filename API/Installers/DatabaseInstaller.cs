using System;
using API.Data;
using API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace API.Installers
{
    public class DatabaseInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<WeekDay>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<FrequencyOption>();

            services.AddDbContext<DataContext>(options => options.UseNpgsql(configuration["DATABASE_CONNECTION_STRING"]));
        }
    }
}
