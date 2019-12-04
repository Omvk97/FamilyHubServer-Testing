using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO.InputDTOs.V1.Identity
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
