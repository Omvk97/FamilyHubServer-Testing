using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.V1.Contracts;

namespace API.Data.Models
{
    public class EventRepeatDetails
    {
        public Guid Id { get; set; }

        [ForeignKey("Id")]
        public Event Event { get; set; }

        [Required]
        public FrequencyOption Frequency { get; set; }

        [RequiredWhenFrequencyWeekday]
        public ICollection<DayOfWeek> WeekDays { get; set; }

        public DateTime? EndRepeat { get; set; }

        public ICollection<RepeatException> Exceptions { get; set; }

        public enum FrequencyOption
        {
            Daily, Weekly, Monthly, Yearly
        }

        public class RepeatException
        {
            public DateTime? ChangedStartTime { get; set; }
            public DateTime? ChangedEndTime { get; set; }
            public DateTime? Removed { get; set; }
        }

        public class RequiredWhenFrequencyWeekdayAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var eventDetails = (EventRepeatDetails) validationContext.ObjectInstance;
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
