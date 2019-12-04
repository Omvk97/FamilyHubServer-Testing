using System;
using System.Threading.Tasks;
using API.Contracts.V1;
using API.Data.Repositories;
using API.DTO.InputDTOs;
using API.DTO.OutputDTOs;
using API.Helpers.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                var userCredential = await _repo.CheckUserInput(userInput.Email, userInput.Password);

                var jwt = _jwtHelper.CreateJwt(userCredential);

                return Ok(new SucessLoginDTO { Jwt = jwt});
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


    }
}
