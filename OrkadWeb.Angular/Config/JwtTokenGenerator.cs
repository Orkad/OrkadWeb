using Microsoft.IdentityModel.Tokens;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Application.Users;
using OrkadWeb.Domain.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OrkadWeb.Angular.Config
{
    public class JwtTokenGenerator : IIdentityTokenGenerator
    {
        private readonly JwtConfig jwtConfig;
        private readonly ITimeProvider timeProvider;

        public JwtTokenGenerator(JwtConfig jwtConfig, ITimeProvider timeProvider)
        {
            this.jwtConfig = jwtConfig;
            this.timeProvider = timeProvider;
        }

        /// <inheritdoc/>
        public string GenerateToken(User user)
        {
            var credentials = new SigningCredentials(jwtConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            var expiration = timeProvider.Now.AddMinutes(JwtConfig.DEFAULT_EXPIRATION);
            var token = new JwtSecurityToken(jwtConfig.Issuer, jwtConfig.Audience, GetClaims(user), expires: expiration, signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private static Claim[] GetClaims(User user)
        {
            return new[] {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Name, user.Username),
                    new Claim("role", user.Role),
                    new Claim("confirmed", (user.Confirmation != null).ToString().ToLower(), ClaimValueTypes.Boolean),
                };
        }
    }
}
