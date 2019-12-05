using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Contracts.V1;
using API.DTO.V1.InputDTOs.UserDTOs;
using API.DTO.V1.OutputDTOs.UserDTOs;
using API.IntegrationTests.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace API.IntegrationTests
{
    public class UserControllerTests : IntegrationTest
    {
        //[Fact]
        //public async Task UserRoutes_InValidCredentials_ReturnsUnauthorized()
        //{
        //    // Arrange
        //    TestClient.DefaultRequestHeaders.Authorization
        //                 = new AuthenticationHeaderValue("Bearer", "asdasdasd");
        //    // Act
        //    var responseGetAllUsers = await TestClient.GetAsync(ApiRoutes.UserRoutes.GetAllUsers);
        //    var responseGetUser = await TestClient.GetAsync(ApiRoutes.UserRoutes.GetUser);
        //    var responseUpdateUser = await TestClient.PatchAsJsonAsync(ApiRoutes.UserRoutes.UpdateUser, new UpdateUserDTO { });
        //    var responseDeleteUser = await TestClient.DeleteAsync(ApiRoutes.UserRoutes.DeleteUser);
        //    var responseGetUserFamily = await TestClient.GetAsync(ApiRoutes.UserRoutes.GetUserFamily);
        //    var responseGetUserEvents = await TestClient.GetAsync(ApiRoutes.UserRoutes.GetUserEvents);

        //    // Assert
        //    responseGetAllUsers.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        //    responseGetUser.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        //    responseUpdateUser.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        //    responseDeleteUser.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        //    responseGetUserFamily.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        //    responseGetUserEvents.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        //}

        //[Fact]
        //public async Task GetAllUsers_ValidCredentialsWithoutInclusions_ReturnsOkAndUsers()
        //{
        //    // Arrange
        //    var userRegistration = await CreateTestUserInDb(false);

        //    TestClient.DefaultRequestHeaders.Authorization
        //                 = new AuthenticationHeaderValue("Bearer", userRegistration.Token);

        //    // Act
        //    var response = await TestClient.GetAsync(ApiRoutes.UserRoutes.GetAllUsers);
        //    var responseContent = await response.Content.ReadAsJsonAsync<ICollection<SuccessGetUserDTO>>();

        //    // Assert
        //    response.StatusCode.Should().Be(StatusCodes.Status200OK);

        //    var testUserFromResponse = responseContent.FirstOrDefault(u => u.Id == userRegistration.User.Id);

        //    testUserFromResponse.Should().NotBeNull();

        //    testUserFromResponse.Family.Should().BeNull();
        //    testUserFromResponse.Events.Should().BeNullOrEmpty();
        //}

        //[Fact]
        //public async Task GetAllUsers_ValidCredentialsWithInclusion_ReturnsOkAndUsers()
        //{
        //    // Arrange
        //    var userRegistration = await CreateTestUserInDb(false);

        //    TestClient.DefaultRequestHeaders.Authorization
        //                 = new AuthenticationHeaderValue("Bearer", userRegistration.Token);
        //    // Act
        //    var response = await TestClient.GetAsync(ApiRoutes.UserRoutes.GetAllUsers + "?includeFamily=true&includeEvents=true");
        //    var responseContent = await response.Content.ReadAsJsonAsync<ICollection<SuccessGetUserDTO>>();

        //    // Assert
        //    response.StatusCode.Should().Be(StatusCodes.Status200OK);

        //    var testUserFromResponse = responseContent.FirstOrDefault(u => u.Id == userRegistration.User.Id);
        //    userRegistration.Should().NotBeNull();

        //    // TODO: Create test user with family
        //    //testUser.Family.Should().NotBeNull();
        //    testUserFromResponse.Events.Should().NotBeNull();
        //}

        //[Fact]
        //public async Task GetUser_ValidCredentials_ReturnsOkAndUser()
        //{
        //    // Arrange
        //    var client = _factory.CreateClient();
        //    var testRegistration = await TestUser.CreateTestUserInDb(client, false);

        //    client.DefaultRequestHeaders.Authorization
        //                 = new AuthenticationHeaderValue("Bearer", testRegistration.Token);
        //    // Act
        //    var response = await client.GetAsync(ApiRoutes.UserRoutes.GetUser);
        //    var responseContent = await response.Content.ReadAsJsonAsync<SuccessGetUserDTO>();

        //    // Assert
        //    response.StatusCode.Should().Be(StatusCodes.Status200OK);

        //    responseContent.Should().NotBeNull();
        //    responseContent.Email.Should().Be(TestUser.Email);
        //    responseContent.Name.Should().Be(TestUser.Name);
        //}

        //[Fact]
        //public async Task UpdateUser_ValidCredentialsAndUpdates_ReturnsOkAndUpdatedUser()
        //{
        //    // Arrange
        //    var client = _factory.CreateClient();

        //    var testRegistration = await TestUser.CreateTestUserInDb(client, false);

        //    client.DefaultRequestHeaders.Authorization
        //     = new AuthenticationHeaderValue("Bearer", testRegistration.Token);

        //    // Act

        //    // TODO: Figure out how to test invalid email, invalid name, invalid password .... Without making a bunch of integration tests
        //    var updateUserDTO = new UpdateUserDTO
        //    {
        //        // TODO: Add valid family Id here as well
        //        NewEmail = "Test@example.com",
        //        NewName = "my new mr. test name",
        //        NewPassword = "Whaaaaaat123.?",
        //        NewProfileColor = "#ffffff"
        //    };

        //    var response = await client.PatchAsJsonAsync(ApiRoutes.UserRoutes.UpdateUser, updateUserDTO);
        //    var responseContent = await response.Content.ReadAsJsonAsync<SuccessGetUserDTO>();

        //    // Assert
        //    response.StatusCode.Should().Be(StatusCodes.Status200OK);

        //    // Check new values
        //    responseContent.Should().NotBeNull();
        //    responseContent.Email.Should().Be(updateUserDTO.NewEmail);
        //    responseContent.Name.Should().Be(updateUserDTO.NewName);
        //    responseContent.ProfileColor.Should().Be(updateUserDTO.NewProfileColor);

        //    // Check if login with new values is possible
        //    var loginResponse = await client.PostAsJsonAsync(ApiRoutes.IdentityRoutes.Login, new LoginDTO
        //    {
        //        Email = updateUserDTO.NewEmail,
        //        Password = updateUserDTO.NewPassword
        //    });
        //    var loginResponseContent = await response.Content.ReadAsJsonAsync<SucessLoginDTO>();

        //    // Assert
        //    loginResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
        //    loginResponseContent.Token.Should().NotBeEmpty();
        //}
    }
}
