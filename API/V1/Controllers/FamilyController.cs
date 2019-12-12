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

                return StatusCode(StatusCodes.Status201Created, (_mapper.Map<SuccessGetFamilyDTO>(createdFamily)));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new UserInputErrorDTO { ErrorMessage = e.Message });
            }
        }

        [HttpGet(ApiRoutes.FamilyRoutes.GetAllFamilies)]
        // TODO: [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<ICollection<SuccessGetFamilyDTO>>> GetAllFamilies()
        {
            var families = await _repo.GetAllFamilies();

            return Ok(_mapper.Map<ICollection<SuccessGetFamilyDTO>>(families));
        }

        [HttpGet(ApiRoutes.FamilyRoutes.GetFamily + "/{familyId}")]
        // TODO: [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<SuccessGetFamilyDTO>> GetFamilyById(Guid familyId)
        {
            try
            {
                var family = await _repo.GetFamilyById(familyId);

                return Ok(_mapper.Map<SuccessGetFamilyDTO>(family));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new UserInputErrorDTO { ErrorMessage = e.Message });
            }
        }

        // NOTE: Should this endpoint be based on users familyId? Maybe two endpoints, one for admins and one for updating a family if the user has family update rights?
        [HttpPatch(ApiRoutes.FamilyRoutes.UpdateFamily + "/{familyId}")]
        public async Task<ActionResult<SuccessGetFamilyDTO>> UpdateFamily(Guid familyId, UpdateFamilyDTO userInput)
        {
            try
            {
                var updatedFamily = await _repo.UpdateFamily(familyId, userInput);
                return Ok(_mapper.Map<SuccessGetFamilyDTO>(updatedFamily));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new UserInputErrorDTO { ErrorMessage = e.Message });
            }
        }

        [HttpDelete(ApiRoutes.FamilyRoutes.DeleteFamily + "/{familyId}")]
        public async Task<ActionResult<Guid>> DeletedFamily(Guid familyId)
        {
            try
            {
                var deletedFamily = await _repo.DeleteFamily(familyId);
                return Ok(deletedFamily.Id);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new UserInputErrorDTO { ErrorMessage = e.Message });
            }
        }
    }
}
