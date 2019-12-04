using System;
using System.Threading.Tasks;
using API.Data.Repositories;
using API.DTO.InputDTOs;
using API.Helpers.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo _repo;
        private readonly IJwt _jwtHelper;

        public AuthController(IAuthRepo repo, IJwt jwt)
        {
            _repo = repo;
            _jwtHelper = jwt;
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginDTO userInput)
        {
            try
            {
                var userCredential = await _repo.CheckUserInput(userInput.Email, userInput.Password);

                var jwt = _jwtHelper.CreateJwt(userCredential);

                return Ok(new { jwt });
            } 
            catch (NullReferenceException)
            {
                return Unauthorized(new { error = "Invalid login" });
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
