using API.DTO.InputDTOs.V1.IdentityDTOs;
using API.DTO.V1.OutputDTOs.UserDTOs;
using API.Models;
using AutoMapper;

namespace API.DTO
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User
            CreateMap<RegisterDTO, User>();
            CreateMap<User, SuccessGetUserDTO>();
            
        }
    }
}
