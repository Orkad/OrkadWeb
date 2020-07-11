using FluentNHibernate.Mapping;
using OrkadWeb.Models;

namespace OrkadWeb.Services.NHibernate.Mapping
{
    public class RefundMap : ClassMap<Refund>
    {
        public RefundMap()
        {
            Table("refund");
            Id(x => x.Id, "id");
            Map(x => x.Amount, "amount");
            Map(x => x.Date, "date");
            References(x => x.Emitter, "user_share_emitter_id");
            References(x => x.Receiver, "user_share_receiver_id");
        }
    }
}
