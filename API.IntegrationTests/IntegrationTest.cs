using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using API.V1.Contracts;
using API.Data;
using API.IntegrationTests.Extensions;
using API.V1.DTO.InputDTOs.IdentityDTOs;
using Microsoft.AspNetCore.Http;
using FluentAssertions;
using API.V1.DTO.OutputDTOs.IdentityDTOs;
using System.Linq;
using API.V1.DTO.InputDTOs.FamilyDTOs;
using API.V1.DTO.OutputDTOs.FamilyDTOs;
using System.Net.Http.Headers;
using API.V1.DTO.InputDTOs.EventDTOs;
using API.V1.DTO.OutputDTOs.EventDTOs;

namespace API.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;
        protected WebApplicationFactory<Startup> _factory;
        private readonly IServiceProvider _serviceProvider;
        protected readonly RegisterDTO TestUser = new RegisterDTO
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
            _factory = appFactory;
            TestClient = appFactory.CreateClient();
        }

        public async Task<SucessRegisterDTO> CreateTestUserInDb(bool withFamily, bool withEvent, RegisterDTO testUser = null)
        {
            if (testUser == null) testUser = TestUser;

            var registerResponse = await TestClient.PostAsJsonAsync(ApiRoutes.IdentityRoutes.Register, testUser);
            registerResponse.StatusCode.Should().Be(StatusCodes.Status201Created);
            var registerResponseContent = await registerResponse.Content.ReadAsJsonAsync<SucessRegisterDTO>();
            registerResponseContent.Token.Should().NotBeNullOrEmpty();


            if (withFamily)
            {
                var familyDTO = new CreateFamilyDTO
                {
                    Name = "Mr. Test family"
                };

                TestClient.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", registerResponseContent.Token);

                var familyResponse = await TestClient.PostAsJsonAsync(ApiRoutes.FamilyRoutes.CreateFamily, familyDTO);
                familyResponse.StatusCode.Should().Be(StatusCodes.Status201Created);
                var familyResponseContent = await familyResponse.Content.ReadAsJsonAsync<SuccessGetFamilyDTO>();

                var testUserFromResponse = familyResponseContent.Members.FirstOrDefault(u => u.Id == registerResponseContent.User.Id);

                testUserFromResponse.Should().NotBeNull();
                familyResponseContent.Name.Should().Be(familyDTO.Name);
            }

            if (withEvent)
            {
                 // User creates a basic new event
                TestClient.DefaultRequestHeaders.Authorization
                            = new AuthenticationHeaderValue("Bearer", registerResponseContent.Token);
                var userEvent = new CreateEventDTO
                {
                    Title = "Event test",
                    Description = "Event description test",
                    AllDay = new DateTime(2019, 12, 24)
                };

                var userEventResponse = await TestClient.PostAsJsonAsync(ApiRoutes.EventRoutes.CreateEvent, userEvent);

                userEventResponse.StatusCode.Should().Be(StatusCodes.Status201Created);
                var userEventResponseContent = await userEventResponse.Content.ReadAsJsonAsync<SuccessGetEventDTO>();

                userEventResponseContent.Should().NotBeNull();
            }

            return registerResponseContent;
        }
    }
}
