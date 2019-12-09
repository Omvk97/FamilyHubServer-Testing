using System;
using System.Threading.Tasks;
using API.V1.DTO.InputDTOs.EventDTOs;
using API.Data.Models;

namespace API.V1.Repositories.EventRepo
{
    public interface IEventRepo
    {
        Task<Event> CreateEvent(CreateEventDTO userInput, Guid creatorId);
    }
}
