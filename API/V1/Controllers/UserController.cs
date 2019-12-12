using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.V1.Contracts;
using API.V1.Repositories.UserRepo;
using API.V1.DTO.InputDTOs.UserDTOs;
using API.V1.DTO.OutputDTOs.EventDTOs;
using API.V1.DTO.OutputDTOs.FamilyDTOs;
using API.V1.DTO.OutputDTOs.UserDTOs;
using API.Helpers.JwtHelper;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.V1.DTO.OutputDTOs;

namespace API.V1.Controllers
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _repo;
        private readonly IMapper _mapper;
        private readonly IJwtHelper _jwtHelper;

        public UserController(IUserRepo userRepo, IMapper mapper, IJwtHelper jwtHelper)
        {
            _repo = userRepo;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
        }

        [HttpGet(ApiRoutes.UserRoutes.GetAllUsers)]
        // TODO: [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<ICollection<SuccessGetUserDTO>>> GetAllUsers(bool includeFamily, bool includeEvents)
        {
            var users = await _repo.GetAllUsers(includeFamily, includeEvents);

            return Ok(_mapper.Map<ICollection<SuccessGetUserDTO>>(users));
        }

        [HttpGet(ApiRoutes.UserRoutes.GetUser)]
        public async Task<ActionResult<SuccessGetUserDTO>> GetUser()
        {
            try
            {
                var tokenClaims = _jwtHelper.ReadVerifiedJwtToken(Request.Headers["Authorization"]);
                var user = await _repo.GetUserById(Guid.Parse(tokenClaims["userId"]));

                return Ok(_mapper.Map<SuccessGetUserDTO>(user));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new UserInputErrorDTO { ErrorMessage = e.Message });
            }
        }

        [HttpPatch(ApiRoutes.UserRoutes.UpdateUser)]
        public async Task<ActionResult<SuccessGetUserDTO>> UpdateUser(UpdateUserDTO userInput)
        {
            var tokenClaims = _jwtHelper.ReadVerifiedJwtToken(Request.Headers["Authorization"]);

            try
            {
                var user = await _repo.GetUserById(Guid.Parse(tokenClaims["userId"]));
                var updatedUser = await _repo.UpdateUser(user.Id, userInput);
                return Ok(_mapper.Map<SuccessGetUserDTO>(updatedUser));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

        }

        // This is for a user to delete their own user, there should maybe be a route for admins
        [HttpDelete(ApiRoutes.UserRoutes.DeleteUser)]
        public async Task<ActionResult<SuccessGetUserDTO>> DeleteUser()
        {
            var tokenClaims = _jwtHelper.ReadVerifiedJwtToken(Request.Headers["Authorization"]);
            try
            {
                var user = await _repo.DeleteUser(Guid.Parse(tokenClaims["userId"]));
                return Ok(_mapper.Map<SuccessGetUserDTO>(user));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new UserInputErrorDTO { ErrorMessage = e.Message });
            }
        }

        [HttpGet(ApiRoutes.UserRoutes.GetUserFamily)]
        public async Task<ActionResult<SuccessGetFamilyDTO>> GetUserFamily()
        {
            var tokenClaims = _jwtHelper.ReadVerifiedJwtToken(Request.Headers["Authorization"]);
            try
            {
                var family = await _repo.GetUserFamily(Guid.Parse(tokenClaims["userId"]));

                return Ok(_mapper.Map<SuccessGetFamilyDTO>(family));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new UserInputErrorDTO { ErrorMessage = e.Message });
            }
        }

        [HttpGet(ApiRoutes.UserRoutes.GetUserEvents)]
        public async Task<ActionResult> GetUserEvents()
        {
            var tokenClaims = _jwtHelper.ReadVerifiedJwtToken(Request.Headers["Authorization"]);
            try
            {
                var events = await _repo.GetUserEvents(Guid.Parse(tokenClaims["userId"]));

                return Ok(_mapper.Map<ICollection<SuccessGetEventDTO>>(events));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new UserInputErrorDTO { ErrorMessage = e.Message });
            }
        }
    }
}
