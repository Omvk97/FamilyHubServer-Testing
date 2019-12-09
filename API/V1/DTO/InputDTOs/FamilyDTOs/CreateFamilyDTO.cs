using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.V1.DTO.InputDTOs.FamilyDTOs
{
    public class CreateFamilyDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        
        public HashSet<Guid> MemberIds { get; set; }
    }
}
