using System;
using System.Net;
using System.Threading.Tasks;
using API.Contracts.V1;
using API.DTO.InputDTOs.V1.IdentityDTOs;
using API.DTO.OutputDTOs.V1.IdentityDTOs;
using API.IntegrationTests.Extensions;
using API.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace API.IntegrationTests
{
    public class IdentityControllerTests : IntegrationTest
    {

        [Fact]
        public async Task Register_IncorrectBody_Returns400BadRequest()
        {
            // Arrange
            var userRegistration = await CreateTestUserInDb(false);

            // Act
            var responseEmpty = await TestClient.PostAsJsonAsync(ApiRoutes.IdentityRoutes.Register, new RegisterDTO
            {
                // EMPTY
            });

            var responseInvalidEmail = await TestClient.PostAsJsonAsync(ApiRoutes.IdentityRoutes.Register, new RegisterDTO
            {
                Email = "asd.dk"
            });

            var responseInvalidPassword = await TestClient.PostAsJsonAsync(ApiRoutes.IdentityRoutes.Register, new RegisterDTO
            {
                Email = "succes@email.com",
                Password = "asd",
                Name = "Mr. test"
            });

            // Assert
            responseEmpty.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseInvalidEmail.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseInvalidPassword.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_InCorrectBody_Returns400BadRequest()
        {
            // Arrange
            var userRegistration = await CreateTestUserInDb(false);

            // Act
            var responseEmpty = await TestClient.PostAsJsonAsync(ApiRoutes.IdentityRoutes.Login, new LoginDTO
            {
                // EMPTY
            });

            var responseInvalidEmail = await TestClient.PostAsJsonAsync(ApiRoutes.IdentityRoutes.Login, new LoginDTO
            {
                Email = "asd.dk"
            });

            // Assert
            responseEmpty.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseInvalidEmail.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_CorrectBody_ReturnsOkAndJWT()
        {
            // Arrange
            var userRegistration = await CreateTestUserInDb(false);
            
            // Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.IdentityRoutes.Login, new LoginDTO
            {
                Email = TestUser.Email,
                Password = TestUser.Password
            });
            var loginResponse = await response.Content.ReadAsJsonAsync<SucessLoginDTO>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            loginResponse.Token.Should().NotBeEmpty();
        }
    }
}
