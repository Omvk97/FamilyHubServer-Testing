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
using System.Collections.Generic;
using API.Cache;

namespace API.V1.Controllers
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
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

        /// <summary>
        /// Creates an event with authorized user as Owner
        /// </summary>
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

        [HttpGet(ApiRoutes.EventRoutes.GetAllEvents)]
        [Cached("event")]
        public async Task<ActionResult<ICollection<SuccessGetEventDTO>>> GetAllEvents(Guid? userId)
        {
            try
            {
                var events = await _repo.GetAllEvents(userId);

                return Ok(_mapper.Map<ICollection<SuccessGetEventDTO>>(events));

            } catch (ArgumentException e)
            {
                return BadRequest(new UserInputErrorDTO { ErrorMessage = e.Message });
            }
        }

        [HttpGet(ApiRoutes.EventRoutes.GetEvent + "/{eventId}")]
        [Cached("event")]
        // TODO: [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<SuccessGetEventDTO>> GetEvent(Guid eventId)
        {
            try
            {
                var fetchedEvent = await _repo.GetEventById(eventId);

                return Ok(_mapper.Map<SuccessGetEventDTO>(fetchedEvent));

            }
            catch (ArgumentException e)
            {
                return BadRequest(new UserInputErrorDTO { ErrorMessage = e.Message });
            }
        }

        [HttpPatch(ApiRoutes.EventRoutes.UpdateEvent + "/{eventId}")]
        // TODO: [Authorize(Policy = "AdminOnly")]
        // TODO: Add another route should be made for when users want to update events only they are own
        public async Task<ActionResult<SuccessGetEventDTO>> UpdateEvent(Guid eventId, UpdateEventDTO userInput)
        {
            try
            {
                var updatedEvent = await _repo.UpdateEvent(eventId, userInput);
                return Ok(_mapper.Map<SuccessGetEventDTO>(updatedEvent));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new UserInputErrorDTO { ErrorMessage = e.Message });
            }
        }

        [HttpDelete(ApiRoutes.EventRoutes.DeleteEvent + "/{eventId}")]
        // TODO: [Authorize(Policy = "AdminOnly")]
        // TODO: Add another route when users want to delete events only they have owner rights on
        public async Task<ActionResult<ICollection<SuccessGetEventDTO>>> DeleteEvent(Guid eventId)
        {
            try
            {
                var deletedEvent = await _repo.DeleteEvent(eventId);
                return Ok(_mapper.Map<SuccessGetEventDTO>(deletedEvent));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new UserInputErrorDTO { ErrorMessage = e.Message });
            }
        }
    }
}
