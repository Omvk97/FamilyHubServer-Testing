using System;
namespace API.Data.Models
{
    public class RepeatException
    {
        public DateTime? ChangedStartTime { get; set; }
        public DateTime? ChangedEndTime { get; set; }
        public DateTime? Removed { get; set; }
    }
}
