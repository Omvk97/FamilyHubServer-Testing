using System;
using API.Data.Models;

namespace API.V1.DTO.OutputDTOs.UserDTOs
{
    public class SuccessGetUserWithoutEventDTO
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string ProfilePicturePath { get; set; }

        public string ProfileColor { get; set; }

        public Family Family { get; set; }
    }
}
