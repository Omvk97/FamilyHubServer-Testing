using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.V1.Contracts;
using API.V1.DTO.InputDTOs.UserDTOs;
using API.V1.DTO.OutputDTOs.EventDTOs;
using API.V1.DTO.OutputDTOs.FamilyDTOs;
using API.V1.DTO.OutputDTOs.UserDTOs;
using API.IntegrationTests.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace API.IntegrationTests
{
    public class UserControllerTests : IntegrationTest
    {
        // TODO: Test admin routes, unauthorized with an account who isn't admin
        // TODO: Test routes with a non existant user (get user by id, update user etc.)
        [Fact]
        public async Task UserRoutes_InValidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            TestClient.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", "asdasdasd");
            // Act
            var responseGetAllUsers = await TestClient.GetAsync(ApiRoutes.UserRoutes.GetAllUsers);
            var responseGetUser = await TestClient.GetAsync(ApiRoutes.UserRoutes.GetUser);
            var responseUpdateUser = await TestClient.PatchAsJsonAsync(ApiRoutes.UserRoutes.UpdateUser, new UpdateUserDTO { });
            var responseDeleteUser = await TestClient.DeleteAsync(ApiRoutes.UserRoutes.DeleteUser);
            var responseGetUserFamily = await TestClient.GetAsync(ApiRoutes.UserRoutes.GetUserFamily);
            var responseGetUserEvents = await TestClient.GetAsync(ApiRoutes.UserRoutes.GetUserEvents);

            // Assert
            responseGetAllUsers.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            responseGetUser.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            responseUpdateUser.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            responseDeleteUser.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            responseGetUserFamily.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            responseGetUserEvents.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task GetAllUsers_ValidCredentialsWithoutInclusions_ReturnsOkAndUsers()
        {
            // Arrange
            var userRegistration = await CreateTestUserInDb(false, false);

            TestClient.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", userRegistration.Token);

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.UserRoutes.GetAllUsers);
            var responseContent = await response.Content.ReadAsJsonAsync<ICollection<SuccessGetUserDTO>>();

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var testUserFromResponse = responseContent.FirstOrDefault(u => u.Id == userRegistration.User.Id);

            testUserFromResponse.Should().NotBeNull();

            testUserFromResponse.Family.Should().BeNull();
            testUserFromResponse.Events.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetAllUsers_ValidCredentialsWithInclusion_ReturnsOkAndUsers()
        {
            // Arrange
            var userRegistration = await CreateTestUserInDb(false, false);

            TestClient.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", userRegistration.Token);
            // Act
            var response = await TestClient.GetAsync(ApiRoutes.UserRoutes.GetAllUsers + "?includeFamily=true&includeEvents=true");
            var responseContent = await response.Content.ReadAsJsonAsync<ICollection<SuccessGetUserDTO>>();

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var testUserFromResponse = responseContent.FirstOrDefault(u => u.Id == userRegistration.User.Id);
            userRegistration.Should().NotBeNull();

            // TODO: Create test user with family
            //testUser.Family.Should().NotBeNull();
            testUserFromResponse.Events.Should().NotBeNull();
        }

        [Fact]
        public async Task GetUser_ValidCredentials_ReturnsOkAndUser()
        {
            // Arrange
            var userRegistration = await CreateTestUserInDb(false, false);

            TestClient.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", userRegistration.Token);
            // Act
            var response = await TestClient.GetAsync(ApiRoutes.UserRoutes.GetUser);
            var responseContent = await response.Content.ReadAsJsonAsync<SuccessGetUserDTO>();

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            responseContent.Should().NotBeNull();
            responseContent.Email.Should().Be(TestUser.Email);
            responseContent.Name.Should().Be(TestUser.Name);
        }

        [Fact]
        public async Task UpdateUser_ValidCredentialsAndUpdates_ReturnsOkAndUpdatedUser()
        {
            // Arrange
            var userRegistration = await CreateTestUserInDb(false, false);

            TestClient.DefaultRequestHeaders.Authorization
             = new AuthenticationHeaderValue("Bearer", userRegistration.Token);

            // Act
            // TODO: Figure out how to test invalid email, invalid name, invalid password .... Without making a bunch of integration tests
            var updateUserDTO = new UpdateUserDTO
            {
                // TODO: Add valid family Id here as well
                NewEmail = "newExample@example.com",
                NewName = "New Mr. test name",
                NewPassword = "Whaaaaaat123.?",
                NewProfileColor = "#ffffff"
            };

            var response = await TestClient.PatchAsJsonAsync(ApiRoutes.UserRoutes.UpdateUser, updateUserDTO);
            var responseContent = await response.Content.ReadAsJsonAsync<SuccessGetUserDTO>();

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            // Check new values
            responseContent.Email.Should().Be(updateUserDTO.NewEmail);
            responseContent.Name.Should().Be(updateUserDTO.NewName);
            responseContent.ProfileColor.Should().Be(updateUserDTO.NewProfileColor);
        }

        [Fact]
        public async Task DeleteUser_ValidCredentials_ReturnsOkAndDeletedUser()
        {
            // Arrange
            var userRegistration = await CreateTestUserInDb(false, false);

            TestClient.DefaultRequestHeaders.Authorization
             = new AuthenticationHeaderValue("Bearer", userRegistration.Token);

            // Act
            var response = await TestClient.DeleteAsync(ApiRoutes.UserRoutes.DeleteUser);
            var responseContent = await response.Content.ReadAsJsonAsync<SuccessGetUserDTO>();

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            // Check new values
            responseContent.Email.Should().Be(userRegistration.User.Email);
            responseContent.Name.Should().Be(userRegistration.User.Name);
        }

        [Fact]
        public async Task GetUserFamily_ValidCredentials_ReturnsOkAndFamily()
        {
            // Arrange
            var userRegistration = await CreateTestUserInDb(true, false);

            TestClient.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", userRegistration.Token);

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.UserRoutes.GetUserFamily);
            var responseContent = await response.Content.ReadAsJsonAsync<SuccessGetFamilyDTO>();

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            // Check if user is in family
            var testUserFromResponse = responseContent.Members.FirstOrDefault(u => u.Id == userRegistration.User.Id);

            testUserFromResponse.Should().NotBeNull();
        }

        [Fact]
        public async Task GetUserEvents_ValidCredentialsParticipantCheck_ReturnsOkAndEvents()
        {
            // Arrange
            var userRegistration = await CreateTestUserInDb(false, true);

            TestClient.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", userRegistration.Token);

            // Act
            // user 1 checks events to only find one event
            var response = await TestClient.GetAsync(ApiRoutes.UserRoutes.GetUserEvents);
            var responseContent = await response.Content.ReadAsJsonAsync<ICollection<SuccessGetEventDTO>>();

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            responseContent.Should().NotBeNullOrEmpty();
            // Check if user is a participant in all the events
            foreach (var eventParticipant in responseContent.Select(e => e.Participants))
            {
                // Check that user is a participant in the event
                var userAsParticipant = eventParticipant.SingleOrDefault(u => u.Id == userRegistration.User.Id);
                userAsParticipant.Should().NotBeNull();
            }
        }
    }
}
