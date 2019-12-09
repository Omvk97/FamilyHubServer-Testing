using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Contracts.V1;
using API.DTO.V1.InputDTOs.UserDTOs;
using API.Helpers.Hashing;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories.V1.UserRepo
{
    public class UserRepo : IUserRepo
    {
        private readonly DataContext _context;
        private readonly IHashing _hashing;

        public UserRepo(DataContext dataContext, IHashing hashing)
        {
            _context = dataContext;
            _hashing = hashing;
        }

        public async Task<IEnumerable<User>> GetAllUsers(bool includeFamily, bool includeEvents)
        {
            var users = _context.Users;
            if (includeFamily) users.Include(u => u.Family);
            if (includeEvents)
            {
                users.Include(u => u.Events).ThenInclude(ue => ue.Event);
            }

            return await users.ToListAsync();
        }

        public async Task<User> GetUserById(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> UpdateUser(Guid userId, UpdateUserDTO userInput)
        {
            var userToUpdate = await GetUserById(userId);

            if (userInput.NewName != null) userToUpdate.Name = userInput.NewName;
            if (userInput.NewPassword != null) userToUpdate.Password = _hashing.Hash(userInput.NewPassword);
            if (userInput.NewProfileColor != null) userToUpdate.ProfileColor = userInput.NewProfileColor;

            if (userInput.NewFamilyId != null)
            {
                var family = await _context.Families.FirstOrDefaultAsync(f => f.Id == userInput.NewFamilyId);
                if (family == null)
                    throw new ArgumentException(ErrorMessages.FamilyDoesNotExist);

                userToUpdate.FamilyId = userInput.NewFamilyId;
            }

            if (userInput.NewEmail != null)
            {
                var emailInUse = await _context.Users.AnyAsync(u => u.Email == userInput.NewEmail);
                if (emailInUse)
                    throw new ArgumentException(ErrorMessages.EmailInuse);

                userToUpdate.Email = userInput.NewEmail;
            }

            await _context.SaveChangesAsync();

            return userToUpdate;
        }

        public async Task<User> DeleteUser(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            _context.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Family> GetUserFamily(Guid userId)
        {
            var user = await _context.Users.Include(u => u.Family)
                .FirstOrDefaultAsync(u => u.Id == userId);
            return user.Family;
        }

        public async Task<IEnumerable<Event>> GetUserEvents(Guid userId)
        {
            var userEvents = await _context.Users
                .Where(ue => ue.Id == userId)
                .Include(ue => ue.Events)
                .ThenInclude(e => e.Event)
                .ToListAsync();
            var test = userEvents.SelectMany(u => u.Events).Select(ue => ue.Event).ToList();

            return test;
        }
    }
}
