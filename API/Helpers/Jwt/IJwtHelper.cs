using System.Collections.Generic;
using API.Data.Models;

namespace API.Helpers.JwtHelper
{
    public interface IJwtHelper
    {
        string CreateJwt(User user);
        bool TestJwt(string token);
        Dictionary<string, string> ReadVerifiedJwtToken(string authorizationHeaderContent);
    }
}
