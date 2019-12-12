using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.V1.Contracts;
using API.V1.DTO.InputDTOs.FamilyDTOs;
using API.Data.Models;
using AutoMapper;
using API.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.V1.Repositories.FamilyRepo
{
    public class FamilyRepo : IFamilyRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FamilyRepo(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Family> CreateFamily(CreateFamilyDTO userInput, Guid creatorId)
        {
            if (userInput.MemberIds == null)
            {
                userInput.MemberIds = new HashSet<Guid>();
                userInput.MemberIds.Add(creatorId);
            }
            else
            {
                if (!userInput.MemberIds.Contains(creatorId))
                {
                    userInput.MemberIds.Add(creatorId);
                }
            }

            // Hashset to avoid duplicates
            HashSet<User> users = new HashSet<User>();

            // Check that all users exists
            foreach (var userId in userInput.MemberIds)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    throw new ArgumentException(ErrorMessages.MemberDoesNotExist);

                users.Add(user);
            }

            var familyToSave = _mapper.Map<Family>(userInput);

            _context.Families.Add(familyToSave);

            // Add family to the users
            foreach (var user in users)
            {
                // User does not already have a family
                if (user.FamilyId == null) { 
                    user.FamilyId = familyToSave.Id;
                }
                else throw new ArgumentException(ErrorMessages.UserAlreadyHasAFamily);
            }

            await _context.SaveChangesAsync();
            return familyToSave;
        }


        public async Task<ICollection<Family>> GetAllFamilies()
        {
            var families = await _context.Families.Include(f => f.Members).ToListAsync();
            return families;
        }

        public async Task<Family> GetFamilyById(Guid familyId)
        {
            var family = await _context.Families
                .Where(f => f.Id == familyId).Include(f => f.Members).SingleOrDefaultAsync();
            if (family == null)
            {
                throw new ArgumentException(ErrorMessages.FamilyDoesNotExist);
            }
            return family;
        }

        public async Task<Family> UpdateFamily(Guid familyId, UpdateFamilyDTO userInput)
        {
            var family = await _context.Families.FindAsync(familyId);

            if (family == null)
            {
                throw new ArgumentException(ErrorMessages.FamilyDoesNotExist);
            }

            if (userInput.NewFamilyName != null)
                family.Name = userInput.NewFamilyName;
            if (userInput.NewMemberIds != null)
            {
                foreach(var memberId in userInput.NewMemberIds)
                {
                    var userToUpdate = await _context.Users.FindAsync(memberId);
                    if (userToUpdate == null)
                    {
                        throw new ArgumentException(ErrorMessages.MemberDoesNotExist);
                    }
                    userToUpdate.FamilyId = family.Id;
                }
            }

            await _context.SaveChangesAsync();
            return family;
        }

        // TODO: Foregin key constraint when deleting family
        public async Task<Family> DeleteFamily(Guid familyId)
        {
            var family = await _context.Families.FindAsync(familyId);
            if (family == null)
            {
                throw new ArgumentException(ErrorMessages.FamilyDoesNotExist);
            }
            _context.Remove(family);
            await _context.SaveChangesAsync();
            return family;
        }
    }
}
