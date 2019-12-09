using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.V1.DTO.InputDTOs.UserDTOs;
using API.Data.Models;

namespace API.V1.Repositories.UserRepo
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
