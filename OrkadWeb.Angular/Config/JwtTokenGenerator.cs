using Microsoft.IdentityModel.Tokens;
using OrkadWeb.Logic.Abstractions;
using OrkadWeb.Logic.Users;
using System.Collections.Generic;
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

        /// <summary>
        /// Generate a new JWT token
        /// </summary>
        /// <param name="claims">Claims to add to the token</param>
        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var credentials = new SigningCredentials(jwtConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            var expiration = timeProvider.Now.AddMinutes(JwtConfig.DEFAULT_EXPIRATION);
            var token = new JwtSecurityToken(jwtConfig.Issuer, jwtConfig.Audience, claims, expires: expiration, signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
