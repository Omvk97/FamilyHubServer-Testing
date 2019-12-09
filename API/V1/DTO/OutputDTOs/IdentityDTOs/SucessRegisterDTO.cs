using System;
using API.V1.DTO.OutputDTOs.UserDTOs;

namespace API.V1.DTO.OutputDTOs.IdentityDTOs
{
    public class SucessRegisterDTO
    {
        public SuccessGetUserDTO User { get; set; }

        public string Token { get; set; }
    }
}
