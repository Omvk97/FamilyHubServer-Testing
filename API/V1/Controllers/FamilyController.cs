using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.V1.Contracts;
using API.V1.Repositories.FamilyRepo;
using API.V1.DTO.OutputDTOs;
using API.V1.DTO.InputDTOs.FamilyDTOs;
using API.V1.DTO.OutputDTOs.FamilyDTOs;
using API.Helpers.JwtHelper;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.V1.Controllers
{
    [ApiController]
    [Authorize]
    public class FamilyController : ControllerBase
    {
        private readonly IFamilyRepo _repo;
        private readonly IMapper _mapper;
        private readonly IJwtHelper _jwtHelper;

        public FamilyController(IFamilyRepo repo, IMapper mapper, IJwtHelper jwtHelper)
        {
            _repo = repo;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
        }

        [HttpPost(ApiRoutes.FamilyRoutes.CreateFamily)]
        public async Task<ActionResult<SuccessGetFamilyDTO>> CreateFamily(CreateFamilyDTO userInput)
        {
            var tokenClaims = _jwtHelper.ReadVerifiedJwtToken(Request.Headers["Authorization"]);
            var userId = Guid.Parse(tokenClaims["userId"]);

            try
            {
                var createdFamily = await _repo.CreateFamily(userInput, userId);

                return StatusCode(StatusCodes.Status201Created,(_mapper.Map<SuccessGetFamilyDTO>(createdFamily)));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new UserInputErrorDTO { ErrorMessage = e.Message });
            }
        }
    }
}
