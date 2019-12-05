using API.DTO.V1.OutputDTOs.UserDTOs;

namespace API.DTO.OutputDTOs.V1.IdentityDTOs
{
    public class SucessRegisterDTO
    {
        public SuccessGetUserDTO User { get; set; }

        public string Token { get; set; }
    }
}
