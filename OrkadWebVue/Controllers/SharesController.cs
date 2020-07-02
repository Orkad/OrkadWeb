using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Services.Business;
using OrkadWeb.Services.DTO.Common;
using OrkadWeb.Services.DTO.Expenses;
using OrkadWeb.Services.DTO.Refunds;
using OrkadWeb.Services.DTO.Shares;
using System.Collections.Generic;
using System.Security.Claims;

namespace OrkadWebVue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SharesController : ControllerBase
    {
        private readonly ShareService shareService;

        public SharesController(ShareService shareService)
        {
            this.shareService = shareService;
        }

        protected int ConnectedUserId
            => int.TryParse((User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.PrimarySid)?.Value, out int id) ? id : 0;

        [HttpGet]
        public List<ShareItem> Get() => shareService.GetSharesForUser(ConnectedUserId);

        [HttpGet("{shareId}")]
        public ShareDetail Get(int shareId) => shareService.GetShareDetail(shareId);

        [HttpGet("{shareId}/others")]
        public List<TextValue> GetOtherUsers(int shareId)
            => shareService.GetOtherUsers(ConnectedUserId, shareId);

        [HttpGet("{shareId}/users/available")]
        public List<TextValue> GetAvailableUsers(int shareId)
            => shareService.GetAvailableUsers(shareId);

        [HttpPost("{shareId}/users/add/{userId}")]
        public UserShareDetail AddUser(int shareId, int userId) => shareService.AddUser(shareId, userId);

        [HttpPost]
        public dynamic Create([FromBody] ShareCreation shareCreation) => shareService.CreateShareForUser(ConnectedUserId, shareCreation);

        [HttpPost("{shareId}/expenses")]
        public dynamic AddExpense(int shareId, [FromBody] ExpenseCreation expense)
            => shareService.AddExpense(ConnectedUserId, shareId, expense);

        [HttpPost("{shareId}/refunds")]
        public RefundItem AddRefund(int shareId, [FromBody] RefundCreation refund)
            => shareService.AddRefund(ConnectedUserId, shareId, refund);

        [HttpDelete("{shareId}")]
        public void Delete(int shareId)
            => shareService.DeleteShare(ConnectedUserId, shareId);

        [HttpDelete("{shareId}/expenses/{expenseId}")]
        public void DeleteExpense(int shareId, int expenseId)
            => shareService.DeleteExpense(ConnectedUserId, shareId, expenseId);

        [HttpDelete("{shareId}/refunds/{refundId}")]
        public void DeleteRefund(int shareId, int refundId)
            => shareService.DeleteRefund(ConnectedUserId, shareId, refundId);
    }
}