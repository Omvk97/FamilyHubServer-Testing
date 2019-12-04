using System;
using System.Threading.Tasks;
using API.DTO.InputDTOs.V1.Identity;
using API.Models;

namespace API.Data.Repositories.V1
{
    public interface IIdentityRepo
    {
        Task<User> CheckUserInput(string email, string password);
        Task<Family> CheckFamilyExists(Guid familyId);
        Task<User> CreateUser(RegisterDTO userInput);
    }
}
