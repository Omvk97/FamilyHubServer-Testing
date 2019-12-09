using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Event
    {
        public Guid Id { get; set; }

        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }

        [StringLength(2400, MinimumLength = 1)]
        public string Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
        
        public DateTime? AllDay { get; set; }

        public Guid? RepeatDetailsId { get; set; }

        public EventRepeatDetails RepeatDetails { get; set; }

        public string Location { get; set; }

        public DateTime? Alert { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        public User Owner { get; set; }

        public ICollection<UserEvent> Participants { get; set; }
    }
}
