using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string RecipeDetails { get; set; }
        public Guid FamilyId { get; set; }
        public Family Family { get; set; }
    }
}
