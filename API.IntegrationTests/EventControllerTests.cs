using System;
namespace API.IntegrationTests
{
    public class EventControllerTests : IntegrationTest
    {
        public EventControllerTests()
        {
        }

        // Get user events probably
        //// TODO: Maybe all these things should be a unit test instead
        //// Create second user with an event which user 1 should not be able to see
        //var registerUser2Response = await TestClient.PostAsJsonAsync(ApiRoutes.IdentityRoutes.Register, new RegisterDTO
        //{
        //    Name = "Test user 2",
        //    Email = "TestUser2Email@example.com",
        //    Password = "Password1234.!"
        //});
        //registerUser2Response.StatusCode.Should().Be(StatusCodes.Status201Created);
        //var registerUser2ResponseContent = await registerUser2Response.Content.ReadAsJsonAsync<SucessRegisterDTO>();
        //registerUser2ResponseContent.Token.Should().NotBeNullOrEmpty();

        //// User 2 creates a basic new event
        //TestClient.DefaultRequestHeaders.Authorization
        //            = new AuthenticationHeaderValue("Bearer", registerUser2ResponseContent.Token);
        //var user2Event = new CreateEventDTO
        //{
        //    Title = "Event test",
        //    Description = "Event description test",
        //    AllDay = new DateTime(2019, 12, 24)
        //};

        //var user2EventResponse = await TestClient.PostAsJsonAsync(ApiRoutes.EventRoutes.CreateEvent, user2Event);

        //user2EventResponse.StatusCode.Should().Be(StatusCodes.Status201Created);
        //var user2EventResponseContent = await user2EventResponse.Content.ReadAsJsonAsync<ICollection<SuccessGetEventDTO>>();

        //user2EventResponseContent.Should().NotBeNullOrEmpty();
    }
}
