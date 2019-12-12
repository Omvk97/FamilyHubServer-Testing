using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.V1.Contracts;

namespace API.V1.DTO.InputDTOs.EventDTOs
{
    public class UpdateEventDTO
    {
        [StringLength(50, MinimumLength = 1)]
        public string NewTitle { get; set; }

        [StringLength(2400, MinimumLength = 1)]
        public string NewDescription { get; set; }

        [RequiredWhenAlldayRemoved]
        public DateTime? NewStartTime { get; set; }

        [RequiredWhenAlldayRemoved]
        public DateTime? NewEndTime { get; set; }

        public DateTime? NewAllDay { get; set; }

        public bool RemoveAllDay { get; set; }

        public UpdateEventRepeatDetails NewRepeatDetails { get; set; }

        public string NewLocation { get; set; }

        public DateTime? NewAlert { get; set; }

        public HashSet<Guid> NewParticipantIds { get; set; }

        public class RequiredWhenAlldayRemovedAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var eventDetails = (UpdateEventDTO) validationContext.ObjectInstance;
                
                if (eventDetails.RemoveAllDay)
                {
                    return new ValidationResult(ErrorMessages.RequiredWhenAllDayRemoved);
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
        }
    }
}
