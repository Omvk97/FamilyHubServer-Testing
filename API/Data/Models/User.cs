using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        public string ProfilePicturePath { get; set; }

        [Required]
        [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Invalid color format. Use Hex color format")]
        public string ProfileColor { get; set; }
        public Guid? FamilyId { get; set; }
        public Family Family { get; set; }
        public ICollection<UserEvent> Events { get; set; }
    }
}
