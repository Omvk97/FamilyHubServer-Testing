using System;
using System.Threading.Tasks;
using API.V1.DTO.InputDTOs.EventDTOs;
using API.Data.Models;
using System.Collections.Generic;

namespace API.V1.Repositories.EventRepo
{
    public interface IEventRepo
    {
        Task<Event> CreateEvent(CreateEventDTO userInput, Guid creatorId);
        Task<ICollection<Event>> GetAllEvents(Guid? userId);
        Task<Event> GetEventById(Guid eventId);
        Task<Event> UpdateEvent(Guid eventId, UpdateEventDTO userInput);
        Task<Event> DeleteEvent(Guid eventId);
    }
}
