using API.DTO.InputDTOs.V1.Identity;
using API.Models;
using AutoMapper;

namespace API.DTO
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterDTO, User>();
        }
    }
}
