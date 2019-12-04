using System.Collections.Generic;
using API.Models;

namespace API.Helpers.Jwt
{
    public interface IJwt
    {
        string CreateJwt(Credential userCredential);
        Dictionary<string, string> ReadVerifiedJwtToken(string verifiedJwtToken);
    }
}
