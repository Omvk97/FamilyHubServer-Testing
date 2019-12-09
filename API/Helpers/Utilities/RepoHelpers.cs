using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.V1.Contracts;
using API.Data.Models;
using API.Data;

namespace API.Helpers.Utilities
{
    public static class RepoHelpers
    {
        public async static Task<ICollection<User>> CheckUsersExist(ICollection<Guid> userIds, DataContext context)
        {
            HashSet<User> users = new HashSet<User>();

            // Check that all users exists
            foreach (var userId in userIds)
            {
                var user = await context.Users.FindAsync(userId);
                if (user == null)
                    throw new ArgumentException(ErrorMessages.MemberDoesNotExist);

                users.Add(user);
            }

            return users;
        }
    }
}
