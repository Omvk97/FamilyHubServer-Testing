using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.Data.Models;
using API.V1.Contracts;

namespace API.V1.DTO.InputDTOs.EventDTOs
{
    public class CreateEventRepeatDetails
    {
        [Required]
        public FrequencyOption Frequency { get; set; }

        [RequiredWhenFrequencyWeekday]
        public ICollection<DayOfWeek> WeekDays { get; set; }

        public DateTime? EndRepeat { get; set; }

        public class RequiredWhenFrequencyWeekdayAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var eventDetails = (CreateEventRepeatDetails)validationContext.ObjectInstance;
                if (eventDetails.Frequency == FrequencyOption.Weekly)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(ErrorMessages.WeekDaysOnlySetIfFrequencyWeekly);
                }
            }
        }
    }
}
