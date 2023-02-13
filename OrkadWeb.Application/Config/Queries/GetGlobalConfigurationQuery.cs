using MediatR;
using OrkadWeb.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Application.Config.Queries
{
    /// <summary>
    /// Get the global configuration of the application.
    /// </summary>
    public class GetGlobalConfigurationQuery : IQuery<GetGlobalConfigurationQuery.Result>
    {
        /// <summary>
        /// Global confiration result
        /// </summary>
        public class Result
        {
            /// <summary>
            /// Minimum username length for registration
            /// </summary>
            public int UsernameMinLength { get; set; }
            /// <summary>
            /// Maximum username length for registration
            /// </summary>
            public int UsernameMaxLength { get; set; }

            /// <summary>
            /// Username Regex acceptation for registration
            /// </summary>
            public string UsernameRegex { get; set; }

            /// <summary>
            /// Minimum password length for registration
            /// </summary>
            public int PasswordMinLength { get; set; }

            /// <summary>
            /// Maximum password length for registration
            /// </summary>
            public int PasswordMaxLength { get; set; }

            /// <summary>
            /// Password Regex acceptation for registration
            /// </summary>
            public string PasswordRegex { get; set; }

            /// <summary>
            /// Regex for emails
            /// </summary>
            public string EmailRegex { get; set; }
        }
        public class Handler : IQueryHandler<GetGlobalConfigurationQuery, Result>
        {
            public async Task<Result> Handle(GetGlobalConfigurationQuery request, CancellationToken cancellationToken)
            {
                return await Task.FromResult(new Result
                {
                    UsernameMinLength = GlobalConfiguration.USERNAME_MIN_LENGHT,
                    UsernameMaxLength = GlobalConfiguration.USERNAME_MAX_LENGHT,
                    UsernameRegex = GlobalConfiguration.USERNAME_REGEX.ToString(),

                    PasswordMinLength = GlobalConfiguration.PASSWORD_MIN_LENGHT,
                    PasswordMaxLength = GlobalConfiguration.PASSWORD_MAX_LENGHT,
                    PasswordRegex = GlobalConfiguration.PASSWORD_REGEX.ToString(),

                    EmailRegex = GlobalConfiguration.EMAIL_REGEX.ToString(),
                });
            }
        }
    }
}
