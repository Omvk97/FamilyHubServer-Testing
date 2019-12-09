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

namespace API.V1.Controllers
{
    [ApiController]
    [Authorize]
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
        public async Task<ActionResult<ICollection<SuccessGetUserDTO>>> GetAllUsers(bool includeFamily, bool includeEvents)
        {
            var users = await _repo.GetAllUsers(includeFamily, includeEvents);

            return Ok(_mapper.Map<ICollection<SuccessGetUserDTO>>(users));
        }

        [HttpGet(ApiRoutes.UserRoutes.GetUser)]
        public async Task<ActionResult<SuccessGetUserDTO>> GetUser()
        {
            var tokenClaims = _jwtHelper.ReadVerifiedJwtToken(Request.Headers["Authorization"]);
            var user = await _repo.GetUserById(Guid.Parse(tokenClaims["userId"]));

            return Ok(_mapper.Map<SuccessGetUserDTO>(user));
        }

        [HttpPatch(ApiRoutes.UserRoutes.UpdateUser)]
        public async Task<ActionResult<SuccessGetUserDTO>> UpdateUser(UpdateUserDTO userInput)
        {
            var tokenClaims = _jwtHelper.ReadVerifiedJwtToken(Request.Headers["Authorization"]);
            var user = await _repo.GetUserById(Guid.Parse(tokenClaims["userId"]));

            try
            {
                var updatedUser = await _repo.UpdateUser(user.Id, userInput);
                return Ok(_mapper.Map<SuccessGetUserDTO>(updatedUser));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpDelete(ApiRoutes.UserRoutes.DeleteUser)]
        public async Task<ActionResult<SuccessGetUserDTO>> DeleteUser()
        {
            var tokenClaims = _jwtHelper.ReadVerifiedJwtToken(Request.Headers["Authorization"]);
            var user = await _repo.DeleteUser(Guid.Parse(tokenClaims["userId"]));

            return Ok(_mapper.Map<SuccessGetUserDTO>(user));
        }

        [HttpGet(ApiRoutes.UserRoutes.GetUserFamily)]
        public async Task<ActionResult<SuccessGetFamilyDTO>> GetUserFamily()
        {
            var tokenClaims = _jwtHelper.ReadVerifiedJwtToken(Request.Headers["Authorization"]);
            var family = await _repo.GetUserFamily(Guid.Parse(tokenClaims["userId"]));

            return Ok(_mapper.Map<SuccessGetFamilyDTO>(family));
        }

        [HttpGet(ApiRoutes.UserRoutes.GetUserEvents)]
        public async Task<ActionResult> GetUserEvents()
        {
            var tokenClaims = _jwtHelper.ReadVerifiedJwtToken(Request.Headers["Authorization"]);
            var events = await _repo.GetUserEvents(Guid.Parse(tokenClaims["userId"]));

            return Ok(_mapper.Map<ICollection<SuccessGetEventDTO>>(events));
        }
    }
}
