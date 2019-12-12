using System;
using System.Threading.Tasks;
using API.V1.Contracts;
using API.V1.DTO.InputDTOs.IdentityDTOs;
using API.V1.DTO.OutputDTOs;
using API.V1.DTO.OutputDTOs.IdentityDTOs;
using API.V1.DTO.OutputDTOs.UserDTOs;
using API.Helpers.JwtHelper;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.V1.Repositories.IdentityRepo;

namespace API.V1.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityRepo _repo;
        private readonly IJwtHelper _jwtHelper;
        private readonly IMapper _mapper;

        public IdentityController(IIdentityRepo repo, IJwtHelper jwt, IMapper mapper)
        {
            _repo = repo;
            _jwtHelper = jwt;
            _mapper = mapper;
        }

        [HttpPost(ApiRoutes.IdentityRoutes.Login)]
        public async Task<ActionResult> Login(LoginDTO userInput)
        {
            try
            {
                var user = await _repo.CheckUserLoginInput(userInput.Email, userInput.Password);
                if (user == null)
                {
                    return BadRequest(new UserInputErrorDTO { ErrorMessage = ErrorMessages.InvalidLogin });
                }
                var jwt = _jwtHelper.CreateJwt(user);

                return Ok(new SucessLoginDTO { Token = jwt });
            }
            catch (FormatException fex)
            {
                // TODO: Proper logging
                Console.WriteLine("ERROR " + fex.InnerException);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages.ServerError);
            }
        }

        [HttpPost(ApiRoutes.IdentityRoutes.Register)]
        public async Task<ActionResult> Register(RegisterDTO userInput)
        {
            try
            {
                var user = await _repo.CreateUser(userInput);
                var token = _jwtHelper.CreateJwt(user);

                return StatusCode(StatusCodes.Status201Created, new SucessRegisterDTO
                {
                    User = _mapper.Map<SuccessGetUserDTO>(user),
                    Token = token
                });
            }
            catch (ArgumentException e)
            {
                return BadRequest(new UserInputErrorDTO { ErrorMessage = e.Message });
            }
        }
    }
}
