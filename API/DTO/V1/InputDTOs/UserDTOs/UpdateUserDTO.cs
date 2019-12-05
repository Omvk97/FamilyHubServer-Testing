﻿using System;
using System.ComponentModel.DataAnnotations;
using API.Contracts.V1;

namespace API.DTO.V1.InputDTOs.UserDTOs
{
    public class UpdateUserDTO
    {
        public string NewName { get; set; }

        [EmailAddress]
        public string NewEmail { get; set; }

        [RegularExpression(@"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$",
        ErrorMessage = ErrorMessages.InvalidPasswordFormat)]
        public string NewPassword { get; set; }

        [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = ErrorMessages.InvalidProfileColor)]
        public string NewProfileColor { get; set; }

        public Guid? NewFamilyId { get; set; }
    }
}
