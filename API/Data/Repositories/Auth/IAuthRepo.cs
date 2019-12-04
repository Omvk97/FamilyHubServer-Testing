using System;
using System.Threading.Tasks;
using API.Models;

namespace API.Data.Repositories
{
    public interface IAuthRepo
    {
        Task<Credential> CheckUserInput(string email, string password);
    }
}
