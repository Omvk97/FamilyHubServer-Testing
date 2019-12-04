using System;
using System.Threading.Tasks;
using API.Helpers.Hashing;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class AuthRepo : IAuthRepo
    {
        private readonly DataContext _context;
        private readonly IHashing _hashing;

        public AuthRepo(DataContext context, IHashing hashing)
        {
            _context = context;
            _hashing = hashing;
        }

        public async Task<Credential> CheckUserInput(string email, string password)
        {
            try
            {
                var userCredential = await _context.Credentials
                .FirstOrDefaultAsync(
                c => c.Email.Equals(email));

                var passwordCorrect = _hashing.Check(userCredential.Password, password);

                return userCredential;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
