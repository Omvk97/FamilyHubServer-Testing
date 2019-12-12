using System;
using System.Collections.Generic;

namespace API.V1.DTO.OutputDTOs.EventDTOs
{
    public class SuccessGetEventRepeatDetailsDTO
    {
        public Guid Id { get; set; }

        public FrequencyOption Frequency { get; set; }

        public ICollection<DayOfWeek> WeekDays { get; set; }

        public DateTime EndRepeat { get; set; }

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
    }
}
