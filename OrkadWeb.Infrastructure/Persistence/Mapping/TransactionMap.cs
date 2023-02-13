using FluentNHibernate.Mapping;
using OrkadWeb.Domain.Entities;

namespace OrkadWeb.Infrastructure.Persistence.Mapping
{
    public class TransactionMap : ClassMap<Transaction>
    {
        public TransactionMap()
        {
            Table("transaction");
            Id(x => x.Id, "id");
            References(x => x.Owner, "owner").Not.Nullable();
            Map(x => x.Amount, "amount");
            Map(x => x.Name, "name");
            Map(x => x.Date, "date");
        }
    }
}
