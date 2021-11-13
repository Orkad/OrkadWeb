using MediatR;
using OrkadWeb.Logic.Shares.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Logic.Shares.Queries.GetPersonalShares
{
    public class GetPersonalSharesRequest : IRequest<IEnumerable<ShareItem>>
    {
        public int UserId { get; set; }
    }
}
