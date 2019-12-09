using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Family
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<User> Members { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
    }
}
