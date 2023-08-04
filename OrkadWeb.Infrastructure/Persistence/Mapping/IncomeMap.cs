using FluentNHibernate.Mapping;
using OrkadWeb.Domain.Entities;

namespace OrkadWeb.Infrastructure.Persistence.Mapping
{
    public class IncomeMap : ClassMap<Income>
    {
        public IncomeMap()
        {
            Table("income");
            Id(x => x.Id, "id");
            Map(x => x.Amount, "amount").Not.Nullable();
            Map(x => x.Name, "name").Length(255).Not.Nullable();
            References(x => x.Owner, "owner").Not.Nullable();
        }
    }
}
