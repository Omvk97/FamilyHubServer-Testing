using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.V1.Contracts;
using API.V1.DTO.InputDTOs.UserDTOs;
using API.Helpers.Hashing;
using API.Data.Models;
using Microsoft.EntityFrameworkCore;
using API.Data;

namespace API.V1.Repositories.UserRepo
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
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new ArgumentException(ErrorMessages.UserDoesNotExist);

            return user;
        }

        public async Task<User> UpdateUser(Guid userId, UpdateUserDTO userInput)
        {
            var userToUpdate = await _context.Users.FindAsync(userId);
            if (userToUpdate == null) throw new ArgumentException(ErrorMessages.UserDoesNotExist);

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
            if (user == null) throw new ArgumentException(ErrorMessages.UserDoesNotExist);
            _context.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Family> GetUserFamily(Guid userId)
        {
            var user = await _context.Users.Include(u => u.Family)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) throw new ArgumentException(ErrorMessages.UserDoesNotExist);

            return user.Family;
        }

        public async Task<IEnumerable<Event>> GetUserEvents(Guid userId)
        {
            // All users fields are also gathered here as it is needed to fill out participants in each event
            var user = await _context.Users
                .Where(ue => ue.Id == userId)
                .Include(ue => ue.Events)
                .ThenInclude(e => e.Event)
                .SingleOrDefaultAsync();

            if (user == null) throw new ArgumentException(ErrorMessages.UserDoesNotExist);

            return user.Events.Select(ue => ue.Event).ToList();
        }
    }
}
