using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.V1.DTO.InputDTOs.FamilyDTOs
{
    public class UpdateFamilyDTO
    {
        [StringLength(50, MinimumLength = 3)]
        public string NewFamilyName { get; set; }

        public HashSet<Guid> NewMemberIds { get; set; }
    }
}
