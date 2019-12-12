using System;
using System.Collections.Generic;
using API.Data.Models;

namespace API.V1.DTO.OutputDTOs.EventDTOs
{
    public class SuccessGetEventRepeatDetailsDTO
    {
        public Guid Id { get; set; }

        public FrequencyOption Frequency { get; set; }

        public ICollection<DayOfWeek> WeekDays { get; set; }

        public DateTime EndRepeat { get; set; }

        public ICollection<RepeatException> Exceptions { get; set; }
    }
}
