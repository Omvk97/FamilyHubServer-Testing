using System;
using System.Collections.Generic;
using API.Data.Models;

namespace API.V1.DTO.OutputDTOs.UserDTOs
{
    public class SuccessGetUserWithoutFamilyDTO
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string ProfilePicturePath { get; set; }

        public string ProfileColor { get; set; }

        public ICollection<UserEvent> Events { get; set; }
    }
}
