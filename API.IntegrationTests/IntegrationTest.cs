using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using API.Contracts.V1;
using API.Data;
using API.IntegrationTests.Extensions;
using API.DTO.InputDTOs.V1.IdentityDTOs;
using Microsoft.AspNetCore.Http;
using FluentAssertions;
using API.DTO.OutputDTOs.V1.IdentityDTOs;
using System.Linq;

namespace API.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;
        private readonly IServiceProvider _serviceProvider;
        public static RegisterDTO TestUser = new RegisterDTO
        {
            Email = "test@example.com",
            Password = "Password1.!",
            Name = "Mr. Test"
        };

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Remove the app's ApplicationDbContext registration.
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                typeof(DbContextOptions<DataContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }
                        // Note: xUnit creates a new instance of the test class for each method marked [Fact] which means this constructor will be called
                        // for each test method. A random database is then used for each test method, so they don't interfere
                        var randomDatabaseName = Guid.NewGuid().ToString();
                        services.AddDbContext<DataContext>(options => options.UseNpgsql("Host=localhost;Port=5433;Database=" + randomDatabaseName + "test-family-hub-server;Username=test-admin-user;Password=test-admin-password"));

                    });
                });

            _serviceProvider = appFactory.Services;
            TestClient = appFactory.CreateClient();
        }

        public async Task<SucessRegisterDTO> CreateTestUserInDb(bool withFamily)
        {
            var registerResponse = await TestClient.PostAsJsonAsync(ApiRoutes.IdentityRoutes.Register, TestUser);
            registerResponse.StatusCode.Should().Be(StatusCodes.Status201Created);
            var responseContent = await registerResponse.Content.ReadAsJsonAsync<SucessRegisterDTO>();
            responseContent.Token.Should().NotBeNullOrEmpty();
            return responseContent;
        }
    }
}
