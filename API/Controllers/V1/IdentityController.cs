using System;
using System.Threading.Tasks;
using API.Contracts.V1;
using API.Data.Repositories.V1;
using API.DTO.InputDTOs.V1.Identity;
using API.DTO.OutputDTOs.V1;
using API.DTO.OutputDTOs.V1.Identity;
using API.Helpers.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.V1
{
    [ApiController]
    [Route(ApiRoutes.Identity.ControllerRoute)]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityRepo _repo;
        private readonly IJwt _jwtHelper;

        public IdentityController(IIdentityRepo repo, IJwt jwt)
        {
            _repo = repo;
            _jwtHelper = jwt;
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<ActionResult> Login(LoginDTO userInput)
        {
            try
            {
                var user = await _repo.CheckUserInput(userInput.Email, userInput.Password);

                var jwt = _jwtHelper.CreateJwt(user);

                return Ok(new SucessLoginDTO { Jwt = jwt });
            }
            catch (NullReferenceException)
            {
                return Unauthorized(new UserInputErrorDTO { ErrorMessage = "Invalid login" });
            }
            catch (FormatException fex)
            {
                // TODO: Proper logging
                Console.WriteLine("ERROR " + fex.InnerException);
                return StatusCode(StatusCodes.Status500InternalServerError, "Please try again later!");
            }
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<ActionResult> Register(RegisterDTO userInput)
        {
            if (userInput.FamilyId != null)
            {
                var family = await _repo.CheckFamilyExists((Guid)userInput.FamilyId);
                if (family == null) return BadRequest(new UserInputErrorDTO { ErrorMessage = "Family does not exist" });
            }

            try
            {
                var user = await _repo.CreateUser(userInput);
                var token = _jwtHelper.CreateJwt(user);

                return StatusCode(StatusCodes.Status201Created, new SucessRegisterDTO
                {
                    User = user,
                    Token = token
                });
            }
            catch (DbUpdateException)
            {
                return BadRequest(new UserInputErrorDTO { ErrorMessage = "Email already exists" });
            }
        }



    }
}
