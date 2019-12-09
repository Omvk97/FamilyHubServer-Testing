using System;
using System.ComponentModel.DataAnnotations;
using API.V1.Contracts;

namespace API.V1.DTO.InputDTOs.IdentityDTOs
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
