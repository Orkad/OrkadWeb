using NHibernate.Linq;
using OrkadWeb.Domain.Consts;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace OrkadWeb.Application.Users.Queries;

/// <summary>
/// Get all registered users
/// </summary>
public class GetAllUsersQuery : IQuery<List<GetAllUsersQuery.Result>>
{
    public class Result
    {
        public int Id { get; set; }
        public string Name { get; init; }
        public string Email { get; set; }
        public string Role { get; init; }
    }

    internal class Handler : IQueryHandler<GetAllUsersQuery, List<Result>>
    {
        private readonly IDataService dataService;
        private readonly IAppUser authenticatedUser;


        public Handler(IDataService dataService, IAppUser authenticatedUser)
        {
            this.dataService = dataService;
            this.authenticatedUser = authenticatedUser;
        }
        public async Task<List<Result>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            if (authenticatedUser.Role != UserRoles.Admin)
            {
                throw new SecurityException();
            }
            return await dataService.Query<User>()
                .Select(u => new Result
                {
                    Id = u.Id,
                    Name = u.Username,
                    Email = u.Email,
                    Role = u.Role,
                })
                .ToListAsync(cancellationToken);
        }
    }
}