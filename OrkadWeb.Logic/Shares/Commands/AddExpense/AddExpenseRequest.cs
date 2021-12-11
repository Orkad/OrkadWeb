using MediatR;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Shares.Commands.AddExpenseOnShare
{
    public class AddExpenseRequest : IRequest<Response>
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public int ShareId { get; set; }
    }

    public class AddExpenseHandler : IRequestHandler<AddExpenseRequest, Response>
    {
        private readonly IDataService dataService;

        public AddExpenseHandler(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task<Response> Handle(AddExpenseRequest request, CancellationToken cancellationToken)
        {
            if (request.Amount <= 0)
            {
                return Response.AsError("Le montant de la dépense doit être positif et différent de zéro");
            }
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Response.AsError("Le nom de la dépense est obligatoire");
            }
            var share = await dataService.GetAsync<Share>(request.ShareId);
            if (share == null)
            {
                return Response.AsError("Impossible d'ajouter la dépense car le partage n'existe pas");
            }
            var userShare = share.UserShares.SingleOrDefault(us => us.User.Id == request.UserId);
            if (userShare == null)
            {
                return Response.AsError("L'utilisateur ne fait pas parti du partage");
            }
            
            await dataService.InsertAsync(new Expense
            {
                Amount = request.Amount,
                Date = DateTime.Now,
                Name = request.Name,
                UserShare = userShare,
            });
            return Response.AsSuccess();
        }
    }
}
