using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Helpers.JwtHelper
{
    public sealed class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // TODO: Find a better place to put this
        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:ISSUER"],
                ValidAudience = configuration["JWT:ISSUER"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:KEY"]))
            };
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

        public Dictionary<string, string> ReadVerifiedJwtToken(string authorizationHeaderContent)
        {
            var verifiedToken = authorizationHeaderContent.Split(" ")[1];

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(verifiedToken);
            Dictionary<string, string> claims = new Dictionary<string, string>();
            foreach (Claim c in token.Claims)
            {
                claims.Add(c.Type, c.Value);
            }
            return claims;
        }

        public bool TestJwt(string token)
        {
            try
            {
                SecurityToken validatedToken;
                var handler = new JwtSecurityTokenHandler();
                handler.ValidateToken(token, GetTokenValidationParameters(_configuration), out validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
