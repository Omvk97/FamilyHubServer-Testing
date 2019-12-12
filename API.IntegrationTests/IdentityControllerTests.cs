using System;
using System.Threading.Tasks;
using API.V1.Contracts;
using API.V1.DTO.InputDTOs.IdentityDTOs;
using API.V1.DTO.OutputDTOs.IdentityDTOs;
using API.IntegrationTests.Extensions;
using FluentAssertions;
using Xunit;
using Microsoft.AspNetCore.Http;
using API.V1.DTO.OutputDTOs;

namespace API.IntegrationTests
{
    // Register User has been left out as this route is being used in almost all other test to create a test user
    public class IdentityControllerTests : IntegrationTest
    {
        [Fact]
        public async Task Register_IncorrectBody_Returns400BadRequest()
        {
            // Arrange
            var userRegistration = await CreateTestUserInDb(false, false);

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
            responseEmpty.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            responseInvalidEmail.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            responseInvalidPassword.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Login_CorrectBody_ReturnsOkAndJWT()
        {
            // Arrange
            var userRegistration = await CreateTestUserInDb(false, false);

            // Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.IdentityRoutes.Login, new LoginDTO
            {
                Email = TestUser.Email,
                Password = TestUser.Password
            });
            var loginResponse = await response.Content.ReadAsJsonAsync<SucessLoginDTO>();

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            loginResponse.Token.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Login_InCorrectBody_ReturnsBadRequestErrorInvalidLogin()
        {
            // Arrange
            var userRegistration = await CreateTestUserInDb(false, false);

            // Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.IdentityRoutes.Login, new LoginDTO
            {
                Email = "thisdoes@notexist.com",
                Password = TestUser.Password
            });
            var loginResponse = await response.Content.ReadAsJsonAsync<UserInputErrorDTO>();

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            loginResponse.ErrorMessage.Should().Be(ErrorMessages.InvalidLogin);
        }
    }
}
