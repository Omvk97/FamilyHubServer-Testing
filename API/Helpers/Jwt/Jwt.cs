using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Helpers.Jwt
{
    public sealed class Jwt : IJwt
    {
        private readonly IConfiguration _configuration;

        public Jwt(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateJwt(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("email", user.Email),
                new Claim("userId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(_configuration["JWT:ISSUER"],
                _configuration["JWT:ISSUER"],
                claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Dictionary<string, string> ReadVerifiedJwtToken(string verifiedJwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(verifiedJwtToken);
            Dictionary<string, string> claims = new Dictionary<string, string>();
            foreach (Claim c in token.Claims)
            {
                claims.Add(c.Type, c.Value);
            }
            return claims;
        }
    }
}
