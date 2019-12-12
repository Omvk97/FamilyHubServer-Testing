using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.IntegrationTests.Extensions;
using API.V1.Contracts;
using API.V1.DTO.InputDTOs.FamilyDTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace API.IntegrationTests
{
    // Create Family is not included as a test as it is used in some others test for testing purposes.
    public class FamilyControllerTests : IntegrationTest
    {
        // TODO: Test admin routes, unauthorized with an account who isn't admin
        // TODO: Test to update family who does not exist, and find one etc.
        [Fact]
        public async Task FamilyRoutes_InvalidCredentials_Returns401UnAuthorized()
        {
            // Arrange
            TestClient.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", "asdasdasd");
            // Act
            var responseCreateFamily = await TestClient.PostAsJsonAsync(ApiRoutes.FamilyRoutes.CreateFamily, new CreateFamilyDTO { });
            var responseGetAllFamilies = await TestClient.GetAsync(ApiRoutes.FamilyRoutes.GetAllFamilies);
            var responseGetFamilyById = await TestClient.GetAsync(ApiRoutes.FamilyRoutes.GetFamily);
            var responseUpdateFamily = await TestClient.PatchAsJsonAsync(ApiRoutes.FamilyRoutes.UpdateFamily, new UpdateFamilyDTO { });
            var responseDeleteFamily = await TestClient.DeleteAsync(ApiRoutes.FamilyRoutes.DeleteFamily);

            // Assert
            responseCreateFamily.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            responseGetAllFamilies.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            responseGetFamilyById.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            responseUpdateFamily.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            responseDeleteFamily.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }
    }
}
