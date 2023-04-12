using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrkadWeb.Domain.Extensions;
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
        public const int DEFAULT_EXPIRATION = 720; //12h

        private readonly IConfiguration configuration;

        public JwtConfig(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Security key for the current application
        /// </summary>
        public SecurityKey SecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetRequiredValue(CONFIG_KEY, CONFIG_KEY_MIN_LENGTH)));

        public string Issuer => configuration.GetRequiredValue(CONFIG_ISSUER);

        public string Audience => configuration.GetRequiredValue(CONFIG_AUDIENCE);

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
