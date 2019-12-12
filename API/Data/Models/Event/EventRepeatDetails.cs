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
        public List<WeekDay> WeekDays { get; set; }

        public DateTime? EndRepeat { get; set; }

        [Column(TypeName = "jsonb")]
        public ICollection<RepeatException> Exceptions { get; set; }

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
