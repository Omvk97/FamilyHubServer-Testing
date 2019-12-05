using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO.V1.InputDTOs.UserDTOs;
using API.Models;

namespace API.Data.Repositories.V1.UserRepo
{
    public interface IUserRepo
    {
        Task<IEnumerable<User>> GetAllUsers(bool includeFamily, bool includeEvents);
        Task<User> GetUserById(Guid userId);
        Task<User> UpdateUser(Guid userId, UpdateUserDTO userInput);
        Task<User> DeleteUser(Guid userId);
        Task<Family> GetUserFamily(Guid userId);
        Task<IEnumerable<Event>> GetUserEvents(Guid userId);
    }
}
