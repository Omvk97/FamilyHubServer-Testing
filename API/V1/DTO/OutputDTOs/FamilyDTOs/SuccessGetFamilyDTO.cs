using System;
using System.Collections.Generic;
using API.V1.DTO.OutputDTOs.UserDTOs;
using API.Data.Models;

namespace API.V1.DTO.OutputDTOs.FamilyDTOs
{
    public class SuccessGetFamilyDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<SuccessGetUserWithoutFamilyDTO> Members { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
    }
}
