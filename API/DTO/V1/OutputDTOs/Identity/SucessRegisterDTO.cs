using System;
using API.Models;

namespace API.DTO.OutputDTOs.V1.Identity
{
    public class SucessRegisterDTO
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
