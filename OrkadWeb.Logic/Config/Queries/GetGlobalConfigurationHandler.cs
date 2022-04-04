using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Config.Queries
{
    public class GetGlobalConfigurationHandler : IRequestHandler<GetGlobalConfigurationQuery, GlobalConfigurationResult>
    {
        public async Task<GlobalConfigurationResult> Handle(GetGlobalConfigurationQuery request, CancellationToken cancellationToken)
        {
            return new GlobalConfigurationResult
            {
                UsernameMinLength = GlobalConfiguration.USERNAME_MIN_LENGHT,
                UsernameMaxLength = GlobalConfiguration.USERNAME_MAX_LENGHT,
                UsernameRegex = GlobalConfiguration.USERNAME_REGEX.ToString(),

                PasswordMinLength = GlobalConfiguration.PASSWORD_MIN_LENGHT,
                PasswordMaxLength = GlobalConfiguration.PASSWORD_MAX_LENGHT,
                PasswordRegex = GlobalConfiguration.PASSWORD_REGEX.ToString(),

                EmailRegex = GlobalConfiguration.EMAIL_REGEX.ToString(),
            };
        }
    }
}
