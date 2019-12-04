using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        [ForeignKey("Id")]
        public Credential Credential { get; set; }
        [Required]
        public string Name { get; set; }
        public Guid FamilyId { get; set; }
        public Family Family { get; set; }
        public ICollection<UserEvent> Events { get; set; }
    }
}
