using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Contracts.V1;
using API.Data.Repositories.V1.UserRepo;
using API.DTO.V1.InputDTOs.UserDTOs;
using API.DTO.V1.OutputDTOs.UserDTOs;
using API.Helpers.JwtHelper;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.V1
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
        public async Task<ActionResult> GetAllUsers(bool includeFamily, bool includeEvents)
        {
            var users = await _repo.GetAllUsers(includeFamily, includeEvents);

            return Ok(_mapper.Map<ICollection<SuccessGetUserDTO>>(users));
        }

        [HttpGet(ApiRoutes.UserRoutes.GetUser)]
        public async Task<ActionResult> GetUser()
        {
            var tokenClaims = _jwtHelper.ReadVerifiedJwtToken(Request.Headers["Authorization"]);
            var user = await _repo.GetUserById(Guid.Parse(tokenClaims["userId"]));

            return Ok(_mapper.Map<SuccessGetUserDTO>(user));
        }

        [HttpPatch(ApiRoutes.UserRoutes.UpdateUser)]
        public async Task<ActionResult> UpdateUser(UpdateUserDTO userInput)
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
        public async Task<ActionResult> DeleteUser()
        {
            var tokenClaims = _jwtHelper.ReadVerifiedJwtToken(Request.Headers["Authorization"]);
            var user = await _repo.DeleteUser(Guid.Parse(tokenClaims["userId"]));

            return Ok(_mapper.Map<SuccessGetUserDTO>(user));
        }

        [HttpGet(ApiRoutes.UserRoutes.GetUserFamily)]
        public async Task<ActionResult> GetUserFamily()
        {
            return Ok();
        }

        [HttpGet(ApiRoutes.UserRoutes.GetUserEvents)]
        public async Task<ActionResult> GetUserEvents()
        {
            return Ok();
        }
    }
}
