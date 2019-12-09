using System;
using System.Threading.Tasks;
using API.V1.DTO.InputDTOs.FamilyDTOs;
using API.Data.Models;

namespace API.V1.Repositories.FamilyRepo
{
    public interface IFamilyRepo
    {
        Task<Family> CreateFamily(CreateFamilyDTO userInput, Guid creatorId);
    }
}
