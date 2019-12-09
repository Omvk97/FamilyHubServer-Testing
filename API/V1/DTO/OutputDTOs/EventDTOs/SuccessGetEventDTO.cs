using System;
using System.Collections.Generic;
using API.V1.DTO.OutputDTOs.UserDTOs;
using API.Data.Models;

namespace API.V1.DTO.OutputDTOs.EventDTOs
{
    public class SuccessGetEventDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime AllDay { get; set; }

        public Guid? RepeatDetailsId { get; set; }

        public EventRepeatDetails RepeatDetails { get; set; }

        public string Location { get; set; }

        public DateTime? Alert { get; set; }

        public Guid OwnerId { get; set; }

        public SuccessGetUserWithoutEventDTO Owner { get; set; }

        public ICollection<SuccessGetUserWithoutEventDTO> Participants { get; set; }
    }
}
