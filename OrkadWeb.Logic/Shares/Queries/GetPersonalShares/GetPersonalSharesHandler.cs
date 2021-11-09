using AutoMapper;
using MediatR;
using NHibernate.Linq;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.Shares.ReadModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Shares.Queries.GetPersonalShares
{
    public class GetPersonalSharesHandler : IRequestHandler<GetPersonalSharesRequest, IEnumerable<ShareItem>>
    {
        private readonly IDataService dataService;
        private readonly IMapper mapper;

        public GetPersonalSharesHandler(IDataService dataService, IMapper mapper)
        {
            this.dataService = dataService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ShareItem>> Handle(GetPersonalSharesRequest request, CancellationToken cancellationToken)
        {
            var query = dataService.Query<Share>().Where(s => s.Owner.Id == request.UserId);
            return await mapper.ProjectTo<ShareItem>(query).ToListAsync();
        }
    }
}
