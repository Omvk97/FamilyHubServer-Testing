using System;
using System.Threading.Tasks;
using API.DTO.V1.InputDTOs.FamilyDTOs;
using API.Models;

namespace API.Data.Repositories.V1.FamilyRepo
{
    public interface IFamilyRepo
    {
        Task<Family> CreateFamily(CreateFamilyDTO userInput, Guid creatorId);
    }
}
