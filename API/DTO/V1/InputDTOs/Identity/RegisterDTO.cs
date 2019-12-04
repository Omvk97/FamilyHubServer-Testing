using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO.InputDTOs.V1.Identity
{
    public class RegisterDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        public string ProfileColor { get; set; }
        public Guid? FamilyId { get; set; }
    }
}
