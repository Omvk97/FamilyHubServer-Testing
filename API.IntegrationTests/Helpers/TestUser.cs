using System;
using System.Net.Http;
using System.Threading.Tasks;
using API.Contracts.V1;
using API.DTO.InputDTOs.V1.IdentityDTOs;
using API.DTO.OutputDTOs.V1.IdentityDTOs;
using API.DTO.V1.OutputDTOs.UserDTOs;
using API.IntegrationTests.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace API.IntegrationTests.Helpers
{
    public static class TestUser
    {
        public static string Email = "test@example.com";
        public static string Password = "Password1.!";
        public static string Name = "Mr. Test";
        public static string FamilyName = "Family Test";

        public static async Task<SucessRegisterDTO> CreateTestUserInDb(HttpClient client, bool withFamily)
        {
            var testUser = new RegisterDTO
            {
                Email = Email,
                Password = Password,
                Name = Name
            };
            if (withFamily)
            {
                // TODO: 
            }
            var registerResponse = await client.PostAsJsonAsync(ApiRoutes.IdentityRoutes.Register, testUser);
            registerResponse.StatusCode.Should().Be(StatusCodes.Status201Created);
            var responseContent = await registerResponse.Content.ReadAsJsonAsync<SucessRegisterDTO>();
            responseContent.Token.Should().NotBeNullOrEmpty();
            return responseContent;
        }
    }
}
