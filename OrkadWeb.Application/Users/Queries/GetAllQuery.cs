using NHibernate.Linq;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Domain.Common;
using OrkadWeb.Domain.Consts;
using OrkadWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Application.Users.Queries
{
    /// <summary>
    /// Get all registered users
    /// </summary>
    public class GetAllUsersQuery : IQuery<List<GetAllUsersQuery.Result>>
    {
        public class Result
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
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
}
