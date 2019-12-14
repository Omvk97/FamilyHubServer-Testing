using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.V1.DTO.InputDTOs.EventDTOs;
using API.Helpers.Utilities;
using API.Data.Models;
using AutoMapper;
using API.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using API.V1.Contracts;
using static API.Data.Models.EventRepeatDetails;

namespace API.V1.Repositories.EventRepo
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

        public async Task<ICollection<Event>> GetAllEvents(Guid? userId)
        {
            IList<Event> events;

            if (userId != null)
            {
                var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
                if (!userExists) throw new ArgumentException(ErrorMessages.UserDoesNotExist);

                events = await _context.Events
                    .Where(e => e.Participants.Select(p => p.UserId).ToList().Contains((Guid)userId))
                    .ToListAsync();
            }
            else
            {
                events = await _context.Events.ToListAsync();
            }

            return events;
        }

        public async Task<Event> GetEventById(Guid eventId)
        {
            var fetchedEvent = await _context.Events.FindAsync(eventId);
            if (fetchedEvent == null) throw new ArgumentException(ErrorMessages.EventDoesNotExist);
            return fetchedEvent;
        }

        // TODO: Find a better way to update fields in an object with validation
        public async Task<Event> UpdateEvent(Guid eventId, UpdateEventDTO userInput)
        {
            var eventToUpdate = await _context.Events
                .Where(e => e.Id == eventId)
                .Include(e => e.Participants)
                .Include(e => e.RepeatDetails)
                .SingleOrDefaultAsync();
            if (eventToUpdate == null) throw new ArgumentException(ErrorMessages.EventDoesNotExist);

            // If allDay should be removed, new start time and new end time is required
            if (userInput.RemoveAllDay)
            {
                eventToUpdate.AllDay = null;
                eventToUpdate.StartTime = (DateTime)userInput.NewStartTime;
                eventToUpdate.EndTime = (DateTime)userInput.NewEndTime;
            }
            else
            {
                if (userInput.NewAllDay != null)
                {
                    var allDayDate = (DateTime)userInput.NewAllDay;
                    userInput.NewAllDay = new DateTime(allDayDate.Year, allDayDate.Month, allDayDate.Day);
                    userInput.NewStartTime = new DateTime(allDayDate.Year, allDayDate.Month, allDayDate.Day, 0, 0, 0);
                    userInput.NewEndTime = new DateTime(allDayDate.Year, allDayDate.Month, allDayDate.Day, 23, 59, 59);
                }
            }

            if (userInput.NewTitle != null) eventToUpdate.Title = userInput.NewTitle;
            if (userInput.NewDescription != null) eventToUpdate.Description = userInput.NewDescription;

            if (userInput.NewStartTime != null)
            {
                eventToUpdate.StartTime = (DateTime)userInput.NewStartTime;
                eventToUpdate.AllDay = null;
            }
            if (userInput.NewEndTime != null)
            {
                eventToUpdate.EndTime = (DateTime)userInput.NewEndTime;
                eventToUpdate.AllDay = null;
            }

            if (userInput.NewRepeatDetails != null)
            {
                var repeatDetails = userInput.NewRepeatDetails;
                if (eventToUpdate.RepeatDetails == null)
                {
                    eventToUpdate.RepeatDetails = new EventRepeatDetails();
                }
                if (repeatDetails.NewFrequency != null)
                {
                    Console.WriteLine((FrequencyOption)Enum.Parse(typeof(FrequencyOption), userInput.NewRepeatDetails.NewFrequency));
                    eventToUpdate.RepeatDetails.Frequency = (FrequencyOption)Enum.Parse(typeof(FrequencyOption), userInput.NewRepeatDetails.NewFrequency);
                    if (eventToUpdate.RepeatDetails.Frequency != FrequencyOption.Weekly)
                    {
                        eventToUpdate.RepeatDetails.WeekDays = null;
                    }
                }
                if (repeatDetails.NewWeekDays != null)
                    eventToUpdate.RepeatDetails.WeekDays = repeatDetails.NewWeekDays;
                if (repeatDetails.NewEndRepeat != null)
                    eventToUpdate.RepeatDetails.EndRepeat = repeatDetails.NewEndRepeat;
                if (repeatDetails.NewRepeatExceptions != null)
                {
                    foreach (var exception in repeatDetails.NewRepeatExceptions)
                    {
                        if (exception.ChangedEndTime == null && exception.ChangedStartTime == null && exception.Removed == null)
                            throw new ArgumentException(ErrorMessages.OneParameterMustBeSetInARepeatException);
                        eventToUpdate.RepeatDetails.Exceptions.Add(exception);
                    }
                }
            }

            if (userInput.NewLocation != null)
                eventToUpdate.Location = userInput.NewLocation;
            if (userInput.NewAlert != null)
                eventToUpdate.Alert = userInput.NewAlert;
            if (userInput.NewParticipantIds != null)
            {
                var users = await RepoHelpers.CheckUsersExist(userInput.NewParticipantIds, _context);
                foreach (var user in users)
                {
                    // Add event to user if user isn't already in the event
                    if (!eventToUpdate.Participants.Select(ue => ue.UserId).ToList().Contains(user.Id))
                    {
                        var userEvent = new UserEvent
                        {
                            EventId = eventToUpdate.Id,
                            UserId = user.Id
                        };

                        _context.UserEvents.Add(userEvent);
                        eventToUpdate.Participants.Add(userEvent);
                        user.Events.Add(userEvent);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return eventToUpdate;
        }

        public async Task<Event> DeleteEvent(Guid eventId)
        {
            var fetchedEvent = await _context.Events.FindAsync(eventId);
            if (fetchedEvent == null) throw new ArgumentException(ErrorMessages.EventDoesNotExist);

            _context.Remove(fetchedEvent);
            await _context.SaveChangesAsync();
            return fetchedEvent;
        }


    }
}
