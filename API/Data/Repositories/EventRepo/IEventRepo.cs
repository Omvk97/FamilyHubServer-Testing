using System;
using System.Threading.Tasks;
using API.DTO.V1.InputDTOs.EventDTOs;
using API.Models;

namespace API.Data.Repositories.V1.EventRepo
{
    public interface IEventRepo
    {
        Task<Event> CreateEvent(CreateEventDTO userInput, Guid creatorId);
    }
}
