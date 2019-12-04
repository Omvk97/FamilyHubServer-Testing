using System.Net;
using System.Threading.Tasks;
using API.Contracts.V1;
using API.DTO.InputDTOs.V1.Identity;
using API.DTO.OutputDTOs.V1.Identity;
using API.IntegrationTests.Extensions;
using FluentAssertions;
using Xunit;

namespace API.IntegrationTests
{
    public class IdentityControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public IdentityControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Login_WithEmptyBody_Returns400BadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var route = ApiRoutes.Identity.ControllerRoute + "/" + ApiRoutes.Identity.Login;
            var response = await client.PostAsJsonAsync(route, new LoginDTO
            {
                // EMPTY
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }

        [Fact]
        public async Task Login_WithCorrectBody_ReturnsOkAndJWT()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act

            // TestUser Registration
            var routeRegister = ApiRoutes.Identity.ControllerRoute + "/" + ApiRoutes.Identity.Register;
            var testUser = new RegisterDTO
            {
                Email = "test@example.com",
                Password = "Password1.!",
                Name = "Mr. Test"
            };
            var registerResponse = await client.PostAsJsonAsync(routeRegister, testUser);
            registerResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // Login
            var route = ApiRoutes.Identity.ControllerRoute + "/" + ApiRoutes.Identity.Login;
            var response = await client.PostAsJsonAsync(route, new LoginDTO
            {
                Email = testUser.Email,
                Password = testUser.Password
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var loginResponse = await response.Content.ReadAsJsonAsync<SucessLoginDTO>();
            loginResponse.Jwt.Should().NotBeEmpty();
        }
    }
}
