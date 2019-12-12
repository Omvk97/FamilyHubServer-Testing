using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.Data.Models;
using API.V1.Contracts;

namespace API.V1.DTO.InputDTOs.EventDTOs
{
    public class UpdateEventRepeatDetails
    {
        [FrequencyOption]
        public string NewFrequency { get; set; }

        public List<WeekDay> NewWeekDays { get; set; }

        public DateTime? NewEndRepeat { get; set; }

        public ICollection<RepeatException> NewRepeatExceptions { get; set; }

        public class FrequencyOptionAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var eventDetails = (UpdateEventRepeatDetails)validationContext.ObjectInstance;
                try
                {
                    FrequencyOption frequencyOption = (FrequencyOption)Enum.Parse(typeof(FrequencyOption), eventDetails.NewFrequency);
                    return ValidationResult.Success;
                } catch
                {
                    return new ValidationResult(ErrorMessages.InvalidFrequencyOption);
                }
            }
        }
    }
}
