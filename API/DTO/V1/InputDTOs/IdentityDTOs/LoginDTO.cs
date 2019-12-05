using System;
using System.ComponentModel.DataAnnotations;
using API.Contracts.V1;

namespace API.DTO.InputDTOs.V1.IdentityDTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress(ErrorMessage = ErrorMessages.InvalidEmail)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
