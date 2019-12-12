using System;
using System.Threading.Tasks;
using API.V1.DTO.InputDTOs.FamilyDTOs;
using API.Data.Models;
using System.Collections.Generic;

namespace API.V1.Repositories.FamilyRepo
{
    public interface IFamilyRepo
    {
        Task<Family> CreateFamily(CreateFamilyDTO userInput, Guid creatorId);
        Task<ICollection<Family>> GetAllFamilies();
        Task<Family> GetFamilyById(Guid familyId);
        Task<Family> UpdateFamily(Guid familyId, UpdateFamilyDTO userInput);
        Task<Family> DeleteFamily(Guid familyId);
    }
}
