﻿using System;
using System.ComponentModel.DataAnnotations;
using API.V1.Contracts;

namespace API.V1.DTO.InputDTOs.IdentityDTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress(ErrorMessage = ErrorMessages.InvalidEmail)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$",
            ErrorMessage = ErrorMessages.InvalidPasswordFormat)]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = ErrorMessages.InvalidProfileColor)]
        public string ProfileColor { get; set; }

        public Guid? FamilyId { get; set; }
    }
}
