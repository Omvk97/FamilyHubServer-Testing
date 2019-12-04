﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class EventRepeatDetails
    {
        public Guid Id { get; set; }
        [ForeignKey("Id")]
        public Event Event { get; set; }
        [Required]
        public FrequencyOption Frequency { get; set; }
        [RequiredWhenFrequencyWeekdayAttribute]
        public ICollection<DayOfWeek> WeekDays { get; set; }
        public DateTime? EndRepeat { get; set; }
        public ICollection<RepeatException> Exceptions { get; set; }

        public enum FrequencyOption
        {
            Daily, Weekly, Monthly, Yearly
        }

        public class RepeatException
        {
            [Required]
            public DateTime ChangedStartTime { get; set; }
            [Required]
            public DateTime ChangedEndTime { get; set; }
            public bool Removed { get; set; }
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
                    return new ValidationResult("This should only be set if Frequency is Weekly");
                }
            }
        }
    }
}
