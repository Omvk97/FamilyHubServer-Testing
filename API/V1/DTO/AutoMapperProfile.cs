using System.Linq;
using API.V1.DTO.InputDTOs.IdentityDTOs;
using API.V1.DTO.InputDTOs.EventDTOs;
using API.V1.DTO.InputDTOs.FamilyDTOs;
using API.V1.DTO.OutputDTOs.EventDTOs;
using API.V1.DTO.OutputDTOs.FamilyDTOs;
using API.V1.DTO.OutputDTOs.UserDTOs;
using API.Data.Models;
using AutoMapper;

namespace API.V1.DTO
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User
            CreateMap<RegisterDTO, User>();
            CreateMap<User, SuccessGetUserDTO>();
            CreateMap<User, SuccessGetUserWithoutFamilyDTO>();
            CreateMap<User, SuccessGetUserWithoutEventDTO>();

            // Family
            CreateMap<Family, SuccessGetFamilyDTO>();
            CreateMap<CreateFamilyDTO, Family>();
            CreateMap<UpdateFamilyDTO, Family>();

            // Event
            CreateMap<CreateEventDTO, Event>();
            CreateMap<UpdateEventDTO, Event>();
            CreateMap<CreateEventRepeatDetails, EventRepeatDetails>();
            CreateMap<EventRepeatDetails, SuccessGetEventRepeatDetailsDTO>();
            CreateMap<Event, SuccessGetEventDTO>()
                .ForMember(destination =>
                destination.Participants, attribute => attribute.MapFrom(ev => ev.Participants.Select(ue => ue.User).ToList()));
        }
    }
}
