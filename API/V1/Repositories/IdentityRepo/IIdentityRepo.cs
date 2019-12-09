using System;
using System.Threading.Tasks;
using API.V1.DTO.InputDTOs.IdentityDTOs;
using API.Data.Models;

namespace API.V1.Repositories.IdentityRepo
{
    public interface IIdentityRepo
    {
        Task<User> CheckUserLoginInput(string email, string password);
        Task<Family> CheckFamilyExists(Guid familyId);
        Task<User> CreateUser(RegisterDTO userInput);
    }
}
