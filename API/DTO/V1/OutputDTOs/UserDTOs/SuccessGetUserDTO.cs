using System;
using System.Collections.Generic;
using API.Models;

namespace API.DTO.V1.OutputDTOs.UserDTOs
{
    public class SuccessGetUserDTO
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string ProfilePicturePath { get; set; }

        public string ProfileColor { get; set; }

        public Family Family { get; set; }

        public ICollection<UserEvent> Events { get; set; }
    }
}
