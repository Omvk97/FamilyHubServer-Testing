using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.V1.Contracts;

namespace API.V1.DTO.InputDTOs.EventDTOs
{
    public class CreateEventDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }

        [StringLength(2400, MinimumLength = 1)]
        public string Description { get; set; }

        [RequiredWhenAlldayNotSet]
        public DateTime? StartTime { get; set; }

        [RequiredWhenAlldayNotSet]
        public DateTime? EndTime { get; set; }

        public DateTime? AllDay { get; set; }

        public CreateEventRepeatDetails RepeatDetails { get; set; }

        public string Location { get; set; }

        public DateTime? Alert { get; set; }
        
        public HashSet<Guid> ParticipantIds { get; set; }

        public class RequiredWhenAlldayNotSetAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var eventDetails = (CreateEventDTO)validationContext.ObjectInstance;
                // If AllDay has been set, the field is not required
                if (eventDetails.AllDay != null)
                {
                    return ValidationResult.Success;
                }
                if (eventDetails.StartTime != null && eventDetails.EndTime != null)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(ErrorMessages.RequiredWhenAllDayNotSet);
            }
        }
    }
}
