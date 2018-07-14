using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Leaf.Authorization
{
    public class JwtTokenProvider
    {
        public JwtTokenProvider(IConfiguration config)
        {
            TokenOptions = new TokenAuthenticationOptions();
            config?.GetSection("authentication").Bind(TokenOptions);
        }

        private TokenAuthenticationOptions TokenOptions { get; }

        public JwtSecurityToken GetJwtSecurityToken(IEnumerable<Claim> claims, int? expireMins = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenOptions.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                TokenOptions.Issuer,
                TokenOptions.Audience,
                claims,
                expires: expireMins.HasValue
                    ? expireMins.Value != 0 ? DateTime.Now.AddMinutes(expireMins.Value) : DateTime.Now.AddYears(1)
                    : DateTime.Now.AddMinutes(TokenOptions.Expires),
                signingCredentials: creds);

            return token;
        }

        public bool ValidateJwtSecurityToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = TokenOptions.Issuer,
                ValidAudience = TokenOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(TokenOptions.Secret))
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

            return principal != null;
        }
    }
}