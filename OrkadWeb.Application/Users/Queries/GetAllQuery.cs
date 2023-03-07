using NHibernate.Linq;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Domain.Common;
using OrkadWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        internal class Handler : IQueryHandler<GetAllUsersQuery, List<GetAllUsersQuery.Result>>
        {
            private readonly IDataService dataService;

            public Handler(IDataService dataService)
            {
                this.dataService = dataService;
            }
            public async Task<List<Result>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
            {
                return await dataService.Query<User>()
                    .Select(u => new Result
                    {
                        Id = u.Id,
                        Name = u.Username,
                        Email = u.Email,
                    })
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
