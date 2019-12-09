using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Contracts.V1;
using API.DTO.V1.InputDTOs.EventDTOs;
using API.Helpers.Utilities;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories.V1.EventRepo
{
    public class EventRepo : IEventRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public EventRepo(DataContext dataContext, IMapper mapper)
        {
            _context = dataContext;
            _mapper = mapper;
        }

        public async Task<Event> CreateEvent(CreateEventDTO userInput, Guid creatorId)
        {
            if (userInput.AllDay != null)
            {
                var allDayDate = (DateTime)userInput.AllDay;
                userInput.AllDay = new DateTime(allDayDate.Year, allDayDate.Month, allDayDate.Day);
                userInput.StartTime = new DateTime(allDayDate.Year, allDayDate.Month, allDayDate.Day, 0, 0, 0);
                userInput.EndTime = new DateTime(allDayDate.Year, allDayDate.Month, allDayDate.Day, 23, 59, 59);
            }
            
            if (userInput.ParticipantIds == null)
            {
                userInput.ParticipantIds = new HashSet<Guid>();
                userInput.ParticipantIds.Add(creatorId);
            }
            else
            {
                if (!userInput.ParticipantIds.Contains(creatorId))
                {
                    userInput.ParticipantIds.Add(creatorId);
                }
            }

            var users = await RepoHelpers.CheckUsersExist(userInput.ParticipantIds, _context);

            var eventToSave = _mapper.Map<Event>(userInput);

            eventToSave.OwnerId = creatorId;

            _context.Events.Add(eventToSave);

            if (userInput.RepeatDetails != null)
            {
                var eventRepeatDetailsToSave = _mapper.Map<EventRepeatDetails>(userInput.RepeatDetails);
                eventRepeatDetailsToSave.Id = eventToSave.Id;

                _context.EventRepeatDetails.Add(eventRepeatDetailsToSave);

                eventToSave.RepeatDetailsId = eventRepeatDetailsToSave.Id;
            }

            // Add event to the users
            foreach (var user in users)
            {
                var userEvent = new UserEvent
                {
                    EventId = eventToSave.Id,
                    UserId = user.Id
                };

                _context.UserEvents.Add(userEvent);
                eventToSave.Participants.Add(userEvent);
                user.Events.Add(userEvent);
            }

            await _context.SaveChangesAsync();

            return eventToSave;
        }
    }
}
