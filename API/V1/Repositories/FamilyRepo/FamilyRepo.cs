using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.V1.Contracts;
using API.V1.DTO.InputDTOs.FamilyDTOs;
using API.Data.Models;
using AutoMapper;
using API.Data;

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
                user.FamilyId = familyToSave.Id;
            }

            await _context.SaveChangesAsync();
            return familyToSave;
        }
    }
}
