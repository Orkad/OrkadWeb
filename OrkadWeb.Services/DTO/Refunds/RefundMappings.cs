using OrkadWeb.Models;

namespace OrkadWeb.Services.DTO.Refunds
{
    public static class RefundMappings
    {
        public static RefundItem ToItem(this Refund refund) => new RefundItem
        {
            Id = refund.Id,
            Amount = refund.Amount,
            EmitterId = refund.Emitter.User.Id,
            EmitterName = refund.Emitter.User.Username,
            ReceiverId = refund.Receiver.User.Id,
            ReceiverName = refund.Receiver.User.Username,
            Date = refund.Date,
        };
    }
}
