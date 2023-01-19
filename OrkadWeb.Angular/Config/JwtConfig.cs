using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrkadWeb.Logic.Abstractions;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrkadWeb.Angular.Config
{
    /// <summary>
    /// Jwt configuration for the current application
    /// </summary>
    public class JwtConfig : IConfigureNamedOptions<JwtBearerOptions>
    {
        public const string CONFIG_KEY = "Jwt:Key";
        public const int CONFIG_KEY_MIN_LENGTH = 16;
        public const string CONFIG_ISSUER = "Jwt:Issuer";
        public const string CONFIG_AUDIENCE = "Jwt:Audience";
        public const int DEFAULT_EXPIRATION = 15;

        private readonly IConfiguration configuration;
        private readonly ITimeProvider timeProvider;

        public JwtConfig(IConfiguration configuration, ITimeProvider timeProvider)
        {
            this.configuration = configuration;
            this.timeProvider = timeProvider;
        }

        /// <summary>
        /// Security key for the current application
        /// </summary>
        public SecurityKey SecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetRequiredValue(CONFIG_KEY, CONFIG_KEY_MIN_LENGTH)));

        public string Issuer => configuration.GetRequiredValue(CONFIG_ISSUER);

        public string Audience => configuration.GetRequiredValue(CONFIG_AUDIENCE);

        /// <summary>
        /// Generate a new JWT token
        /// </summary>
        /// <param name="claims">Claims to add to the token</param>
        /// <param name="exp">(optionnal) expiration in minutes from nom</param>
        public string GenerateToken(IEnumerable<Claim> claims, int exp = DEFAULT_EXPIRATION)
        {
            var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            var expiration = timeProvider.Now.AddMinutes(exp);
            var token = new JwtSecurityToken(Issuer, Audience, claims, expires: expiration, signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public void Configure(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = SecurityKey,
            };
        }

        public void Configure(string name, JwtBearerOptions options)
        {
            Configure(options);
        }
    }
}
