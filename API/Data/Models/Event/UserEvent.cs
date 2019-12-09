using System;
using System.ComponentModel.DataAnnotations;

namespace API.Data.Models
{
    public class UserEvent
    {
        [Required]
        public Guid UserId { get; set; }

        public User User { get; set; }

        [Required]
        public Guid EventId { get; set; }

        public Event Event { get; set; }
    }
}