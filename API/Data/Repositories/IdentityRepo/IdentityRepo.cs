using System;
using System.Threading.Tasks;
using API.Contracts.V1;
using API.DTO.InputDTOs.V1.IdentityDTOs;
using API.Helpers.Hashing;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories.V1
{
    public class IdentityRepo : IIdentityRepo
    {
        private readonly DataContext _context;
        private readonly IHashing _hashing;
        private readonly IMapper _mapper;

        public IdentityRepo(DataContext context, IHashing hashing, IMapper mapper)
        {
            _context = context;
            _hashing = hashing;
            _mapper = mapper;
        }

        public async Task<User> CheckUserLoginInput(string email, string password)
        {
            try
            {
                var user = await _context.Users
                .SingleOrDefaultAsync(
                c => c.Email.Equals(email));

                if (user == null) return null;

                var passwordCorrect = _hashing.Check(user.Password, password);

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Family> CheckFamilyExists(Guid familyId)
        {
            return await _context.Families.FirstOrDefaultAsync(f => f.Id == familyId);
        }

        public async Task<User> CreateUser(RegisterDTO userInput)
        {
            var emailInUse = await _context.Users.AnyAsync(u => u.Email == userInput.Email);
            if (emailInUse)
                throw new ArgumentException(ErrorMessages.EmailInuse);

            if (userInput.FamilyId != null)
            {
                var family = await CheckFamilyExists((Guid)userInput.FamilyId);
                if (family == null)
                    throw new ArgumentException(ErrorMessages.FamilyDoesNotExist);
            }

            var user = _mapper.Map<User>(userInput);
            user.Password = _hashing.Hash(user.Password);

            _context.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
