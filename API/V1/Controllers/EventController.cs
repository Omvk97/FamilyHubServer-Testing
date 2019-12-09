using System;
using System.Threading.Tasks;
using API.V1.Contracts;
using API.V1.Repositories.EventRepo;
using API.V1.DTO.OutputDTOs;
using API.V1.DTO.InputDTOs.EventDTOs;
using API.V1.DTO.OutputDTOs.EventDTOs;
using API.Helpers.JwtHelper;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.V1.Controllers
{
    [ApiController]
    [Authorize]
    public class EventController : ControllerBase
    {
        private readonly IEventRepo _repo;
        private readonly IMapper _mapper;
        private readonly IJwtHelper _jwtHelper;

        public EventController(IEventRepo repo, IMapper mapper, IJwtHelper jwtHelper)
        {
            _repo = repo;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
        }

        [HttpPost(ApiRoutes.EventRoutes.CreateEvent)]
        public async Task<ActionResult<SuccessGetEventDTO>> GreateEvent(CreateEventDTO userInput)
        {
            var tokenClaims = _jwtHelper.ReadVerifiedJwtToken(Request.Headers["Authorization"]);
            var userId = Guid.Parse(tokenClaims["userId"]);

            try
            {
                var createdEvent = await _repo.CreateEvent(userInput, userId);

                return StatusCode(StatusCodes.Status201Created, (_mapper.Map<SuccessGetEventDTO>(createdEvent)));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new UserInputErrorDTO { ErrorMessage = e.Message });
            }
        }
    }
}
